using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSlotIndicator : MonoBehaviour
{
    [SerializeField] private WeaponSlot weapon_slot;

    [SerializeField] private Animator animator;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (weapon_slot != null)
        {
            weapon_slot.Subscribe_WeaponActivated(WeaponSlot_WeaponActivated);
            weapon_slot.Subscribe_WeaponDisabled(WeaponSlot_WeaponDisabled);
            weapon_slot.Subscribe_WeaponEnabled(WeaponSlot_WeaponEnabled);
        }
    }

    private void WeaponSlot_WeaponActivated()
    {
        if (weapon_slot != null)
        {
            if (animator != null)
            {
                animator.Play("Activate", 0, 0f);
            }
        }
    }

    private void WeaponSlot_WeaponDisabled()
    {
        if (icon != null) { icon.color = new Color(1f, 1f, 1f, 0.5f); }
        if (text != null) { text.color = new Color(1f, 1f, 1f, 0.5f); }
    }

    private void WeaponSlot_WeaponEnabled()
    {
        if (icon != null) { icon.color = Color.white; }
        if (text != null) { text.color = Color.white; }
    }
}
