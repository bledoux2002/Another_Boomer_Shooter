using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public GameObject pauseMenu; //set pause menu
    private bool isPaused;

    CharacterController charCtrl; //set character controller of player
    Vector3 moveDir = Vector3.zero; //???
    float rotationX = 0; //???

    public float walkSpeed = 7.0f; //determine speed of normal movement
    public float sprintSpeed = 10.0f; //determine speed of sprinting
    public float crouchSpeed = 4.0f; //determine speed of sneak movement

    public float gravScale = 20.0f; //set scale of gravity inflicted on Player
    public float jumpForce = 7.0f; //set power of jump

    public Camera cam; //set cam
    public float sensitivity = 2.0f; //set sensitivity of camera turning
    public float lookXLim = 45.0f; //set degree limit of camera turning
    public float fov = 75.0f; //set field of view

    public int ammoCount = 0; //hold # of ammo in inv
    private bool canFire = true; //if player can fire weapon
    public float fireRate; //hold rate of fire in sec for weapon
    public Text ammoText; //display # of ammo in inventory
    public Text fireText; //appears when firing bullet
    private Vector3 fireTextPos;

    private System.Random rand;

    [HideInInspector]
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = GameObject.FindWithTag("canvas").GetComponent<PauseMenu>().GamePaused;
        charCtrl = GetComponent<CharacterController>();

        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam.fieldOfView = fov;

        UpdateAmmo(0);
        fireText.text = "";
        fireTextPos = fireText.transform.position;

        rand = new Random();
    }

    // FixedUpdate is called at a fixed interval, independent of frame rate
    void Update()
    {
        isPaused = GameObject.FindWithTag("canvas").GetComponent<PauseMenu>().GamePaused;
        if (!isPaused)
        {
            // ???
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Sprinting
            //will have to make mroe complicated for crouch function, either swap ternary conditionals for if elif statements or more complex ternary conds
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl);
            float curSpeedX = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0; // bool ? if true (bool ? if true sprinting: if false walking) * W/S key : if false no movement
            float curSpeedY = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0; // "ternary conditional operator ?:
            float moveDirY = moveDir.y;
            moveDir = (forward * curSpeedX) + (right * curSpeedY);

            // Jumping
            if (Input.GetButton("Jump") && canMove && charCtrl.isGrounded)
            {
                moveDir.y = jumpForce;
            }
            else
            {
                moveDir.y = moveDirY;
            }

            // Gravity
            if (!charCtrl.isGrounded)
            {
                moveDir.y -= gravScale * Time.deltaTime;
            }

            // Movement
            charCtrl.Move(moveDir * Time.deltaTime);

            // Rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
                rotationX = Mathf.Clamp(rotationX, -lookXLim, lookXLim); //Clamp prevents looking further than directly up or down
                cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
            }

            // Constantly check if Player fired weapon
            StartCoroutine(FireWeapon());

            // Constantly check if Player is aiming down sights
            StartCoroutine(AimDownSights());
        }

        // Pause Menu
        //possibly a way to store current movement values/velocity to reapply after pause? prevents player form dropping open resume
        if (Input.GetKeyDown(KeyCode.Escape)) //change to 'escape', doesn't work in editor
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = isPaused;
        }

        // Old player controller scripting, before adding Character Controller Component. Can be removed but keeping temporarily to study for future reference
        /*        // Move Player
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveZ = Input.GetAxisRaw("Vertical");
                //        float moveY = rb.velocity.y;
                rb.AddForce(Physics.gravity * (gravScale - 1) * rb.mass);

                // Check if player on ground, if so allow for jump, else they fall down
                if (Physics.Raycast(transform.position, Vector3.down, distCheck))
                {
                    isGrounded = true;
                    rb.drag = 1;
                } else {
                    isGrounded = false;
                    rb.drag = 0;
                }
                if (isGrounded && Input.GetKey("space"))
                {
                    //            moveY = jumpForce;
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                } /* else {
                    moveY = -0.375f;
                } * 
                Vector3 movement = new Vector3(moveX, 0.0f, moveZ) * speed;
                rb.AddForce(transform.TransformDirection(movement));

                // Rotate Player
                if ((-90 <= rotateX) && (rotateX <= 90))
                {
                    rotateX = rotateX - (sensitivity * Input.GetAxis("Mouse Y"));
                } else if (rotateX < -90)
                {
                    rotateX = -90;
                } else if (rotateX > 90)
                {
                    rotateX = 90;
                }
                rotateY = rotateY + (sensitivity * Input.GetAxis("Mouse X"));
                Vector3 camRotation = new Vector3(rotateX, rotateY, 0);
                cam.transform.eulerAngles = camRotation;
                Vector3 rotation = new Vector3(0, rotateY, 0);
                transform.eulerAngles = rotation;
        */
    }

    // Update Ammo Count Text
    public void UpdateAmmo(int num = -1)
    {
        ammoCount += num;
        if (ammoCount > 100) ammoCount = 100;
        ammoText.text = ammoCount.ToString() + " | 100";
    }

    // Fire one shot of ammo
    IEnumerator FireWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammoCount > 0)
            {
                canFire = false;
                UpdateAmmo();
                //fireText.transform.position = new Vector3((float)rand.Next(-250, 250), (float)rand.Next(-100, 100), 0f);
                fireText.text = "pew"; // available ammo to be shot has been fired
                StartCoroutine(FireRateHandler());
            }
            else
            {
                //fireText.transform.position = fireTextPos;
                fireText.text = "click"; // no available ammo to be shot
            }
            yield return new WaitForSeconds(1f); // leave effect on screen for 1 sec
            fireText.text = "";
        }
    }

    IEnumerator FireRateHandler()
    {
        float timeToFire = 1 / fireRate;
        yield return new WaitForSeconds(timeToFire);
        canFire = true;
    }

    // ADS
    IEnumerator AimDownSights()
    {
        // while right-click is pressed down, zoom fov in
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 1; i < 11; i++)
            {
                cam.fieldOfView = fov - (fov * 0.05f * i); //float affects dist
                yield return new WaitForSeconds(1 / 10);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            for (int i = 10; i > 0; i--)
            {
                cam.fieldOfView = fov - (fov * 0.05f * i);
                yield return new WaitForSeconds(1 / 10);
            }
        }
    }
}
