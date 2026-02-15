
namespace Units
{
    public enum UnitType
    {
        Melee,
        Ranged,
    }

    public interface IUnit
    {
        public int Health { get; }
        public int Attack { get; }
        public float Speed { get; }
        public float AttackRange { get; }
        public int Armor { get; }
        public float AttackCooldown { get; }
        public UnitType Type { get;  }

        public void AttackTarget(IDamageable target);

        public void MoveTo(float x, float y);

        public void Die();
    }
}
