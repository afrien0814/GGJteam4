using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Rigidbody2D get_velocity;
    Animator anim;

    public Move skill_info;
    public Chaser chase_info;

    Item.item_type former_item;

    private void Start()
    {
        get_velocity = this.transform.parent.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        //idle or walk
        anim.SetFloat("velocity", get_velocity.velocity.magnitude);

        //

        //skill
        if(former_item!=Item.item_type.nothing 
            && skill_info.item_holding == Item.item_type.nothing)
        {
            anim.SetTrigger("skill");
        }
        former_item = skill_info.item_holding;

        if(skill_info.jumping)
        {
            StartCoroutine(Jump());
        }

        //chasing
        anim.SetBool("find_target", chase_info.chasing);
        anim.SetBool("chased", chase_info.isChased);
    }

    IEnumerator Jump()
    {
        for(int i=0; i<10; i++)
        {
            this.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return null;
        }
        for(int i=0; i<10; i++)
        {
            this.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return null;
        }
        yield break;
    }
}
