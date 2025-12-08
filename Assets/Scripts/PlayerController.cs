using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public bool Paused { get; set; }

    CharacterController charCtrl; //set character controller of player
    Vector3 moveDir = Vector3.zero; //??
    float rotationX = 0; //??

    public float walkSpeed = 7.0f; //determine speed of normal movement
    public float sprintSpeed = 10.0f; //determine speed of sprinting
    public float crouchSpeed = 4.0f; //determine speed of sneak movement

    public float gravScale = 20.0f; //set scale of gravity inflicted on Player
    public float jumpForce = 7.0f; //set power of jump

    public Camera cam; //set cam
    public float sensitivity = 2.0f; //set sensitivity of camera turning
    public float lookXLim = 45.0f; //set degree limit of camera turning
    public float fov; //set field of view
    public float ads; //set ads field of view
    public int adsRate; //time to ADS
    public float interactDistance;
    public LayerMask interactLayer;


    public GameObject[] weapons;
    public int currentWeapon;
    public Dictionary<string, Dictionary<string, int>> ammo = new Dictionary<string, Dictionary<string, int>>();
    public Dictionary<string, bool> unlocked = new Dictionary<string, bool>();
    private float scrollSensitivity = 1.0f;

    private System.Random rand;

    [HideInInspector]
    public bool canMove = true;
    
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();

        // hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam.fieldOfView = fov;

        rand = new Random();

        unlocked["pistol"] = true;
        unlocked["shotgun"] = false;

        //ammo
        ammo["pistol"] = new Dictionary<string, int>();
        ammo["pistol"]["mag"] = 0;
        ammo["pistol"]["magMax"] = 12;
        ammo["pistol"]["inv"] = 0;
        ammo["pistol"]["invMax"] = 100;
        ammo["pistol"]["box"] = 36;
        ammo["shotgun"] = new Dictionary<string, int>();
        ammo["shotgun"]["mag"] = 0;
        ammo["shotgun"]["magMax"] = 2;
        ammo["shotgun"]["inv"] = 0;
        ammo["shotgun"]["invMax"] = 50;
        ammo["shotgun"]["box"] = 12;
    }

    // FixedUpdate is called at a fixed interval, independent of frame rate
    void Update()
    {
        if (!Paused)
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

            float scrollAmount = Input.mouseScrollDelta.y * scrollSensitivity;
            if (scrollAmount > 0)
            {
                changeWeapon(1);
            } else if (scrollAmount < 0) {
                changeWeapon(-1);
            }

            // Constantly check if Player fired or reloaded the weapon
            // StartCoroutine(weapons[currentWeapon].GetComponent<Weapon>().WeaponHandler());

            // Constantly check if Player is aiming down sights
            // StartCoroutine(AimDownSights());

        }

        // Pause Menu
        //possibly a way to store current movement values/velocity to reapply after pause? prevents player form dropping open resume


        // Interact
        //raycast to see if player interacts with IInteractable (door, button, etc)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactDistance, interactLayer))
            {
                IInteractable target = hit.transform.gameObject.GetComponent<IInteractable>();
                target.Interact();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Pickup pickup = other.gameObject.GetComponent<Pickup>();

        switch (pickup)
        {
            case Health healthPack:
                break;

            case Armor armorPack:
                break;

            case AmmoBox ammoBox:
                break;
            
            case Key key:
                break;
            
            case Weapon weapon:
                break;
        }
    }

    // ADS
    IEnumerator AimDownSights()
    {
        // while right-click is pressed down, zoom fov in
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("ADS");
            for (int i = 0; i < adsRate; i++)
            {
                cam.fieldOfView -= (fov - ads) / adsRate; //float affects dist
                yield return new WaitForSeconds(1 / adsRate);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            for (int i = 0; i < adsRate; i++)
            {
                cam.fieldOfView += (fov - ads) / adsRate;
                yield return new WaitForSeconds(1 / adsRate);
            }
        }
    }

    void changeWeapon(int dir)
    {
        currentWeapon += dir;
    }


    public void Freeze(bool gamePaused)
    {
        Paused = gamePaused;
    }
}
