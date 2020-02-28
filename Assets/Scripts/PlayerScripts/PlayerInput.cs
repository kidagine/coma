using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] private PlayerMovement _playerMovement;
	private PlayerInputActions _playerInputActions;


	private void Awake()
	{
		_playerInputActions = new PlayerInputActions();
		_playerInputActions.PlayerControls.Movement.performed += SetMove;
	}

	private void SetMove(InputAction.CallbackContext context)
	{
		_playerMovement.MovementInput = context.ReadValue<Vector2>();
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
