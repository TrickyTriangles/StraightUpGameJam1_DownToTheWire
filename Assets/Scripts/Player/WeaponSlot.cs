using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject attack_prefab;
    private bool is_active = true;
    public bool IsActive
    {
        get { return is_active; }
    }

    #region Events and Subscriber Methods

    private Action WeaponActivated;
    public void Subscribe_WeaponActivated(Action sub) { WeaponActivated += sub; }
    public void Unsubscribe_WeaponActivated(Action sub) { WeaponActivated -= sub; }

    private Action WeaponDisabled;
    public void Subscribe_WeaponDisabled(Action sub) { WeaponDisabled += sub; }
    public void Unsubscribe_WeaponDisabled(Action sub) { WeaponDisabled -= sub; }

    private Action WeaponEnabled;
    public void Subscribe_WeaponEnabled(Action sub) { WeaponEnabled += sub; }
    public void Unsubscribe_WeaponEnabled(Action sub) { WeaponEnabled -= sub; }

    #endregion

    public void UseWeapon()
    {
        if (is_active)
        {
            if (attack_prefab != null)
            {
                SwordSwing swing = Instantiate(attack_prefab, transform.position, Quaternion.identity).GetComponent<SwordSwing>();

                if (swing != null)
                {
                    swing.Swing(player, player.direction);
                }
            }

            WeaponActivated?.Invoke();
        }
    }

    public void DisableWeapon()
    {
        is_active = false;
        WeaponDisabled?.Invoke();
    }

    public void EnableWeapon()
    {
        is_active = true;
        WeaponEnabled?.Invoke();
    }
}
