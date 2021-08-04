using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int number_of_keys;
    [SerializeField] [Multiline(3)] private List<string> dialogue;
    private bool is_empty;

    #region Events and Subscriber Methods

    private Action EventEnded;
    public void Subscribe_EventEnded(Action sub) { EventEnded += sub; }
    public void Unubscribe_EventEnded(Action sub) { EventEnded -= sub; }

    #endregion

    void IInteractable.Interact(PlayerController player, System.Action InteractionEnded)
    {
        if (!is_empty)
        {
            StartCoroutine(KeyChestRoutine(player, InteractionEnded));
        }
    }

    private void OnEnable()
    {
        if (is_empty)
        {
            if (animator != null)
            {
                animator.Play("Open", 0, 0f);
            }
        }
    }

    private IEnumerator KeyChestRoutine(PlayerController player, System.Action InteractionEnded)
    {
        Time.timeScale = 0f;

        CommonResources.PlaySound("ChestOpen", 0.6f);
        if (animator != null)
        {
            animator.Play("Open", 0, 0f);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 0; i < dialogue.Count; i++)
        {
            DialogueBox db = new DialogueBox(dialogue[i], CommonResources.TextSpeedFast, "TextBlip2");
            yield return db.ProcessDialgoueBox();
        }

        new GiveKeys(player, number_of_keys);
        CommonResources.PlaySound("KeyPickup", 0.6f);

        is_empty = true;
        Time.timeScale = 1f;
        EventEnded?.Invoke();
        InteractionEnded();
    }
}
