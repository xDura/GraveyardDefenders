using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace XD
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoCustomInspector : Editor
    {
        public MonoBehaviour mono;
        public Type targetType;
        
        public Dictionary<ParameterInfo, System.Object> parameterCaches = new Dictionary<ParameterInfo, System.Object>();

        private void OnEnable()
        {
            mono = target as MonoBehaviour;
            targetType = mono.GetType();
            FillParameterCaches();
        }

        void FillParameterCaches()
        {
            if (!XDEditor.HasAnyCustomButton(mono.GetType())) return;
            List<CustomButtonData> buttonMethods = XDEditor.buttonsForClass[targetType];
            foreach (CustomButtonData buttonData in buttonMethods)
            {
                ParameterInfo[] args = buttonData.targetMethod.GetParameters();
                foreach (ParameterInfo arg in args)
                {
                    if (!parameterCaches.ContainsKey(arg)) { parameterCaches.Add(arg, CreateVarByType(arg)); }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawCustomButtons();
        }

        public void DrawCustomButtons()
        {
            if (!XDEditor.HasAnyCustomButton(mono.GetType())) return;

            List<CustomButtonData> buttonMethods = XDEditor.buttonsForClass[targetType];
            foreach (CustomButtonData buttonData in buttonMethods)
            {
                DrawButton(buttonData);
                GUILayout.Space(5);
            }
        }

        public void DrawButton(CustomButtonData data)
        {
            ParameterInfo[] args = data.targetMethod.GetParameters();
            int buttonHeight = data.attribute.Height();
            if (args.Length == 0)
            {
                if(GUILayout.Button(data.targetMethod.Name, GUILayout.Height(buttonHeight)))
                    data.targetMethod.Invoke(mono, null);
            }
            else
            {
                GUILayout.BeginVertical("Button");
                GUILayout.Space(2);
                if (GUILayout.Button(data.targetMethod.Name, GUILayout.Height(buttonHeight)))
                {
                    object[] realArgs = new object[args.Length];
                    for (int i = 0; i < args.Length; i++)
                        realArgs[i] = parameterCaches[args[i]];
                    data.targetMethod.Invoke(mono, realArgs);
                }
                GUILayout.Space(2);
                foreach (ParameterInfo arg in args)
                {
                    if (!parameterCaches.ContainsKey(arg)) continue;
                    parameterCaches[arg] = DrawPropByType(arg.Name, parameterCaches[arg], arg.ParameterType);
                }
                GUILayout.Space(2);
                GUILayout.EndVertical();
            }
        }


        public System.Object DrawPropByType(string name, System.Object obj, Type type)
        {
            if (type == typeof(Vector3)) return  EditorGUILayout.Vector3Field(name, (Vector3)obj);
            else if (type == typeof(Vector3Int)) return EditorGUILayout.Vector3IntField(name, (Vector3Int)obj);
            else if (type == typeof(Vector4)) return EditorGUILayout.Vector4Field(name, (Vector4)obj);
            else if (type == typeof(Vector2)) return EditorGUILayout.Vector2Field(name, (Vector2)obj);
            else if (type == typeof(Vector2Int)) return EditorGUILayout.Vector2IntField(name, (Vector2Int)obj);
            else if (type == typeof(bool)) return EditorGUILayout.Toggle(name, (bool)obj); 
            else if (type == typeof(int)) return EditorGUILayout.IntField(name, (int)obj);
            else if (type == typeof(float)) return EditorGUILayout.FloatField(name, (float)obj);
            else if (type == typeof(double)) return EditorGUILayout.DoubleField(name, (double)obj);
            else if (type == typeof(Color)) return EditorGUILayout.ColorField(name, (Color)obj);
            else if (type.IsEnum && type.IsDefined(typeof(FlagsAttribute))) return EditorGUILayout.EnumFlagsField(name, (Enum)obj);
            else if (type.IsEnum) return EditorGUILayout.EnumPopup(name, (Enum)obj);
            else if (type == typeof(string)) return EditorGUILayout.TextField(name, (string)obj);
            else
                return EditorGUILayout.ObjectField(name, (UnityEngine.Object)obj, type, true);
        }

        public System.Object CreateVarByType(ParameterInfo info)
        {
            Type type = info.ParameterType;
            if (type == typeof(Vector3)) return info.HasDefaultValue ? info.DefaultValue : (Vector3) default;
            else if (type == typeof(Vector3Int)) return info.HasDefaultValue ? info.DefaultValue : (Vector3Int)default;
            else if (type == typeof(Vector4)) return info.HasDefaultValue ? info.DefaultValue : (Vector4) default;
            else if (type == typeof(Vector2)) return info.HasDefaultValue ? info.DefaultValue : (Vector2)default;
            else if (type == typeof(Vector2Int)) return info.HasDefaultValue ? info.DefaultValue : (Vector2Int)default;
            else if (type == typeof(bool)) return info.HasDefaultValue ? info.DefaultValue : (bool)default;
            else if (type == typeof(int)) return info.HasDefaultValue ? info.DefaultValue : (int)default;
            else if (type == typeof(float)) return info.HasDefaultValue ? info.DefaultValue : (float)default;
            else if (type == typeof(double)) return info.HasDefaultValue ? info.DefaultValue : (double)default;
            else if (type == typeof(Color)) return info.HasDefaultValue ? info.DefaultValue : (Color)default;
            else if (type.IsEnum && type.IsDefined(typeof(FlagsAttribute))) return info.HasDefaultValue ? info.DefaultValue : default;
            else if (type.IsEnum) return info.HasDefaultValue ? info.DefaultValue : default;
            else if (type == typeof(string)) return info.HasDefaultValue ? info.DefaultValue : default;
            else
                return null;
        }
    }   
}
