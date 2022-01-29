using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.Utils.Scripts
{
    public static class UnityEngineExtensions
    {
        public static T[] GetComponentsInFirstLayerChildren<T>(this Transform transform ) where T: Component
        {
            var result = new List<T>(); 
            
            for (var i = 0; i != transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var component =  child.GetComponent<T>();
                
                if(component == null)
                    continue;
                
                result.Add(component);
            }

            return result.ToArray();
        }
        
        public static Component[] GetComponentsInFirstLayerChildren(this Transform transform, Type type)
        {
            var result = new List<Component>(); 
            
            for (var i = 0; i != transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var component =  child.GetComponent(type);
                
                if(component == null)
                    continue;
                
                result.Add(component);
            }

            return result.ToArray();
        }

        public static T[] GetComponentsInFirstLayerChildren<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.transform.GetComponentsInFirstLayerChildren<T>();
        }
        
        public static T[] GetComponentsInFirstLayerChildren<T>(this Component component) where T : Component
        {
            return component.transform.GetComponentsInFirstLayerChildren<T>();
        }
        
        public static Component[] GetComponentsInFirstLayerChildren<T>(this GameObject gameObject, Type type)
        {
            return gameObject.transform.GetComponentsInFirstLayerChildren(type);
        }
        
        public static Component[] GetComponentsInFirstLayerChildren<T>(this Component component, Type type)
        {
            return component.transform.GetComponentsInFirstLayerChildren(type);
        }
    }
}