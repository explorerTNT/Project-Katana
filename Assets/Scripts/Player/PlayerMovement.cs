using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // ������������� ���������
    public float walkSpeed = 6f;      // �������� ������
    public float runSpeed = 10f;      // �������� ����
    public float jumpForce = 5f;      // ���� ������
    public float gravity = -9.81f;    // ����������
    public int maxJumps = 2;          // ������������ ���������� ������� (2 ��� �������� ������)
    public float dashSpeed = 20f;     // �������� �����
    public float dashDuration = 0.2f; // ������������ �����
    public float dashCooldown = 1f;   // ������� �����

    // ���������� ����������
    private CharacterController controller;
    private Vector3 velocity;         // ������ �������� ��� ������������� ��������
    private int jumpCount = 0;        // ������� �������
    private float speed;              // ������� ��������
    private float shiftPressTime;     // ����� ������� Shift
    private bool isDashing = false;   // ���� �����
    private float lastDashTime;       // ����� ���������� �����
    private InputHandler input;        // ������ �� InputHandler

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = FindObjectOfType<InputHandler>();
    }

    void Update()
    {
        // ������������ ����, ���� ������� �������
        DebugConsole console = FindObjectOfType<DebugConsole>();
        if (console != null && console.IsConsoleActive)
        {
            return;
        }

        // ������������ ������� Shift
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

        // ����������� ������� ��������
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

        // �������������� ��������
        Vector3 move = new Vector3(input.MoveInput.x, 0, input.MoveInput.y).normalized;
        controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);

        // �������� ����������
        if (controller.isGrounded)
        {
            velocity.y = 0f;         // ����� ������������ ��������
            jumpCount = 0;           // ����� �������� �������
        }

        // ������
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = jumpForce;  // ���������� ���� ������
            jumpCount++;             // ���������� �������� �������
        }

        // ���������� ����������
        velocity.y += gravity * Time.deltaTime;

        // ���������� ������������� ��������
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            Vector3 dashDirection = transform.forward; // ����������� ����� ������
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}