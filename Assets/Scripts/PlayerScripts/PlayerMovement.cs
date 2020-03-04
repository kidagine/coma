﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private Animator _animator = default;
	private readonly float _minimumToAngleValue = 1.0f;
	private readonly float _maximumToAngleValue = 0.75f;
	private readonly float _moveSpeed = 2.5f;
	private Vector2 _movementDirection;
	private float footstepCooldown;
	private float currentFootstepSpeed = 0.3f;
	public Vector2 MovementInput { private get; set; }


	void Update()
    {
		MovementDirection();
		Footsteps();
	}

	void FixedUpdate()
	{
		Movement();
	}

	private void MovementDirection()
	{
		_animator.SetFloat("Vertical", _movementDirection.y);
		_animator.SetFloat("Horizontal", _movementDirection.x);
		if (MovementInput != Vector2.zero)
		{
			if (Mathf.Abs(MovementInput.x) <= _maximumToAngleValue)
			{
				if (MovementInput.x > 0.0f)
				{
					if (MovementInput.y > 0.0f)
					{
						_movementDirection.x = 0.75f;
						_movementDirection.y = 0.75f;
					}
					else
					{
						_movementDirection.x = 0.75f;
						_movementDirection.y = -0.75f;
					}
				}
				else
				{
					if (MovementInput.y > 0.0f)
					{
						_movementDirection.x = -0.75f;
						_movementDirection.y = 0.75f;
					}
					else
					{
						_movementDirection.x = -0.75f;
						_movementDirection.y = -0.75f;
					}
				}
			}
			if (Mathf.Abs(MovementInput.y) != 1.0f)
			{
				if (Mathf.Abs(MovementInput.x) <= _minimumToAngleValue && Mathf.Abs(MovementInput.x) >= _maximumToAngleValue)
				{
					if (MovementInput.x > 0.0f)
					{
						_movementDirection.x = 1.0f;
						_movementDirection.y = 0.0f;
					}
					else
					{
						_movementDirection.x = -1.0f;
						_movementDirection.y = 0.0f;
					}
				}
			}	
			if (Mathf.Abs(MovementInput.x) != 1.0f)
			{
				if (Mathf.Abs(MovementInput.y) <= _minimumToAngleValue && Mathf.Abs(MovementInput.y) >= _maximumToAngleValue)
				{
					if (MovementInput.y > 0.0f)
					{
						_movementDirection.y = 1.0f;
						_movementDirection.x = 0.0f;
					}
					else
					{
						_movementDirection.y = -1.0f;
						_movementDirection.x = 0.0f;
					}
				}
			}
		}
		else
		{
			_movementDirection = Vector2.zero;
		}
	}

	private void Movement()
	{
		_rigidbody.MovePosition(_rigidbody.position + _movementDirection * _moveSpeed * Time.fixedDeltaTime);
	}

	private void Footsteps()
	{
		if (MovementInput.magnitude > 0.0f)
		{
			footstepCooldown -= Time.deltaTime;
			if (footstepCooldown <= 0)
			{
				AudioManager.Instance.PlayRandomFromSoundGroup("PlayerFootsteps");
				footstepCooldown = currentFootstepSpeed;
			}
		}
	}
}
