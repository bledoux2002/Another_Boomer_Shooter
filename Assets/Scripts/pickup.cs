using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerController playerController;
    public Weapon weapon; // prefab for instantiation when picked up
    public Vector3 positionOffset;// = new Vector3(0, 1.5f, 0.5f);
    public Vector3 rotation;// = new Vector3(-90, 0, 0);

    private Dictionary<string, int> ammo;

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

    // Event Trigger
    public void HandlePickup()
    {
        switch (gameObject.tag)
        {
            case "ammo": // have to change depending on ammo type, swap ammo to player controller
                ammo = playerController.ammo[weapon.type];
                if (ammo["inv"] < ammo["invMax"])
                {
                    weapon.UpdateAmmo(ammo["box"]);
                    gameObject.SetActive(false);
                }
                break;
            case "weapon":
                ammo = playerController.ammo[weapon.type];
                if (ammo["inv"] < ammo["invMax"])
                {
                    weapon.UpdateAmmo(ammo["box"]);

                    if (!playerController.unlocked[weapon.type])
                    {
                        playerController.weapons[playerController.currentWeapon].SetActive(false);
                        playerController.unlocked[weapon.type] = true;
                        weapon.gameObject.SetActive(true);
                    }
                    
                    //weapon.SetActive(true);
                    gameObject.SetActive(false);
                }
                break;
            case "health": //100-200 only for smaller sources?
                break;
            case "armor": //same with health, not following doom/quake for uniqueness?
                break;
        }
    }
}
