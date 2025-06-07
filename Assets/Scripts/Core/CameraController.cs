using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // ������ �� ��������� ������
    public Vector3 normalOffset = new Vector3(0, 2, -5); // ������� ��������� ������
    public Vector3 aimOffset = new Vector3(0.5f, 1.5f, -1); // ��������� � ������ ������������
    public float transitionSpeed = 10f; // �������� �������� ������
    public float mouseSensitivity = 100f; // ���������������� ����
    public float verticalRotationLimit = 80f; // ����������� ������������� ��������

    private float yaw = 0f;  // �������������� ����
    private float pitch = 0f; // ������������ ����
    private bool isAiming;
    private InputHandler input;

    void Start()
    {
        input = FindObjectOfType<InputHandler>();
        Cursor.lockState = CursorLockMode.Locked; // ���������� �������
    }

    void LateUpdate()
    {
        isAiming = input.IsAiming; // ��������, ���������� �� ����� ���

        // ���������� ����� �� ������ ����� ����
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);

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
            transform.LookAt(player.position);
        }
    }
}