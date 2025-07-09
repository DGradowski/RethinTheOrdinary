using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "InventoryItems/InventoryItemData", order = 1)]
public class InventoryItemData : ScriptableObject
{
	[SerializeField, Range(1, 4)] private int width = 1;
	[SerializeField, Range(1, 4)] private int height = 1;
	[SerializeField, TextArea(4,4)] private string shape = string.Empty;
	[SerializeField] private Sprite sprite = null;

	//TODO: ItemType

	public InventoryItemData(int width, int height, string shape, Sprite sprite)
	{
		this.width = width;
		this.height = height;
		this.shape = shape;
		this.sprite = sprite;
	}

	public int Width { get => width; }
	public int Height { get => height; }
	public Sprite Sprite { get => sprite; }
}
