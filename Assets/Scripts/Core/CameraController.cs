using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Ссылка на трансформ игрока
    public Vector3 normalOffset = new Vector3(0, 2, -5); // Обычное положение камеры
    public Vector3 aimOffset = new Vector3(0.5f, 1.5f, -1); // Положение в режиме прицеливания
    public float transitionSpeed = 10f; // Скорость перехода камеры
    public float mouseSensitivity = 100f; // Чувствительность мыши
    public float verticalRotationLimit = 80f; // Ограничение вертикального вращения

    private float yaw = 0f;  // Горизонтальный угол
    private float pitch = 0f; // Вертикальный угол
    private bool isAiming;
    private InputHandler input;

    void Start()
    {
        input = FindObjectOfType<InputHandler>();
        Cursor.lockState = CursorLockMode.Locked; // Блокировка курсора
    }

    void LateUpdate()
    {
        isAiming = input.IsAiming; // Проверка, удерживает ли игрок ПКМ

        // Обновление углов на основе ввода мыши
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);

        // Вычисление целевой позиции камеры
        Vector3 offset = isAiming ? aimOffset : normalOffset;
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        Vector3 targetPosition = player.position + rotation * offset;

        // Плавный переход к целевой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);

        // Установка ориентации камеры
        if (isAiming)
        {
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
        else
        {
            transform.LookAt(player.position);
        }
    }
}