using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // ������ �� UI Slider
    [SerializeField] private Canvas canvas; // ������ �� Canvas
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (canvas != null)
        {
            canvas.worldCamera = mainCamera; // ������������� ������ ��� Canvas
        }
    }

    void LateUpdate()
    {
        if (canvas != null && mainCamera != null)
        {
            // ������������ HP-��� � ������ (��������������)
            canvas.transform.rotation = mainCamera.transform.rotation;
        }
    }

    public void SetMaxHealth(float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void UpdateHealth(float health)
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }
}