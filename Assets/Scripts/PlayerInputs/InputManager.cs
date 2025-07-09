using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public static PlayerInput PlayerInput;

	public static Vector2 MoveVector;

	public static bool UseWasPressed;

	public static bool RotateWasPressed;

	private InputAction moveAction;
	private InputAction useAction;
	private InputAction rotateAction;

	private void Awake()
	{
		PlayerInput = GetComponent<PlayerInput>();

		moveAction = PlayerInput.actions["Move"];
		useAction = PlayerInput.actions["Use"];
		rotateAction = PlayerInput.actions["Rotate"];
	}

	// Update is called once per frame
	void Update()
	{
		MoveVector = moveAction.ReadValue<Vector2>();

		UseWasPressed = useAction.WasPerformedThisFrame();
		RotateWasPressed = rotateAction.WasPerformedThisFrame();
	}
}
