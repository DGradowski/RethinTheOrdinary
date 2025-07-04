using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Values")]
	public float MovementSpeed;

	[Header("Player Game Objects")]
	[SerializeField] private Animator animator;

	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	private void FixedUpdate()
	{
		if (InputManager.MoveVector != Vector2.zero)
		{
			float x = InputManager.MoveVector.x * MovementSpeed * Time.fixedDeltaTime;
			float y = InputManager.MoveVector.y * MovementSpeed * Time.fixedDeltaTime;
			rb.velocity = new Vector2(x, y);
		}
		else
		{
			rb.velocity = Vector2.zero;
		}
	}
}
