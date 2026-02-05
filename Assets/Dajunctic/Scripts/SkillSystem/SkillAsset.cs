using UnityEngine;
using UnityEngine.Playables;

namespace Dajunctic
{
    public abstract class SkillAsset<TBehaviour, TData>: PlayableAsset, ISkillAsset where TBehaviour: PlayableBehaviour, ISettingsBehaviour<TData>, new()
    {
        public TData Settings;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.Settings = Settings;
            return playable;
        }
    }

    public interface ISkillAsset
    {
        
    }
}