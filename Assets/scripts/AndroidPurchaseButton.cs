using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public sealed class AndroidPurchaseButton : MonoBehaviour
{
    public Button purchaseButton;
    public CodelessIAPButton billingButton;
    public eventgetter screen;
    private Coroutine waitingForStore;

    private void OnEnable()
    {
        purchaseButton.onClick.AddListener(Purchase);
    }

    private void OnDisable()
    {
        purchaseButton.onClick.RemoveListener(Purchase);
        if (waitingForStore != null)
            StopCoroutine(waitingForStore);
        waitingForStore = null;
    }

    private void Purchase()
    {
        if (waitingForStore == null)
            waitingForStore = StartCoroutine(WaitForStore());
    }

    private IEnumerator WaitForStore()
    {
        // Defer one frame so a synchronous failure cannot leave a stale coroutine handle.
        yield return null;
        var store = CodelessIAPStoreListener.Instance;
        float deadline = Time.realtimeSinceStartup + 15f;
        while (!store.IsInitialized() && Time.realtimeSinceStartup < deadline)
            yield return null;
        waitingForStore = null;

        if (!store.IsInitialized())
        {
            ShowMessage("Google Play couldn't connect. Check your internet connection and try again.");
            yield break;
        }

        Product product = store.GetProduct(billingButton.productId);
        if (product == null || !product.availableToPurchase)
        {
            Debug.LogWarning($"Google Play subscription unavailable: {billingButton.productId}");
            ShowMessage("This subscription is currently unavailable from Google Play. Please try again later.");
            yield break;
        }

        store.InitiatePurchase(billingButton.productId);
    }

    public void OnPurchaseFailed(FailedOrder order)
    {
        if (order.FailureReason == PurchaseFailureReason.UserCancelled)
            return;
        Debug.LogWarning($"Google Play purchase failed: {order.FailureReason}");
        ShowMessage("Google Play couldn't start or complete this purchase. Please try again.");
    }

    public void OnOrderDeferred(DeferredOrder order)
    {
        ShowMessage("Google Play is waiting for payment approval. Your subscription will activate after payment completes.");
    }

    private void ShowMessage(string message)
    {
        if (screen == null || screen.popuppanel == null)
            return;
        screen.loader.SetActive(false);
        screen.UpgradeBtn.SetActive(false);
        screen.popupclose.SetActive(true);
        screen.popuprefuel.SetActive(false);
        screen.popup.SetActive(true);
        screen.popuppanel.header.text = "Google Play purchase";
        screen.popuppanel.description.text = message;
        screen.popuppanel.pp.Play("popp");
    }
}
