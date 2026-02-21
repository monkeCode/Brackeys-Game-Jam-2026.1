using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Merger;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public class BaseBuilding : MonoBehaviour, IBuilding, IDamageable, IMergable< BaseBuilding>, IPointerClickHandler
    {
        [field: SerializeField] public int Health { get; protected set; }

        [field: SerializeField] public int Cost { get; protected set; }

        [field: SerializeField] public Vector2Int Size { get; protected set; }

        public IReadOnlyList<IBuildingAction> Actions { get => actions; }

        [field: SerializeField] public Command Command { get; protected set; }

        [field: SerializeField] public int MaxHealth { get; protected set; }

        [SerializeField] protected List<BuildingAction> actions;

        public int Lvl {get; protected set;}

        public int UpPrice => (int)Math.Ceiling(Lvl*0.5 + Cost);

        [field: SerializeField] public SpriteRenderer Lb {get; private set;}
        [field: SerializeField] public SpriteRenderer Rb {get; private set;}
        [field: SerializeField] public SpriteRenderer Rt {get; private set;}
        [field: SerializeField] public SpriteRenderer Lt {get; private set;}


        [ContextMenu("Destroy")]
        public void Destroy()
        {
            if(Command == Command.Player)
            {
                ResourcesManager.Instance.DeletePlayerBuilding(this);
            }
            else if(Command == Command.Enemy)
            {
                ResourcesManager.Instance.DeleteEnemyBuilding(this);
            }
            Destroy(gameObject);
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

            if(Command == Command.Player)
            {
                ResourcesManager.Instance.AddPlayerBuilding(this);
            }
            else if (Command == Command.Enemy)
            {
                ResourcesManager.Instance.AddPlayerBuilding(this);
            }
        }

        protected void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
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
            Health = math.min((int)((first.Health + second.Health) / 2.0f), MaxHealth);
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

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<b>Health</b>: {Health}/{MaxHealth}");
            stringBuilder.AppendLine($"<b>Up price</b>: {UpPrice}");
            foreach (var action in actions)
            {
                stringBuilder.AppendLine(action.ToString());
            }
            return stringBuilder.ToString();
        }

        public void Up()
        {
            Lvl++;
            MaxHealth += (int)(MaxHealth * 1.05f);
            Repair(MaxHealth);
            Debug.Log($"Up to {Lvl}");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            transform.DOScale(new Vector2(1.1f,1.1f), 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() => transform.localScale = Vector3.one);
            if(Command == Command.Player)
            {
                Debug.Log("click");
                UiManager.Instance.ShowBuildingUi(this);
            }
        }
    }
}