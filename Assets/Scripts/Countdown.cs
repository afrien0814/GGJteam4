using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    bool counting;
    Text text;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.gameManager.callUI += CountdownStart;
        text = transform.GetChild(0).GetComponent<Text>();
        transform.localScale = Vector2.zero;
    }

    private void Update()
    {
        if (counting)
        {
            if (GameManager.gameManager.timer > -1) text.text = "" + GameManager.gameManager.timer;
            if (GameManager.gameManager.timer == -1) text.text = "Start !";
            if (GameManager.gameManager.timer < -1)
            {
                counting = false;
                transform.localScale = Vector2.zero;
            }
        }
  
    }

    // Update is called once per frame
    void CountdownStart(string callName)
    {
        if (callName != this.name) return;
        transform.localScale = Vector2.one;
        counting = true;
    }
    
}
