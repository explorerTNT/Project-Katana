using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsAiming { get; private set; }
    public bool ShootPressed { get; private set; }
    public bool MeleePressed { get; private set; }

    void Update()
    {
        // Проверяем, активна ли консоль
        DebugConsole console = FindObjectOfType<DebugConsole>();
        if (console != null && console.IsConsoleActive)
        {
            // Сбрасываем ввод, если консоль открыта
            MoveInput = Vector2.zero;
            IsAiming = false;
            ShootPressed = false;
            MeleePressed = false;
            return;
        }

        // Обычная обработка ввода
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        IsAiming = Input.GetMouseButton(1);
        ShootPressed = Input.GetMouseButtonDown(0);
        MeleePressed = Input.GetKeyDown(KeyCode.E);
    }
}