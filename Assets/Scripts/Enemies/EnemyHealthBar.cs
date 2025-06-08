using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // Ссылка на UI Slider
    [SerializeField] private Canvas canvas; // Ссылка на Canvas
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (canvas != null)
        {
            canvas.worldCamera = mainCamera; // Устанавливаем камеру для Canvas
        }
    }

    void LateUpdate()
    {
        if (canvas != null && mainCamera != null)
        {
            // Поворачиваем HP-бар к камере (оптимизировано)
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