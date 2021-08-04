using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeEffect : MonoBehaviour
{
    [SerializeField] private Image fade_object;
    private Coroutine fade_routine;
    public Color FadeObjectColor
    {
        get { return fade_object.color; }
    }

    public void StartFadeIn(float duration)
    {
        if (fade_routine != null)
        {
            StopCoroutine(fade_routine);
        }

        fade_routine = StartCoroutine(FadeRoutine(FadeObjectColor, new Color(FadeObjectColor.r, FadeObjectColor.g, FadeObjectColor.b, 0f), duration));
    }

    public void StartFadeOut(Color target_color, float duration)
    {
        if (fade_routine != null)
        {
            StopCoroutine(fade_routine);
        }

        fade_routine = StartCoroutine(FadeRoutine(FadeObjectColor, target_color, duration));
    }

    public void FadeInTransition(Color start_color, float duration)
    {
        fade_object.color = start_color;
        if (fade_routine != null)
        {
            StopCoroutine(fade_routine);
        }

        fade_routine = StartCoroutine(FadeRoutine(start_color, new Color(start_color.r, start_color.g, start_color.b, 0f), duration));
    }

    public IEnumerator FadeRoutine(Color start_color, Color target_color, float duration)
    {
        if (fade_object != null)
        {
            float timer = 0f;
            duration = duration <= 0f ? 1f : duration;

            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                float ratio = timer / duration;

                fade_object.color = Color.Lerp(start_color, target_color, ratio);

                yield return null;
            }
        }
    }
}
