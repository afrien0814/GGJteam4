using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public bool isChased, chasing;
    private int playerId, targetId;
    Collider2D m_collider;

    private void Start()
    {
        Move parent = transform.parent.GetComponent<Move>();
        playerId = parent.playerId;

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
        if (move.playerId == targetId) chasing = true;
        if (move.targetId == playerId) isChased = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Move move = collision.gameObject.GetComponent<Move>();
        if (!move)
        {
            Debug.LogError(collision.gameObject.name + " no move!");
            return;
        }
        if (move.playerId == targetId) chasing = false;
        if (move.targetId == playerId) isChased = false;
    }
}
