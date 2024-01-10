using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopsViewer : MonoBehaviour
{
    private Text _text = default;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        SetText(Troops.Value);
        Troops.OnSet.AddListener(SetText);
    }

    private void OnDisable()
    {
        Troops.OnSet.RemoveListener(SetText);
    }


    private void SetText(int value)
    {
        _text.text = value.ToString();
    }
}
