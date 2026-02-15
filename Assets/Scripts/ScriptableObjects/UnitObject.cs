using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "BaseUnit", menuName = "Units/GameSettingsScriptableObject", order = 2)]
    public class BaseUnitObject: ScriptableObject, IUnit
    {

        [field: SerializeField]  public int Health { get; set; }

        [field: SerializeField]public int Attack {get; set;}

        [field: SerializeField]public float Speed {get; set;}

        [field: SerializeField]public float AttackRange {get; set;}
        [field: SerializeField]public int Armor {get; set;}

        [field: SerializeField]public float AttackCooldown {get; set;}

        [field: SerializeField] public UnitType Type {get; set;}
        
        public void AttackTarget(IDamageable target)
        {
            throw new System.NotImplementedException();
        }

        public void Die()
        {
            throw new System.NotImplementedException();
        }

        public void MoveTo(float x, float y)
        {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            throw new System.NotImplementedException();
        }
    }
}