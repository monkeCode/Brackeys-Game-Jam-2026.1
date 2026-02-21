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
        [field: SerializeField] public virtual float ViewRange { get; protected set; }

        [field: SerializeField] public virtual int Armor { get; protected set;}
        [field: SerializeField] public virtual float AttackCooldown { get; protected set;}
        [field: SerializeField] public virtual UnitType Type { get; protected set;}

        [field: SerializeField] public virtual Command Command {get; protected set;}

        [field: SerializeField]  public virtual int MaxHealth {get; protected set;}

        [SerializeField] protected LayerMask enemyLayer;

        [Header("Knockback")]
        [SerializeField] private float knockbackForce = 3f;
        [SerializeField] private float knockbackDuration = 0.25f;

        [Header("Ranged Settings")]
        [SerializeField] private Bullet projectilePrefab;

        private float _knockbackTimer;

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

            ApplyKnockback();

            if (Health <= 0)
            {
                Die();
            }
        }

        protected virtual void ApplyKnockback()
        {
            _knockbackTimer = knockbackDuration;
            Vector2 knockDir = -MoveDirection;
            rb.linearVelocity = knockDir * knockbackForce;
        }

        public virtual void Die()
        {
            Destroy(gameObject);
        }

        public void AttackTarget(IDamageable target)
        {
            if (Type == UnitType.Ranged)
            {
                Bullet projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.FindAndDestroy(target, Attack);
            }
            else
            {
                target.TakeDamage(Attack);
            }

            _cooldownTimer = AttackCooldown;
        }

        public void MoveTo(IDamageable target)
        {
            if (target is not MonoBehaviour mb)
            {
                MoveForward();
                return;
            }

            rb.linearVelocity = ((mb.transform.position - transform.position) * Speed).normalized;
        }

        protected virtual bool IsTargetInRange(IDamageable target, float range)
        {
            if (target == null) return false;
            if (target is not MonoBehaviour mb) return false;

            float dist = Vector2.Distance(transform.position, mb.transform.position);
            return dist <= range;
        }

        protected virtual IDamageable FindClosestTarget(float range)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

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
            if (_knockbackTimer > 0f)
            {
                _knockbackTimer -= Time.deltaTime;
                rb.linearVelocity = (-MoveDirection) * knockbackForce;
                return;
            }

            if (CurrentTarget is MonoBehaviour targetMb)
            {
                if (targetMb == null || !IsTargetInRange(CurrentTarget, ViewRange))
                    CurrentTarget = null;
            }
            else if (CurrentTarget != null)
            {
                CurrentTarget = null;
            }

            CurrentTarget ??= FindClosestTarget(ViewRange);

            if (CurrentTarget == null)
            {
                MoveForward();
                return;
            }
            
            if (IsTargetInRange(CurrentTarget, AttackRange))
            {
                StopMoving();
                if (_cooldownTimer <= 0f)
                    AttackTarget(CurrentTarget);
            }
            else
            {
                MoveTo(CurrentTarget);
            }
        }

        public virtual void ScaleStats(int level)
        {
            if (level <= 1)
                return;

            float multiplier = 1f + (level - 1) * 0.1f; 

            Health = Mathf.RoundToInt(Health * multiplier);
            Attack = Mathf.RoundToInt(Attack * multiplier);
            Speed *= multiplier;
            AttackCooldown /= multiplier;
        }
    }
}