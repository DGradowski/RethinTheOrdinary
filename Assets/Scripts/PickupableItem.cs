using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickupableItem : MonoBehaviour, IInteractable
{
    [SerializeField] public GameObject uiItemPrefab;
    [TextArea(4, 4)] public string ShapeString = "##..\n##..\n....\n....";
    
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
}
