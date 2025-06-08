using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Здоровье
    public float maxHealth = 100f; // Максимальное здоровье
    // Убрана зависимость от EnemyHealthBar, так как HP-бар не нужен

    void Start()
    {
        health = maxHealth; // Инициализация здоровья
    }

    void Update()
    {
        // Враг стоит на месте
    }

    public void ApplyEffect(BulletEffect effect)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            switch (effect)
            {
                case BulletEffect.Lift:
                    rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                    break;
                case BulletEffect.Pull:
                    StartCoroutine(PullToPlayer());
                    break;
                case BulletEffect.Stagger:
                    StartCoroutine(Stagger());
                    break;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Enemy hit, health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.SpawnAtPosition(transform.position); // Спавн нового врага
        }
        Destroy(gameObject); // Удаление текущего врага
    }

    IEnumerator PullToPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;
        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 20 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Stagger()
    {
        yield return new WaitForSeconds(1f); // Простая пауза
    }
}