using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public float moveSpeed = 10f;
    public float RotationSpeed = 5f;
    Rigidbody body;
    Animator animator;
    Vector3 movement;

    void Awake() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        move(h,v);
        turn(h,v);
    }

    void move(float h, float v){
        
        movement.Set(h, 0, v);
        if(h==0 && v==0){
            animator.SetBool("IsMoving", false);
        }
        else{
            animator.SetBool("IsMoving", true);
        }
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        body.MovePosition(transform.position + movement);
    }
    void turn(float h, float v){
        if(h==0 && v==0){
            return;
        }
        Quaternion Rotation = Quaternion.LookRotation(movement);
        body.rotation = Quaternion.Slerp(body.rotation, Rotation, RotationSpeed*Time.deltaTime);
    }
}
