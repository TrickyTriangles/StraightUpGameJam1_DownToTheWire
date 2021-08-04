using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private Canvas ui_canvas;
    [SerializeField] private ScreenFadeEffect fade;
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Room> rooms;
    [SerializeField] private Room active_room;
    private Vector2 room_dimensions = new Vector2(16f, 10f);

    private Coroutine active_transition;
    private float room_transition_time = 2f;

    [SerializeField] private bool skip_opening_cutscene;

    #region Events and Subscriber Methods

    private Action<Direction, float> RoomTransitionBeginCallback;
    public void Subscribe_RoomTransitionBeginCallback(Action<Direction, float> sub) { RoomTransitionBeginCallback += sub; }
    public void Unsubscribe_RoomTransitionBeginCallback(Action<Direction, float> sub) { RoomTransitionBeginCallback -= sub; }

    private Action RoomTransitionEndCallback;
    public void Subscribe_RoomTransitionEndCallback(Action sub) { RoomTransitionEndCallback += sub; }
    public void Unsubscribe_RoomTransitionEndCallback(Action sub) { RoomTransitionEndCallback -= sub; }

    #endregion

    #region Properties

    public AudioSource Audio_Source
    {
        get { return audio_source; }
    }

    public PlayerController Player
    {
        get { return player; }
    }

    #endregion

    public void Awake()
    {
        CommonResources.Initialize(this);
        InitialRoomSetup();

        if (player != null)
        {
            player.Subscribe_OnDeath(PlayerController_OnDeath);
        }

        if (fade != null)
        {
            fade.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        if (skip_opening_cutscene)
        {
            StartCoroutine(fade.FadeRoutine(Color.black, new Color(0f, 0f, 0f, 0f), 0.01f));
        }
        else
        {
            StartCoroutine(IntroSequence());
        }
    }

    private void InitialRoomSetup()
    {
        if (active_room != null && rooms != null)
        {
            foreach (var room in rooms)
            {
                if (room != active_room)
                {
                    room.gameObject.SetActive(false);
                }
            }
        }
    }

    private void PlayerController_OnDeath()
    {
        StartCoroutine(GameOverRoutine());
    }

    private void InitializeRoom(Room target_room)
    {
        target_room.gameObject.SetActive(true);
        target_room.ResetRoomObjects();
    }

    private void DeactivateRoom(Room target_room)
    {
        target_room.gameObject.SetActive(false);
    }

    public void MoveToRoom(Room new_room, Direction direction)
    {
        if (active_transition == null)
        {
            active_transition = StartCoroutine(RoomTransitionRoutine(new_room, direction));
        }
    }

    // Move 16 units in the x direction for standard sized rooms
    // Move 10 units in the y direction for standard sized rooms
    private IEnumerator RoomTransitionRoutine(Room target_room, Direction direction)
    {
        RoomTransitionBeginCallback?.Invoke(direction, room_transition_time);

        Time.timeScale = 0f;
        float timer = 0f;
        Vector3 camera_start_pos = _camera.transform.position;
        Vector3 camera_target = new Vector3(target_room.transform.position.x, target_room.transform.position.y, camera_start_pos.z);

        InitializeRoom(target_room);

        while ((timer += Time.unscaledDeltaTime) < room_transition_time)
        {
            float lerp_index = MathUtils.SmootherStep(0f, room_transition_time, timer);
            _camera.transform.position = Vector3.Lerp(camera_start_pos, camera_target, lerp_index);

            yield return null;
        }

        active_transition = null;
        DeactivateRoom(active_room);
        active_room = target_room;

        Time.timeScale = 1f;
        RoomTransitionEndCallback?.Invoke();
    }

    public IEnumerator FadeScreen(Color start_color, Color end_color, float duration)
    {
        if (fade != null)
        {
            yield return fade.FadeRoutine(start_color, end_color, duration);
        }
    }

    private IEnumerator IntroSequence()
    {
        Time.timeScale = 0f;
        player.paused = true;

        if (fade != null)
        {
            yield return fade.FadeRoutine(Color.black, new Color(0f, 0f, 0f, 0f), 3f);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        DialogueBox db = new DialogueBox("Ugh...", CommonResources.TextSpeedSlow, "TextBlip2");
        BGM.Instance.SetVolume(0.6f);
        BGM.Instance.PlaySong("78_Ominous", 2f);

        yield return db.ProcessDialgoueBox();

        db = new DialogueBox("Looks like that fall could've been a whole lot worse.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        db = new DialogueBox("I'm still pretty injured though. Should take a potion to fix myself up.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        db = new DialogueBox("I need to be careful though. Not many of these left. And who knows where the exit is?", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        yield return new WaitForSecondsRealtime(1f);

        db = new DialogueBox("Let's try to keep the mistakes to a minimum from here on out.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        Time.timeScale = 1f;
        player.paused = false;
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);

        yield return fade.FadeRoutine(new Color(0f, 0f, 0f, 0f), Color.black, 2f);
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 0f;

        DialogueBox db = new DialogueBox("Looks like this is the end of the line.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        db = new DialogueBox("Maybe it was bad luck, maybe I took too many risks.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        db = new DialogueBox("But next time I'll get out of here for sure.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        db = new DialogueBox("Next time, for sure...", CommonResources.TextSpeedSlow, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
