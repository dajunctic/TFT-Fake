namespace KBCore.Refs
{
    public abstract class SceneRefFilter
    {
        internal abstract bool IncludeSceneRef(object obj);
    }

    public abstract class SceneRefFilter<T> : SceneRefFilter
        where T : class
    {

        internal override bool IncludeSceneRef(object obj) 
            => this.IncludeSceneRef((T) obj);

        public abstract bool IncludeSceneRef(T obj);
    }
}
