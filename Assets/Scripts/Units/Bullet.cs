using Units;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected float speed;

    protected BaseUnit target;
    protected int damage;

    private Vector3 dir;

    public virtual void FindAndDestroy(BaseUnit target, int damage)
    {
        this.target = target;
        this.damage = damage;

        dir = (target.transform.position - transform.position).normalized;
        transform.Rotate(new Vector3(0,0, Vector2.Angle(Vector2.right, dir)));
    }
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    protected virtual void Update()
    {
        transform.position += speed * Time.deltaTime * dir;
        if(Vector2.Distance(transform.position, target.transform.position) < 0.1)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
