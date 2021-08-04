using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyReadout : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI readout_front;
    [SerializeField] private TextMeshProUGUI readout_back;

    private void Awake()
    {
        if (player != null)
        {
            player.Subscribe_KeyValueUpdated(PlayerController_KeysUpdated);
        }
    }

    private void PlayerController_KeysUpdated()
    {
        if (readout_front != null)
        {
            readout_front.text = "x" + player.KeyCount;
        }

        if (readout_back != null)
        {
            readout_back.text = "x" + player.KeyCount;
        }
    }
}
