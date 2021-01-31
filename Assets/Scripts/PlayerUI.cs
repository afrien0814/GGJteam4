using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Sprite[] targets = new Sprite[4], chasers = new Sprite[4], items;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameManager.callUI += OnGameStart;
        GameManager.gameManager.itemUpdate += OnItemChanged;
        transform.localScale = Vector2.zero;
    }

    // Update is called once per frame
    private void OnGameStart(string callName)
    {
        if (callName != this.name) return;
        for(int i = 0; i < 4; i++)
        {
            //Debug.Log((GameManager.gameManager.target_list[i] - 1) + " " + (GameManager.gameManager.chaser_list[i] - 1));
            transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Image>().sprite = targets[GameManager.gameManager.target_list[i] - 1];
            transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<Image>().sprite = chasers[GameManager.gameManager.chaser_list[i] - 1];
        }
        transform.localScale = Vector2.one;
    }
    private void OnItemChanged(int pId, int iId)
    {
        transform.GetChild(pId - 1).GetChild(2).GetChild(0).GetComponent<Image>().sprite = items[iId];
    }
}
