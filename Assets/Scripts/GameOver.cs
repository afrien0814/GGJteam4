using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    //AudioSource audioSource;

    AudioSource SE;
    void Start()
    {
        GameManager.gameManager.callUI += OnGameEnd;
        transform.localScale = Vector2.zero;
        //audioSource = GetComponent<AudioSource>();
        SE = GameObject.Find("SE").GetComponent<AudioSource>();
    }

    public void OnGameEnd(string callName)
    {
        if (callName != this.name) return;
        StartCoroutine(TBContinued());

        /*
        //change icon here
        transform.localScale = Vector3.one;
        audioSource.time = 0.5f;
        audioSource.Play();
        */
    }
    public void OnRestartClick()
    {
        transform.localScale = Vector2.zero;
        EffectReset();
        GameManager.gameManager.LoadGame();
    }
    public void OnExitClick()
    {
        Application.Quit();
    }

    Rect former_rect;
    void GameOverEffect()
    {
        GameObject.Find("player1").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("player2").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("player3").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("player4").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("player" + GameManager.gameManager.winnerId.ToString()).transform.GetChild(0).gameObject.SetActive(true);
        former_rect = GameObject.Find("player" + GameManager.gameManager.winnerId.ToString()).transform.GetChild(0).GetComponent<Camera>().rect;
        GameObject.Find("player" + GameManager.gameManager.winnerId.ToString()).transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);

        this.transform.parent.GetChild(0).localScale = Vector2.zero;
        this.transform.parent.GetChild(1).localScale = Vector2.zero;
    }
    void EffectReset()
    {
        SE.Stop();
        GameObject.Find("player" + GameManager.gameManager.winnerId.ToString()).transform.GetChild(0).GetComponent<Camera>().rect = former_rect;
        GameObject.Find("player1").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("player2").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("player3").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("player4").transform.GetChild(0).gameObject.SetActive(true);
        this.transform.parent.GetChild(0).localScale = Vector2.one;
        this.transform.parent.GetChild(1).localScale = Vector2.one;
    }

    IEnumerator TBContinued()
    {
        GameOverEffect();

        SE.clip = SE.GetComponent<SECtrl>().SE[1];
        SE.volume = 1;
        SE.Play();
        FindObjectOfType<PPControl>().start_gray = true;
        yield return new WaitForSeconds(4f);
        FindObjectOfType<PPControl>().return_normal_color();

        transform.localScale = Vector3.one;

        yield break;
    }
}
