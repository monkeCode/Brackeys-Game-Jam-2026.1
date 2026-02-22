using System;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "BuildingActions/ShootAction", fileName = "ShootAction")]
    public class ShootAction : BuildingAction
    {
        [field: SerializeField] public Bullet bullet {get; private set;}
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float range;
        [SerializeField] private int damage;
        
        [SerializeField] protected int UpPercent;
        public override void DoAction(GameObject building)
        {
            var targets = Physics2D.OverlapCircleAll(building.transform.position, range, targetLayer).Select(t => t.TryGetComponent(out IDamageable targ)?targ:null).ToList();
            if(targets.Count > 0)
                Instantiate(bullet, building.transform.position, Quaternion.identity).FindAndDestroy(targets[UnityEngine.Random.Range(0, targets.Count)], damage);
        }

        public override void Up()
        {
            ActionCooldownTime -= (int)(UpPercent/100.0f * ActionCooldownTime);
        }

        public override string ToString()
        {
            return $"Shoot {damage} dmg arror each {ActionCooldownTime} ticks";
        }
    }

}