using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int number_of_potions;
    private bool is_empty;

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

    void IInteractable.Interact(PlayerController player, System.Action InteractionEnded)
    {
        if (!is_empty)
        {
            StartCoroutine(PotionChestRoutine(player, InteractionEnded));
        }
        else
        {
            InteractionEnded();
        }
    }

    private IEnumerator PotionChestRoutine(PlayerController player, System.Action InteractionEnded)
    {
        Time.timeScale = 0f;

        CommonResources.PlaySound("ChestOpen", 0.6f);

        if (animator != null)
        {
            animator.Play("Open", 0, 0f);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        string text = (number_of_potions > 1) ? $"Inside the chest were {number_of_potions} potions!" : "Inside the chest was a potion!";

        DialogueBox db = new DialogueBox(text, CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        new GivePotion(player, number_of_potions);

        is_empty = true;
        Time.timeScale = 1f;
        InteractionEnded();
    }
}
