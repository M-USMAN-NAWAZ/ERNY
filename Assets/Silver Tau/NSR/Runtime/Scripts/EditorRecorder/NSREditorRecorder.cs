using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Media;
#endif

namespace SilverTau.NSR.Core.Editor
{
	public class NSREditorRecorder : IRecorder
	{
		public RecorderSettings Settings { get; set; }
		public RecorderStatus Status { get; set; } = RecorderStatus.None;
		public bool IsRecording { get; set; } = false;

		public UnityAction<Texture> OnFrameRender { get; set; }
		
		public string OutputVideoPath { get; set; }
		
		private bool _isInit;
		private string _outputFilePath;
		
#if UNITY_EDITOR
		private MediaEncoder _mediaEncoder;
		
		private bool _editorUpdateHooked = false;
#endif
		
		private NSREditorFrameProcessor _nsrEditorFrameProcessor = new NSREditorFrameProcessor();
		private NSREditorAudioProcessor _nsrEditorAudioProcessor = new NSREditorAudioProcessor();
		
		private void EditorUpdate()
		{
#if UNITY_EDITOR
			if (!IsRecording || Status != RecorderStatus.Recording) return;
			if (!Settings.IsRecordAudio) return;
			_nsrEditorAudioProcessor.FlushQueuedSamples(_mediaEncoder);
#endif
		}
		
		public void Init()
		{
#if UNITY_EDITOR
			_isInit = true;
			Status = _isInit ? RecorderStatus.Initiated : RecorderStatus.None;
#endif
		}

		public void StartRecording(string filePath)
		{
			if (!_isInit) return;
			if (Settings == null) return;

			_outputFilePath = filePath;
#if UNITY_EDITOR
			
			PrepareRecorder();

			IsRecording = true;
			Status = RecorderStatus.Recording;

			if (!_editorUpdateHooked)
			{
				EditorApplication.update += EditorUpdate;
				_editorUpdateHooked = true;
			}
			
			if (_nsrEditorFrameProcessor != null) _nsrEditorFrameProcessor.IsRecording = true;
#endif
		}

		public void StopRecording()
		{
			if (!_isInit) return;
#if UNITY_EDITOR
			OutputVideoPath = _outputFilePath;

			if (_editorUpdateHooked)
			{
				EditorApplication.update -= EditorUpdate;
				_editorUpdateHooked = false;
			}
			
			_mediaEncoder?.Dispose();
			_nsrEditorFrameProcessor?.Dispose();

			if (Settings.IsRecordAudio)
			{
				_nsrEditorAudioProcessor?.Dispose();
			}
            
			IsRecording = false;
			Status = RecorderStatus.Stopped;
#endif
		}

		public void PauseRecording()
		{
			if (!_isInit) return;
			
#if UNITY_EDITOR
			if (_nsrEditorFrameProcessor != null) _nsrEditorFrameProcessor.IsPaused = true;
			
			IsRecording = false;
			Status = RecorderStatus.Paused;
#endif
		}

		public void ResumeRecording()
		{
			if (!_isInit) return;
			
#if UNITY_EDITOR
			if (_nsrEditorFrameProcessor != null) _nsrEditorFrameProcessor.IsPaused = false;
			
			IsRecording = true;
			Status = RecorderStatus.Recording;
#endif
		}

		public void SendFrame(IntPtr dataPtr, int size, long timestamp)
		{
#if UNITY_EDITOR
			_nsrEditorFrameProcessor.CaptureFrame(_mediaEncoder, dataPtr, size, OnFrameRender);
#endif
		}

		public void SendSample(float[] data, long timestamp)
		{
#if UNITY_EDITOR
			if (!Settings.IsRecordAudio) return;
			_nsrEditorAudioProcessor.EnqueueSamples(data);
#endif
		}

		public void Dispose()
		{
			_isInit = false;
			
#if UNITY_EDITOR
			if (_editorUpdateHooked)
			{
				EditorApplication.update -= EditorUpdate;
				_editorUpdateHooked = false;
			}
#endif
            
			IsRecording = false;
			Status = RecorderStatus.None;
		}
		
		private void PrepareRecorder()	
		{
#if UNITY_EDITOR
			
			_nsrEditorFrameProcessor.Initialize((int)Settings.Width, (int)Settings.Height);

			var targetEditorEncoder = NSREditorEncoderSettings.TargetEditorEncoder;
			var targetVideoBitrateMode = NSREditorRecorderSettings.TargetVideoBitrateMode;
			var targetH264VideoEncodingProfile = NSREditorRecorderSettings.TargetH264VideoEncodingProfile;
			var targetIncludeAlpha = NSREditorRecorderSettings.TargetIncludeAlpha;
			
#if UNITY_2021_2_OR_NEWER
			_nsrEditorFrameProcessor.InitEncoder((int)Settings.FrameRate, targetEditorEncoder, targetVideoBitrateMode, targetH264VideoEncodingProfile, targetIncludeAlpha, out var fileFormat);
#else
			_nsrEditorFrameProcessor.InitEncoder((int)Settings.FrameRate, TargetEditorEncoder, TargetVideoBitrateMode, TargetIncludeAlpha, out var fileFormat);
#endif

			if (Settings.IsRecordAudio)
			{
				_nsrEditorAudioProcessor.Initialize((int)Settings.FrameRate);
				_nsrEditorAudioProcessor.SetupAudioAttributes((int)Settings.FrameRate);
			}

			if (!_outputFilePath.EndsWith(fileFormat, StringComparison.OrdinalIgnoreCase))
			{
				if (targetEditorEncoder == EditorEncoder.VP8)
				{
					_outputFilePath = _outputFilePath.Replace(".mp4", "");
				}
				
				_outputFilePath += fileFormat;
			}
			
			
			_mediaEncoder = Settings.IsRecordAudio ? new MediaEncoder(_outputFilePath, _nsrEditorFrameProcessor.VideoAttributes, _nsrEditorAudioProcessor.AudioAttributes) : new MediaEncoder(_outputFilePath, _nsrEditorFrameProcessor.VideoAttributes);
#endif
		}
	}
}
