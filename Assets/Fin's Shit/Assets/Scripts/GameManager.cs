using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void PlayerDie(int teamID)
    {
        // Do stuff, update score, start correct timer to respawn player
        Debug.Log(teamID);
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
