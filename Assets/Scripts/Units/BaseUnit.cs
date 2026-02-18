using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseUnit : MonoBehaviour, IUnit, IDamageable
    {
        [field: SerializeField] public virtual int Health { get; protected set;}
        [field: SerializeField]public virtual int Attack { get; protected set;}
        [field: SerializeField] public virtual float Speed { get; protected set;}
        [field: SerializeField] public virtual float AttackRange { get; protected set;}

        [field: SerializeField] public virtual int Armor { get; protected set;}
        [field: SerializeField] public virtual float AttackCooldown { get; protected set;}
        [field: SerializeField] public virtual UnitType Type { get; protected set;}

        [field: SerializeField] public virtual Command Command {get; protected set;}

        [field: SerializeField]  public virtual int MaxHealth {get; protected set;}


        protected Rigidbody2D rb;

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
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            rb.linearVelocity = Vector2.right * Speed;
        }

    }
}