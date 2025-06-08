using UnityEngine;

public enum BulletEffect { Lift, Pull, Stagger }

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float damage = 20f; // ���� ����
    public BulletEffect effect;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ������� ����
                enemy.ApplyEffect(effect); // ��������� ������
                ComboManager comboManager = FindObjectOfType<ComboManager>();
                if (comboManager != null)
                {
                    comboManager.RegisterHit(); // ������������ �����
                }
            }
            Destroy(gameObject);
        }
    }
}
