using System;
using System.Collections.Generic;

namespace TinyECA
{
    public class EntityScene
    {
        public Entity CreateEntity(string name = "New Entity")
        {
            Entity entity = new Entity(name, this);
            entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity) => entities.Remove(entity);

        public Entity[] FindEntitiesWithName(string name) => entities.FindAll(entity => entity.Name == name).ToArray();

        public void CallComponentMethod(string methodName, object[] parameters)
        {
            foreach (Entity entity in entities)
                entity.CallComponentMethod(methodName, parameters);
        }
        
        public void CallComponentMethod<T>(string methodName, object[] parameters) where T : EntityComponent
        {
            foreach (Entity entity in entities)
                entity.CallComponentMethod<T>(methodName, parameters);
        }

        public void CallComponentMethod(string methodName) => CallComponentMethod(methodName, new object[] { });
        public void CallComponentMethod<T>(string methodName) where T : EntityComponent => CallComponentMethod<T>(methodName, new object[] { });

        private List<Entity> entities = new List<Entity>();
    }
}