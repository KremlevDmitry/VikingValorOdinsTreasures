using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField]
    private int _price = default;
    [SerializeField]
    private GameObject _map = default;
    [SerializeField]
    private GameObject _game = default;
    private Button button = default;
    private Button _button => button ??= GetComponent<Button>();


    private void OnEnable()
    {
        _button.interactable = Troops.Value >= _price;
    }

    private void Awake()
    {
        _button.onClick.AddListener(OpenLevel);
    }


    private void OpenLevel()
    {
        Troops.Last = _price;
        _map.SetActive(false);
        _game.SetActive(true);
    }
}
