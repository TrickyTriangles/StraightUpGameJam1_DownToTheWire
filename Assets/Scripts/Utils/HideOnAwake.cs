using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnAwake : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    private void Awake()
    {
        if (sprite != null)
        {
            sprite.enabled = false;
        }
    }
}
