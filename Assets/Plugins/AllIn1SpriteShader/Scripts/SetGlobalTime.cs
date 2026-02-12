using UnityEngine;

namespace AllIn1SpriteShader
{
    //This script is made with Unity version 2019 and onward in mind
    //This script will pass in the Unscaled Time to the shader to animate the effects even when the game is paused
    //Set shaders to Scaled Time variant and add this script to an active GameObject to see the results
    //Video tutorial about it: https://youtu.be/7_BggIufV-w
    [ExecuteInEditMode]
    public class SetGlobalTime : MonoBehaviour
    {
        int globalTime;

        private void Start()
        {
            globalTime = Shader.PropertyToID("globalUnscaledTime");
        }

        private void Update()
        {
            Shader.SetGlobalFloat(globalTime, Time.unscaledTime / 20f);
        }
    }
}