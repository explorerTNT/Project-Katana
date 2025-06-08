using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Ссылка на трансформ игрока
    [SerializeField] private Vector3 normalOffset = new Vector3(0, 5, -7); // Обычное положение камеры
    [SerializeField] private Vector3 aimOffset = new Vector3(0.5f, 4f, 3); // Положение в режиме прицеливания
    [SerializeField] private float transitionSpeed = 10f; // Скорость перехода камеры
    [SerializeField] private float mouseSensitivity = 100f; // Чувствительность мыши
    [SerializeField] private float verticalRotationLimit = 60f; // Ограничение вертикального вращения

    private float yaw; // Горизонтальный угол
    private float pitch; // Вертикальный угол
    private bool isAiming;
    private InputHandler input;

    // Публичное свойство для доступа к направлению камеры
    public Vector3 ForwardDirection => transform.forward;

    void Start()
    {
        input = FindObjectOfType<InputHandler>();
        if (input == null)
        {
            Debug.LogError("InputHandler not found in scene. Please ensure an active object with InputHandler component exists.");
        }

        if (player == null)
        {
            Debug.LogError("Player Transform not assigned in CameraController. Please assign the player's Transform in the Inspector.");
        }

        Cursor.lockState = CursorLockMode.Locked; // Блокировка курсора
    }

    void LateUpdate()
    {
        if (input == null || player == null)
        {
            return; // Прерываем, чтобы избежать ошибок
        }

        isAiming = input.IsAiming; // Проверка, удерживает ли ПКМ

        // Обновление углов на основе ввода мыши
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);

        // Поворачиваем игрока по горизонтали в соответствии с yaw
        player.rotation = Quaternion.Euler(0, yaw, 0);

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
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }
}