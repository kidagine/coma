﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private PlayerMovement _playerMovement;
	private PlayerInputActions _playerInputActions;


	private void Awake()
	{
		_playerInputActions = new PlayerInputActions();
		_playerInputActions.PlayerControls.Movement.performed += SetMove;
		_playerInputActions.PlayerControls.Attack.performed += Attack;
		_playerInputActions.PlayerControls.Interact.performed += PickUp;
		_playerInputActions.PlayerControls.Throw.performed += Throw;
		_playerInputActions.PlayerControls.Interact.performed += Interact;
	}

	private void SetMove(InputAction.CallbackContext context)
	{
		_playerMovement.MovementInput = context.ReadValue<Vector2>();
	}

	private void Attack(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_player.Attack();
		}
	}

	private void PickUp(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_player.PickUp();
		}
	}

	private void Throw(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_player.Throw();
		}
	}

	private void Interact(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			_player.Interact();
		}
	}


	private void OnEnable()
	{
		_playerInputActions.Enable();
	}

	private void OnDisable()
	{
		_playerInputActions.Disable();
	}
}
