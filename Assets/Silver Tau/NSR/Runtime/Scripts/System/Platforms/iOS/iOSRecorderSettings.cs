using UnityEngine;

namespace SilverTau.NSR.Core.iOS
{
    [CreateAssetMenu(fileName = "iOS Recorder Settings", menuName = "Silver Tau/NSR/Recorders/iOS Recorder Settings", order = 1)]
    public class iOSRecorderSettings : ScriptableObject
    {
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public iOSMediaFormat.VideoFormat videoFormat = iOSMediaFormat.VideoFormat.MP4;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public iOSMediaFormat.VideoCodec videoCodec = iOSMediaFormat.VideoCodec.H264;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public iOSMediaFormat.AudioCodec audioCodec = iOSMediaFormat.AudioCodec.MPEG4AAC;

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