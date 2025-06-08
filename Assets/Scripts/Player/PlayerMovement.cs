using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 6f; // ��������� ���� ��� ������������
    [SerializeField] private float runSpeed = 10f; // �������� ����
    [SerializeField] private float jumpForce = 5f; // ���� ������
    [SerializeField] private float gravity = -9.81f; // ����������
    [SerializeField] private int maxJumps = 2; // ������������ ���������� �������
    [SerializeField] private float dashSpeed = 20f; // �������� �����
    [SerializeField] private float dashDuration = 0.2f; // ������������ �����
    [SerializeField] private float dashCooldown = 1f; // ������� �����

    // ��������� �������� ��� walkSpeed
    public float WalkSpeed
    {
        get => _walkSpeed;
        set => _walkSpeed = value;
    }

    private CharacterController controller;
    private Vector3 velocity; // ������ �������� ��� ������������� ��������
    private int jumpCount = 0; // ������� �������
    private float speed; // ������� ��������
    private float shiftPressTime; // ����� ������� Shift
    private bool isDashing; // ���� �����
    private float lastDashTime; // ����� ���������� �����
    private InputHandler input; // ������ �� InputHandler
    private CameraController cameraController; // ������ �� CameraController

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
        // ���������� ����, ���� ������� �������
        DebugConsole console = FindObjectOfType<DebugConsole>();
        if (console != null && console.IsConsoleActive)
        {
            return;
        }

        // ������������ ������� Shift
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

        // ���������� ������� ��������
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
            speed = _walkSpeed; // ���������� ��������� ����
        }

        // �������������� �������� ������������ ������
        Vector3 move = new Vector3(input.MoveInput.x, 0, input.MoveInput.y).normalized;
        if (cameraController != null && move.magnitude > 0)
        {
            // �������� ����������� ������ (���������� ������������ ������)
            Vector3 cameraForward = cameraController.ForwardDirection;
            cameraForward.y = 0; // ���������� �� ��������� XZ
            cameraForward.Normalize();

            // ��������� ����������� �������� ������������ ������
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 moveDirection = (cameraRight * move.x + cameraForward * move.z).normalized;
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
        else
        {
            // ���� ������ �� �������, ���������� ��������� �����������
            controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);
        }

        // �������� ����������
        if (controller.isGrounded)
        {
            velocity.y = 0f; // ����� ������������ ��������
            jumpCount = 0; // ����� �������� �������
        }

        // ������
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = jumpForce; // ���������� ���� ������
            jumpCount++; // ���������� �������� �������
        }

        // ���������� ����������
        velocity.y += gravity * Time.deltaTime;

        // ���������� ������������� ��������
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.unscaledTime;
        float startTime = Time.unscaledTime;

        // ����������� ����� � � ������� ������ ��� ������ ������
        Vector3 dashDirection = (cameraController != null) ? cameraController.ForwardDirection : transform.forward;
        dashDirection.y = 0; // ���������� ������������ ���������
        dashDirection.Normalize();

        while (Time.unscaledTime < startTime + dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}