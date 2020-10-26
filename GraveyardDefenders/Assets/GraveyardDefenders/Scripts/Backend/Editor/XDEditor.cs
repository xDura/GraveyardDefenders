using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace XD
{
    public class CustomButtonData
    {
        public MethodInfo targetMethod;
        public ButtonAttribute attribute;

    }

    public class XDEditor
    {
        public static Dictionary<Type, List<CustomButtonData>> buttonsForClass = new Dictionary<Type, List<CustomButtonData>>();
        public static bool HasAnyCustomButton(Type type) => buttonsForClass.ContainsKey(type);
        //returns a list of the base types that have buttons on them - needed for inheritance to work

        [DidReloadScripts]
        private static void OnDidReloadScripts()
        {
            UpdateButtonsAttributesCacheData();
        }

        static void UpdateButtonsAttributesCacheData()
        {
            buttonsForClass.Clear();
            var allButtons = TypeCache.GetMethodsWithAttribute(typeof(ButtonAttribute));
            foreach (MethodInfo methodInfo in allButtons)
            {
                Type targetClassType = methodInfo.DeclaringType;
                CustomButtonData data = new CustomButtonData()
                {
                    targetMethod = methodInfo,
                    attribute = methodInfo.GetCustomAttribute<ButtonAttribute>(),
                };
                AddButtonForClass(targetClassType, data);
                foreach (Type type in TypeCache.GetTypesDerivedFrom(targetClassType)) //add the buttons also for the children
                    AddButtonForClass(type, data);
            }
        }

        static void AddButtonForClass(Type classType, CustomButtonData data)
        {
            if (HasAnyCustomButton(classType))
                buttonsForClass[classType].Add(data);
            else
                buttonsForClass[classType] = new List<CustomButtonData>() { data };
        }
    }   
}
