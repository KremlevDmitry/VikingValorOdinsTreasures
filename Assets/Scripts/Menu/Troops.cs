using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Troops
{
    public static UnityEvent<int> OnSet = new UnityEvent<int>();

    public static int Last = 1;

    public static int Value
    {
        get
        {
            if (!PlayerPrefs.HasKey("TroopsValue"))
            {
                Value = 1;
            }
            return PlayerPrefs.GetInt("TroopsValue");
        }
        set
        {
            if (value < 1) { value = 1; }
            PlayerPrefs.SetInt("TroopsValue", value);
            OnSet.Invoke(value);
        }
    }
}
