using System;
using Buildings;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingUI : MonoBehaviour, IPointerExitHandler
{

    private BaseBuilding activeBuilding;    

    [SerializeField] private TMP_Text text;

    public void ShowUi(BaseBuilding building)
    {
        activeBuilding = building;
        text.text = activeBuilding.ToString();
    }

    public void DestroyBuilding()
    {
        activeBuilding.Destroy();
        gameObject.SetActive(false);
    }

    public void UpdateBuilding()
    {
        //TODO: деньги чекнуть
        activeBuilding.Up();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
