using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BirdFishing_Math
{
    public static float Approach(float a, float b, float factor, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Pow(factor, dt));
    }

    public static float PythagoreanDistance(float a, float b)
    {
        return Mathf.Sqrt( a*a + b*b );
    }

    public static float LinearMap(float value, float inputLow, float inputHigh, float outputLow, float outputHigh, bool clamp = false ) 
    {
        float output = outputLow + (outputHigh - outputLow) * (value - inputLow) / (inputHigh - inputLow);

        if( clamp )
        {
            output = Mathf.Clamp( output, outputLow, outputHigh );
        }

        return output;
    }
}
