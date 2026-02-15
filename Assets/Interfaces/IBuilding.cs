
namespace Buildings
{
    public interface IBuilding
    {
        public int Health { get; }

        public float UnitProductionTime { get; }

        public int Cost { get; }

        public Units.IUnit Unit { get; }

        public void TakeDamage(int damage);

        public void Repair(int amount);

        public void SpawnUnit();

        public void Destroy();

        public void Merge(IBuilding other);
    }
}