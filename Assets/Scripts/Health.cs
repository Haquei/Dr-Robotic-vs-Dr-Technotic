using UnityEngine;
using UnityEngine.Events;

// Quick and simple health component.
public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    public UnityEvent onDied;

    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0) Die();
    }

    public void Die()
    {
        onDied.Invoke();
    }

    public void AddHealth(float amount)
    {
        // Don't do any checks just add it.
        currentHealth += amount;
    }
}
