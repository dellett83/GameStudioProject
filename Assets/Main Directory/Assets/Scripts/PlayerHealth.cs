using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public PlayerHUD HUDScript;
    public Team teamScript;
    public ragdollPlayerMovement ragdollScript;

    public int maxHealth;
    [SyncVar]
    public float healthRegenTime;
    public float healthRegenRate;
    [SyncVar]
    public float health;

    [SyncVar]
    private float regenTimer = 0;

    [SyncVar]
    public bool justDied = false;

    public AudioSource AS;

    public AudioClip[] damageSounds;
    public AudioClip deathSound;

    public AudioSource almostDeadSound;
    [SyncVar]
    public bool playAlmostDead = true;


    // Start is called before the first frame update
    void Start()
    {
        //if (!isLocalPlayer) return;
        health = maxHealth;
        justDied = false;
    }

    void FixedUpdate()
    {
        //if (!isLocalPlayer) return;
        if (regenTimer > healthRegenTime && health < maxHealth)
        {
            health += healthRegenRate;

            if (health > maxHealth) health = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HUDScript.healthValue = health;
        //if (!isLocalPlayer) return;
        regenTimer += Time.deltaTime;

        if (health < 25 && playAlmostDead)
        {
            almostDeadSound.Play();
            playAlmostDead = false;
        }
        else if (health > 25)
        {
            almostDeadSound.Stop();
            playAlmostDead = true;
        }

        if (health <= 0 && !justDied)
        {
            health = 0;
            
            Die(teamScript.teamID);
        }
        else if (health <= 0)
        {
            health = 0;
        }
    }

    //[ServerCallback]
    public void DamagePlayer(int _damage, int _bulletsTeam)
    {
        // If I'm not on team I don't get damaged
        if (teamScript.teamID == 0) return;

        // If bullet belongs to someone not on a team, I don't get damaged
        if (_bulletsTeam == 0) return;

        // If me and bullet are on same team, I don't get damaged
        if (_bulletsTeam == teamScript.teamID) return;

        int length = damageSounds.Length;
        int rand = Random.Range(0, length);
        AS.clip = damageSounds[rand];
        AS.Play();

        health -= _damage;

        regenTimer = 0;
        // ui also update to show new health
    }

    void Die(int _teamID)
    {
        AS.clip = deathSound;
        AS.Play();
        justDied = true;
        ragdollScript.dead = true;
        GameManager.Instance.PlayerDie(_teamID);
    }
}
