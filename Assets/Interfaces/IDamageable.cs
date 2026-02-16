

public enum Command
{
    Player,
    Enemy
}

public interface IDamageable 
{
    public int Health { get; }

    public Command Command {get; }

    public void TakeDamage(int damage);
}