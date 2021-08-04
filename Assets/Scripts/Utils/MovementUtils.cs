using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementUtils
{
    public static Direction CheckMovementDirection(Vector2 looking)
    {
        Direction direction;

        if (looking.x == 0f && looking.y > 0f)
        {
            direction = Direction.NORTH;
        }
        else if (looking.x > 0f && looking.y == 0f)
        {
            direction = Direction.EAST;
        }
        else if (looking.x == 0f && looking.y < 0f)
        {
            direction = Direction.SOUTH;
        }
        else if (looking.x < 0f && looking.y == 0f)
        {
            direction = Direction.WEST;
        }
        else
        {
            direction = Direction.NONE;
        }

        return direction;
    }
}
