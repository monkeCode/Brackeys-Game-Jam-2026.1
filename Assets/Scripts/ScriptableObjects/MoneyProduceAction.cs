using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "BuildingActions/MoneyProducer", fileName = "MoneyProducer")]
    public class ProduceMoneyAction : BuildingAction
    {
        [Min(0)]
        [field: SerializeField] public int Amount {get; private set;}
        [field: SerializeField] public int UpPercent {get; private set;}
        [field: SerializeField] public GameObject MoneyPrefab {get; private set;}

        public override void DoAction(GameObject building)
        {
            ResourcesManager.Instance.GetMoney(Amount);
            Instantiate(MoneyPrefab, building.transform.position, Quaternion.identity);
        }

        public override void Up()
        {
            Amount += (int)(UpPercent/100.0f * Amount);
            ActionCooldownTime -= (int)(UpPercent/100.0f * ActionCooldownTime);
        }

        public override string ToString()
        {
            return $"Makes money: {Amount} each {ActionCooldownTime} ticks";
        }
    }
}