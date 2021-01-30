using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Item : MonoBehaviour
{
    public enum item_type
    {
        nothing,
        dash,
        jump,
        trac
    }
    public item_type item_holding;
    public virtual IEnumerator Dash() { yield return 0; }
    public virtual IEnumerator Jumping() { yield return 0; }
    private int player;
    private Transform target;
    private Move move;
    [SerializeField]
    private GameObject trackingArrowPrefab;
    private GameObject trackingArrow;
    private Vector3 trackingForward;
    private bool tracking, warning;
    [SerializeField]
    private float spawnTime = 5f;

    public void ItemUpdate()
    {
        if (tracking)
        {
            Transform arrow = trackingArrow.transform;
            arrow.position = transform.position;
            trackingForward = target.position - transform.position;
            if (trackingForward.x > 0) arrow.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(trackingForward, Vector2.up));
            else arrow.rotation = Quaternion.Euler(0, 0, Vector2.Angle(trackingForward, Vector2.up));
        }
    }

    private IEnumerator SpawnTimer(GameObject arrow)
    {
        tracking = true;
        yield return new WaitForSeconds(spawnTime);
        tracking = false;
        Destroy(arrow);
    }

    public void ItemInit()
    {
        move = GetComponent<Move>();
        player = move.playerId;
        target = GameObject.Find("player"+ move.targetId).transform;
        Debug.Log(player + target.name);
    }

    public void useItem()
    {
        if (item_holding == item_type.nothing) return;
        if (item_holding == item_type.dash) StartCoroutine(Dash());
        if (item_holding == item_type.jump) StartCoroutine(Jumping());
        if(item_holding == item_type.trac)
        {
            trackingForward = target.position - transform.position;
            if (trackingForward.x > 0)trackingArrow = Instantiate(trackingArrowPrefab, transform.position, Quaternion.Euler(0, 0, -Vector2.Angle(trackingForward, Vector2.up)));
            else trackingArrow = Instantiate(trackingArrowPrefab, transform.position, Quaternion.Euler(0, 0, Vector2.Angle(trackingForward, Vector2.up)));
            StartCoroutine(SpawnTimer(trackingArrow));
        }
        item_holding = item_type.nothing;
    }
    void track(GameObject gameObject)
    {
        Transform arrow = gameObject.transform;
        arrow.position = transform.position;
        trackingForward = target.position - transform.position;
        if (trackingForward.x > 0) arrow.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(trackingForward, Vector2.up));
        else arrow.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(trackingForward, Vector2.up));
    }
    
}
