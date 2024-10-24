using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // Ссылка на объект двери
    public float openAngle = 90f; // Угол открытия двери
    public float moveDistance = 3f; // Расстояние, на которое дверь отъедет назад по оси Z
    public float speed = 2f; // Скорость открытия и закрытия двери

    private Quaternion initialRotation; // Начальная ротация двери
    private Quaternion openRotation; // Ротация двери при открытии
    private Vector3 initialPosition; // Начальная позиция двери
    private Vector3 openPosition; // Позиция двери при открытии
    private bool isOpening = false; // Флаг для открытия двери

    void Start()
    {
        // Сохранить начальную ротацию и позицию двери
        initialRotation = door.rotation;
        initialPosition = door.position;

        // Вычислить конечную ротацию (поворот на 90 градусов по оси X) и позицию двери при открытии (отъезд назад по оси Z)
        openRotation = Quaternion.Euler(initialRotation.eulerAngles.x - openAngle, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);
        openPosition = initialPosition + new Vector3(0, 0, -moveDistance);
    }

    void OnTriggerEnter(Collider other)
    {
        // Если игрок входит в триггер, начинаем открывать дверь
        if (other.CompareTag("Player"))
        {
            isOpening = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Если игрок выходит из триггера, начинаем закрывать дверь
        if (other.CompareTag("Player"))
        {
            isOpening = false;
        }
    }

    void Update()
    {
        // Открываем дверь: одновременно поворачиваем и смещаем назад по оси Z
        if (isOpening)
        {
            door.rotation = Quaternion.Slerp(door.rotation, openRotation, Time.deltaTime * speed);
            door.position = Vector3.Lerp(door.position, openPosition, Time.deltaTime * speed);
        }
        // Закрываем дверь: возвращаем ротацию и позицию в исходное состояние
        else
        {
            door.rotation = Quaternion.Slerp(door.rotation, initialRotation, Time.deltaTime * speed);
            door.position = Vector3.Lerp(door.position, initialPosition, Time.deltaTime * speed);
        }
    }
}
