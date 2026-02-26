namespace Dajunctic
{
    public interface IAnimatorPlayer: IEntity
    {
        public void PlayAnim(string animName, float transitionDuration = 0.1f);
        public void ResetAnim();
        public bool IsAnimFinished { get; }
    
    }
}