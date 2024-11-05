using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ExtensionMethod
{
    public static void ClearChild(this Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            MonoBehaviour.Destroy(t.GetChild(i).gameObject);
        }
    }

    public static string ToCurrency(this int number)
    {
        // return "Rp." + number.ToString("N0").Replace(",", ".");
        if (number == 0)
        {
            return $"{number} Tz";
        }
        return $"{number.ToString("N0").Replace(",", ".")}.000 Tz";
        // return $"Rp.<size=120%><#21bf82>{number}.000";
    }
}