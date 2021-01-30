using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager.callUI += OnGameStart;
    }

    public void OnGameStart(string callName)
    {
        if (callName == this.name)
        {
            transform.localScale = Vector3.one;
        }
    }
    public void OnClicked()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(GameManager.gameManager.GameStart()); 
    }
}
