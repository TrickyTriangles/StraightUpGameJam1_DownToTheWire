using UnityEngine;

public struct DamageProfile
{
    public GameObject attacker;
    public Vector3 position_hit;
    public int power;

    public DamageProfile(GameObject atk, int pow)
    {
        attacker = atk;
        power = pow;
        position_hit = atk.transform.position;
    }
}
