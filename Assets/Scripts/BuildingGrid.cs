using System;
using System.Collections.Generic;
using Buildings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(AudioSource))]
public class BuildingGrid : MonoBehaviour
{
    [SerializeField] public GameObject CeilFiller;
    [SerializeField] public Vector2Int GridSize = new(20, 10);
    [SerializeField] private Camera mainCamera;
    private GameObject flyingBuilding;
    // private SpriteRenderer flyingBuildingSpriteRenderer;
    private BaseBuilding[,] grid;
    private List<GameObject> places = new();
    private BaseBuilding building;
    private GameObject flyingBuildingSprite;
    private AudioSource audioSource;
    enum PlaceTaken
    {
        NotTaken,
        PartiallyTaken,
        FullyTaken
    }
    PlaceTaken status;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        grid = new BaseBuilding[GridSize.x, GridSize.y];
        audioSource = GetComponent<AudioSource>();
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                places.Add(Instantiate(CeilFiller, new Vector3(x, y, 0), Quaternion.identity));
            }
        }
    }


    public void StartPlacingBuilding(BaseBuilding buildingPrefab)
    {
        foreach (var p in places)
        {
            p.SetActive(true);
        }
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding);
        }
        Transform gb = buildingPrefab.transform.GetChild(0);
        // SpriteRenderer sp = gb.GetComponent<SpriteRenderer>();
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
        foreach (var sr in flyingBuildingSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = 100;
        }
        //     // flyingBuilding.transform.localScale = new Vector3(gb.localScale.x * buildingPrefab.transform.localScale.x, gb.localScale.y * buildingPrefab.transform.localScale.y, gb.localScale.z * buildingPrefab.transform.localScale.z);
        // }
        building = buildingPrefab;
    }

    private void Update()
    {
        if (flyingBuilding == null)
        {
            foreach (var p in places)
            {
                p.SetActive(false);
            }
        }
        if (Mouse.current.rightButton.IsPressed())
        {
            Destroy(flyingBuilding);
            building = null;
            UiManager.Instance.HideBuildingUi();
        }
        else if (flyingBuilding != null)
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
                if (available) status = IsPlaceTaken(x, y);
                if (available && status == PlaceTaken.PartiallyTaken) available = false;

                flyingBuilding.transform.position = new Vector3(x, y, 0);
                foreach (var sr in flyingBuildingSprite.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = SetColor(available);
                }
                // flyingBuildingSprite.GetComponent<SpriteRenderer>().color = setColor(available);

                if (available && Mouse.current.leftButton.IsPressed())
                {
                    PlaceBuilding(x, y, status);
                }
            }
        }
    }

    private Color SetColor(bool available)
    {
        if (available)
        {
            return Color.green;
        }
        return Color.red;
    }

    private PlaceTaken IsPlaceTaken(int placeX, int placeY)
    {
        int n = 0;
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) n++;
            }
        }
        if (n < building.Size.x * building.Size.y)
        {
            return n == 0 ? PlaceTaken.NotTaken : PlaceTaken.PartiallyTaken;
        }
        else
        {
            return PlaceTaken.FullyTaken;
        }
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

    private void PlaceBuilding(int placeX, int placeY, PlaceTaken status)
    {
        if (ResourcesManager.Instance != null)
        {
            if (!ResourcesManager.Instance.SpendMoney(building.Cost))
            {
                return;
            }
        }
        if (status == PlaceTaken.NotTaken)
        {
            var b = Instantiate(building, new Vector3(placeX, placeY, 0), Quaternion.identity);
            for (int x = 0; x < building.Size.x; x++)
            {
                for (int y = 0; y < building.Size.y; y++)
                {
                    grid[placeX + x, placeY + y] = b;
                }
            }
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        if (status == PlaceTaken.FullyTaken)
        {
            MergeBuildings(building, grid[placeX, placeY]);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        Destroy(flyingBuilding);
        building = null;
    }

    public void MergeBuildings(BaseBuilding newBuilding, BaseBuilding oldBuilding)
    {
        oldBuilding.Merge(newBuilding);
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
