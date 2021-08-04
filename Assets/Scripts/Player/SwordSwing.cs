using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    private PlayerController attacker;
    private float start_rotation;
    private float swing_amount = 75f;
    private Coroutine swing_routine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slime"))
        {
            IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(new DamageProfile(attacker.gameObject, 1));
            }
        }
        else
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(new DamageProfile(attacker.gameObject, 1));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(new DamageProfile(attacker.gameObject, 1));
        }
    }

    public void Swing(PlayerController m_attacker, Direction direction)
    {
        attacker = m_attacker;

        switch (direction)
        {
            case Direction.NONE:
                start_rotation = 0f;
                break;
            case Direction.NORTH:
                start_rotation = 90f + swing_amount * 0.5f;
                break;
            case Direction.EAST:
                start_rotation = 0f + swing_amount * 0.5f;
                break;
            case Direction.SOUTH:
                start_rotation = -90f + swing_amount * 0.5f;
                break;
            case Direction.WEST:
                start_rotation = -180f + swing_amount * 0.5f;
                break;
            default:
                break;
        }

        swing_routine = StartCoroutine(SwingRoutine());
    }

    private IEnumerator SwingRoutine()
    {
        float swing_time = 0.1f;
        float timer = 0f;

        while (timer < swing_time)
        {
            timer += Time.deltaTime;

            Quaternion new_rotation = Quaternion.Euler(0f, 0f, start_rotation - (swing_amount * (timer / swing_time)));
            transform.rotation = new_rotation;

            yield return null;
        }

        Destroy(gameObject);
    }
}
