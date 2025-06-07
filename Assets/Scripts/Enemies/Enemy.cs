using UnityEngine;
using System.Collections; // Добавлено для поддержки IEnumerator

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Добавляем здоровье

    void Update()
    {
        // Оставляем пустым, чтобы враг не двигался
    }

    public void ApplyEffect(BulletEffect effect)
    {
        switch (effect)
        {
            case BulletEffect.Lift:
                GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
                break;
            case BulletEffect.Pull:
                StartCoroutine(PullToPlayer());
                break;
            case BulletEffect.Stagger:
                StartCoroutine(Stagger());
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Destroy(gameObject); // Удаление при смерти
    }

    IEnumerator PullToPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
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