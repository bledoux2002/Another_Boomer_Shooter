using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public int ammoCount = 0; //ammo in mag
    public int magSize; // mag size
    public int ammoInv; // ammo in inv
    public int maxAmmo; // ammo inv size
    private bool canFire = true; //if player can fire weapon
    public float fireRate; //hold rate of fire in sec for weapon
    public bool isAutomatic;

    public Text ammoText; //display # of ammo in inventory
    private Vector3 fireTextPos;

    public AudioSource equipSound;
    public AudioSource fireSound;
    public AudioSource reloadSound;
    public AudioSource emptySound;

    // Start is called before the first frame update
    void Start()
    {
        UpdateAmmo(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update Ammo Count Text
    public void UpdateAmmo(int num = -1)
    {
        if (num > 0)
        {
            ammoInv += num;
            if (ammoInv > maxAmmo) ammoCount = maxAmmo;
        }
        else if (num < 0)
        {
            ammoCount += num;
        }
        ammoText.text = ammoCount.ToString() + " / " + ammoInv.ToString();
    }

    // Fire one shot of ammo or reload weapon (return IEnumerator for adaptation to automatic
    public IEnumerator WeaponHandler()
    {
//        while (Input.GetMouseButton(0))
        if (Input.GetMouseButtonDown(0))
        {
            if (ammoCount > 0 && canFire)
            {
                canFire = false;
                UpdateAmmo();
                fireSound.Play(); // available ammo to be shot has been fired
                StartCoroutine(FireRateHandler());
                yield return new WaitForSeconds(1f);
            }
            else
            {
                emptySound.Play(); // no available ammo to be shot
                yield return new WaitForSeconds(0f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && (ammoCount < magSize) && (ammoInv > 0))
        {
            reloadSound.Play();
            if (ammoInv < magSize)
            {
                ammoCount = ammoInv;
                ammoInv = 0;
            }
            else
            {
                ammoCount = magSize;
                ammoInv -= magSize;
            }
            UpdateAmmo(0);
            yield return new WaitForSeconds(0f);
        }
    }

    IEnumerator FireRateHandler()
    {
        yield return new WaitForSeconds(1 / fireRate);
        canFire = true;
    }

}
