using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemData : ScriptableObject
{
	private int row = 0;
	private int column = 0;
	private int width = 0;
	private int height = 0;
	private string shape = string.Empty;
	private Sprite sprite = null;

	//TODO: ItemType

	public InventoryItemData(int row, int column, int width, int height, string shape, Sprite sprite)
	{
		this.row = row;
		this.column = column;
		this.width = width;
		this.height = height;
		this.shape = shape;
		this.sprite = sprite;
	}

	public int Row { get => row; }
	public int Column { get => column; }
	public int Width { get => width; }
	public int Height { get => height; }
	public Sprite Sprite { get => sprite; }
}
