
using System;
using UnityEngine;

namespace Buildings
{
    public interface IBuilding
    {
        public int Health { get; }

        public int UnitProductionTime { get; }

        public int Cost { get; }

        public Vector2Int Size { get; }

        public Units.IUnit Unit { get; }

        public void Repair(int amount);

        public void SpawnUnit()
        {

        }

        public void Destroy();

        public void Merge(IBuilding other);

        public void setTransparentColor(Boolean available);

        public void setNormalColor();
    }
}