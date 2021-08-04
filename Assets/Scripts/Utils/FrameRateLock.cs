using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLock : MonoBehaviour
{
    [SerializeField] private int frames_per_second;

    void Awake()
    {
        if (frames_per_second > 1)
        {
            Application.targetFrameRate = frames_per_second;
        }
    }

    void Update()
    {
        if (Application.targetFrameRate != frames_per_second)
        {
            Application.targetFrameRate = frames_per_second;
        }
    }
}
