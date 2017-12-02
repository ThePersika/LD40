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
    private bool canAirJump;

    private float extraWeight;
    [Header("UI")]
    [SerializeField]
    private TextMesh weightText;

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
            {
                velocity.y = 0;
                canAirJump = true;
            }
            velocity.y -= gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                StopAllCoroutines();
                StartCoroutine(TryJump());
            }

            cont.Move(velocity * Time.deltaTime);
        }

        weightText.text = "Size: " + extraWeight * 100 + "%";
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddWeight();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void AddWeight()
    {
        extraWeight += 0.05f;
        transform.Translate(Vector2.up * 0.025f);
        transform.localScale = Vector3.one + Vector3.one * extraWeight;
        cont.CalculateRaySpacing();
    }

    private void OnCollisionH()
    {
        Die();
    }

    private IEnumerator TryJump()
    {
        for (float i = 0; i < 0.05f; i += Time.deltaTime)
        {
            if (cont.collInfo.bottom || canAirJump)
            {
                velocity.y = jumpVelocity;
                cont.collInfo.bottom = false;
                canAirJump = false;
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
