using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] protected int health;
    protected int maxHealth;

    protected virtual void Start()
    {
        maxHealth = health;
    }

    public virtual void TakeHit(int damage)
    {
        if (damage <= 0) return;
        health -= damage;

        if (health <= 0) Death();
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
