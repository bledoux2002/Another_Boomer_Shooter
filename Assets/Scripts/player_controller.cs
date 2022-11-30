using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_controller : MonoBehaviour
{
    public GameObject pauseMenu; //set pause menu

    private Rigidbody rb; //set rigidbody of player
    private Transform movement; //set movement

    public float speed; //determine speed of movement

    public Camera cam; //set cam
    public float sensitivity; //set sensitivity of camera turning
    public float fov; //set field of view

    private float rotateX; //set yaw
    private float rotateY; //set pitch

    public float gravScale; //set scale of gravity inflicted on Player
    public float jumpForce; //set power of jump
    public float distCheck; //how close character is to ground
    private bool isGrounded; //determines if player on the ground

    public int ammoCount; //hold # of ammo in inv
    public float rateOfFire; //hold rate of fire in sec for weapon
    public Text ammoText; //display # of ammo in inventory
    public Text fireText; //appears when firing bullet


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        cam.fieldOfView = fov;

        rotateX = 0.0f;
        rotateY = 0.0f;

//        jump = new Vector3(0.0f, 2.0f, 0.0f);

        SetAmmoText();
        fireText.text = "";
    }

    // FixedUpdate is called at a fixed interval, independent of frame rate
    void Update()
    {
        // Move Player
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
        } */ 
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

/*    }

    // Update is called once per frame
    void Update()
    {*/
        // Pause Menu
        if (Input.GetKeyDown("p")) //change to 'escape', doesn't work in editor
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        // Constantly check if Player fired weapon
        StartCoroutine(ShootAmmo());

        // Constantly check if Player is aiming down sights
        StartCoroutine(AimDownSights());
    }

    // Event Trigger
    void OnTriggerEnter(Collider other)
    {
        // If hit pickup and space in inv, add to inv
        if (other.gameObject.CompareTag("pickup") && ammoCount < 12)
        {
            // Remove old pickup and update UI
            other.gameObject.SetActive(false);
            ammoCount++;
            SetAmmoText();

            // Create new pickup in a random location in Test_Room

        }
    }

    // Update Ammo Count Text
    void SetAmmoText()
    {
        if (ammoCount < 12)
        {
            ammoText.text = "Ammo: " + ammoCount.ToString();
        } else {
            ammoText.text = "Ammo: FULL";
        }
    }

    // Fire one shot of ammo
    IEnumerator ShootAmmo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*
            float timer = 0.0f;
            timer = timer + Time.deltaTime;
            if ((timer == 0) || (timer >= rateOfFire)) //not working
            { */
                if (ammoCount > 0)
                {
                    ammoCount--;
                    SetAmmoText();
                    fireText.text = "pew"; // available ammo to be shot has been fired
                } else {
                    fireText.text = "click"; // no available ammo to be shot
                }
                yield return new WaitForSeconds(1f); // leave effect on screen for 1 sec
                fireText.text = "";
            //}
        }
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
        } else if (Input.GetMouseButtonUp(1)) {
            for (int i = 10; i > 0; i--)
            {
                cam.fieldOfView = fov - (fov * 0.05f *i);
                yield return new WaitForSeconds(1 / 10);
            }
        }
    }

}
