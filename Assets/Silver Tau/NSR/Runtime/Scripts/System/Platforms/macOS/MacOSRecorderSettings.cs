using UnityEngine;

namespace SilverTau.NSR.Core.MacOS
{
    [CreateAssetMenu(fileName = "MacOS Recorder Settings", menuName = "Silver Tau/NSR/Recorders/MacOS Recorder Settings", order = 1)]
    public class MacOSRecorderSettings : ScriptableObject
    {
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public MacOSMediaFormat.VideoFormat videoFormat = MacOSMediaFormat.VideoFormat.MP4;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public MacOSMediaFormat.VideoCodec videoCodec = MacOSMediaFormat.VideoCodec.H264;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public MacOSMediaFormat.AudioCodec audioCodec = MacOSMediaFormat.AudioCodec.MPEG4AAC;

        public MediaSettings GetMediaSettings()
        {
            return new MediaSettings
            {
                VideoFormat = videoFormat,
                VideoCodec = videoCodec,
                AudioCodec = audioCodec
            };
        }
    }
}