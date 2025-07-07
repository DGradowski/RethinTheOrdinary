using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField, Range(1, 4)] private int width = 1;
	[SerializeField, Range(1, 4)] private int height = 1;

	[TextArea(4, 4)] public string Shape = "####\n####\n####\n####";

	[SerializeField] private GraphicRaycaster raycaster;

	[SerializeField] private EventSystem eventSystem;

	public int Width { get => width; }
	public int Height { get => height; }

	[SerializeField] private InventoryTile currentTile;

	private Vector2 offsetVector = Vector2.zero;

	private void Start()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();

		float x = rectTransform.rect.width / (Width * 2);
		x *= (Width - 1);
		float y = rectTransform.rect.height / (Height * 2);
		y *= (Height - 1);

		offsetVector = new Vector2(-1 * x, y);	
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		GetComponent<Image>().raycastTarget = false;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Vector2 vectorCheck = (Vector2)transform.position + offsetVector;

		if (currentTile != null)
		{
			currentTile.Take(this);
		}

		GameObject uiUnderCursor = GetUIObjectAtScreenPoint(vectorCheck);

		if (uiUnderCursor != null)
		{
			InventoryTile tile;
			if (tile = uiUnderCursor.GetComponent<InventoryTile>())
			{
				if (tile.Place(this))
				{
					currentTile = tile;
				}
			}
		}
		currentTile.Place(this);
		MoveItem(currentTile);
		GetComponent<Image>().raycastTarget = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		float x = eventData.position.x;
		float y = eventData.position.y;

		transform.position = new Vector3(x, y, 0);
	}

	public void MoveItem(InventoryTile tile)
	{
		currentTile = tile;
		transform.position = (Vector2)tile.transform.position - offsetVector;
	}

	private GameObject GetUIObjectAtScreenPoint(Vector2 screenPoint)
	{
		PointerEventData pointerEventData = new PointerEventData(eventSystem)
		{
			position = screenPoint
		};

		List<RaycastResult> results = new List<RaycastResult>();
		raycaster.Raycast(pointerEventData, results);

		if (results.Count > 0)
		{
			return results[0].gameObject;
		}

		return null;
	}
}
