using SilverTau.NSR.Recorders.Video;
using UnityEngine;
using UnityEngine.UI;

namespace SilverTau.NSR.Samples
{
    public class RecordUIManager : MonoBehaviour
    {
        
        [Tooltip("Targeted UI canvas.")]
        [SerializeField]private Canvas canvas;
        
        [Tooltip("Targeted UI toggle.")]
        [SerializeField] private Toggle tgRecordUI;
        
        private void Start()
        {
            tgRecordUI.onValueChanged.AddListener(ChangeRecordUIStatus);
        }

        /// <summary>
        /// A function that changes the Render mode of the target UI canvas.
        /// </summary>
        /// <param name="value">Toggle value.</param>
        private void ChangeRecordUIStatus(bool value)
        {
            var uvr = UniversalVideoRecorder.Instance;
            
            if(uvr.NSRVideoRecorder == null) return;
            
            if (!value)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                if (uvr) uvr.NSRVideoRecorder.ConfigInputUILayer = false;
                return;
            }
            
            var targetCamera = uvr.NSRVideoRecorder.FindActiveTargetCamera();
            canvas.worldCamera = uvr != null ? targetCamera : Camera.main;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            if (uvr) uvr.NSRVideoRecorder.ConfigInputUILayer = true;
        }
    }
}
