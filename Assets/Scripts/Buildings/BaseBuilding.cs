using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    class BaseBuilding : MonoBehaviour, IBuilding, IDamageable
    {
        [field: SerializeField]  public int Health { get; protected set;}

        [field: SerializeField]  public int Cost { get; protected set;}

        [field: SerializeField]  public Vector2Int Size { get; protected set;}

        public IReadOnlyList<IBuildingAction> Actions { get => actions;}

        [field: SerializeField]  public Command Command { get; protected set;}

        [field: SerializeField]  public int MaxHealth {get;protected set;}

        [SerializeField] protected List<BuildingAction> actions;


        private SpriteRenderer sp;
        public SpriteRenderer Sprite {get => sp = sp != null ? sp : GetComponent<SpriteRenderer>();}

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }
            Destroy();
        }

        public void UpdateTimeTick()
        {
            foreach (var action in Actions)
            {
                action.UpdateTimeTick(gameObject);
            }
        }

        public void Repair(int amount)
        {
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }
    }
}