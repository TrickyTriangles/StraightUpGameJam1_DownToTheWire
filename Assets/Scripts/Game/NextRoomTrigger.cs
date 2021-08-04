using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoomTrigger : MonoBehaviour
{
    [SerializeField] private Room next_room;
    [SerializeField] private SpriteRenderer sprite;

    public Room NextRoom
    {
        get { return next_room; }
    }

    [SerializeField] private Direction direction;
    public Direction MoveDirection
    {
        get { return direction; }
    }

    private void Awake()
    {
        if (sprite != null)
        {
            sprite.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
