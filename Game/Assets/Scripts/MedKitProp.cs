using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitProp : MonoBehaviour
{
    /*
        Код надо будет переписать. Сейчас это - placeHolder
    */

    public int amount;
    bool isEmpty = false;

    void Update()
    {
        if(isEmpty) Destroy(gameObject);
    }

    public int restoreAmount(int health, int maxHealth)
    {    
        isEmpty = true;
        if(maxHealth - health <= amount) return maxHealth-health;
        else return amount;
    }
}
