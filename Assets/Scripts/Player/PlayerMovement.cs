using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Настраиваемые параметры
    public float walkSpeed = 6f;      // Скорость ходьбы
    public float runSpeed = 10f;      // Скорость бега
    public float jumpForce = 5f;      // Сила прыжка
    public float gravity = -9.81f;    // Гравитация
    public int maxJumps = 2;          // Максимальное количество прыжков (2 для двойного прыжка)
    public float dashSpeed = 20f;     // Скорость рывка
    public float dashDuration = 0.2f; // Длительность рывка
    public float dashCooldown = 1f;   // Кулдаун рывка

    // Внутренние переменные
    private CharacterController controller;
    private Vector3 velocity;         // Вектор скорости для вертикального движения
    private int jumpCount = 0;        // Счетчик прыжков
    private float speed;              // Текущая скорость
    private float shiftPressTime;     // Время нажатия Shift
    private bool isDashing = false;   // Флаг рывка
    private float lastDashTime;       // Время последнего рывка
    private InputHandler input;        // Ссылка на InputHandler

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = FindObjectOfType<InputHandler>();
    }

    void Update()
    {
        // Игнорировать ввод, если консоль активна
        DebugConsole console = FindObjectOfType<DebugConsole>();
        if (console != null && console.IsConsoleActive)
        {
            return;
        }

        // Отслеживание нажатия Shift
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            shiftPressTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            float pressDuration = Time.time - shiftPressTime;
            if (pressDuration < 0.2f && !isDashing && Time.time - lastDashTime > dashCooldown)
            {
                StartCoroutine(Dash());
            }
        }

        // Определение текущей скорости
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
            speed = walkSpeed;
        }

        // Горизонтальное движение
        Vector3 move = new Vector3(input.MoveInput.x, 0, input.MoveInput.y).normalized;
        controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);

        // Проверка заземления
        if (controller.isGrounded)
        {
            velocity.y = 0f;         // Сброс вертикальной скорости
            jumpCount = 0;           // Сброс счетчика прыжков
        }

        // Прыжок
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = jumpForce;  // Применение силы прыжка
            jumpCount++;             // Увеличение счетчика прыжков
        }

        // Применение гравитации
        velocity.y += gravity * Time.deltaTime;

        // Применение вертикального движения
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            Vector3 dashDirection = transform.forward; // Направление рывка вперед
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}