using System;
using Buildings;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
    
    
    public static UiManager Instance {get; private set;}

    [SerializeField] private BuildingUI buildingUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowBuildingUi(BaseBuilding building)
    {
        buildingUI.gameObject.SetActive(true);
        buildingUI.ShowUi(building);
        // buildingUI.transform.position = building.transform.position + Vector3.up * 10;
    }

}
