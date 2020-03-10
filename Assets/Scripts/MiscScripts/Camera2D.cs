using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    private CameraBorder _cameraBorder;
    private float _speed = 2.5f;
    private float _smoothedPositionX;
    private float _smoothedPositionY;
    private bool _isFollowingPlayer;


    void Update()
    {
        transform.position +=  Vector3.right * Time.deltaTime * 3f;
    }

    public void SetFollowPlayer(bool isFollowingPlayer, CameraBorder cameraBorder)
    {
        _smoothedPositionX = transform.position.x;
        _smoothedPositionY = transform.position.y;
        _cameraBorder = cameraBorder;
        _isFollowingPlayer = isFollowingPlayer;
    }
}
