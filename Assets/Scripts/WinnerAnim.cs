using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerAnim : MonoBehaviour
{
    Animator anim;
    bool tmp;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if(tmp && !GameManager.gameManager.gaming)
        {
            anim.Play(GameManager.gameManager.winnerId.ToString()); 
        }
        tmp = GameManager.gameManager.gaming;
    }

}
