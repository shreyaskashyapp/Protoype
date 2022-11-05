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
  public float gravity = -9.81f;


  float turnSmoothVelocity;
  public float turnSmoothTime = 0.1f;

  private Rigidbody rg;
  private Animator animator;

  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask groundMask;

  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      Debug.Log("working");
      velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
      velocity.y += gravity * Time.deltaTime;
      controller.Move(velocity * Time.deltaTime);
    }
    //gravity
    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);


    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    if (direction.magnitude >= 0.1f)
    {
      animator.SetBool("isMoving", true);
      float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      //rg.velocity=new Vector3(horizontal*speed,rg.velocity.y,vertical*speed);
      controller.Move(moveDir * speed * Time.deltaTime);
    }

    else
    {
      animator.SetBool("isMoving", false);
    }
  }
}