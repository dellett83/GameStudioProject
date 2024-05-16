using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : NetworkBehaviour
{
    [SyncVar]
    public int teamID = 0;

    public GameObject teamIndicator;
    public Material blue;
    public Material red;
    private Renderer renderer;
    

    // Start is called before the first frame update
    void Start()
    {
        renderer = teamIndicator.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(teamID == 1)
        {
            renderer.material = blue;
        }
        else if (teamID == 2)
        {
            renderer.material = red;
        }

        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            JoinBlue();
            // Update UI for player on blue team
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            JoinRed();
            // Update UI for player on red team
        }
    }

    [Command]
    void JoinBlue()
    {
        teamID = 1;
    }

    [Command]
    void JoinRed()
    {
        teamID = 2;  
    }
}
