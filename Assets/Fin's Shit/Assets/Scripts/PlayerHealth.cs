using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public Team teamScript;
    public ragdollPlayerMovement ragdollScript;

    public int maxHealth;
    public float healthRegenTime;
    public float healthRegenRate;
    public float health;

    private float regenTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) return;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (regenTimer > healthRegenRate && health < maxHealth)
        {
            health += healthRegenRate;

            if (health > maxHealth) health = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        regenTimer += Time.deltaTime;

        if (health <= 0)
        {
            health = 0;
            Debug.Log("health is 0");
            ragdollScript.dead = true;
            Die(teamScript.teamID);
             // Maybe use [command]?
        }
    }

    public void DamagePlayer(int _damage, int _bulletsTeam)
    {
        // If I'm not on team I don't get damaged
        if (teamScript.teamID == 0) return;

        // If bullet belongs to someone not on a team, I don't get damaged
        if (_bulletsTeam == 0) return;

        // If me and bullet are on same team, I don't get damaged
        if (_bulletsTeam == teamScript.teamID) return;

        health -= _damage;
        Debug.Log(health);

        regenTimer = 0;
        // ui also update to show new health
    }

    [Command]
    void Die(int _teamID)
    {
        GameManager.Instance.PlayerDie(_teamID);
    }
}
