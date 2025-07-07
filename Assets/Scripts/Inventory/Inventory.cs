using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Rows;
    public int Columns;

    public InventoryTile[,] Tiles;

	private void Awake()
	{
		Tiles = new InventoryTile[Rows, Columns];
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
		for (int x = 0; x < item.Width; x++)
		{
            for (int y = 0; y < item.Height; y++)
            {
                int i = x * item.Width + y;
                string shape = item.Shape.Replace("\n", "").Replace("\r", "");
                if (shape[i] != '0')
                {
                    Tiles[row + x, col + y].Occupied = false;
                }
			}
		}
	}

    public bool PlaceItemOnTile(int row, int col, InventoryItem item)
    {
        if (row + item.Width > Rows) return false;
        if (col + item.Height > Columns) return false;
        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
				int i = x * item.Width + y;
				string shape = item.Shape.Replace("\n", "").Replace("\r", "");
				if (shape[i] != '0')
				{
					if (Tiles[row + x, col + y].Occupied == true) return false;
				}
			}
        }

		for (int x = 0; x < item.Width; x++)
		{
			for (int y = 0; y < item.Height; y++)
			{
				int i = x * item.Width + y;
				string shape = item.Shape.Replace("\n", "").Replace("\r", "");
				if (shape[i] != '0')
				{
                    Tiles[row + x, col + y].Occupied = true;
				}
			}
		}


		return true;
    }
}
