using System;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "BuildingActions/UnitsProducer", fileName = "UnitsProducer")]
    public class ProduceUnitAction : BuildingAction
    {
        [field: SerializeField] public Units.BaseUnit Unit {get; private set;}
        [SerializeField] protected int UpPercent;
        public override void DoAction(GameObject building)
        {
            Instantiate(Unit, building.transform.position, Quaternion.identity).ScaleStats(building.GetComponent<BaseBuilding>().Lvl);
        }

        public override void Up()
        {
            ActionCooldownTime -= (int)(UpPercent/100.0f * ActionCooldownTime);
        }
    }

}