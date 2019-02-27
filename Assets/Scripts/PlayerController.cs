using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int STATE_IDLE = 0;
    const int STATE_WALK = 1;
    const int STATE_RUN = 2;
    const int STATE_JUMP = 3;

    public float speedX;
    public float jumpSpeedY;

    private bool facingRight;
    private bool jumping;
    private float speed;

    private Rigidbody2D rigidBody;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        facingRight = true;
    }

    void Update()
    {
        MovePlayer(speed);
        Flip();
        //left player movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RunLeft();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Stop();
        }
        //right player movement
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RunRight();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Stop();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Barrier")
        {
            jumping = false;
            animator.SetInteger("State", STATE_IDLE);
        }
    }

    void Flip()
    {
        if (speed > 0 && !facingRight || speed < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 tmp = transform.localScale;
            tmp.x = -tmp.x;
            transform.localScale = tmp;
        }
    }
    private void MovePlayer(float playerSpeed)
    {
        if (playerSpeed < 0 && !jumping || playerSpeed > 0 && !jumping)
        {
            animator.SetInteger("State", STATE_RUN);
        }
        if (playerSpeed == 0 && !jumping)
        {
            animator.SetInteger("State", STATE_IDLE);
        }
        rigidBody.velocity = new Vector3(playerSpeed, rigidBody.velocity.y, 0);

    }

    public void RunLeft()
    {
        speed = -speedX;
    }

    public void Stop()
    {
        speed = 0;
    }

    public void RunRight()
    {
        speed = speedX;
    }

    public void Jump()
    {
        jumping = true;
        rigidBody.AddForce(new Vector2(rigidBody.velocity.x, jumpSpeedY));
        animator.SetInteger("State", STATE_JUMP);
    }
}
