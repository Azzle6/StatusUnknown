namespace Core.Helpers
{
    using System;
    using UnityEditor;
    using UnityEngine;
    
    [Serializable]
    public class ScriptReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField]
        MonoScript scriptAsset;
#endif
 
        [SerializeField]
        string typeName;
 
        public Type ScriptType { get; private set; }
 
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(this.typeName))
                this.ScriptType = null;
            else
                this.ScriptType = Type.GetType(this.typeName);
        }
 
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (this.scriptAsset)
                this.typeName = this.scriptAsset.GetClass()?.AssemblyQualifiedName;
            else
                this.typeName = null;
#endif
        }
 
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ScriptReference))]
        class ScriptReferenceDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.ObjectField(position, property.FindPropertyRelative(nameof(scriptAsset)), label);
            }
        }
#endif
    }
}