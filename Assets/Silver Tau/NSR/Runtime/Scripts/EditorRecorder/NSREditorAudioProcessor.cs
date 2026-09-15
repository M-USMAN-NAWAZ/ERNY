using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Media;
#endif

namespace SilverTau.NSR.Core.Editor
{
    public class NSREditorAudioProcessor
    {
#if UNITY_EDITOR
        private List<float> _audioBuffer = new List<float>();
        private readonly object _audioBufferLock = new object();
        private int _sampleFramesPerVideoFrame;
        
        public AudioTrackAttributes AudioAttributes { get; private set; }
        
        public void Initialize(int targetFrameRate)
        {
            int outputSampleRate = AudioSettings.outputSampleRate;
            int outputChannelCount = (int)AudioSettings.speakerMode > 2 ? 2 : (int)AudioSettings.speakerMode;
            _sampleFramesPerVideoFrame = outputChannelCount * outputSampleRate / targetFrameRate;
        }

        public void SetupAudioAttributes(int targetFrameRate)
        {
            var outputSampleRate = AudioSettings.outputSampleRate;
            var outputChannelCount = (int)AudioSettings.speakerMode > 2 ? (ushort)2 : (ushort)AudioSettings.speakerMode;
			
            AudioAttributes = new AudioTrackAttributes
            {
                sampleRate = new MediaRational(outputSampleRate),
                channelCount = outputChannelCount,
                language = "en"
            };
            
            _sampleFramesPerVideoFrame = AudioAttributes.channelCount * AudioAttributes.sampleRate.numerator / targetFrameRate;
        }
        
        public void EnqueueSamples(float[] data)
        {
            lock (_audioBufferLock)
            {
                _audioBuffer.AddRange(data);
            }
        }
        
        public void FlushQueuedSamples(MediaEncoder mediaEncoder)
        {
            if (mediaEncoder == null) return;

            float[] local;
            lock (_audioBufferLock)
            {
                if (_audioBuffer.Count == 0) return;
                local = _audioBuffer.ToArray();
                _audioBuffer.Clear();
            }
            
            using (var native = new NativeArray<float>(local, Allocator.Temp))
            {
                mediaEncoder.AddSamples(native);
            }
        }
        
        public void CaptureAudio(MediaEncoder mediaEncoder, float[] data)
        {
            using (var nativeBuffer = new NativeArray<float>(data, Allocator.Temp))
            {
                mediaEncoder?.AddSamples(nativeBuffer);
            }
        }
        
        public void Dispose()
        {
            lock (_audioBufferLock)
            {
                _audioBuffer.Clear();
                _audioBuffer = new List<float>();
            }
        }
#endif
    }
}