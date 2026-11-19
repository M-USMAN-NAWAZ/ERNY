using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class AndroidPurchaseButtonSetup : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        BuildTarget target = report != null ? report.summary.platform : EditorUserBuildSettings.activeBuildTarget;
        if (target != BuildTarget.Android)
            return;

        var screens = scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<eventgetter>(true));
        foreach (eventgetter screen in screens)
        {
            Configure(screen.Android_btn_Monthly, "1_month", screen.buysubscibtion, screen);
            Configure(screen.Android_btn_Quaterly, "3_months", screen.buyquarterlysubscibtion, screen);
            Configure(screen.Android_Btn_Yearly, "1_year", screen.buyyearlysubscibtion, screen);
        }
    }

    private static void Configure(Button button, string productId, UnityAction onPurchased, eventgetter screen)
    {
        if (button == null)
            throw new BuildFailedException("An Android subscription button is missing.");

        if (!ProductCatalog.LoadDefaultCatalog().allValidProducts.Any(product => product.id == productId))
            throw new BuildFailedException($"Subscription {productId} is missing from the IAP catalog.");

        CodelessIAPButton billing = button.GetComponent<CodelessIAPButton>();
        if (billing == null)
            billing = button.gameObject.AddComponent<CodelessIAPButton>();
        billing.productId = productId;
        billing.buttonType = CodelessButtonType.Purchase;
        billing.automaticallyConfirmTransaction = true;

        // The wrapper waits for store initialization before submitting the purchase.
        billing.button = null;
        var handler = button.GetComponent<AndroidPurchaseButton>();
        if (handler == null)
            handler = button.gameObject.AddComponent<AndroidPurchaseButton>();
        handler.purchaseButton = button;
        handler.billingButton = billing;
        handler.screen = screen;

        billing.onOrderPending = new CodelessIAPButton.OnOrderPendingEvent();
        UnityEventTools.AddVoidPersistentListener(billing.onOrderPending, onPurchased);
        billing.onPurchaseFailed = new CodelessIAPButton.OnPurchaseFailedEvent();
        UnityEventTools.AddPersistentListener(billing.onPurchaseFailed, handler.OnPurchaseFailed);
        billing.onOrderDeferred = new CodelessIAPButton.OnOrderDeferredEvent();
        UnityEventTools.AddPersistentListener(billing.onOrderDeferred, handler.OnOrderDeferred);

#pragma warning disable CS0612, CS0618
        billing.onPurchaseCompleteLegacy = new CodelessIAPButton.OnPurchaseCompletedLegacyEvent();
        billing.onPurchaseFailedLegacy = new CodelessIAPButton.OnPurchaseFailedLegacyEvent();
        foreach (IAPButton legacy in button.GetComponents<IAPButton>())
            Object.DestroyImmediate(legacy);
#pragma warning restore CS0612, CS0618

        Debug.Log($"Configured Android purchase button {button.name} for {productId}.");
    }
}
