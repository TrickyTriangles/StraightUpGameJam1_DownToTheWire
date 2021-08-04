using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveKeys
{
    public GiveKeys(PlayerController player, int quantity)
    {
        player.AwardKeys(quantity);
    }
}
