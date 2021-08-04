using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSlot : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private int quantity = 1;
    public int Quantity
    {
        get { return quantity; }
    }

    private bool is_active = true;
    public bool IsActive
    {
        get { return is_active; }
    }

    #region Events and Subscriber Methods

    private Action PotionActivated;
    public void Subscribe_PotionActivated(Action sub) { PotionActivated += sub; }
    public void Unsubscribe_PotionActivated(Action sub) { PotionActivated -= sub; }

    private Action PotionDisabled;
    public void Subscribe_PotionDisabled(Action sub) { PotionDisabled += sub; }
    public void Unsubscribe_PotionDisabled(Action sub) { PotionDisabled -= sub; }

    private Action PotionEnabled;
    public void Subscribe_PotionEnabled(Action sub) { PotionEnabled += sub; }
    public void Unsubscribe_PotionEnabled(Action sub) { PotionEnabled -= sub; }

    private Action UpdatePotionQuantity;
    public void Subscribe_UpdatePotionQuantity(Action sub) { UpdatePotionQuantity += sub; }
    public void Unsubscribe_UpdatePotionQuantity(Action sub) { UpdatePotionQuantity -= sub; }

    #endregion

    private void Start()
    {
        UpdatePotionQuantity?.Invoke();
    }

    public void UsePotion()
    {
        if (is_active && quantity > 0)
        {
            if (player != null)
            {
                if (!player.IsFullHealth)
                {
                    player.RestoreMaxHealth();
                    quantity--;
                    CommonResources.PlaySound("Potion", 0.4f);

                    PotionActivated?.Invoke();
                }
            }
        }
    }

    public void DisablePotion()
    {
        is_active = false;
        PotionDisabled?.Invoke();
    }

    public void EnablePotion()
    {
        is_active = true;
        PotionEnabled?.Invoke();
    }

    public void AwardPotion(int number)
    {
        quantity += number;
        UpdatePotionQuantity?.Invoke();
    }
}
