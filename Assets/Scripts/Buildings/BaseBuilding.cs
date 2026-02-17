using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BaseBuilding : MonoBehaviour, IBuilding, IDamageable
    {
        [field: SerializeField] public int Health { get; protected set; }

        [field: SerializeField] public int Cost { get; protected set; }

        [field: SerializeField] public Vector2Int Size { get; protected set; }

        public IReadOnlyList<IBuildingAction> Actions { get => actions; }

        [field: SerializeField] public Command Command { get; protected set; }

        [field: SerializeField] public int MaxHealth { get; protected set; }

        [SerializeField] protected List<BuildingAction> actions;


        private SpriteRenderer sp;
        public SpriteRenderer Sprite { get => sp = sp != null ? sp : GetComponent<SpriteRenderer>(); }

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

        void OnEnable()
        {
            Timer.Instance.onTimerUpdate += UpdateTimeTick;
        }
        void OnDisable()
        {
            Timer.Instance.onTimerUpdate -= UpdateTimeTick;
        }

        private void OnDrawGizmos()
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    if ((x + y) % 2 == 0) Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                    else Gizmos.color = new Color(1f, 0f, 0f, 0.3f);

                    Gizmos.DrawCube(transform.position + new Vector3(x, y, 0), new Vector3(1, 1, .1f));
                }
            }
        }

        // private void OnDrawGizmos()
        // {
        //     int xd, yd;
        //     for (int x = 0; x < Size.x; x++)
        //     {
        //         if (x % 2 != 0) xd = (x + 1) / 2; else xd = -x / 2;
        //         for (int y = 0; y < Size.y; y++)
        //         {
        //             if (y % 2 != 0) yd = (y + 1) / 2; else yd = -y / 2;
        //             if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
        //             else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);
        //             Gizmos.DrawCube(transform.position + new Vector3(xd, yd, 0), new Vector3(1, 1, .1f));
        //         }
        //     }
        // }
    }
}