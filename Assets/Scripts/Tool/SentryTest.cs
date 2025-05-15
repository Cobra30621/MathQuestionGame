using System;
using Sentry;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool
{
    public class SentryTest : MonoBehaviour
    {
        // private void Awake()
        // {
        //     SentrySdk.CaptureMessage("Test event");
        // }

        [Button]
        public void TestError()
        {
            Debug.LogError("Test");
        }
    }
}