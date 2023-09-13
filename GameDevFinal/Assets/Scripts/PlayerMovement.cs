using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour {
    private Rigidbody2D body;
    public float horizontal;
    public float vertical;
    private float moveLimiter = 0.7f;
    public float runSpeed = 5f;

    void Start() {
        body = GetComponent<Rigidbody2D>();
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
        
    }

    void FixedUpdate() {
        if (horizontal != 0 && vertical != 0) {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
