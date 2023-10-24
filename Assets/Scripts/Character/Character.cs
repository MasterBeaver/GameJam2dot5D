using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float currentHealth;
    [SerializeField] private float health = 100.0f;
    void Awake()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject, 0.25f);
        }
    }

    public void Takendamage (float damage)
    {
        currentHealth -= damage;
    }
}
