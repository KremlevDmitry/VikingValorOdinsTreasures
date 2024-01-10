using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTroopsPage : MonoBehaviour
{
    [SerializeField]
    private Button _plusButton = default;
    [SerializeField]
    private Button _minusButton = default;
    [SerializeField]
    private Text _troopsText = default;
    [SerializeField]
    private Text _priceText = default;
    [SerializeField]
    private Button _buyButton = default;
    private int _troopsCount = default;
    private int _price = default;


    private void OnEnable()
    {
        SetValue(1);
    }

    private void Awake()
    {
        _plusButton.onClick.AddListener(Plus);
        _minusButton.onClick.AddListener(Minus);
        _buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (Wallet.Value >= _price)
        {
            Wallet.Value -= _price;
            Troops.Value += _troopsCount;
            SetValue(_troopsCount);
        }
    }

    private void SetValue(int count)
    {
        _troopsCount = count;
        if (_troopsCount < 1) { _troopsCount = 1; }
        _price = CalculateSum(_troopsCount, 100 + Troops.Value * 10);

        _troopsText.text = _troopsCount.ToString();
        _priceText.text = _price.ToString();
        _minusButton.interactable = _troopsCount > 1;
    }

    private void Plus()
    {
        if (_troopsCount > 9)
        {
            SetValue(_troopsCount + 10);
        }
        else
        {
            SetValue(_troopsCount + 1);
        }
    }

    private void Minus()
    {
        if (_troopsCount > 9)
        {
            SetValue(_troopsCount - 10);
        }
        else
        {
            SetValue(_troopsCount - 1);
        }
    }

    public int CalculateSum(int n, int startWith)
    {
        int sum = 0;
        int currentElement = startWith;

        for (int i = 0; i < n; i++)
        {
            sum += currentElement;
            currentElement += 1;
        }

        return sum;
    }
}
