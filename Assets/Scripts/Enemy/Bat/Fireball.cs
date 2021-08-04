using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Bat
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private Vector3 target;
        private Vector3 direction;
        private float lifetime = 5f;
        private float move_speed = 2f;

        private void GameManager_RoomTransitionBegin(Direction d, float f)
        {
            CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
            Destroy(gameObject);
        }

        private void Start()
        {
            direction = -(target - transform.position).normalized;
            StartCoroutine(FireballRoutine());
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                rb.MovePosition(rb.position - ((Vector2)direction * Time.fixedDeltaTime * move_speed));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(new DamageProfile(gameObject, 1));
                }
            }

            CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(new DamageProfile(gameObject, 1));
                }
            }

            CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
            Destroy(gameObject);
        }

        public void SetTarget(Vector3 new_target)
        {
            target = new_target;
        }

        private IEnumerator FireballRoutine()
        {
            float timer = 0f;
            CommonResources.game_manager.Subscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);

            while (timer < lifetime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
            Destroy(gameObject);
        }
    }
}