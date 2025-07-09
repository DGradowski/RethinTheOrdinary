using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickupableItem : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject uiItemPrefab;

    void IInteractable.Interact()
    {
        Debug.Log("Interaction with item");

    }

    void IInteractable.Show()
    {
        
    }

}
