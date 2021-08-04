using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour, IInteractable
{
    [SerializeField] [Multiline(3)] private List<string> dialogue_boxes;

    void IInteractable.Interact(PlayerController player, Action EventEnded)
    {
        StartCoroutine(ReadSign(EventEnded));
    }

    private IEnumerator ReadSign(Action EventEnded)
    {
        Time.timeScale = 0f;

        for (int i = 0; i < dialogue_boxes.Count; i++)
        {
            DialogueBox db = new DialogueBox(dialogue_boxes[i], CommonResources.TextSpeedFast, "TextBlip2");
            yield return db.ProcessDialgoueBox();
        }

        Time.timeScale = 1f;
        EventEnded();
    }
}
