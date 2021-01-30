using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int playerId, target;
    [SerializeField]
    private KeyCode upKey, downKey, leftKey, rightKey, dashKey;
    private float moveSpeed = 5f;
    private Vector2 dir = Vector2.zero, lastDir = Vector2.zero;
    private Rigidbody2D rigidBody2D;
    private Collider2D myCollider;
    private List<KeyCode> keys;

    [SerializeField]
    private float dashCD = 1f, dashForce = 25f;
    private bool dashing, dashInCD;
    public Vector2 facing;
    int facing_int;

    private Transform sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        keys = new List<KeyCode>();
        keys.Add(upKey); keys.Add(rightKey); keys.Add(downKey); keys.Add(leftKey);
        keys.AddRange(keys);
        keys.RemoveRange(4 + playerId - 1, 4 - playerId + 1);
        keys.RemoveRange(0, playerId - 1);
        //for (int i = 0; i < keys.Count; i++) Debug.Log(keys[i].ToString());
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90 * (playerId - 1));
        sprite = transform.GetChild(1);
        sprite.rotation = Quaternion.Euler(0, 0, 90 * (playerId - 1));
        facing_int = playerId;
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyInput();
        rigidBody2D.velocity=dir;
        
        if (facing == new Vector2(0, 1)) facing_int = 3;
        else if (facing == new Vector2(1, 0)) facing_int = 2;
        else if (facing == new Vector2(0, -1)) facing_int = 1;
        else if (facing == new Vector2(-1, 0)) facing_int = 4;
        sprite.rotation = Quaternion.Euler(0, 0, 90 * (facing_int - 1));
    }

    private void GetKeyInput()
    {
        if (dashing) return;
        if (dir != Vector2.zero) facing = dir.normalized;
        dir = Vector2.zero;
        if (Input.GetKey(keys[0])) dir.y = moveSpeed;
        if (Input.GetKey(keys[1])) dir.x = moveSpeed;
        if (Input.GetKey(keys[2])) dir.y = -moveSpeed;
        if (Input.GetKey(keys[3])) dir.x = -moveSpeed;
        if (!dashInCD && Input.GetKeyDown(dashKey)) StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        dashing = true;
        dir = facing * dashForce;
        float acc = (dashForce - moveSpeed) / 3;
        for(int timer = 0; timer < 3; timer++)
        {
            dir -= acc * facing;
            yield return new WaitForSeconds(0.1f);
        }
        dashing = false;
        dashInCD = true;
        yield return new WaitForSeconds(dashCD);
        dashInCD = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
    }


}
