using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateReadout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerController player;

    private void PlayerController_HasStateChanged(string display_text)
    {
        if (text != null)
        {
            text.text = "Active state: " + display_text;
        }
    }

    private void Start()
    {
        if (player != null)
        {
            player.Subscribe_StateHasChanged(PlayerController_HasStateChanged);
        }
    }
}
