using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] private TMP_Text healthText;
    public int maxHealth;

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = health.ToString();

        if(health <= 0)
        {
            Debug.Log("Player Dead");
        }
    }

    public void Heal(int amount)
    {
        health += amount;
    }
}
