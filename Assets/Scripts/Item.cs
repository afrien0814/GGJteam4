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
        trac,
        warn,
        hide,
        end
    }
    public item_type item_holding;
    public virtual IEnumerator Dash() { yield return 0; }
    public virtual bool Jump() { return false; }
    private Transform target, chaser;
    private Move move;
    [SerializeField]
    private GameObject trackingArrowPrefab, warningArrowPrefab;
    private GameObject trackingArrow, warningArrow;
    private Vector3 trackingForward, warningForward;
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
        if (warning)
        {
            Transform arrow = warningArrow.transform;
            arrow.position = transform.position;
            warningForward = chaser.position - transform.position;
            if (warningForward.x > 0) arrow.rotation = Quaternion.Euler(0, 0, -Vector2.Angle(warningForward, Vector2.up));
            else arrow.rotation = Quaternion.Euler(0, 0, Vector2.Angle(warningForward, Vector2.up));
        }
    }

    private IEnumerator SpawnTimer(GameObject arrow)
    {
        if (arrow.name[0] == 't') tracking = true;
        else warning = true;
        yield return new WaitForSeconds(spawnTime);
        if (arrow.name[0] == 't') tracking = false;
        else warning = false;
        Destroy(arrow);
    }

    public void ItemInit()
    {
        if (!move) move = GetComponent<Move>();
        GameManager.gameManager.callInit += RestartInit;
        RestartInit();
    }
    public void RestartInit()
    {
        item_holding = item_type.nothing;
        target = GameObject.Find("player" + move.targetId).transform;
        chaser = GameObject.Find("player" + move.chaserId).transform;
    }

    public void useItem()
    {
        if (item_holding == item_type.nothing) return;
        if (item_holding == item_type.dash) StartCoroutine(Dash());
        if (item_holding == item_type.jump)
        {
            bool canJump = Jump();
            print(canJump);
            if(canJump){
                item_holding = item_type.nothing;
                GameManager.gameManager.ItemManage(move.playerId, (int)item_holding);
            }
            return;
        }
        if(item_holding == item_type.trac)
        {
            trackingForward = target.position - transform.position;
            if (trackingForward.x > 0)trackingArrow = Instantiate(trackingArrowPrefab, transform.position, Quaternion.Euler(0, 0, -Vector2.Angle(trackingForward, Vector2.up)));
            else trackingArrow = Instantiate(trackingArrowPrefab, transform.position, Quaternion.Euler(0, 0, Vector2.Angle(trackingForward, Vector2.up)));
            StartCoroutine(SpawnTimer(trackingArrow));
        }
        if (item_holding == item_type.warn)
        {
            warningForward = chaser.position - transform.position;
            if (warningForward.x > 0) warningArrow = Instantiate(warningArrowPrefab, transform.position, Quaternion.Euler(0, 0, -Vector2.Angle(warningForward, Vector2.up)));
            else warningArrow = Instantiate(warningArrowPrefab, transform.position, Quaternion.Euler(0, 0, Vector2.Angle(warningForward, Vector2.up)));
            StartCoroutine(SpawnTimer(warningArrow));
        }
        item_holding = item_type.nothing;
        GameManager.gameManager.ItemManage(move.playerId, (int)item_holding);
    }
    
}
