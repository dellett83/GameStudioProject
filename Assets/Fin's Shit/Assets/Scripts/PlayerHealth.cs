using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Team teamScript;

    public int maxHealth;
    public float healthRegenTime;
    public float healthRegenRate;
    public float health;

    private float regenTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (regenTimer > healthRegenRate && health < maxHealth)
        {
            health += healthRegenRate;

            if (health > maxHealth) health = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        regenTimer += Time.deltaTime;

        if (health <= 0)
        {
            health = 0;
            GameManager.Instance.PlayerDie(teamScript.teamID); // Maybe use [command]?
        }
    }

    public void DamagePlayer(int _damage)
    {
        health -= _damage;

        regenTimer = 0;
        // ui also update to show new health
    }
}
