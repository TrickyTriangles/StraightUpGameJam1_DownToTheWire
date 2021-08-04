using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : StatusEffect
{
    private IDamageable damageable;

    public override void ActivateEffect(MonoBehaviour effect_target)
    {
        damageable = effect_target as IDamageable;
        active_effect = effect_target.StartCoroutine(RunEffect());

        base.ActivateEffect(effect_target);
    }

    public override void DeactivateEffect()
    {
        if (active_effect != null)
        {
            target.StopCoroutine(active_effect);
        }

        base.DeactivateEffect();
    }

    private IEnumerator RunEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.1f);
            damageable.Damage(new DamageProfile(target.gameObject, 1));
        }
    }
}
