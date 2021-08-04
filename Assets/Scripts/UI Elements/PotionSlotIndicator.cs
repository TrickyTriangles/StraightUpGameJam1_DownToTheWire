using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionSlotIndicator : MonoBehaviour
{
    [SerializeField] private PotionSlot potion_slot;

    [SerializeField] private Animator animator;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity_readout;
    [SerializeField] private TextMeshProUGUI quantity_readout_back;

    private void Awake()
    {
        if (potion_slot != null)
        {
            potion_slot.Subscribe_PotionActivated(PotionSlot_ActivatePotion);
            potion_slot.Subscribe_PotionDisabled(PotionSlot_DisablePotion);
            potion_slot.Subscribe_PotionEnabled(PotionSlot_EnablePotion);
            potion_slot.Subscribe_UpdatePotionQuantity(PotionSlot_UpdatePotionQuantity);
        }
    }

    private void PotionSlot_ActivatePotion()
    {
        if (animator != null)
        {
            animator.Play("Activate", 0, 0f);
        }

        UpdateText();
    }

    private void PotionSlot_DisablePotion()
    {
        if (icon != null) { icon.color = new Color(1f, 1f, 1f, 0.5f); }
    }

    private void PotionSlot_EnablePotion()
    {
        if (icon != null) { icon.color = new Color(1f, 1f, 1f, 0.5f); }
    }

    private void PotionSlot_UpdatePotionQuantity()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (quantity_readout != null)
        {
            quantity_readout.text = $"({potion_slot.Quantity})";
        }

        if (quantity_readout_back != null)
        {
            quantity_readout_back.text = $"({potion_slot.Quantity})";
        }
    }
}