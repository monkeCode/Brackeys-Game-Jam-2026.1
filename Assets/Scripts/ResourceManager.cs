using System;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { get; private set; }

    [SerializeField] public int money = 500;

    private List<IBuilding> _playerBuildings = new();
    private List<IBuilding> _enemyBuildings = new();

    public IReadOnlyList<IBuilding> PlayerBuildings => _playerBuildings;
    public IReadOnlyList<IBuilding> EnemyBuildings => _enemyBuildings;

    public Action EnemyWin;
    public Action PlayerWin;

    public System.Action<int> OnGoldChanged;

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

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            OnGoldChanged?.Invoke(money);
            return true;
        }
        return false;
    }

    public void GetMoney(int amount)
    {
        money += amount;
        OnGoldChanged?.Invoke(money);
    }

    public void AddPlayerBuilding(IBuilding building)
    {
        _playerBuildings.Add(building);
    }

    public void AddEnemyBuilding(IBuilding building)
    {
        _enemyBuildings.Add(building);
    }

    public void DeletePlayerBuilding(IBuilding building)
    {
        _playerBuildings.Remove(building);
        Debug.Log(_playerBuildings.Count);
        if (_playerBuildings.Count == 0)
        {
            EnemyWin?.Invoke();
            SceneManager.LoadScene("Scenes/GameOverMenu");
        }
    }

    public void DeleteEnemyBuilding(IBuilding building)
    {
        _enemyBuildings.Remove(building);
        if (_enemyBuildings.Count == 0)
        {
            PlayerWin?.Invoke();
            SceneManager.LoadScene("Scenes/VictoryMenu");
        }
    }
}
