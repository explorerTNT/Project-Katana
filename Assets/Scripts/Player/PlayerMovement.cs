using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 6f; // Приватное поле для сериализации
    [SerializeField] private float runSpeed = 10f; // Скорость бега
    [SerializeField] private float jumpForce = 5f; // Сила прыжка
    [SerializeField] private float gravity = -9.81f; // Гравитация
    [SerializeField] private int maxJumps = 2; // Максимальное количество прыжков
    [SerializeField] private float dashSpeed = 20f; // Скорость рывка
    [SerializeField] private float dashDuration = 0.2f; // Длительность рывка
    [SerializeField] private float dashCooldown = 1f; // Кулдаун рывка

    // Публичное свойство для walkSpeed
    public float WalkSpeed
    {
        get => _walkSpeed;
        set => _walkSpeed = value;
    }

    private CharacterController controller;
    private Vector3 velocity; // Вектор скорости для вертикального движения
    private int jumpCount = 0; // Счетчик прыжков
    private float speed; // Текущая скорость
    private float shiftPressTime; // Время нажатия Shift
    private bool isDashing; // Флаг рывка
    private float lastDashTime; // Время последнего рывка
    private InputHandler input; // Ссылка на InputHandler
    private CameraController cameraController; // Ссылка на CameraController

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = FindObjectOfType<InputHandler>();
        cameraController = FindObjectOfType<CameraController>();
        if (input == null)
        {
            Debug.LogError("InputHandler not found in PlayerMovement.");
        }
        if (cameraController == null)
        {
            Debug.LogError("CameraController not found in scene. Player movement will use local forward direction.");
        }
    }

    void Update()
    {
        // Игнорируем ввод, если консоль активна
        DebugConsole console = FindObjectOfType<DebugConsole>();
        if (console != null && console.IsConsoleActive)
        {
            return;
        }

        // Отслеживание нажатия Shift
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            shiftPressTime = Time.unscaledTime;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            float pressDuration = Time.unscaledTime - shiftPressTime;
            if (pressDuration < 0.2f && !isDashing && Time.unscaledTime - lastDashTime > dashCooldown)
            {
                StartCoroutine(Dash());
            }
        }

        // Определяем текущую скорость
        if (isDashing)
        {
            speed = dashSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = _walkSpeed; // Используем приватное поле
        }

        // Горизонтальное движение относительно камеры
        Vector3 move = new Vector3(input.MoveInput.x, 0, input.MoveInput.y).normalized;
        if (cameraController != null && move.magnitude > 0)
        {
            // Получаем направление камеры (игнорируем вертикальный наклон)
            Vector3 cameraForward = cameraController.ForwardDirection;
            cameraForward.y = 0; // Проецируем на плоскость XZ
            cameraForward.Normalize();

            // Вычисляем направление движения относительно камеры
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 moveDirection = (cameraRight * move.x + cameraForward * move.z).normalized;
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
        else
        {
            // Если камера не найдена, используем локальное направление
            controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);
        }

        // Проверка заземления
        if (controller.isGrounded)
        {
            velocity.y = 0f; // Сброс вертикальной скорости
            jumpCount = 0; // Сброс счетчика прыжков
        }

        // Прыжок
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = jumpForce; // Применение силы прыжка
            jumpCount++; // Увеличение счетчика прыжков
        }

        // Применение гравитации
        velocity.y += gravity * Time.deltaTime;

        // Применение вертикального движения
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.unscaledTime;
        float startTime = Time.unscaledTime;

        // Направление рывка — в сторону камеры или вперед игрока
        Vector3 dashDirection = (cameraController != null) ? cameraController.ForwardDirection : transform.forward;
        dashDirection.y = 0; // Игнорируем вертикальный компонент
        dashDirection.Normalize();

        while (Time.unscaledTime < startTime + dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}