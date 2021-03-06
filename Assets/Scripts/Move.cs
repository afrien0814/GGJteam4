﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Item
{
    public int playerId, targetId, chaserId;
    [SerializeField]
    private KeyCode upKey, downKey, leftKey, rightKey, useKey;
    private float moveSpeed = 5f;
    private Vector2 dir = Vector2.zero;
    private Rigidbody2D rigidBody2D;
    private Transform sprite;
    private List<KeyCode> keys;

    [SerializeField]
    private float dashSpeed = 7.5f, dashTime=5f;
    [HideInInspector]
    public bool jumping;
    [HideInInspector]
    public Vector2 facing;
    int facing_int;

    //public item_type item_holding;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        keys = new List<KeyCode>();
        keys.Add(upKey); keys.Add(rightKey); keys.Add(downKey); keys.Add(leftKey);
        keys.AddRange(keys);
        keys.RemoveRange(4 + playerId - 1, 4 - playerId + 1);
        keys.RemoveRange(0, playerId - 1);
        //for (int i = 0; i < keys.Count; i++) Debug.Log(keys[i].ToString());
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90 * (playerId - 1));
        sprite = transform.GetChild(1);
        sprite.rotation = Quaternion.Euler(0, 0, 90 * (playerId - 1));
        ItemInit();
        GameManager.gameManager.callInit += MoveInit;
        GameManager.gameManager.startGame += StartItem;
    }

    void MoveInit()
    {
        facing_int = playerId;
        jumping = false;
    }
    
    void StartItem()
    {
        item_holding = item_type.trac;
        useItem();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyInput();
        rigidBody2D.velocity=dir;
        
        if (facing.y == 1) facing_int = 3;
        else if (facing.x == 1) facing_int = 2;
        else if (facing.y == -1) facing_int = 1;
        else if (facing.x == -1) facing_int = 4;
        sprite.rotation = Quaternion.Euler(0, 0, 90 * (facing_int - 1));
        ItemUpdate();
    }

    private void GetKeyInput()
    {
        if (!GameManager.gameManager.gaming)
        {
            dir = Vector2.zero;
            return;
        }
        if (jumping) return;
        if (dir != Vector2.zero) facing = dir.normalized;
        dir = Vector2.zero;
        if (Input.GetKey(keys[0])) dir.y = moveSpeed;
        if (Input.GetKey(keys[1])) dir.x = moveSpeed;
        if (Input.GetKey(keys[2])) dir.y = -moveSpeed;
        if (Input.GetKey(keys[3])) dir.x = -moveSpeed;
        if (Input.GetKeyDown(useKey)) useItem();
    }
    
    public override IEnumerator Dash()
    {
        float tmp = moveSpeed;
        item_holding = item_type.nothing;
        moveSpeed = dashSpeed;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = tmp;
    }
    /*private IEnumerator Dash()
    {
        dashing = true;
        item_holding = item_type.nothing;
        dir = facing * dashForce;
        float acc = (dashForce - moveSpeed) / 3;
        for(int timer = 0; timer < 3; timer++)
        {
            dir -= acc * facing;
            yield return new WaitForSeconds(0.1f);
        }
        dashing = false;
    }*/
    /*
    private void OnDrawGizmosSelected()
    {
        if (this.transform != null)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + facing * 1.5f, 0.3f);
        }
    }*/

    public override bool Jump()
    {
        if (facing.x + facing.y != 1) // handle 45 degree direction
        {
            if (facing_int == 3) facing = new Vector2(0, 1);
            else if (facing_int == 2) facing = new Vector2(1, 0);
            else if (facing_int == 1) facing = new Vector2(0, -1);
            else if (facing_int == 4) facing = new Vector2(-1, 0);
        }
        LayerMask layerMask = new LayerMask();
        layerMask = 1 << LayerMask.NameToLayer("Wall");
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, facing, MapGenerator.mapGenerator.unit_scale.x * 2, layerMask);
        Debug.DrawRay(transform.position, facing * MapGenerator.mapGenerator.unit_scale.x * 2, Color.red, 3f);
        if (hit.Length > 1) return false;
        if (hit.Length == 1) {
            if (hit[0].distance > MapGenerator.mapGenerator.unit_scale.x || hit[0].collider.tag == "wall") return false;
        }
        
        StartCoroutine(Jumping());
        return true;
    }
    private IEnumerator Jumping()
    {
        jumping = true;
        Debug.Log("jump");
        gameObject.layer = LayerMask.NameToLayer("superPlayer");
        dir = moveSpeed * facing;
        yield return new WaitForSeconds(0.4f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        jumping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        //Debug.Log(this.name + " hit " + other.name);
        if (other.layer == LayerMask.NameToLayer("Player"))
        {
            Move other_p = other.gameObject.GetComponent<Move>();
            if (!other_p) return;
            if(other_p.playerId == GetComponent<Move>().targetId)
            {
                GameManager.gameManager.win(playerId);
            }
        }
        if(other.layer== LayerMask.NameToLayer("Item"))
        {
            //if (item_holding != item_type.nothing) return;
            item_holding = (item_type)System.Enum.Parse(typeof(item_type), other.name.Substring(0,4));
            GameManager.gameManager.ItemManage(playerId, (int)item_holding);
            Destroy(other);
            //Debug.Log("get item: " + item_holding);
        }
    }

}
