#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SilverTau.NSR.Core.Android
{
    [CustomEditor(typeof(AndroidRecorderSettings))]
    public class AndroidRecorderSettingsEditor : NSRPackageEditor
    {
        private AndroidRecorderSettings _target;
        
        private SerializedProperty _videoFormat;
        private SerializedProperty _videoCodec;
        private SerializedProperty _audioCodec;
        
        public override void Awake()
        {
            base.Awake();
        }
        
        private void OnEnable()
        {
            if (target) _target = (AndroidRecorderSettings)target;
            
            _videoFormat = serializedObject.FindProperty("videoFormat");
            _videoCodec = serializedObject.FindProperty("videoCodec");
            _audioCodec = serializedObject.FindProperty("audioCodec");
        
            serializedObject.ApplyModifiedProperties();
        }

        private bool _isPlaying;
        
        public override void OnInspectorGUI()
        {
            BoxLogo(null, " <b><color=#ffffff>Android Recorder Settings</color></b>");
            
            //base.OnInspectorGUI();
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);
            
            EditorGUILayout.PropertyField(_videoFormat);
            EditorGUILayout.PropertyField(_videoCodec);
            EditorGUILayout.PropertyField(_audioCodec);
            
            GUILayout.Space(10);
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_target);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

#endif
