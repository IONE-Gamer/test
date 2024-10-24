using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 5f; // Дистанция взаимодействия
    public LayerMask interactableLayer; // Слой интерактивных объектов
    public KeyCode interactKey = KeyCode.E; // Клавиша для взаимодействия
    public KeyCode dropKey = KeyCode.G; // Клавиша для сброса предмета

    private Camera playerCamera; // Камера игрока
    private Interactable currentInteractable; // Текущий объект для взаимодействия
    private Interactable pickedUpItem; // Объект, который был поднят игроком

    public Text interactionText; // Ссылка на UI текст для взаимодействия
    private string interactMessage; // Сообщение для взаимодействия
    public Transform holdPosition; // Позиция удержания предмета

    void Start()
    {
        // Инициализируем камеру
        playerCamera = Camera.main;
        // Скрываем текст на старте
        interactionText.text = "";
    }

    void Update()
    {
        if (pickedUpItem == null)
        {
            // Если игрок не держит предмет, проверяем интерактивные объекты перед ним
            DetectInteractable();

            // Проверяем нажатие клавиши взаимодействия
            if (Input.GetKeyDown(interactKey))
            {
                InteractWithObject();
            }
        }
        else
        {
            // Если предмет поднят, обновляем текст с информацией для сброса
            interactionText.text = string.Format(interactMessage = "{0} - бросить", dropKey);

            // Проверяем клавишу для сброса предмета
            if (Input.GetKeyDown(dropKey))
            {
                DropItem();
            }
        }
    }

    void InteractWithObject()
    {
        if (currentInteractable != null)
        {
            // Поднимаем предмет
            PickUpItem(currentInteractable);
        }
    }

    void DetectInteractable()
    {
        // Луч из центра экрана
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Проверяем, попадает ли луч в интерактивный объект
        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                // Обновляем текст с названием объекта и подсказкой для взаимодействия
                interactionText.text = interactable.itemName + "\n" + string.Format(interactMessage = "{0} - взять", interactKey);
                return;
            }
        }

        // Если нет объекта для взаимодействия, скрываем текст
        currentInteractable = null;
        interactionText.text = "";
    }

    void PickUpItem(Interactable item)
    {
        // Привязываем предмет к игроку
        item.transform.SetParent(holdPosition);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // Отключаем физику, чтобы предмет не падал
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        pickedUpItem = item;
        Debug.Log("Игрок взял " + item.itemName);

        // Очищаем текст
        interactionText.text = "";
    }

    void DropItem()
    {
        if (pickedUpItem != null)
        {
            // Убираем родителя и возвращаем физику
            pickedUpItem.transform.SetParent(null);

            Rigidbody rb = pickedUpItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;

                // Добавляем импульс в направлении взгляда игрока
                //rb.AddForce(playerCamera.transform.forward * 500f); // 500f — сила импульса, можно настроить
            }

            Debug.Log("Игрок отпустил " + pickedUpItem.itemName);
            pickedUpItem = null;

            // Очищаем текст после сброса предмета
            interactionText.text = "";
        }
    }
}
