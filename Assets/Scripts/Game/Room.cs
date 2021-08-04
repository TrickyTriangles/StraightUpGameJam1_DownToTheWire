using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Mob> mobs;

    private void Start()
    {
        if (mobs != null)
        {
            foreach (var mob in mobs)
            {
                mob.Subscribe_OnDeath(Mob_OnDeath);
            }
        }
    }

    private void Mob_OnDeath(object sender)
    {
        var mob = sender as Mob;

        if (mob != null)
        {
            mobs.Remove(mob);
        }
    }

    public void ResetRoomObjects()
    {
        if (mobs != null)
        {
            foreach (var mob in mobs)
            {
                mob.ResetMob();
            }
        }
    }
}
