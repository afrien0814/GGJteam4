using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Rigidbody2D get_velocity;
    Animator anim;

    public Chaser chase_info;

    private void Start()
    {
        get_velocity = this.transform.parent.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat("velocity", get_velocity.velocity.magnitude);
        //anim.SetTrigger("skill",true);
        anim.SetBool("find_target", chase_info.chasing);
        anim.SetBool("chased", chase_info.isChased);
    }
}
