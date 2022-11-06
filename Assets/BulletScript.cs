using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private bool collided=false;
    void OnCollisionEnter(Collision co) {
        if(co.gameObject.tag !="Bullet" && co.gameObject.tag !="Player" &&!collided){
            collided=true;
            Destroy(gameObject);
        }
    }
}
