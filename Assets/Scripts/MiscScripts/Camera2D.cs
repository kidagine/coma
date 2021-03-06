﻿using System.Collections;
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
        if (_isFollowingPlayer)
        {
            transform.position += Vector3.right * Time.deltaTime * 3f;
            switch (_cameraBorder)
            {
                case CameraBorder.TopBorder:
                    _smoothedPositionY += Time.deltaTime * _speed;
                    transform.position = new Vector3(transform.position.x, _smoothedPositionY, transform.position.z);
                    break;
                case CameraBorder.BottomBorder:
                    _smoothedPositionY -= Time.deltaTime * _speed;
                    transform.position = new Vector3(transform.position.x, _smoothedPositionY, transform.position.z);
                    break;
                case CameraBorder.RightBorder:
                    _smoothedPositionX -= Time.deltaTime * _speed;
                    transform.position = new Vector3(_smoothedPositionX, transform.position.y, transform.position.z);
                    break;
                case CameraBorder.LeftBorder:
                    _smoothedPositionX += Time.deltaTime * _speed;
                    transform.position = new Vector3(_smoothedPositionX, transform.position.y, transform.position.z);
                    break;
            }
        }
    }

    public void SetFollowPlayer(bool isFollowingPlayer, CameraBorder cameraBorder)
    {
        _smoothedPositionX = transform.position.x;
        _smoothedPositionY = transform.position.y;
        _cameraBorder = cameraBorder;
        _isFollowingPlayer = isFollowingPlayer;
    }
}
