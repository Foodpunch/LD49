using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;


    public float moveSpeed;
    Rigidbody2D _rb;

    public Transform gunHolder;
    public SpriteRenderer gunSprite;


    Vector2 playerInput;
    Vector2 moveVelocity;
    Vector2 mouseInput;
    Vector2 mouseDirection;
    //assuming player is using some capsule collider

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if ! game paused
        PlayerMovementInput();
        SetGunToFaceMouse();
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + moveVelocity * Time.fixedDeltaTime);
       // _rb.velocity = moveVelocity;
    }
    void PlayerMovementInput()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = playerInput * moveSpeed;
    }
    void SetGunToFaceMouse()
    {
        mouseInput = Input.mousePosition;
        Vector3 mousePosInWord = Camera.main.ScreenToWorldPoint(mouseInput);
        mousePosInWord.z = 0;
        mouseDirection = mousePosInWord - transform.position;
        gunHolder.transform.right = mouseDirection;

        //Hacky sprite flip here
        float angle = gunHolder.transform.rotation.eulerAngles.z;
        gunSprite.flipY = (angle > 90f && angle < 270f);        //angle goes from 0 to 360. 
    }
}
