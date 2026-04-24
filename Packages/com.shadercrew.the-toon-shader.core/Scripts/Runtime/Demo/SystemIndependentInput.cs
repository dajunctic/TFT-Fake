using UnityEngine;

#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif
namespace ShaderCrew.TheToonShader
{
    public static class SystemIndependentInput
    {
        public static bool GetKeyDown(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM
            Keyboard kb = Keyboard.current;
            if (kb == null) return false;


            Key newKey;
            if (TryConvertKeyCode(key, out newKey))
                return kb[newKey].wasPressedThisFrame;

            return false;
#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(key);
#else
        return false;
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private static bool TryConvertKeyCode(KeyCode oldKey, out Key newKey)
        {
            switch (oldKey)
            {
                case KeyCode.D: newKey = Key.D; return true;
                case KeyCode.A: newKey = Key.A; return true;
                case KeyCode.W: newKey = Key.W; return true;
                case KeyCode.S: newKey = Key.S; return true;

                case KeyCode.Space: newKey = Key.Space; return true;
                case KeyCode.Escape: newKey = Key.Escape; return true;
                case KeyCode.UpArrow: newKey = Key.UpArrow; return true;
                case KeyCode.DownArrow: newKey = Key.DownArrow; return true;
                case KeyCode.LeftArrow: newKey = Key.LeftArrow; return true;
                case KeyCode.RightArrow: newKey = Key.RightArrow; return true;
                default:
                    newKey = Key.None;
                    return false;
            }
        }
#endif
    }
}