using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ������ �� ��������� ������
    [SerializeField] private Vector3 normalOffset = new Vector3(0, 5, -7); // ������� ��������� ������
    [SerializeField] private Vector3 aimOffset = new Vector3(0.5f, 4f, 3); // ��������� � ������ ������������
    [SerializeField] private float transitionSpeed = 10f; // �������� �������� ������
    [SerializeField] private float mouseSensitivity = 100f; // ���������������� ����
    [SerializeField] private float verticalRotationLimit = 60f; // ����������� ������������� ��������

    private float yaw; // �������������� ����
    private float pitch; // ������������ ����
    private bool isAiming;
    private InputHandler input;

    // ��������� �������� ��� ������� � ����������� ������
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

        Cursor.lockState = CursorLockMode.Locked; // ���������� �������
    }

    void LateUpdate()
    {
        if (input == null || player == null)
        {
            return; // ���������, ����� �������� ������
        }

        isAiming = input.IsAiming; // ��������, ���������� �� ���

        // ���������� ����� �� ������ ����� ����
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);

        // ������������ ������ �� ����������� � ������������ � yaw
        player.rotation = Quaternion.Euler(0, yaw, 0);

        // ���������� ������� ������� ������
        Vector3 offset = isAiming ? aimOffset : normalOffset;
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);
        Vector3 targetPosition = player.position + rotation * offset;

        // ������� ������� � ������� �������
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);

        // ��������� ���������� ������
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