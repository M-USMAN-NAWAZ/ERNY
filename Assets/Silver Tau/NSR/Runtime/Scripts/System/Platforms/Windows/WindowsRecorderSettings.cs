using UnityEngine;

namespace SilverTau.NSR.Core.Windows
{
    [CreateAssetMenu(fileName = "Windows Recorder Settings", menuName = "Silver Tau/NSR/Recorders/Windows Recorder Settings", order = 1)]
    public class WindowsRecorderSettings : ScriptableObject
    {
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public WindowsMediaFormat.VideoFormat videoFormat = WindowsMediaFormat.VideoFormat.MP4;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public WindowsMediaFormat.VideoCodec videoCodec = WindowsMediaFormat.VideoCodec.H264;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public WindowsMediaFormat.AudioCodec audioCodec = WindowsMediaFormat.AudioCodec.MPEG4AAC;

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