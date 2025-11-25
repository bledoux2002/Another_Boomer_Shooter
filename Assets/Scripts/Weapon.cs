using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum FireMode
{
    Bolt,
    SemiAuto,
    Burst,
    Automatic
}

public class Weapon : Pickup<int> // this is annoying, maybe have health/armor be an overridden subclass with two types and the rest can just be one type?
{
    public string Caliber { get; protected set; } // 12 gauge, 9mm, .45, .50, etc
    public string AmmoType { get; protected set; } // FMJ, HP, AP, etc
    public int MagCount { get; protected set; } // ammo in mag
    public int MagSize { get; protected set; } // mag size
    public bool PlusOne { get; protected set; } // can the weapon have an extra round in the chamber
    public float DamageMod { get; protected set; } // how much do weapon mods affect the damage of the ammo caliber/type
    public float FireRate { get; protected set; } // how often the weapon can fire
    public FireMode FireMode { get; protected set; } // is weapon able to be fired continuously
    public float ReloadSpeed { get; protected set; } // how long reloading takes
    public Dictionary<string, float> Mods { get; protected set; } // which mods the weapon has and the damage modifier of each
    
    private bool _canFire; //if player can fire weapon
    
    [SerializeField]protected AudioSource equipSound;
    [SerializeField]protected AudioSource fireSound;
    [SerializeField]protected AudioSource reloadSound;
    protected AudioSource emptySound;

    // Start is called before the first frame update
    protected override void OnStart()
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
            if (ammoCount > 0 && _canFire)
            {
                _canFire = false;
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
                ammoInv -= magSize - ammoCount;
                ammoCount = magSize;
            }
            UpdateAmmo(0);
            yield return new WaitForSeconds(0f);
        }
    }

    IEnumerator FireRateHandler()
    {
        yield return new WaitForSeconds(1 / fireRate);
        _canFire = true;
    }

}
