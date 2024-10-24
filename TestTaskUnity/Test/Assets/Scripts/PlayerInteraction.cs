using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 5f; // ��������� ��������������
    public LayerMask interactableLayer; // ���� ������������� ��������
    public KeyCode interactKey = KeyCode.E; // ������� ��� ��������������
    public KeyCode dropKey = KeyCode.G; // ������� ��� ������ ��������

    private Camera playerCamera; // ������ ������
    private Interactable currentInteractable; // ������� ������ ��� ��������������
    private Interactable pickedUpItem; // ������, ������� ��� ������ �������

    public Text interactionText; // ������ �� UI ����� ��� ��������������
    private string interactMessage; // ��������� ��� ��������������
    public Transform holdPosition; // ������� ��������� ��������

    void Start()
    {
        // �������������� ������
        playerCamera = Camera.main;
        // �������� ����� �� ������
        interactionText.text = "";
    }

    void Update()
    {
        if (pickedUpItem == null)
        {
            // ���� ����� �� ������ �������, ��������� ������������� ������� ����� ���
            DetectInteractable();

            // ��������� ������� ������� ��������������
            if (Input.GetKeyDown(interactKey))
            {
                InteractWithObject();
            }
        }
        else
        {
            // ���� ������� ������, ��������� ����� � ����������� ��� ������
            interactionText.text = string.Format(interactMessage = "{0} - �������", dropKey);

            // ��������� ������� ��� ������ ��������
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
            // ��������� �������
            PickUpItem(currentInteractable);
        }
    }

    void DetectInteractable()
    {
        // ��� �� ������ ������
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // ���������, �������� �� ��� � ������������� ������
        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                // ��������� ����� � ��������� ������� � ���������� ��� ��������������
                interactionText.text = interactable.itemName + "\n" + string.Format(interactMessage = "{0} - �����", interactKey);
                return;
            }
        }

        // ���� ��� ������� ��� ��������������, �������� �����
        currentInteractable = null;
        interactionText.text = "";
    }

    void PickUpItem(Interactable item)
    {
        // ����������� ������� � ������
        item.transform.SetParent(holdPosition);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // ��������� ������, ����� ������� �� �����
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        pickedUpItem = item;
        Debug.Log("����� ���� " + item.itemName);

        // ������� �����
        interactionText.text = "";
    }

    void DropItem()
    {
        if (pickedUpItem != null)
        {
            // ������� �������� � ���������� ������
            pickedUpItem.transform.SetParent(null);

            Rigidbody rb = pickedUpItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;

                // ��������� ������� � ����������� ������� ������
                //rb.AddForce(playerCamera.transform.forward * 500f); // 500f � ���� ��������, ����� ���������
            }

            Debug.Log("����� �������� " + pickedUpItem.itemName);
            pickedUpItem = null;

            // ������� ����� ����� ������ ��������
            interactionText.text = "";
        }
    }
}
