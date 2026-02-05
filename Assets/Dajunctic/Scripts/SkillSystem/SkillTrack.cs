using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Dajunctic
{
    [TrackColor(0f, 1f, 1f)]
    [TrackBindingType(typeof(GameObject))]
    [TrackClipType(typeof(ISkillAsset))]
    public class SkillTrack: TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SkillMixerBehaviour>.Create(graph, inputCount);
        }
    }
}