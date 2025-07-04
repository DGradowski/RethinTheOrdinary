using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public static PlayerInput PlayerInput;

	public static Vector2 MoveVector;

	public static bool UseWasPressed;

	private InputAction moveAction;
	private InputAction useAction;

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();

		moveAction = PlayerInput.actions["Move"];

		useAction = PlayerInput.actions["Use"];
	}

	// Update is called once per frame
	void Update()
	{
		MoveVector = moveAction.ReadValue<Vector2>();

		UseWasPressed = useAction.WasPerformedThisFrame();
	}
}
