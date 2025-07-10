using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTile : MonoBehaviour
{
	public int Row = 0;
	public int Column = 0;

	public bool Occupied = false;

	[SerializeField] private Inventory inventory;

	public Inventory GetInventory()
	{
		return inventory;
	}

	public void Take(InventoryItem item)
	{
		inventory.TakeItem(Row, Column, item);
	}

	public bool Place(InventoryItem item)
	{
		return inventory.PlaceItemOnTile(Row, Column, item);
	}

	public bool CheckIfCanPlace(InventoryItem item)
	{
		return inventory.PlaceItemOnTile(Row, Column, item);
	}

	public void DrawOnTile(InventoryItem item)
	{
		inventory.StartDrawingTile(Row, Column, item);
	}
}
