using Buildings;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int GridSize = new(20, 10);
    private BaseBuilding[,] grid;
    private Sprite buildingSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        grid = new BaseBuilding[GridSize.x, GridSize.y];
    }


    public void StartPlacingBuilding(Sprite sprite)
    {
        if (buildingSprite != null)
        {
            Destroy(buildingSprite);
        }
        buildingSprite = Instantiate(sprite);
    }

    void Update()
    {

    }
}
