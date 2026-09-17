using UnityEngine;

namespace SilverTau.NSR.Core.Android
{
    [CreateAssetMenu(fileName = "Android Recorder Settings", menuName = "Silver Tau/NSR/Recorders/Android Recorder Settings", order = 1)]
    public class AndroidRecorderSettings : ScriptableObject
    {
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public AndroidMediaFormat.VideoFormat videoFormat = AndroidMediaFormat.VideoFormat.MP4;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public AndroidMediaFormat.VideoCodec videoCodec = AndroidMediaFormat.VideoCodec.AVC;
        
        [Tooltip("Layers that will be displayed when you take a screenshot.")]
        public AndroidMediaFormat.AudioCodec audioCodec = AndroidMediaFormat.AudioCodec.AAC;

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