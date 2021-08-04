using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private KeyChest keychest;

    private void Start()
    {
        if (keychest != null)
        {
            keychest.Subscribe_EventEnded(KeyChest_EventFinished);
        }
    }

    private void KeyChest_EventFinished()
    {
        Destroy(gameObject);
    }
}
