using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    void Start()
    {
        GameManager.gameManager.callUI += OnGameEnd;
    }

    public void OnGameEnd(string callName)
    {
        if (callName != this.name) return;
        //change icon here
        transform.localScale = Vector3.one;
        
    }
    
}
