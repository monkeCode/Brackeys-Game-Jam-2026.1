using System;
using Buildings;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int GridSize = new(20, 10);
    [SerializeField] private Camera mainCamera;
    private GameObject flyingBuilding;
    private SpriteRenderer flyingBuildingRenderer;
    private BaseBuilding[,] grid;
    private Sprite buildingSprite;
    private BaseBuilding building;
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
        flyingBuilding = new GameObject("CursorSprite");
        flyingBuildingRenderer = flyingBuilding.AddComponent<SpriteRenderer>();
        if (buildingSprite != null)
        {
            flyingBuildingRenderer.sprite = buildingSprite;
        }
        building = buildingPrefab;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - building.Size.x) available = false;
                if (y < 0 || y > GridSize.y - building.Size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuildingRenderer.color = setColor(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceBuilding(x, y);
                }
            }
        }
    }

    private Color setColor(Boolean available)
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

    private void PlaceBuilding(int placeX, int placeY)
    {
        building.transform.position = new Vector3(placeX, 0, placeY);
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
}
