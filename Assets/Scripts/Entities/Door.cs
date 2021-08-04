using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] [Multiline(3)] private List<string> dialogue;
    private bool is_open;

    void IInteractable.Interact(PlayerController player, System.Action InteractionEndedCallback)
    {
        if (!is_open && player.HasKey)
        {
            is_open = true;
            StartCoroutine(DoorOpenRoutine(player, InteractionEndedCallback));
        }
        else
        {
            InteractionEndedCallback();
        }
    }

    private IEnumerator DoorOpenRoutine(PlayerController player, System.Action InteractionEndedCallback)
    {
        Time.timeScale = 0f;
        player.paused = true;

        for (int i = 0; i < dialogue.Count; i++)
        {
            DialogueBox db = new DialogueBox(dialogue[i], CommonResources.TextSpeedFast, "TextBlip2");
            yield return db.ProcessDialgoueBox();
        }

        player.UseKey();
        CommonResources.PlaySound("DoorOpen", 0.6f);
        InteractionEndedCallback();

        Time.timeScale = 1f;
        player.paused = false;
        Destroy(gameObject);
    }
}
