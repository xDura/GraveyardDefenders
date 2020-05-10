#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace XD
{
    [CustomEditor(typeof(ParticleSystemManager))]
    public class ParticleSystemManagerInspector : Editor
    {
        ParticleSystemManager manager { get { return target as ParticleSystemManager; } }
        private UnityEngine.Object button_pool_prefab;
        int size = 0;

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            GUILayout.Label("Add New Particle Pool");
            button_pool_prefab = EditorGUILayout.ObjectField(button_pool_prefab, typeof(GameObject), false);
            serializedObject.ApplyModifiedProperties();
            size = EditorGUILayout.IntField(size);
            if (GUILayout.Button("Add"))
                manager.AddPool(button_pool_prefab as GameObject, size);
        }
    }
}
#endif
