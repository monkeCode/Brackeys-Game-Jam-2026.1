using System;
using Buildings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int GridSize = new(20, 10);
    [SerializeField] private Camera mainCamera;
    private GameObject flyingBuilding;
    // private SpriteRenderer flyingBuildingSpriteRenderer;
    private BaseBuilding[,] grid;
    private BaseBuilding building;
    private GameObject flyingBuildingSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        grid = new BaseBuilding[GridSize.x, GridSize.y];
    }


    public void StartPlacingBuilding(BaseBuilding buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding);
        }
        Transform gb = buildingPrefab.transform.GetChild(0);
        SpriteRenderer sp = gb.GetComponent<SpriteRenderer>();
        GameObject copy = Instantiate(gb.gameObject);
        flyingBuilding = new GameObject("CursorObject");
        flyingBuildingSprite = copy;
        flyingBuildingSprite.transform.SetParent(flyingBuilding.transform);
        flyingBuildingSprite.transform.position = new Vector3(gb.localPosition.x, gb.localPosition.y, gb.localPosition.z);
        // flyingBuildingSpriteRenderer = flyingBuildingSprite.AddComponent<SpriteRenderer>();
        // if (sp.sprite != null)
        // {
        //     flyingBuildingSpriteRenderer.sprite = sp.sprite;
        //     flyingBuildingSpriteRenderer.color = sp.color;
        //     flyingBuildingSpriteRenderer.sortingOrder = 100;
        flyingBuilding.transform.localScale = buildingPrefab.transform.localScale;
        flyingBuildingSprite.transform.localScale = gb.localScale;
        //     // flyingBuilding.transform.localScale = new Vector3(gb.localScale.x * buildingPrefab.transform.localScale.x, gb.localScale.y * buildingPrefab.transform.localScale.y, gb.localScale.z * buildingPrefab.transform.localScale.z);
        // }
        building = buildingPrefab;
    }

    private void Update()
    {
        if (Mouse.current.rightButton.IsPressed())
        {
            Destroy(flyingBuilding);
            building = null;
        }
        else if (flyingBuilding != null && !EventSystem.current.IsPointerOverGameObject())
        {
            var groundPlane = new Plane(Vector3.forward, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.y);

                bool available = true;

                if (x < 0 || x > GridSize.x - building.Size.x) available = false;
                if (y < 0 || y > GridSize.y - building.Size.y) available = false;

                // if (x < -building.Size.x / 2 || x > GridSize.x - (building.Size.x + 1) / 2) available = false;
                // if (y < -building.Size.y / 2 || y > GridSize.y - (building.Size.y + 1) / 2) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = new Vector3(x, y, 0);
                foreach (var sr in flyingBuildingSprite.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = SetColor(available);
                }
                // flyingBuildingSprite.GetComponent<SpriteRenderer>().color = setColor(available);

                if (available && Mouse.current.leftButton.IsPressed())
                {
                    PlaceBuilding(x, y);
                }
            }
        }
    }

    private Color SetColor(Boolean available)
    {
        if (available)
        {
            return Color.green;
        }
        return Color.red;
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }

    // private bool IsPlaceTaken(int placeX, int placeY)
    // {
    //     for (int x = 0; x < building.Size.x; x++)
    //     {
    //         if (x % 2 != 0)
    //         {
    //             for (int y = 0; y < building.Size.y; y++)
    //             {
    //                 if (y % 2 != 0)
    //                 {
    //                     if (grid[placeX + x, placeY + y] != null) return true;
    //                 }
    //                 else
    //                 {
    //                     if (grid[placeX + x, placeY - y] != null) return true;
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             for (int y = 0; y < building.Size.y; y++)
    //             {
    //                 if (y % 2 != 0)
    //                 {
    //                     if (grid[placeX - x, placeY + y] != null) return true;
    //                 }
    //                 else
    //                 {
    //                     if (grid[placeX - x, placeY - y] != null) return true;
    //                 }
    //             }
    //         }
    //     }

    //     return false;
    // }

    private void PlaceBuilding(int placeX, int placeY)
    {
        if (ResourcesManager.Instance != null)
        {
            if (!ResourcesManager.Instance.SpendMoney(building.Cost))
            {
                return;
            }
        }
        building.transform.position = new Vector3(placeX, placeY, 0);
        Instantiate(building);
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = building;
            }
        }

        Destroy(flyingBuilding);
        building = null;
    }

    // private void PlaceBuilding(int placeX, int placeY)
    // {
    //     building.transform.position = new Vector3(placeX, placeY, 0);
    //     Instantiate(building);
    //     for (int x = 0; x < building.Size.x; x++)
    //     {
    //         if (x % 2 != 0)
    //         {
    //             for (int y = 0; y < building.Size.y; y++)
    //             {
    //                 if (y % 2 != 0)
    //                 {
    //                     grid[placeX + x, placeY + y] = building;
    //                 }
    //                 else
    //                 {
    //                     grid[placeX + x, placeY - y] = building;
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             for (int y = 0; y < building.Size.y; y++)
    //             {
    //                 if (y % 2 != 0)
    //                 {
    //                     grid[placeX - x, placeY + y] = building;
    //                 }
    //                 else
    //                 {
    //                     grid[placeX - x, placeY - y] = building;
    //                 }
    //             }
    //         }
    //     }

    //     Destroy(flyingBuilding);
    //     building = null;
    // }
}
