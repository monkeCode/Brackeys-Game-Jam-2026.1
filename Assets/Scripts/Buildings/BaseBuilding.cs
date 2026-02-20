using System.Collections.Generic;
using System.Linq;
using Merger;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildings
{
    public class BaseBuilding : MonoBehaviour, IBuilding, IDamageable, IMergable< BaseBuilding>
    {
        [field: SerializeField] public int Health { get; protected set; }

        [field: SerializeField] public int Cost { get; protected set; }

        [field: SerializeField] public Vector2Int Size { get; protected set; }

        public IReadOnlyList<IBuildingAction> Actions { get => actions; }

        [field: SerializeField] public Command Command { get; protected set; }

        [field: SerializeField] public int MaxHealth { get; protected set; }

        [SerializeField] protected List<BuildingAction> actions;

        [field: SerializeField] public SpriteRenderer Lb {get; private set;}
        [field: SerializeField] public SpriteRenderer Rb {get; private set;}
        [field: SerializeField] public SpriteRenderer Rt {get; private set;}
        [field: SerializeField] public SpriteRenderer Lt {get; private set;}

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
        
        private void InitActions()
        {
            actions = actions.Select(x => Instantiate(x)).ToList();

        }

        protected void Start()
        {
            InitActions();
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

        public void SetActions(ICollection<IBuildingAction> actions)
        {
            foreach(var action in this.actions)
            {
                Destroy(action);
            }
            actions.Clear();
            this.actions = actions.Select(x => Instantiate(x as BuildingAction)).ToList();
        }

        public void AddAction(IBuildingAction action)
        {
            actions.Add(Instantiate(action as BuildingAction));
        }

        public void Merge(BaseBuilding first)
        {
            Merge(first, this);
        }
        public void Merge(BaseBuilding first, BaseBuilding second)
        {
            MaxHealth = UnityEngine.Random.Range(math.min(first.MaxHealth, second.MaxHealth), math.max(first.MaxHealth, second.MaxHealth)+1);
            Command = first.Command;

            float mean = (first.Actions.Count + first.Actions.Count) / 2.0f;
            int actionsCount = (int)UnityEngine.Random.Range(math.min(math.floor(mean)-1,1), math.ceil(mean)*2);
            Debug.Log(actionsCount);
            var totalActions =  first.actions.ToList();
            totalActions.AddRange(second.actions);
            List<BuildingAction> newActions = new();

            for(int i = 0; i < actionsCount;i++)
            {
                int index = UnityEngine.Random.Range(0, totalActions.Count);
                newActions.Add(totalActions[index]);
                totalActions.RemoveAt(index);
            }
            actions.Clear();
            actions.AddRange(newActions);

            static T ChooseOneOf<T>(T one, T two)
            {
                if(UnityEngine.Random.value > 0.5)
                    return one;
                return two;
            };

            Lb.sprite = ChooseOneOf(first.Lb.sprite, second.Lb.sprite);
            Rb.sprite = ChooseOneOf(first.Rb.sprite, second.Rb.sprite);
            Rt.sprite = ChooseOneOf(first.Rt.sprite, second.Rt.sprite);
            Lt.sprite = ChooseOneOf(first.Lt.sprite, second.Lt.sprite);
            
        }
    }
}