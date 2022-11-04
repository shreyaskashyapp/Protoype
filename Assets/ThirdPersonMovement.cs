using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6;

    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;


    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    private Rigidbody rg;
    private Animator animator;
     [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask layer;

    void Start(){
        animator=GetComponent<Animator>();
        rg=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //jump
       // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // if (isGrounded && velocity.y < 0)
        // {
        //     velocity.y = -2f;
        // }

       
        //gravity
        // velocity.y += gravity * Time.deltaTime;
        // controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            animator.SetBool("isMoving",true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rg.velocity=new Vector3(horizontal*speed,rg.velocity.y,vertical*speed);
        
        }

        else{
            animator.SetBool("isMoving",false);
        }

         if(Input.GetButtonDown("Jump")&& GroundCheck() ){
            Debug.Log("jump");
            animator.SetBool("isJumping",true);
            rg.velocity=new Vector3(0,5f,0);
        }
        else{
            animator.SetBool("isJumping",false);
        }
    }
     bool GroundCheck(){
        Debug.Log(Physics.CheckSphere(groundCheck.position,0.5f,layer));
        return Physics.CheckSphere(groundCheck.position,0.5f,layer);
    }
}