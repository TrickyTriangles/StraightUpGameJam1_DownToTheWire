using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlowingText : MonoBehaviour
{
    [SerializeField] private TitleScreen title_screen;
    [SerializeField] private TextMeshProUGUI front;
    private Color front_color;

    [SerializeField] private TextMeshProUGUI back;
    private Color back_color;

    [SerializeField] private float glow_time = 1.5f;

    private void TitleScreen_HideElements()
    {
        front_color.a = 0f;
        front.color = front_color;
        back_color.a = 0f;
        back.color = back_color;
    }

    private void TitleScreen_FadeInComplete()
    {
        StartCoroutine(GlowRoutine());
    }

    private void TitleScreen_MenuExited()
    {
        glow_time *= 0.1f;
    }

    private void Awake()
    {
        if (title_screen != null)
        {
            title_screen.Subscribe_HideElements(TitleScreen_HideElements);
            title_screen.Subscribe_FadeInComplete(TitleScreen_FadeInComplete);
            title_screen.Subscribe_MenuExited(TitleScreen_MenuExited);
        }

        if (front != null)
        {
            front_color = front.color;
        }

        if (back != null)
        {
            back_color = back.color;
        }
    }

    private IEnumerator GlowRoutine()
    {
        float timer = 0f;

        while (true)
        {
            if (glow_time > 0f)
            {
                timer += Time.unscaledDeltaTime;
                timer %= glow_time;
            }
            
            front_color.a = (glow_time > 0f) ? Mathf.Lerp(0f, 1f, timer / glow_time) : 1f;
            back_color.a = (glow_time > 0f) ? Mathf.Lerp(0f, 1f, timer / glow_time) : 1f;

            front.color = front_color;
            back.color = back_color;

            yield return null;
        }
    }
}
