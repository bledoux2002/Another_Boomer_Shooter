using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
//    public GameObject pauseMenu; //set pause menu
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
    public int adsRate; //time to ADS
    public float interactDistance;
    public LayerMask interactLayer;



    public GameObject currentWeapon;
    public Dictionary<string, Dictionary<string, int>> ammo = new Dictionary<string, Dictionary<string, int>>();

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

            // Constantly check if Player fired or reloaded the weapon
            StartCoroutine(currentWeapon.GetComponent<Weapon>().WeaponHandler());

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

        // Interact
        //raycast to see if player is interacting with environment (door, button, etc)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactDistance, interactLayer))
            {
                GameObject obj = hit.transform.gameObject;
                switch (obj.tag)
                {
                    case "door":
                        StartCoroutine(obj.GetComponent<Door>().Operate());
                        break;
                    case "button":
                        break;
                }
            }


        }
    }

    // ADS
    IEnumerator AimDownSights()
    {
        // while right-click is pressed down, zoom fov in
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("ADS");
            for (int i = 1; i <= adsRate; i++)
            {
                cam.fieldOfView = fov - (fov * 0.05f * i); //float affects dist
                yield return new WaitForSeconds(1 / adsRate);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            for (int i = adsRate; i > 0; i--)
            {
                cam.fieldOfView = fov - (fov * 0.05f * i);
                yield return new WaitForSeconds(1 / adsRate);
            }
        }
    }

}
