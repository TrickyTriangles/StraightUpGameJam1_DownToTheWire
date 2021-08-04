using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePotion
{
    public GivePotion(PlayerController player, int number)
    {
        player.AwardPotions(number);
    }
}
