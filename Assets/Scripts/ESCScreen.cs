using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector2.zero;
        GameManager.gameManager.callUI += OnGamePause;
    }

    // Update is called once per frame
    void OnGamePause(string callName)
    {
        if (callName != this.name) return;
        transform.localScale = Vector2.one;
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
