using Merger;
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

        [SerializeField] protected LayerMask enemyLayer;

        protected Rigidbody2D rb;
        protected Vector2 MoveDirection;
        protected IDamageable CurrentTarget;
        private float _cooldownTimer;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _cooldownTimer = 0f;
            MoveDirection = Command == Command.Player ? Vector2.right : Vector2.left;
        }

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
            Destroy(gameObject);
        }

        public void AttackTarget(IDamageable target)
        {
            target.TakeDamage(Attack);
            _cooldownTimer = AttackCooldown;
        }

        public void MoveTo(float x, float y)
        {
            // не надо 
            throw new System.NotImplementedException();
        }

        protected virtual bool IsTargetInRange(IDamageable target)
        {
            if (target == null) return false;

            var mb = target as MonoBehaviour;
            if (mb == null) return false;

            float dist = Vector2.Distance(transform.position, mb.transform.position);
            return dist <= AttackRange;
        }

        protected virtual IDamageable FindClosestTargetInRange()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, enemyLayer);

            float best = float.MaxValue;
            IDamageable bestTarget = null;
            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit == null) continue;

                if (!hit.TryGetComponent<IDamageable>(out var dmg)) continue;

                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < best)
                {
                    best = dist;
                    bestTarget = dmg;
                }
            }

            return bestTarget;
        }

        protected virtual void MoveForward()
        {
            rb.linearVelocity = MoveDirection * Speed;
        }

        protected virtual void StopMoving()
        {
            rb.linearVelocity = Vector2.zero;
        }

        private void Start()
        {
            // Initialize unit properties here
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;

            if (CurrentTarget != null && !IsTargetInRange(CurrentTarget))
                CurrentTarget = null;

            CurrentTarget ??= FindClosestTargetInRange();

            if (CurrentTarget == null)
            {
                MoveForward();
            }
            else
            {
                StopMoving();
                Debug.Log(_cooldownTimer);
                if (_cooldownTimer < 0f)
                {
                    AttackTarget(CurrentTarget);
                }
            }
        }
    }
}