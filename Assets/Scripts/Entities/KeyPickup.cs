using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(PlayerController player, System.Action InteractionEnded)
    {
        player.AwardKeys(1);
        CommonResources.PlaySound("KeyPickup", 0.6f);
        Destroy(gameObject);
    }
}
