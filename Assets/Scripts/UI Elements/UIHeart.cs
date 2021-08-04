using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Image panel;
    public Image image;

    public Animator Animator
    {
        get { return animator; }
    }

    public void RestoreHeart(float animation_start_time)
    {
        if (animator != null)
        {
            animator.Play("Idle", 0, animation_start_time);
        }
    }

    public void LoseHeart(float animation_start_time)
    {
        if (animator != null)
        {
            animator.Play("Hit", 0, animation_start_time);
        }
    }

    public void SetWarning()
    {
        if (animator != null)
        {
            animator.Play("Warning", 0, 0f);
        }
    }
}
