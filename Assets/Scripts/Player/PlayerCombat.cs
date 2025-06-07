using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float shootCooldown = 0.5f;

    private InputHandler input;
    private float lastShotTime;

    void Start()
    {
        input = FindObjectOfType<InputHandler>();
    }

    void Update()
    {
        if (input.IsAiming && input.ShootPressed && Time.time - lastShotTime > shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }

        if (input.MeleePressed)
            Melee();
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        TimeManager.Instance.SlowTime(0.3f, 1f);
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
                    FindObjectOfType<ComboManager>().RegisterHit();
                }
            }
        }
    }
}