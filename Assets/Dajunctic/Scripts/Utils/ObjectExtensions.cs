using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Dajunctic.Utils
{
    public static class ObjectExtension
    {
#if UNITY_EDITOR
        public static bool IsSceneObject(this Object obj)
        {
            if (obj == null)
                return false;

            var isSceneType = obj is GameObject || obj is Component;
            if (!isSceneType)
            {
                return false;
            }

            return !PrefabUtility.IsPartOfPrefabAsset(obj);
        }

        public static bool IsPrefab(this Object obj)
        {
            if (obj == null)
                return false;

            return PrefabUtility.IsPartOfPrefabAsset(obj);
        }
#endif

        public static Coroutine DelayFrame(
            this MonoBehaviour monoBehaviour,
            int frame,
            Action onCompleted
        )
        {
            return monoBehaviour.StartCoroutine(IEDelayFrame(frame, onCompleted));
        }

        static IEnumerator IEDelayFrame(int frame, Action onCompleted)
        {
            for (var i = 0; i < frame; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            onCompleted?.Invoke();
        }

        public static Coroutine DelaySecond(
            this MonoBehaviour monoBehaviour,
            float second,
            bool realTime,
            Action onCompleted
        )
        {
            return monoBehaviour.StartCoroutine(IEDelaySecond(second, realTime, onCompleted));
        }

        static IEnumerator IEDelaySecond(float second, bool realTime, Action onCompleted)
        {
            if (realTime)
            {
                yield return new WaitForSecondsRealtime(second);
            }
            else
            {
                yield return new WaitForSeconds(second);
            }
            onCompleted?.Invoke();
        }

        public static Coroutine WaitForEndOfFrame(
            this MonoBehaviour monoBehaviour,
            Action onCompleted
        )
        {
            return monoBehaviour.StartCoroutine(IEWaitForEndOfFrame(onCompleted));
        }

        static IEnumerator IEWaitForEndOfFrame(Action onCompleted)
        {
            yield return new WaitForEndOfFrame();
            onCompleted?.Invoke();
        }
    }
}
