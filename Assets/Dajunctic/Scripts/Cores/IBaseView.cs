namespace Dajunctic
{
    public interface IBaseView: ILifeCycle
    {
        public virtual void DoEnable()
        {

        }

        public virtual void DoDisable()
        {

        }

        public virtual void ListenEvents()
        {

        }

        public virtual void StopListenEvents()
        {

        }

        public virtual void Tick()
        {
            
        }
    }
}