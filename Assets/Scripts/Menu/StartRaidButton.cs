using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRaidButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { Troops.Last = 1; });
    }
}
