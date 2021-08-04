using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Canvas main_canvas;
    [SerializeField] private Canvas instruction_canvas;
    [SerializeField] private Image screen_fade;
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private AudioClip start_chime;
    private bool on_main_canvas;
    private bool has_exited;
    private bool can_select;

    #region Events and Subscriber Methods

    private Action HideElements;
    public void Subscribe_HideElements(Action sub) { HideElements += sub; }
    public void Unsubscribe_HideElements(Action sub) { HideElements -= sub; }

    private Action FadeInComplete;
    public void Subscribe_FadeInComplete(Action sub) { FadeInComplete += sub; }
    public void Unsubscribe_FadeInComplete(Action sub) { FadeInComplete -= sub; }

    private Action MenuExited;
    public void Subscribe_MenuExited(Action sub) { MenuExited += sub; }
    public void Unsubscribe_MenuExited(Action sub) { MenuExited -= sub; }

    #endregion

    private void Awake()
    {
        if (screen_fade != null)
        {
            screen_fade.color = Color.black;
        }
    }

    private void Start()
    {
        ToggleCanvas();
        StartCoroutine(IntroFadeInRoutine());
    }

    void Update()
    {
        if (!has_exited && can_select)
        {
            if (Input.GetKeyDown(KeyCode.Return) && on_main_canvas)
            {
                if (audio_source != null && start_chime != null)
                {
                    audio_source.PlayOneShot(start_chime);
                }

                has_exited = true;
                StartCoroutine(BeginGameFadeoutRoutine());
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleCanvas();
            }
        }
    }

    private void ToggleCanvas()
    {
        on_main_canvas = !on_main_canvas;

        if (on_main_canvas)
        {
            main_canvas.gameObject.SetActive(true);
            instruction_canvas.gameObject.SetActive(false);

            if (can_select)
            {
                FadeInComplete?.Invoke();
            }
        }
        else
        {
            main_canvas.gameObject.SetActive(false);
            instruction_canvas.gameObject.SetActive(true);
        }
    }

    private IEnumerator IntroFadeInRoutine()
    {
        HideElements?.Invoke();
        screen_fade.CrossFadeAlpha(0f, 2f, true);
        yield return new WaitForSecondsRealtime(2.5f);

        BGM.Instance.PlaySong("78_Ominous", 2f);

        FadeInComplete?.Invoke();
        can_select = true;
    }

    private IEnumerator BeginGameFadeoutRoutine()
    {
        MenuExited?.Invoke();

        BGM.Instance.FadeOut(3f);
        screen_fade.CrossFadeAlpha(1f, 3f, true);
        yield return new WaitForSecondsRealtime(3.5f);

        SceneManager.LoadScene("GameShell");
    }
}
