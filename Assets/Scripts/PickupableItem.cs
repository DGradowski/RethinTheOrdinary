using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickupableItem : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject uiItemPrefab;
    [SerializeField] public GameObject inventoryItemPrefab;
    
    [TextArea(4, 4)] public string ShapeString = "##..\n##..\n....\n....";
    [SerializeField, Range(1, 4)] public int width = 2;
    [SerializeField, Range(1, 4)] public int height = 2;

    void IInteractable.Interact()
    {
        Debug.Log("Interaction with item");

    }

    void IInteractable.Show()
    {
        
    }

    public string[] GetShape()
    {
        string[] shape = ShapeString.Split('\n');
        for (int i = 0; i < shape.Length; i++)
        {
            shape[i] = shape[i].Replace("\r", "");
        }
        return shape;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
