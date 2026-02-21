using Units;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected float speed;

    protected MonoBehaviour target;
    protected int damage;

    private Vector3 dir;

    public virtual void FindAndDestroy(IDamageable target, int damage)
    {
        if (target is not MonoBehaviour mb) return;

        this.target = mb;
        this.damage = damage;
    }
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }

        dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        transform.position += speed * Time.deltaTime * dir;
        if(Vector2.Distance(transform.position, target.transform.position) < 0.1)
        {
            if (target is IDamageable damageableTarget)
            {
                damageableTarget.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
