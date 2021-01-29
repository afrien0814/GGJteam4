using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int playerId;
    [SerializeField]
    private KeyCode upKey, downKey, leftKey, rightKey;
    private float moveSpeed = 3;
    private Vector2 dir = Vector2.zero;
    private Rigidbody2D rigidBody2D;
    private Collider2D myCollider;
    private List<KeyCode> keys;
    
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
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90 * (playerId - 1));

    }

    // Update is called once per frame
    void Update()
    {
        dir = Vector2.zero;
        GetKeyInput();
        rigidBody2D.velocity=dir;
    }

    private void GetKeyInput()
    {
        if (Input.GetKey(keys[0])) dir.y = moveSpeed;
        if (Input.GetKey(keys[1])) dir.x = moveSpeed;
        if (Input.GetKey(keys[2])) dir.y = -moveSpeed;
        if (Input.GetKey(keys[3])) dir.x = -moveSpeed;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
    }

}
