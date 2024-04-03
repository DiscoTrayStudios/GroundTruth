using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    public float horizontal = 0;
    public float vertical = 0;

    private float moveLimiter = 0.7f;

    public float runSpeed = 5f;

    public bool left = false;
    public bool right = true;
    private Animator animator;

    private AudioSource walking;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        walking = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!GameManager.Instance.GetPlayerBusy()){
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        }
        else{
            walking.Stop();
            horizontal = 0;
            vertical = 0;
        }
        if (horizontal < -0.001 || horizontal > 0.001) {
            animator.SetTrigger("Walking");
        } else if (vertical < -0.001) {
            animator.SetTrigger("Down");
        } else if (vertical > 0.001) {
            animator.SetTrigger("Up");
        } else {
            animator.SetTrigger("Idle");
            animator.ResetTrigger("Walking");
            animator.ResetTrigger("Down");
            animator.ResetTrigger("Up");
            walking.Stop();
        }
        if((!walking.isPlaying) & (horizontal != 0 || vertical != 0)){
            walking.Play();
        }
    }


    void FixedUpdate() {
        if (horizontal != 0 && vertical != 0) {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        if (horizontal > 0) {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            left = false;
            right = true;
        } else if (horizontal < 0) {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            left = true;
            right = false;
        }
    }

    public bool GetLook() {
        return left;
    }
}