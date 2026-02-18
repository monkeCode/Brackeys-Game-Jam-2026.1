using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class TextMoney : MonoBehaviour
{
    TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TMP_Text>();
        if (ResourcesManager.Instance != null)
        {
            ResourcesManager.Instance.OnGoldChanged += UpdateText;
            text.text = ResourcesManager.Instance.money.ToString();
        }
    }

    void UpdateText(int money)
    {
        text.text = money.ToString();
    }
}
