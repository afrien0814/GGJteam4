using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    public string directrion;
    [SerializeField]
    private KeyCode upKey, downKey, leftKey, rightKey;
    private float moveSpeed = 3;
    private Vector2 dir = Vector2.zero;
    private Rigidbody2D rigidBody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
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
        if (Input.GetKey(upKey)) dir.y = moveSpeed;
        if (Input.GetKey(downKey)) dir.y = -moveSpeed;
        if (Input.GetKey(rightKey)) dir.x = moveSpeed;
        if (Input.GetKey(leftKey)) dir.x = -moveSpeed;
        
    }

}
