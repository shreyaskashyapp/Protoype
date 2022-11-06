using System.Collections;


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class movement : MonoBehaviour
{
  public CharacterController controller;
  public Transform cam;

  public float speed = 6;
  public float gravity = -9.81f;
  public float jumpHeight = 3;
  Vector3 velocity;
  bool isGrounded;

  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask groundMask;

  float turnSmoothVelocity;
  public float turnSmoothTime = 0.1f;
  private Animator animator;
  private bool isAttackPressed;
  private bool isAttacking;
  public float attackDelay = 2f;
  public Camera cam1;
  private Vector3 destination;
  public Transform fpoint;
  public GameObject projectile;
  public float ProjectileSpeed = 30;

  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    //jump
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    // if (isGrounded && velocity.y < 0)
    // {
    //     velocity.y = -2f;
    // }

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
      animator.SetBool("isJumping", true);
    }
    else
    {
      animator.SetBool("isJumping", false);
    }
    //gravity
    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
    //walk
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    if (direction.magnitude >= 0.1f)
    {
      float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      animator.SetBool("isMoving", true);
      if (!isAttacking)
      {
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
      }
    }
    else
    {
      animator.SetBool("isMoving", false);

    }

    if (Input.GetKeyDown(KeyCode.RightControl))
    {
      Debug.Log("fire");
      isAttackPressed = true;
      Invoke("fire", 1f);
    }

    if (isAttackPressed)
    {
      isAttackPressed = false;

      if (!isAttacking)
      {
        isAttacking = true;

        animator.Play("fire");

        Invoke("AttackComplete", attackDelay);
      }
    }
  }

  void FixedUpdate()
  {

  }
  void AttackComplete()
  {
    Debug.Log("ATTACK COMPLETE");
    animator.Play("idle");
    isAttacking = false;
  }

  void fire()
  {
    Ray ray = cam1.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
      destination = hit.point;
    }
    else
    {
      destination = ray.GetPoint(1000);
    }
    if(!isAttacking)
    InstantiateProjectile(fpoint);
  }

  void InstantiateProjectile(Transform firepoint)
  {
    var ProjectileObj = Instantiate(projectile, firepoint.position, Quaternion.identity) as GameObject;
    ProjectileObj.GetComponent<Rigidbody>().velocity = (destination - firepoint.position).normalized * ProjectileSpeed;
  }
}