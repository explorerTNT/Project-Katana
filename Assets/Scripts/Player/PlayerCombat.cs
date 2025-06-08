using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float shootCooldown = 0.5f;

    private InputHandler input;
    private float lastShotTime;
    private ComboManager comboManager;

    void Start()
    {
        input = FindObjectOfType<InputHandler>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    void Update()
    {
        if (input.IsAiming && input.ShootPressed && Time.time - lastShotTime > shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }

        if (input.MeleePressed)
        {
            Melee();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.SlowTime(0.3f, 1f);
        }
        else
        {
            Debug.LogWarning("TimeManager.Instance is null");
        }
    }

    void Melee()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(30f); // Урон от ближней атаки
                    if (comboManager != null)
                    {
                        comboManager.RegisterHit(); // Регистрируем комбо
                        Debug.Log("Melee hit, combo increased");
                    }
                }
            }
        }
    }
}