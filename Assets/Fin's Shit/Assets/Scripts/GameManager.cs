using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public float respawnTime = 5;

    public Transform[] spawnPoints;

    private bool startBlueRespawnTimer = false;
    private float blueRespawnTimer = 0;

    private bool startRedRespawnTimer = false;
    private float redRespawnTimer = 0;

    public int scoreToWin;
    private int redScore = 0;
    private int blueScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (!isServer) return;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;

        if (startBlueRespawnTimer) blueRespawnTimer += Time.deltaTime;
        if (blueRespawnTimer > respawnTime)
        {
            startBlueRespawnTimer = false;
            // Function to respawn blue player maybe?
        }

        if (startRedRespawnTimer) redRespawnTimer += Time.deltaTime;
        if (redRespawnTimer > respawnTime)
        {
            startRedRespawnTimer = false;
        }

        if (blueScore >= scoreToWin)
        {
            BlueWins();
        }
        else if (redScore >= scoreToWin)
        {
            RedWins();
        }
    }

    public void PlayerDie(int teamID) // 1 is blue team, 2 is red team
    {
        // Do stuff, update score, start correct timer to respawn player
        Debug.Log(teamID);

        if (teamID == 1)
        {
            startBlueRespawnTimer = true;
            redScore++;
            // Update UI
        }
        else if (teamID == 2)
        {
            startRedRespawnTimer = true;
            blueScore++;
            // Update UI
        }
        else
        {
            Debug.Log("Player died not on a team");
        }
    }

    void BlueWins()
    {
        // idk
    }

    void RedWins()
    {
        // idk
    }
}
