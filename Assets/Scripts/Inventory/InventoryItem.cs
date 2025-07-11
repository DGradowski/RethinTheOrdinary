using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.WSA;
using static UnityEditor.Progress;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler
{
	[SerializeField, Range(1, 4)] private int width = 1;
	[SerializeField, Range(1, 4)] private int height = 1;

	[TextArea(4, 4)] public string Shape = "####\n####\n####\n####";
	private string originalShape;

	[SerializeField] private GraphicRaycaster raycaster;

	[SerializeField] private EventSystem eventSystem;

	[SerializeField] private InventoryTile currentTile;

	private Vector2 offsetVector = Vector2.zero;

	private RectTransform rectTransform;
	[SerializeField] RectTransform spriteTransform;
	[SerializeField] GameObject hitboxPrefab;

	private string lastShape;
	private float lastRotation;
	private int lastWidth;
	private int lastHeight;

	private bool isHeld = false;

	public int Width { get => width; set => width = value; }
	public int Height { get => height;  set => height = value; }

	public Sprite Sprite { get => spriteTransform.GetComponent<Image>().sprite; }

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		lastShape = Shape;
		lastRotation = spriteTransform.eulerAngles.z;
		lastWidth = Width;
		lastHeight = Height;
		originalShape = Shape;
		for (int i = 0; i < Width * Height; i++)
		{
			Instantiate(hitboxPrefab, spriteTransform.transform);
		}
	}

	private void Start()
	{
		if (raycaster == null)
		{
			raycaster = transform.GetComponentInParent<GraphicRaycaster>();
		}
		float x = rectTransform.rect.width / (Width * 2);
		x *= (Width - 1);
		float y = rectTransform.rect.height / (Height * 2);
		y *= (Height - 1);
		spriteTransform.pivot = new Vector2(0, 1);
		offsetVector = new Vector2(-1 * x, y);
	}

	private void Update()
	{
		if (isHeld)
		{
			if (InputManager.RotateWasPressed)
			{
				(width, height) = (height, width);
				Shape = RotateString(Shape);
				spriteTransform.Rotate(0, 0, -90);
				UpdateImagePivot();
			}
			Vector2 vectorCheck = (Vector2)transform.position - offsetVector;
			InventoryTile tile = GetUIObjectAtScreenPoint(vectorCheck);
			if (tile != null)
			{
				tile.DrawOnTile(this);
			}
		}
		
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		UpdateImageHitbox(true);
		GetComponent<Image>().raycastTarget = false;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		if (currentTile != null)
		{
			currentTile.Take(this);
		}
		isHeld = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Vector2 vectorCheck = (Vector2)transform.position - offsetVector;

		InventoryTile tile = GetUIObjectAtScreenPoint(vectorCheck);

		if (tile != null)
		{
			if (tile.CheckIfCanPlace(this))
			{
				currentTile = tile;
				lastShape = Shape;
				lastRotation = spriteTransform.eulerAngles.z;
				lastWidth = Width;
				lastHeight = Height;
				tile.Place(this);
			}
		}
		GetComponent<Image>().raycastTarget = true;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		UpdateImageHitbox(false);
		isHeld = false;
	}

	public void Place(InventoryTile tile)
	{
		if (tile != null)
		{
			currentTile = tile;
			UpdateImagePivot();
			currentTile.Place(this);
			MoveItem(currentTile);
			rectTransform.pivot = new Vector2(0, 1);
			transform.SetParent(currentTile.transform);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			spriteTransform.anchoredPosition = Vector2.zero;
			spriteTransform.SetParent(transform);
		}
		GetComponent<Image>().raycastTarget = true;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		UpdateImageHitbox(false);
	}

	public void OnDrag(PointerEventData eventData)
	{
		float x = eventData.position.x - 50;
		float y = eventData.position.y + 50;

		transform.position = new Vector2(x, y);

		Vector2 vectorCheck = (Vector2)transform.position;

		InventoryTile tile = GetUIObjectAtScreenPoint(vectorCheck);


	}

	public void MoveItem(InventoryTile tile)
	{
		RectTransform thisRect = GetComponent<RectTransform>();
		RectTransform tileRect = tile.GetComponent<RectTransform>();
		RectTransform invRect = tile.GetInventory().GetComponent<RectTransform>();

		// Ustaw lokalnie wzglêdem rodzica
		thisRect.anchoredPosition = tileRect.anchoredPosition - offsetVector + invRect.anchoredPosition;
	}

	private InventoryTile GetUIObjectAtScreenPoint(Vector2 screenPoint)
	{
		PointerEventData pointerEventData = new PointerEventData(eventSystem)
		{
			position = screenPoint
		};

		List<RaycastResult> results = new List<RaycastResult>();
		raycaster.Raycast(pointerEventData, results);

		if (results.Count > 0)
		{
			foreach (RaycastResult result in results)
			{
				InventoryTile tile;
				if (tile = result.gameObject.GetComponent<InventoryTile>())
				{
					return tile;
				}
			}
		}

		return null;
	}

	private void UpdateImagePivot()
	{
		Vector2 pivot = new Vector2(0, 1);
		switch (Mathf.Round(spriteTransform.eulerAngles.z))
		{
			case 0:
				pivot = new Vector2(0, 1);
				break;
			case 90:
				pivot = new Vector2(1, 1);
				break;
			case 180:
				pivot = new Vector2(1, 0);
				break;
			case 270:
				pivot = new Vector2(0, 0);
				break;
		}
		spriteTransform.pivot = pivot;
	}

	private void UpdateImageHitbox(bool enable)
	{
		string s = originalShape.Replace("\n", "").Replace("\r", "");
		for (int i = 0; i < Width * Height; i++)
		{
			if (s[i] == '0')
			{
				spriteTransform.transform.GetChild(i).GetComponent<Image>().raycastTarget = enable;
				spriteTransform.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
			}
			else
			{
				spriteTransform.transform.GetChild(i).GetComponent<Image>().raycastTarget = true;
				spriteTransform.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
			}
			
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		
	}

	private string RotateString(string text)
	{
		var lines = text.Split('\n');
		int rows = lines.Length;
		int cols = lines.Max(line => line.Length);

		// Wyrównaj d³ugoœci wierszy spacjami
		for (int i = 0; i < rows; i++)
		{
			lines[i] = lines[i].PadRight(cols);
		}

		char[,] grid = new char[rows, cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				grid[i, j] = lines[i][j];
			}
		}

		// Budujemy wynik
		string[] rotatedLines = new string[cols];
		for (int i = 0; i < cols; i++)
		{
			char[] newRow = new char[rows];
			for (int j = 0; j < rows; j++)
			{
				newRow[j] = grid[rows - j - 1, i];
			}
			rotatedLines[i] = new string(newRow);
		}

		return string.Join("\n", rotatedLines);
	}
}
