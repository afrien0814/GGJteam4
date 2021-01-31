using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    void Start()
    {
        GameManager.gameManager.callUI += OnGameEnd;
        transform.localScale = Vector2.zero;
    }

    public void OnGameEnd(string callName)
    {
        if (callName != this.name) return;
        //change icon here
        transform.localScale = Vector3.one;
        
    }
    public void OnRestartClick()
    {
        transform.localScale = Vector2.zero;
        GameManager.gameManager.LoadGame();
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
