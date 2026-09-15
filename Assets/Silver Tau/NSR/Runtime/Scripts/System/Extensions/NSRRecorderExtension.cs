using UnityEngine;
using SilverTau.NSR.Core.Editor;
using SilverTau.NSR.Recorders.Internal;

namespace SilverTau.NSR.Core
{
    public static class NSRRecorderExtension
    {
        public static void SetupRecorder(this NSR_VideoRecorder nsrVideoRecorder)
        {
            nsrVideoRecorder.SetRecorder(CreateRecorder());
        }
        
        private static IRecorder CreateRecorder()
        {
#if UNITY_EDITOR
            return new NSREditorRecorder();
#else
            return new NSRUniversalRecorder();
#endif
        }
    }
}