using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public int Rows;
    public int Columns;

    public InventoryTile[,] Tiles;

	List<InventoryItemData> itemsData;

	private void Awake()
	{
		Tiles = new InventoryTile[Rows, Columns];
		itemsData = new List<InventoryItemData>();
	}

	// Start is called before the first frame update
	void Start()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                int i = row * Columns + col;
                GameObject child = transform.GetChild(i).gameObject;
                if (child != null)
                {
                    InventoryTile tile = child.GetComponent<InventoryTile>();
                    if (tile != null)
                    {
                        tile.Row = row;
                        tile.Column = col;
                        Tiles[row, col] = tile;
                        tile.Occupied = false;
                    }
                }
            }
        }
    }

    public void TakeItem(int row, int col, InventoryItem item)
    {
		for (int x = 0; x < item.Height; x++)
		{
            for (int y = 0; y < item.Width; y++)
            {
                int i = x * item.Width + y;
                string shape = item.Shape.Replace("\n", "").Replace("\r", "");
                if (shape[i] != '0')
                {
                    Tiles[row + x, col + y].Occupied = false;
                    Tiles[row + x, col + y].GetComponent<Image>().color = Color.white;

				}
			}
		}
	}

    public bool CheckIfCanPlace(int row, int col, InventoryItem item)
    {
		if (row + item.Height > Rows) return false;
		if (col + item.Width > Columns) return false;
		for (int x = 0; x < item.Height; x++)
		{
			for (int y = 0; y < item.Width; y++)
			{
				int i = x * item.Width + y;
				string shape = item.Shape.Replace("\n", "").Replace("\r", "");
				if (shape[i] != '0')
				{
					if (Tiles[row + x, col + y].Occupied == true) return false;
				}
			}
		}

		for (int x = 0; x < item.Height; x++)
		{
			for (int y = 0; y < item.Width; y++)
			{
				int i = x * item.Width + y;
				string shape = item.Shape.Replace("\n", "").Replace("\r", "");
				if (shape[i] != '0')
				{
					Tiles[row + x, col + y].GetComponent<Image>().color = Color.blue;
				}
			}
		}
		return true;
	}

    public bool PlaceItemOnTile(int row, int col, InventoryItem item)
    {
        if (row + item.Height > Rows) return false;
        if (col + item.Width > Columns) return false;
        for (int x = 0; x < item.Height; x++)
        {
            for (int y = 0; y < item.Width; y++)
            {
				int i = x * item.Width + y;
				string shape = item.Shape.Replace("\n", "").Replace("\r", "");
				if (shape[i] != '0')
				{
					if (Tiles[row + x, col + y].Occupied == true) return false;
				}
			}
        }

		for (int x = 0; x < item.Height; x++)
		{
			for (int y = 0; y < item.Width; y++)
			{
				int i = x * item.Width + y;
				string shape = item.Shape.Replace("\n", "").Replace("\r", "");
				if (shape[i] != '0')
				{
                    Tiles[row + x, col + y].Occupied = true;
					Tiles[row + x, col + y].GetComponent<Image>().color = Color.red;
				}
			}
		}


		return true;
    }
}
