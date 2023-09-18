using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private bool _isDead;

    void Start()
    {
        health = maxHealth;
    }

    public void takeDamage(float damage) 
    {
        health -= damage;
    }

    public bool isDead() 
    {
        if (health <= 0) 
        { 
            _isDead = true; 
        }

        else
        {
            _isDead = false;
        }

        return _isDead;
    }

    private void Update()
    {
        if (isDead() == true) 
        {
            health = 0;
            SceneManager.LoadScene(2);
        }
    }
}
