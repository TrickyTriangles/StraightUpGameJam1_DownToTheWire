using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonExit : MonoBehaviour, IInteractable
{
    [SerializeField] [Multiline(3)] private List<string> pre_fade_dialogue;
    [SerializeField] [Multiline(3)] private List<string> post_fade_dialogue;
    [SerializeField] private GameManager game_manager;

    private void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = new Color(0f, 0f, 0f, 0f);
        }
    }

    void IInteractable.Interact(PlayerController player, System.Action InteractionEnded)
    {
        StartCoroutine(ExitDungeonRoutine(player));
    }

    private IEnumerator ExitDungeonRoutine(PlayerController player)
    {
        Time.timeScale = 0f;
        player.paused = true;

        BGM.Instance.FadeOut(2f);
        yield return new WaitForSecondsRealtime(2.5f);

        for (int i = 0; i < pre_fade_dialogue.Count; i++)
        {
            DialogueBox db = new DialogueBox(pre_fade_dialogue[i], CommonResources.TextSpeedFast, "TextBlip2");
            yield return db.ProcessDialgoueBox();
        }

        if (game_manager != null)
        {
            yield return game_manager.FadeScreen(new Color(1f, 1f, 1f, 0f), Color.white, 3f);
            yield return new WaitForSecondsRealtime(1f);
        }

        for (int i = 0; i < post_fade_dialogue.Count; i++)
        {
            DialogueBox db = new DialogueBox(post_fade_dialogue[i], CommonResources.TextSpeedFast, "TextBlip2");
            yield return db.ProcessDialgoueBox();
        }

        if (game_manager != null)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            yield return game_manager.FadeScreen(Color.white, Color.black, 4f);
            yield return new WaitForSecondsRealtime(1f);
        }

        DialogueBox dbx = new DialogueBox("Congratulations on finishing the game! Let's hope our escape has no unintended consequences...", CommonResources.TextSpeedFast, "TextBlip2");
        yield return dbx.ProcessDialgoueBox();

        dbx = new DialogueBox("Special thanks to the organizers of the StraightUpGameJam for hosting this event. It's been a blast!", CommonResources.TextSpeedFast, "TextBlip2");
        yield return dbx.ProcessDialgoueBox();

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
