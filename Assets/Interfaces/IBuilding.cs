
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{

    public interface IBuildingAction
    {
        public int ActionCooldownTime { get; }
        public void DoAction(GameObject building);

        public void UpdateTimeTick(GameObject building);
    }    

    public interface IBuilding
    {
        public int Health { get; }

        public int Cost { get; }

        public Vector2Int Size { get; }

        public IReadOnlyList<IBuildingAction> Actions {get; }

        public void Repair(int amount);

        public void UpdateTimeTick();

        public void SetActions(ICollection<IBuildingAction> actions);

        public void AddAction(IBuildingAction action);

        public void Destroy();

    }
}