using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public bool isChased, chasing;
    private int playerId, targetId;
    Collider2D m_collider;
    Move parent;

    Coroutine SE_coroutine;
    AudioSource SE;

    private void Start()
    {
        parent = transform.parent.GetComponent<Move>();
        playerId = parent.playerId;
        targetId = parent.targetId;
        GameManager.gameManager.callInit += ChaserInit;
        ChaserInit();

        SE = GameObject.Find("SE").GetComponent<AudioSource>();
    }

    private void ChaserInit()
    {
        isChased = chasing = false;
        targetId = parent.targetId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Move move = collision.gameObject.GetComponent<Move>();
        if (!move)
        {
            Debug.LogError(collision.gameObject.name + " no move!");
            return;
        }
        Debug.Log(playerId + " hits " + move.playerId + "(my target is)" + targetId + " and its target is" + move.targetId);
        if (move.playerId == targetId) chasing = true;
        if (move.targetId == playerId) isChased = true;

        if(chasing)
        {
            SE_coroutine = StartCoroutine(PlaySE(collision.transform));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Move move = collision.gameObject.GetComponent<Move>();
        if (!move)
        {
            Debug.LogError(collision.gameObject.name + " no move!");
            return;
        }
        Debug.Log("" + move.playerId + " " + targetId + ", " + move.targetId + " " + playerId);
        if (move.playerId == targetId) chasing = false;
        if (move.targetId == playerId) isChased = false;

        if(!chasing)
        {
            if (SE_coroutine!=null) StopCoroutine(SE_coroutine);
            SE.Stop();
            SE.volume = 1;
        }
    }

    IEnumerator PlaySE(Transform col)
    {
        while(GameManager.gameManager.gaming)
        {
            Debug.Log((this.transform.position - col.position).magnitude.ToString());
            if ((this.transform.position - col.position).magnitude < 3f && !SE.isPlaying)
            {
                SE.clip = SE.GetComponent<SECtrl>().SE[0];
                SE.Play();
                SE.volume = 0;
                for (int i=0; i<10; i++)
                {
                    SE.volume += 0.1f;
                    yield return null;
                }
            }
            yield return null;
        }
    }
}
