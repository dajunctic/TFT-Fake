using UnityEngine;

namespace DTT.Utils.Extensions.Demo
{
    public class EnumBehaviour : MonoBehaviour
    {
        /// <summary>
        /// An enum type for testing character casting.
        /// </summary>
        public enum CharacterEnum
        {
            ONE = 'o',
            TWO = 't',
            THREE = 'r'
        }

        [SerializeField]
        private CharacterEnum _character;

        private void Awake()
        {
            // Retrieve the next value. It will loop around if at the last value.
            CharacterEnum next = _character.Next();

            // Retrieve the previous value. It will loop around if at the first value.
            CharacterEnum previous = _character.Previous();
        }
    }

}