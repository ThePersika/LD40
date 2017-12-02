﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpHeight, jumpTime;
    private float jumpVelocity, gravity;
    private Vector2 velocity;

    private Controller cont;

    private void Start()
    {
        cont = GetComponent<Controller>();

        // d = 0.5 * a * t * t   <=>   a = d / (0.5 * t * t)
        gravity = jumpHeight / (0.5f * Mathf.Pow(jumpTime, 2));
        // v = a * t
        jumpVelocity = gravity * jumpTime;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        velocity.x = (GameManager.gameState == GameManager.GameState.Playing) ? moveSpeed : 0;

        if (GameManager.gameState == GameManager.GameState.Playing)
        {
            if (cont.collInfo.bottom || cont.collInfo.top)
                velocity.y = 0;
            velocity.y -= gravity * Time.deltaTime;
            if (cont.collInfo.bottom && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
                velocity.y = jumpVelocity;

            cont.Move(velocity * Time.deltaTime);
        }
    }
}