using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class TextMoney : MonoBehaviour
{
    TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        ResourcesManager.Instance.OnGoldChanged += UpdateText;
        if (ResourcesManager.Instance != null)
        {
            text.text = ResourcesManager.Instance.money.ToString();
        }
    }

    void UpdateText(int money)
    {
        text.text = money.ToString();
    }

    void OnDestroy()
    {
        ResourcesManager.Instance.OnGoldChanged -= UpdateText;
    }
}
