using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthMeter : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject heart_prefab;
    [SerializeField] private Vector2 initial_position;
    [SerializeField] private Vector2 offsets;
    [SerializeField] private uint hearts_per_row;
    private List<UIHeart> hearts;
    private int current_health;
    private int current_maximum;
    private float animation_offset = 0.1f;

    private void Awake()
    {
        if (player != null)
        {
            player.Subscribe_HealthValueUpdated(PlayerController_HealthValueChanged);

            current_health = player.CurrentHealth;
            current_maximum = player.CurrentMaxHealth;
        }

        hearts = new List<UIHeart>();
        EvaluateHearts();
    }

    private void PlayerController_HealthValueChanged(int current, int max)
    {
        // Update life bar if max health has changed
        if (max != current_maximum)
        {
            EvaluateHearts();
            current_maximum = max;
        }

        if (current < current_health) // If losing health
        {
            int hearts_lost = 0;

            for (int i = 0; i < max; i++)
            {
                if (current == 1)
                {
                    hearts[0].SetWarning();
                }

                if (i >= current && i < current_health)
                {
                    hearts[i].LoseHeart(0f + (hearts_lost * animation_offset));
                    hearts_lost++;
                }
            }
        }
        else // If gaining health
        {
            for (int i = 0; i < current; i++)
            {
                hearts[i].RestoreHeart(0f);
            }
        }

        current_health = current;
    }

    private void EvaluateHearts()
    {
        if (player != null)
        {
            foreach (var image in hearts)
            {
                Destroy(image.gameObject);
            }

            hearts.Clear();

            for (int i = 0; i < player.CurrentMaxHealth; i++)
            {
                UIHeart new_heart = Instantiate(heart_prefab, new Vector3(), Quaternion.identity, canvas.transform).GetComponent<UIHeart>();
                hearts.Add(new_heart);

                Vector3 heart_position = new Vector3(initial_position.x + (offsets.x * (i % hearts_per_row)),
                                                     initial_position.y - (offsets.y * (i / hearts_per_row)),
                                                     0f);

                new_heart.panel.rectTransform.position = heart_position;

                if (i > player.CurrentHealth)
                {
                    new_heart.Animator.Play("Hit", 0, 1f);
                }
            }
        }
    }
}
