using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour {
    private Rigidbody2D body;
    public float horizontal;
    public float vertical;
    private float moveLimiter = 0.7f;
    public float runSpeed = 5f;

    private Animator animator;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(!GameManager.Instance.GetPlayerBusy()){
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");   
        }
        else{
            horizontal = 0;
            vertical = 0;
        }
        animator.SetFloat("horizontal", horizontal);
        if (horizontal > 0) {
            animator.SetTrigger("Right");
        } else if (horizontal < 0) {
            animator.SetTrigger("Left");
        } else if (vertical < 0) {
            animator.SetTrigger("Down");
        } else if (vertical > 0) {
            animator.SetTrigger("Up");
        } else {
            animator.SetTrigger("Idle");
        }
    }

    void ResetTriggers() {
        animator.ResetTrigger("Right");
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Up");
        animator.ResetTrigger("Down");
    }

    void FixedUpdate() {
        if (horizontal != 0 && vertical != 0) {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
