using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Smoothstep
    /// </summary>
    /// <returns>3x^2 - 2x^3</returns>
    public static float SmoothStep(float min, float max, float value)
    {
        float p = Mathf.Clamp((value - min) / (max - min), 0f, 1f);

        return p * p * (3 - 2 * p);
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Smoothstep
    /// </summary>
    /// <returns>6x^5 - 15x^4 + 10x^3</returns>
    public static float SmootherStep(float min, float max, float value)
    {
        float p = Mathf.Clamp((value - min) / (max - min), 0f, 1f);

        return p * p * p * (p * (p * 6 - 15) + 10);
    }
}
