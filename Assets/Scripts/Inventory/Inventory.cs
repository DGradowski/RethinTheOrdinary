using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public int Rows;
    public int Columns;

    public InventoryTile[,] Tiles;

	public InventoryItemData[,] TilesData;

	/* DRAWING TILES */
	private InventoryItem drawnItem;
	private (int row, int col) drawnTilePos;

	private void Awake()
	{
		Tiles = new InventoryTile[Rows, Columns];
		TilesData = new InventoryItemData[Rows, Columns];
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

	private void Update()
	{
		ClearTilesColor();
		DrawSelectedTiles();
		drawnItem = null;
		DrawOccupiedTiles();
	}

	public void TakeItem(int row, int col, InventoryItem item)
    {
		int width = item.Width;
		int height = item.Height;
		string shape = item.Shape.Replace("\n", "").Replace("\r", "");

		TilesData[row, col] = null;
		for (int x = 0; x < height; x++)
		{
            for (int y = 0; y < width; y++)
            {
                int i = x * width + y;
                if (shape[i] != '0')
                {
                    Tiles[row + x, col + y].Occupied = false;
                    Tiles[row + x, col + y].GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

    public bool CanPlace(int row, int col, InventoryItem item)
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
		return true;
	}

    public bool PlaceItemOnTile(int row, int col, InventoryItem item)
    {
        if (!CanPlace(row, col, item)) return false;
		int width = item.Width;
		int height = item.Height;
		string shape = item.Shape.Replace("\n", "").Replace("\r", "");

		InventoryItemData data = ScriptableObject.CreateInstance<InventoryItemData>();
		data.Set(width, height, item.Shape, item.Sprite);
		TilesData[row, col] = data;
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				int i = x * width + y;
				if (shape[i] != '0')
				{
                    Tiles[row + x, col + y].Occupied = true;
				}
			}
		}
		item.Place(Tiles[row, col]);
		return true;
    }

	public void ClearTilesColor()
	{
		for (int row = 0; row < Rows; row++)
		{
			for (int col = 0; col < Columns; col++)
			{
				Tiles[row, col].GetComponent<Image>().color = Color.white;
			}
		}
	}

	public void DrawSelectedTiles()
	{
		if (drawnItem == null) return;
		int width = drawnItem.Width;
		int height = drawnItem.Height;
		string shape = drawnItem.Shape.Replace("\n", "").Replace("\r", "");
		Color color;
		if (CanPlace(drawnTilePos.row, drawnTilePos.col, drawnItem))
		{
			color = Color.green;
		}
		else
		{
			color = Color.yellow;
		}
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				int i = x * width + y;
				if (drawnTilePos.col + y > Columns - 1) continue;
				if (drawnTilePos.row + x > Rows - 1) continue;
				if (shape[i] != '0')
				{
					Tiles[drawnTilePos.row + x, drawnTilePos.col + y].GetComponent<Image>().color = color;
				}
			}
		}

	}

	public void DrawOccupiedTiles()
	{
		for (int row = 0; row < Rows; row++)
		{
			for (int col = 0; col < Columns; col++)
			{
				if (Tiles[row, col].Occupied == true)
				{
					Tiles[row, col].GetComponent<Image>().color = Color.red;
				}
			}
		}
	}

	public void StartDrawingTile(int row, int col, InventoryItem item)
	{
		drawnItem = item;
		drawnTilePos = (row, col);
	}
}
