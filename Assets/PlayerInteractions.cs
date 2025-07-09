using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollableRect;
    public List<IInteractable> nearbyItems = new List<IInteractable>();

    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("E key pressed for interaction");
            UpdateNearbyItemsUI();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PickupableItem>(out PickupableItem nearbyItem))
        {
            nearbyItems.Add(nearbyItem);
            UpdateNearbyItemsUI();
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
            Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;

            GameObject UIBlock = Instantiate(item.uiItemPrefab, scrollableRect.content);
            UIBlock.transform.Find("Sprite").GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        }
        
    }
    
    public void TakeItem()
    {
        Debug.Log("Taking item");
    }
}
