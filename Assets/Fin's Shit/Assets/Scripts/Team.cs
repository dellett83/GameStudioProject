using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : NetworkBehaviour
{
    public int teamID = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [Command]
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            teamID = 1;
            // Update UI for player on blue team
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            teamID = 2;
            // Update UI for player on red team
        }
    }
}
