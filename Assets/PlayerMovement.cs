using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rg;
    [SerializeField]float speed=5f;
    [SerializeField]float jump=10f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        rg=GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal=Input.GetAxis("Horizontal");
        float Vertical=Input.GetAxis("Vertical");
        rg.velocity=new Vector3(Horizontal*speed,rg.velocity.y,Vertical*speed);

        if(Input.GetButtonDown("Jump")&& GroundCheck() ){
            Debug.Log("jump");
            rg.velocity=new Vector3(0,jump,0);
        }
    }
    bool GroundCheck(){
        Debug.Log(Physics.CheckSphere(groundCheck.position,0.5f,layer));
        return Physics.CheckSphere(groundCheck.position,0.5f,layer);
    }
}