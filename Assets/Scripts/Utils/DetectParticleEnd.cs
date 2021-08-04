using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectParticleEnd : MonoBehaviour
{
    private Action callback;
    public void Subscribe_Callback(Action sub) { callback += sub; }
    public void Unsubsrive_Callback(Action sub) { callback -= sub; }

    public void OnParticleSystemStopped()
    {
        callback?.Invoke();
    }
}
