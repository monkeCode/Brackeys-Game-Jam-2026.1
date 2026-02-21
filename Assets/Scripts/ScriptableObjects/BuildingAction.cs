using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public abstract class BuildingAction : ScriptableObject, IBuildingAction
    {
        [field: SerializeField] public virtual int ActionCooldownTime {get; set;}

        private int _timer = 0;

        public abstract void DoAction(GameObject building);

        public virtual void UpdateTimeTick(GameObject building)
        {
            _timer +=1;
            if(_timer == ActionCooldownTime)
            {
                _timer = 0;
                DoAction(building);
            }
        }

        public abstract void Up();
    }
}