using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "BuildingActions/UnitsProducer", fileName = "UnitsProducer")]
    public class ProduceUnitAction : BuildingAction
    {
        [field: SerializeField] public Units.BaseUnit Unit {get; private set;}

        public override void DoAction(GameObject building)
        {
            Instantiate(Unit, building.transform.position, Quaternion.identity);
        }
    }

}