namespace TinyECA
{
    public class EntityComponent
    {
        internal EntityScene _scene = null;
        protected EntityScene Scene => _scene;

        internal Entity _parent = null;
        protected Entity Parent => _parent;

        protected T AddComponent<T>() where T : EntityComponent => Parent.AddComponent<T>();
        protected T GetComponent<T>() where T : EntityComponent => Parent.GetComponent<T>();
        protected void RemoveComponent<T>() where T : EntityComponent => Parent.RemoveComponent<T>();
    }
}