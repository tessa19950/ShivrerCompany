using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ShivrerCompany.util
{
    internal class TaskUtil
    {
        public static void RunTaskLater(MonoBehaviour taskOwner, UnityAction action, float delay)
        {
            taskOwner.StartCoroutine(RunTaskLater(action, delay));
        }

        private static IEnumerator RunTaskLater(UnityAction action, float delay)
        {
            float t = delay;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            action.Invoke();
        }
    }
}
