using Buildings;
using Units;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int GridSize = new(20, 10);
    private IBuilding[,] grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
