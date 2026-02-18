using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "BuildingActions/MoneyProducer", fileName = "MoneyProducer")]
    public class ProduceMoneyAction : BuildingAction
    {
        [Min(0)]
        [field: SerializeField] public int Amount {get; private set;}

        public override void DoAction(GameObject building)
        {
            ResourcesManager.Instance.GetMoney(Amount);
        }
    }
}