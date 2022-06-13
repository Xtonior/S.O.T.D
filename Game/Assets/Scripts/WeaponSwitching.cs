using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private Transform[] weapons;
    [SerializeField] private float switchTime;
    [SerializeField] private KeyCode[] bindings;

    private int selectedWeapon;
    private float lastSwitchTime;

    private void Start()
    {
        SetWeapons();
        Select(selectedWeapon);

        lastSwitchTime = 0f;
    }

    private void Update()
    {
        int prevWeapon = selectedWeapon;

        for (int i = 0; i < bindings.Length; i++)
        {
            if(Input.GetKeyDown(bindings[i]) && lastSwitchTime >= switchTime)
            {
                selectedWeapon = i;
            }
        }

        if(prevWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        lastSwitchTime += Time.deltaTime;
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if(bindings == null)
        {
            bindings = new KeyCode[weapons.Length];
        }
    }

    private void Select(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }

        lastSwitchTime = 0f;    
    }
}
