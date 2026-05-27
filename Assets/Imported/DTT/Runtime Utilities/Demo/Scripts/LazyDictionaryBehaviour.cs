using UnityEngine;

namespace DTT.Utils.Optimization.Demo
{
   internal class LazyDictionaryBehaviour : MonoBehaviour
   {
      /// <summary>
      /// This dictionary holds, for each tag, a reference to all the game objects with that tag. The operation
      /// to retrieve the game objects will only be done the first time the value is used.
      /// </summary>
      private readonly LazyDictionary<string, GameObject[]>
         _lazyDictionary = new LazyDictionary<string, GameObject[]>();

      /// <summary>
      /// The tag of enemies.
      /// </summary>
      private const string ENEMIES_TAG = "Enemies";

      /// <summary>
      /// The tag of friends.
      /// </summary>
      private const string FRIENDS_TAG = "Friends";

      /// <summary>
      /// Initializes the lazy dictionary with the operations to retrieve the tagged game objects.
      /// </summary>
      private void Awake()
      {
         _lazyDictionary.Add(ENEMIES_TAG, () => GameObject.FindGameObjectsWithTag(ENEMIES_TAG));
         _lazyDictionary.Add(FRIENDS_TAG, () => GameObject.FindGameObjectsWithTag(FRIENDS_TAG));
      }
   }
}