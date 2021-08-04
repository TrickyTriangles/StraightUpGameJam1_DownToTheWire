using System.Collections;
using UnityEngine;

public class StatusEffect
{
    protected MonoBehaviour target;
    protected Coroutine active_effect;

    public virtual void ActivateEffect(MonoBehaviour effect_target)
    {
        target = effect_target;
    }

    public virtual void DeactivateEffect()
    {
        target = null;
    }
}
