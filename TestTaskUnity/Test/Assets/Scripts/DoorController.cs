using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // ������ �� ������ �����
    public float openAngle = 90f; // ���� �������� �����
    public float moveDistance = 3f; // ����������, �� ������� ����� ������� ����� �� ��� Z
    public float speed = 2f; // �������� �������� � �������� �����

    private Quaternion initialRotation; // ��������� ������� �����
    private Quaternion openRotation; // ������� ����� ��� ��������
    private Vector3 initialPosition; // ��������� ������� �����
    private Vector3 openPosition; // ������� ����� ��� ��������
    private bool isOpening = false; // ���� ��� �������� �����

    void Start()
    {
        // ��������� ��������� ������� � ������� �����
        initialRotation = door.rotation;
        initialPosition = door.position;

        // ��������� �������� ������� (������� �� 90 �������� �� ��� X) � ������� ����� ��� �������� (������ ����� �� ��� Z)
        openRotation = Quaternion.Euler(initialRotation.eulerAngles.x - openAngle, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);
        openPosition = initialPosition + new Vector3(0, 0, -moveDistance);
    }

    void OnTriggerEnter(Collider other)
    {
        // ���� ����� ������ � �������, �������� ��������� �����
        if (other.CompareTag("Player"))
        {
            isOpening = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ���� ����� ������� �� ��������, �������� ��������� �����
        if (other.CompareTag("Player"))
        {
            isOpening = false;
        }
    }

    void Update()
    {
        // ��������� �����: ������������ ������������ � ������� ����� �� ��� Z
        if (isOpening)
        {
            door.rotation = Quaternion.Slerp(door.rotation, openRotation, Time.deltaTime * speed);
            door.position = Vector3.Lerp(door.position, openPosition, Time.deltaTime * speed);
        }
        // ��������� �����: ���������� ������� � ������� � �������� ���������
        else
        {
            door.rotation = Quaternion.Slerp(door.rotation, initialRotation, Time.deltaTime * speed);
            door.position = Vector3.Lerp(door.position, initialPosition, Time.deltaTime * speed);
        }
    }
}
