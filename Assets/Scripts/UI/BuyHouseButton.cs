using Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class BuyHouseButton : MonoBehaviour
{
    [SerializeField] public BaseBuilding buildingPrefab;
    private TMP_Text text;
    private Image buttonImage;
    private Button buttonComponent;
    private Color buttonColor;
    private Color textColor;
    Image image;
    [SerializeField] public Color unavailableButtonColor = new(1, 0, 0, 0.3f);
    [SerializeField] public Color unavailableTextColor = new(0.6f, 0.2f, 0.2f, 0.7f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonComponent = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonColor = buttonImage.color;
        text = GetComponentInChildren<TMP_Text>();
        textColor = text.color;
        text.text = buildingPrefab.Cost.ToString();
        Transform gb = buildingPrefab.transform.GetChild(0);
        Transform child = gb.Find("rt");
        image = transform.GetChild(1).GetComponent<Image>();
        image.sprite = child.GetComponent<SpriteRenderer>().sprite;
        image.color = new Color(255, 255, 255, 255);
        if (ResourcesManager.Instance != null)
        {
            ResourcesManager.Instance.OnGoldChanged += UpdateButton;
            if (ResourcesManager.Instance.money < buildingPrefab.Cost)
            {
                buttonImage.color = unavailableButtonColor;
                image.color = unavailableButtonColor;
                buttonComponent.interactable = false;
                text.color = unavailableTextColor;
            }
        }
    }

    public void UpdateButton(int money)
    {
        if (money < buildingPrefab.Cost)
        {
            buttonImage.color = unavailableButtonColor;
            image.color = unavailableButtonColor;
            buttonComponent.interactable = false;
            text.color = unavailableTextColor;
        }
        else
        {
            buttonImage.color = buttonColor;
            image.color = new Color(255, 255, 255, 255);
            buttonComponent.interactable = true;
            text.color = textColor;
        }
    }
}
