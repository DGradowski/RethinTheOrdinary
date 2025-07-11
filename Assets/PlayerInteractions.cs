using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollableRect;
    public List<IInteractable> nearbyItems = new List<IInteractable>();

    private IInteractable currentInteractable;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("E key pressed for interaction");

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PickupableItem>(out PickupableItem nearbyItem))
        {
            nearbyItems.Add(nearbyItem);
            UpdateNearbyItemsUI();
        }
        else if (other.TryGetComponent<IInteractable>(out IInteractable interactableItem))
        {
            //show interact button
            currentInteractable = interactableItem;
        }
        else
        {
            Debug.Log("Player entered interaction with non-pickupable item");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PickupableItem>(out PickupableItem nearbyItem))
        {
            nearbyItems.Remove(nearbyItem);
            Sprite sprite = nearbyItem.GetComponent<SpriteRenderer>().sprite;
            UpdateNearbyItemsUI();
        }
    }

    void UpdateNearbyItemsUI()
    {
        for (int i = 0; i < scrollableRect.content.childCount; i++)
        {
            Destroy(scrollableRect.content.GetChild(i).gameObject);
        }
        
        foreach (PickupableItem item in nearbyItems)
        {
            AddItemToUI(item);
        }
    }

    void AddItemToUI(PickupableItem item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;

        GameObject UIBlock = Instantiate(item.uiItemPrefab, scrollableRect.content);
        UIBlock.transform.Find("Sprite").GetComponent<UnityEngine.UI.Image>().sprite = sprite;

        Transform grid = UIBlock.transform.Find("Grid");
        List<Transform> cells = new List<Transform>();
        foreach (Transform child in grid)
        {
            cells.Add(child);
        }

        string[] itemShape = item.GetShape();
        for (int y = 0; y < itemShape.Length; y++)
        {
            var row = itemShape[y];
            for (int x = 0; x < row.Length; x++)
            {
                int i = y * row.Length + x;
                if (row[x] == '#')
                {
                    cells[i].GetComponent<Image>().color = Color.red;
                }
            }
        }

        GameObject interactButton = UIBlock.transform.Find("InteractButton").gameObject;
        interactButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            TakeItem(item);
            
        });
    }
    
    public void TakeItem(PickupableItem item)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject inventoryItem = Instantiate(item.inventoryItemPrefab, canvas.transform);
        GameObject inventory = GameObject.Find("Inventory");

        InventoryItem realItem = inventoryItem.GetComponent<InventoryItem>();
        //realItem.Shape = item.ShapeString;
        //realItem.width = item.width;
        //realItem.height = item.height;
        //inventory.GetComponent<Inventory>().PlaceItemOnTile(1, 1, realItem); // tu zmiana jeszcze - to chwilowe to [1,1,]

        item.DestroySelf();
    }
}
