using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : Singleton<BGM>
{
    // Player objects
    [SerializeField] private AudioSource player;
    [SerializeField] private List<AudioClip> music_prefabs;
    public Dictionary<string, AudioClip> bgm;

    // Key state variables and settings
    private BGMPlayerState state;
    private BGMTrackChangeState tc_state;
    private BGMTrackPitchState tp_state;
    private float bgm_volume;

    // Fade in & fade out controls
    private string next_track;
    private float next_track_fadein_time;
    private float fade_timer;
    private float time_to_fade;

    // Pitch control
    private float pitch_fade_timer;
    private float pitch_time_to_fade;
    private float vibrato_speed;
    private float vibrato_depth;

    private void Start()
    {
        bgm = new Dictionary<string, AudioClip>();
        bgm_volume = player.volume;

        state = BGMPlayerState.DEFAULT;
        tc_state = BGMTrackChangeState.NO_CHANGE;
        tp_state = BGMTrackPitchState.DEFAULT;

        if (music_prefabs != null)
        {
            foreach (var item in music_prefabs)
            {
                if (!bgm.ContainsKey(item.name))
                    bgm.Add(item.name, item);
                else
                    Debug.Log("BGM object with name " + item.name + " already exists in bgm database.");
            }
        }
    }

    void Update()
    {
        UpdateFade();
        UpdateVolume();
        UpdatePitch();
    }

    /// <summary>
    /// Handles logic relating to fade-in and fade-out transitions.
    /// </summary>
    void UpdateFade()
    {
        if (fade_timer < time_to_fade)
        {
            fade_timer += Time.unscaledDeltaTime;
        }
        else
        {
            if (tc_state == BGMTrackChangeState.TRACK_CHANGE)
            {
                PlaySong(next_track);
                state = BGMPlayerState.DEFAULT;
            }
            else if (tc_state == BGMTrackChangeState.TRACK_CHANGE_WITH_FADE)
            {
                PlaySong(next_track, next_track_fadein_time);
            }
            else if (tc_state == BGMTrackChangeState.FADE_TO_PAUSE)
            {
                PauseSong();
            }

            tc_state = BGMTrackChangeState.NO_CHANGE;
        }
    }

    /// <summary>
    /// Handles adjustment of the player volume during fade-in and fade-out transitions.
    /// </summary>
    void UpdateVolume()
    {
        float ratio = Mathf.Clamp(fade_timer / time_to_fade, 0f, 1f);

        switch (state)
        {
            case BGMPlayerState.DEFAULT:
                player.volume = bgm_volume;
                break;
            case BGMPlayerState.FADE_OUT:
                player.volume = bgm_volume - (bgm_volume * ratio);
                break;
            case BGMPlayerState.FADE_IN:
                player.volume = 0f + (bgm_volume * ratio);
                break;
            default:
                player.volume = bgm_volume;
                break;
        }
    }

    /// <summary>
    /// Handles logic relating to pitch-shifting effects.
    /// </summary>
    void UpdatePitch()
    {
        if (tp_state == BGMTrackPitchState.PITCH_FADE_OUT || tp_state == BGMTrackPitchState.PITCH_FADE_IN)
        {
            if ((pitch_fade_timer += Time.unscaledDeltaTime) >= pitch_time_to_fade)
            {
                if (tp_state == BGMTrackPitchState.PITCH_FADE_OUT)
                    player.Pause();

                tp_state = BGMTrackPitchState.DEFAULT;
                player.pitch = 1f;
            }
        }

        float ratio = Mathf.Clamp01(pitch_fade_timer / pitch_time_to_fade);

        switch (tp_state)
        {
            case BGMTrackPitchState.DEFAULT:
                player.pitch = 1f;
                break;
            case BGMTrackPitchState.PITCH_FADE_OUT:
                player.pitch = 1f - ratio;
                break;
            case BGMTrackPitchState.PITCH_FADE_IN:
                player.pitch = ratio;
                break;
            case BGMTrackPitchState.VIBRATO:
                player.pitch = 1f + (Mathf.Sin(Time.timeSinceLevelLoad * vibrato_speed) * (vibrato_depth * 0.01f));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets the baseline audio level. Will mostly be used by an options menu.
    /// </summary>
    /// <param name="volume">The new player volume on a scale from 0 (muted) to 1 (full volume).</param>
    public void SetVolume(float volume)
    {
        player.volume = Mathf.Clamp01(volume);
        bgm_volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Adjusts the vibrato settings.
    /// </summary>
    /// <param name="m_speed">Speed of the vibrato.</param>
    /// <param name="m_depth">Vibrato depth (in 100ths of a pitch unit)</param>
    public void SetVibrato(float m_speed, float m_depth)
    {
        tp_state = BGMTrackPitchState.VIBRATO;
        vibrato_speed = m_speed;
        vibrato_depth = m_depth;
    }

    /// <summary>
    /// Adjusts the speed of the vibrato.
    /// </summary>
    /// <param name="m_speed">New speed.</param>
    public void SetVibratoSpeed(float m_speed)
    {
        vibrato_speed = m_speed;
    }

    /// <summary>
    /// Adjusts the depth of the vibrato.
    /// </summary>
    /// <param name="m_depth">Vibrato depth (in 100ths of a pitch unit)</param>
    public void SetVibratoDepth(float m_depth)
    {
        vibrato_depth = m_depth;
    }

    /// <summary>
    /// Resumes playing the current song in the audio source.
    /// </summary>
    public void PlaySong()
    {
        player.UnPause();
        state = BGMPlayerState.DEFAULT;
    }

    /// <summary>
    /// Change the currently playing track.
    /// </summary>
    /// <param name="name">Name of the new song.</param>
    public void PlaySong(string name)
    {
        if (!bgm.ContainsKey(name))
        {
            Debug.Log("Song " + name + " could not be played.");
            return;
        }

        AudioClip ac = bgm[name];

        if (player.isPlaying)
            player.Stop();

        player.clip = ac;
        player.Play();

        state = BGMPlayerState.DEFAULT;
        tc_state = BGMTrackChangeState.NO_CHANGE;
    }

    /// <summary>
    /// Change the currently playing track, and fade it in.
    /// </summary>
    /// <param name="name">Name of the new song.</param>
    /// <param name="m_fadein">Number of seconds to fade in the track.</param>
    public void PlaySong(string name, float m_fadein)
    {
        if (!bgm.ContainsKey(name))
        {
            Debug.Log("Song " + name + " could not be played.");
            return;
        }

        AudioClip ac = bgm[name];

        if (player.isPlaying)
            player.Stop();

        player.clip = ac;
        player.Play();

        fade_timer = 0f;
        time_to_fade = m_fadein;

        state = BGMPlayerState.FADE_IN;
        tc_state = BGMTrackChangeState.NO_CHANGE;
    }

    /// <summary>
    /// Pauses the currently playing song.
    /// </summary>
    public void PauseSong()
    {
        if (player.isPlaying)
            player.Pause();
    }

    /// <summary>
    /// If the current song is paused, it will resume playing.
    /// </summary>
    public void UnpauseSong()
    {
        player.UnPause();
    }

    /// <summary>
    /// Stops the currently playing song.
    /// </summary>
    public void Stop()
    {
        player.Stop();
    }

    /// <summary>
    /// Stops the currently playing song but with a pitch-down effect.
    /// </summary>
    /// <param name="pd_time">Duration of the fadeout effect, in seconds.</param>
    public void Stop(float pd_time)
    {
        pitch_fade_timer = 0f;
        pitch_time_to_fade = pd_time;
        tp_state = BGMTrackPitchState.PITCH_FADE_OUT;
    }

    /// <summary>
    /// Fades out the currently playing song.
    /// </summary>
    /// <param name="seconds">Number of seconds to fade out the current song.</param>
    public void FadeOut(float seconds)
    {
        state = BGMPlayerState.FADE_OUT;
        tc_state = BGMTrackChangeState.FADE_TO_PAUSE;
        time_to_fade = (seconds <= 0.1f) ? 0.1f : seconds;
        fade_timer = 0f;
    }

    /// <summary>
    /// Fades in the currently playing song.
    /// </summary>
    /// <param name="seconds">Number of seconds to fade in the current song.</param>
    public void FadeIn(float seconds)
    {
        player.UnPause();

        state = BGMPlayerState.FADE_IN;
        time_to_fade = (seconds <= 0.1f) ? 0.1f : seconds;
        fade_timer = 0f;
    }

    /// <summary>
    /// Fade out to a new song with no fade-in.
    /// </summary>
    /// <param name="name">Name of the new song.</param>
    /// <param name="seconds">Number of seconds to fade out current song.</param>
    public void FadeToSong(string name, float seconds)
    {
        state = BGMPlayerState.FADE_OUT;
        tc_state = BGMTrackChangeState.TRACK_CHANGE;

        next_track = name;
        fade_timer = 0f;
        time_to_fade = seconds;
    }

    /// <summary>
    /// Fade out to a new song and fade-in the new track.
    /// </summary>
    /// <param name="name">Name of the new song.</param>
    /// <param name="seconds">Number of seconds to fade out current song.</param>
    /// <param name="fade_time">Number of seconds to fade in next song.</param>
    public void FadeToSong(string name, float seconds, float fade_time)
    {
        state = BGMPlayerState.FADE_OUT;
        tc_state = BGMTrackChangeState.TRACK_CHANGE_WITH_FADE;

        next_track = name;
        next_track_fadein_time = fade_time;

        fade_timer = 0f;
        time_to_fade = seconds;
    }
}

public enum BGMPlayerState
{
    DEFAULT,
    FADE_OUT,
    FADE_IN,
}

public enum BGMTrackChangeState
{
    NO_CHANGE,
    FADE_TO_PAUSE,
    TRACK_CHANGE,
    TRACK_CHANGE_WITH_FADE
}

public enum BGMTrackPitchState
{
    DEFAULT,
    PITCH_FADE_OUT,
    PITCH_FADE_IN,
    VIBRATO
}
