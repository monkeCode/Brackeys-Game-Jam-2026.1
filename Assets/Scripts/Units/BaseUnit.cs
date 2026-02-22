using System.Collections.Generic;
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

        [Header("Attack Animation (2 frames)")]
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite attackSprite;
        [SerializeField] private float attackAnimTime = 0.25f;

        private float _knockbackTimer;

        protected Rigidbody2D rb;
        protected SpriteRenderer sp;
        protected Vector2 MoveDirection;
        protected IDamageable CurrentTarget;
        private float _cooldownTimer;
        private float _attackAnimTimer;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _cooldownTimer = 0f;
            MoveDirection = Command == Command.Player ? Vector2.right : Vector2.left;
        }

        public virtual void TakeDamage(int damage)
        {
            dmg = Math.Max(1, damage - Armor);
            Health -= dmg;

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
            PlayAttackAnim();
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
            IDamageable bestTarget = null;
            List<(IDamageable target, float weight)> weightedTargets = new List<(IDamageable, float)>();
            float totalWeight = 0f;

            for (int i = 0; i < hits.Length; i++)
            {
                var hit = hits[i];
                if (hit == null || !hit.TryGetComponent<IDamageable>(out var dmg)) continue;

                float dist = Vector2.Distance(transform.position, hit.transform.position);
                float weight = 1f / (dist + 0.01f); // Инверсия расстояния
                
                weightedTargets.Add((dmg, weight));
                totalWeight += weight;
            }

            // Выбираем случайную цель
            float randomPoint = Random.value * totalWeight;

            foreach (var (target, weight) in weightedTargets)
            {
                if (randomPoint < weight)
                {
                    bestTarget = target;
                    break;
                }
                randomPoint -= weight;
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

        private void PlayAttackAnim()
        {
            sp.sprite = attackSprite;
            _attackAnimTimer = attackAnimTime;
        }

        private void Start()
        {
            // Initialize unit properties here
            rb = GetComponent<Rigidbody2D>();
            sp = GetComponent<SpriteRenderer>();
            sp.sprite = idleSprite;
        }

        private void Update()
        {
            sp.flipX = Command != Command.Player;
            _cooldownTimer -= Time.deltaTime;
            if (_knockbackTimer > 0f)
            {
                _knockbackTimer -= Time.deltaTime;
                rb.linearVelocity = (-MoveDirection) * knockbackForce;
                return;
            }

            if (_attackAnimTimer > 0f)
            {
                _attackAnimTimer -= Time.deltaTime;
                if (_attackAnimTimer <= 0f)
                    sp.sprite = idleSprite;
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