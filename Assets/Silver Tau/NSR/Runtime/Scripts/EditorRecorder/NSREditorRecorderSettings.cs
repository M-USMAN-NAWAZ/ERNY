using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SilverTau.NSR.Core.Editor
{
    public static class NSREditorRecorderSettings
    {
#if UNITY_EDITOR
        public static VideoBitrateMode TargetVideoBitrateMode { get; set; } = VideoBitrateMode.Medium;
		
#if UNITY_2021_2_OR_NEWER
        public static VideoEncodingProfile TargetH264VideoEncodingProfile { get; set; } = VideoEncodingProfile.H264Main;
#endif
        public static bool TargetIncludeAlpha { get; set; } = false;
#endif
    }
}