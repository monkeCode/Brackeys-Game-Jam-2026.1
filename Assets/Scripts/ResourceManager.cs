using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { get; private set; }

    [SerializeField] public int money = 500;

    public System.Action<int> OnGoldChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
}
