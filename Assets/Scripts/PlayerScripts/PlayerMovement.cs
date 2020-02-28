using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private Animator _animator = default;
	private Vector2 _direction;
	private Vector2 _lastDirection;
	private readonly int _moveSpeed = 3;

	public Vector2 MovementInput { private get; set; }


	void Update()
    {
		if (_direction.y != 1.0f && _direction.y != -1.0f)
		{
			_direction.x = Mathf.RoundToInt(MovementInput.x);
			if (_direction.x != 0)
			{
				_animator.SetFloat("Vertical", 0.0f);
				_animator.SetFloat("Horizontal", _lastDirection.x);
				_lastDirection.y = 0.0f;
				_lastDirection.x = _direction.x;
			}
		}
		if (_direction.x != 1.0f && _direction.x != -1.0f)
		{
			_direction.y = Mathf.RoundToInt(MovementInput.y);
			if (_direction.y != 0.0f)
			{
				_animator.SetFloat("Horizontal", 0.0f);
				_animator.SetFloat("Vertical", _lastDirection.y);
				_lastDirection.x = 0.0f;
				_lastDirection.y = _direction.y;
			}
		}

		if (_direction == Vector2.zero)
		{
			_animator.speed = 0;
		}
		else
		{
			_animator.speed = 1;
		}
	}

	void FixedUpdate()
	{
		Movement();
	}

	private void Movement()
	{
		_rigidbody.MovePosition(_rigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
	}
}
