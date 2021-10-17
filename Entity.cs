using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyECA
{
    public class Entity
    {
        public string Name => name;
        public EntityScene Scene => scene;
        
        public Entity(string name, EntityScene scene)
        {
            this.scene = scene;
            this.name = name;
        }

        public T AddComponent<T>() where T : EntityComponent
        {
            if (components.Exists(component => component.GetType() == typeof(T)))
                return null;
            
            T component = Activator.CreateInstance<T>();
            components.Add(component);
            component._parent = this;
            component._scene = scene;
            #if TINYECA_DISABLE_CALL_ONCREATE
            #else
            CallComponentMethod<T>("OnCreate");
            #endif
            return component;
        }

        public T GetComponent<T>() where T : EntityComponent
        {
            return (T)components.Find(component => component.GetType() == typeof(T));
        }
        
        public void RemoveComponent<T>() where T : EntityComponent
        {
            foreach (EntityComponent component in components.Where(component => component.GetType() == typeof(T)))
            {
                #if TINYECA_DISABLE_CALL_ONDESTROY
                #else
                CallComponentMethod<T>("OnDestroy");
                #endif
                components.Remove(component);
                return;
            }
        }

        public void CallComponentMethod<T>(string methodName, object[] parameters) where T : EntityComponent
        {
            T component = (T)components.Find(c => c.GetType() == typeof(T));
            if (component is null) return;
            CallComponentMethod(component, methodName, parameters);
        }
        
        public void CallComponentMethod(string methodName, object[] parameters)
        {
            foreach (EntityComponent component in components)
                CallComponentMethod(methodName, parameters);
        }
        
        internal void CallComponentMethod(EntityComponent component, string methodName, object[] parameters)
        {
            MethodInfo info = component.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            info?.Invoke(component, parameters);
        }
        
        public void CallComponentMethod<T>(string methodName) where T : EntityComponent => CallComponentMethod<T>(methodName, new object[] { });
        public void CallComponentMethod(string methodName) => CallComponentMethod(methodName, new object[] { });
        internal void CallComponentMethod(EntityComponent component, string methodName) => CallComponentMethod(component, methodName, new object[] { });
        
        private List<EntityComponent> components = new List<EntityComponent>();
        
        private EntityScene scene;
        private string name;
    }
}