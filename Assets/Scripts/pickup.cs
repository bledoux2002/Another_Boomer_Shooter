using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerController playerController;
    public Weapon weapon; //shotgun prefab for instantiation when picked up
    public Vector3 positionOffset;// = new Vector3(0, 1.5f, 0.5f);
    public Vector3 rotation;// = new Vector3(-90, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously rotate object
        transform.Rotate(rotation * Time.deltaTime);
        
        // Object floats up and down

    }

    // Event Trigger, should prob move to scripts for each of these objects
    void OnTriggerEnter(Collider playerCollider)
    {
        switch (gameObject.tag)
        {
            case "ammo": // have to change depending on ammo type
                if (weapon.ammoInv < weapon.maxAmmo)
                {
                    weapon.UpdateAmmo(20);
                    gameObject.SetActive(false);
                }
                break;
            case "weapon":
                if (weapon.ammoInv < weapon.maxAmmo)
                {
                    weapon.UpdateAmmo(10);
                    /*
                    Transform camTransform = playerController.cam.transform;
                    Vector3 shotgunPosition = camTransform.position + camTransform.TransformDirection(positionOffset);
                    Quaternion shotgunRotation = camTransform.rotation * Quaternion.Euler(rotationOffset);
                    Instantiate(shotgun, shotgunPosition, shotgunRotation, camTransform);
                    */
                    //weapon.SetActive(true);
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
