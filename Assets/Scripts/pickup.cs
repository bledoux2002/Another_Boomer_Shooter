using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject shotgun; //shotgun prefab for instantiation when picked up
    public Vector3 shotgunPositionOffset = new Vector3(0, 1.5f, 0.5f);
    public Vector3 shotgunRotationOffset = new Vector3(-90, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously rotate object
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
        
        // Object floats up and down

    }

    // Event Trigger, should prob move to scripts for each of these objects
    void OnTriggerEnter(Collider playerCollider)
    {
        switch (gameObject.tag)
        {
            case "ammo":
                if (playerController.ammoCount < 100)
                {
                    playerController.UpdateAmmo(10);
                    gameObject.SetActive(false);
                }
                break;
            case "weapon":
                if (playerController.ammoCount < 100)
                {
                    playerController.UpdateAmmo(12);
                    Transform camTransform = playerController.cam.transform;
                    Vector3 shotgunPosition = camTransform.position + camTransform.TransformDirection(shotgunPositionOffset);
                    Quaternion shotgunRotation = camTransform.rotation * Quaternion.Euler(shotgunRotationOffset);
                    Instantiate(shotgun, shotgunPosition, shotgunRotation, camTransform);
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
