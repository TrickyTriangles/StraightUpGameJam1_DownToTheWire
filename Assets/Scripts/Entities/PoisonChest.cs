using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] [Multiline(3)] private List<string> dialogue;
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
            StartCoroutine(RunChestRoutine(player, InteractionEnded));
        }
        else
        {
            InteractionEnded();
        }
    }

    private IEnumerator RunChestRoutine(PlayerController player, System.Action InteractionEnded)
    {
        Time.timeScale = 0f;

        DialogueBox db = new DialogueBox(dialogue[0], CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        CommonResources.PlaySound("ChestOpen", 0.6f);
        if (animator != null)
        {
            animator.Play("Open", 0, 0f);
        }

        BGM.Instance.FadeOut(1f);
        yield return new WaitForSecondsRealtime(2f);

        // Particle effect will go here
        CommonResources.PlaySound("PoisonCloud", 0.6f);
        GameObject p = CommonResources.GetPrefab("PoisonCloud");

        if (p != null)
        {
            PoisonChestParticle particle = Instantiate(p, transform.position, transform.rotation, transform).GetComponent<PoisonChestParticle>();

            if (particle != null)
            {
                yield return particle.ProcessParticleSystem();
            }
        }

        for (int i = 1; i < dialogue.Count; i++)
        {
            DialogueBox dbx = new DialogueBox(dialogue[i], CommonResources.TextSpeedFast, "TextBlip2");
            yield return dbx.ProcessDialgoueBox();
        }

        yield return new WaitForSecondsRealtime(0.3f);
        db = new DialogueBox("At the bottom of the chest you spot a key.", CommonResources.TextSpeedFast, "TextBlip2");
        yield return db.ProcessDialgoueBox();

        CommonResources.PlaySound("KeyPickup", 0.6f);

        new PoisonPlayerEffect(player);
        new GiveKeys(player, 1);

        BGM.Instance.SetVolume(0.6f);
        BGM.Instance.PlaySong("160_Lost", 2f);

        Time.timeScale = 1f;
        InteractionEnded();
        is_empty = true;
    }
}
