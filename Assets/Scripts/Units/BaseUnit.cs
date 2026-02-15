using System.Collections;
using UnityEngine;

namespace Units
{
    public class BaseUnit : MonoBehaviour, IUnit, IDamageable
    {

        [SerializeField] protected BaseUnitObject unitObject;

        public virtual int Health { get => unitObject.Health; protected set => unitObject.Health = value; }
        public virtual int Attack => unitObject.Attack;
        public virtual float Speed => unitObject.Speed;
        public virtual float AttackRange => unitObject.AttackRange;

        public virtual int Armor => unitObject.Armor;
        public virtual float AttackCooldown => unitObject.AttackCooldown;
        public virtual UnitType Type => unitObject.Type;

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            // Handle death logic here (e.g., play animation, remove from game, etc.)
        }

        public void AttackTarget(IDamageable target)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTo(float x, float y)
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            // Initialize unit properties here
        }

        private void Update()
        {
            // Handle unit behavior here (e.g., movement, attacking, etc.)
        }

    }
}