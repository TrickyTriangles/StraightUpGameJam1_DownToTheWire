using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    protected Action<object> OnDeath;
    public void Subscribe_OnDeath(Action<object> sub) { OnDeath += sub; }
    public void Unsubscribe_OnDeath(Action<object> sub) { OnDeath -= sub; }

    public virtual void ResetMob() { }
}
