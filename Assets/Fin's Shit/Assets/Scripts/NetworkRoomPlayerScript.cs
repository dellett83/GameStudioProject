using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-room-player
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomPlayer.html
*/

/// <summary>
/// This component works in conjunction with the NetworkRoomManager to make up the multiplayer room system.
/// The RoomPrefab object of the NetworkRoomManager must have this component on it.
/// This component holds basic room player data required for the room to function.
/// Game specific data for room players can be put in other components on the RoomPrefab or in scripts derived from NetworkRoomPlayer.
/// </summary>
public class NetworkRoomPlayerScript : NetworkRoomPlayer
{

    public GameObject uiPrefab;
    public GameObject joinGameIcon;
    public GameObject canvasPrefab;
    private static GameObject canvasInstance;
    private GameObject uiInstance;

    [SyncVar]
    int playersInGame = 0;
    [SyncVar]
    int numberOnScreen = 0;
    //int numberToPrint;

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            // Instantiate the UI prefab
            uiInstance = Instantiate(uiPrefab);
            UIManager uiManager = uiInstance.GetComponent<UIManager>();

            if (uiManager != null)
            {
                uiManager.SetPlayer(this);
                uiManager.UpdateStatus("You are the local player.");
            }
            else
            {
                Debug.LogError("UIManager component not found in UI prefab!");
            }
        }
    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (uiInstance != null)
        {
            UIManager uiManager = uiInstance.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateStatus("Local player setup complete.");
            }
        }
    }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority"/> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnectionToClient parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    #endregion

    #region Room Client Callbacks

    /// <summary>
    /// This is a hook that is invoked on all player objects when entering the room.
    /// <para>Note: isLocalPlayer is not guaranteed to be set until OnStartLocalPlayer is called.</para>
    /// </summary>
    [Command]
    public override void OnClientEnterRoom()
    {
        base.OnClientEnterRoom();
        Debug.Log("OnClientEnterRoom called.");

        if (isLocalPlayer && uiInstance != null)
        {
            UIManager uiManager = uiInstance.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateStatus("You are connected to: " + NetworkManager.singleton.networkAddress);
            }
            AddPlayer();
            Debug.Log("Players in game: " + playersInGame);
        }
        Debug.Log("Players in game: " + playersInGame);
        //numberToPrint = playersInGame - numberOnScreen;
        for (int numberToPrint = playersInGame - numberOnScreen; numberToPrint > 0; numberToPrint--)
        {
            Debug.Log("Number to print: " + numberToPrint);
            RcpShowJoinGameIcon();
            numberOnScreen++;
        }

    }

    [ClientCallback]
    public void AddPlayer()
    {
        playersInGame += 1;
    }

    /// <summary>
    /// This is a hook that is invoked on all player objects when exiting the room.
    /// </summary>
    public override void OnClientExitRoom()
    {

        base.OnClientExitRoom();
        Debug.Log("OnClientExitRoom called.");

        if (isLocalPlayer && uiInstance != null)
        {
            UIManager uiManager = uiInstance.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateStatus("You have exited the room.");
            }
            //uiInstance.SetActive(false);
            Destroy(uiInstance);
        }

    }

    #endregion

    #region SyncVar Hooks

    /// <summary>
    /// This is a hook that is invoked on clients when the index changes.
    /// </summary>
    /// <param name="oldIndex">The old index value</param>
    /// <param name="newIndex">The new index value</param>
    public override void IndexChanged(int oldIndex, int newIndex)
    {

        base.IndexChanged(oldIndex, newIndex);
        Debug.Log($"IndexChanged called: {oldIndex} -> {newIndex}");

        if (isLocalPlayer && uiInstance != null)
        {
            UIManager uiManager = uiInstance.GetComponent<UIManager>();
            if (uiManager != null)
            {
                //uiManager.UpdateStatus($"Player index changed from {oldIndex} to {newIndex}");
                uiManager.UpdateStatus("You are connected to: " + NetworkManager.singleton.networkAddress);
            }
        }

    }

    /// <summary>
    /// This is a hook that is invoked on clients when a RoomPlayer switches between ready or not ready.
    /// <para>This function is called when the a client player calls SendReadyToBeginMessage() or SendNotReadyToBeginMessage().</para>
    /// </summary>
    /// <param name="oldReadyState">The old readyState value</param>
    /// <param name="newReadyState">The new readyState value</param>
    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {

        base.ReadyStateChanged(oldReadyState, newReadyState);
        Debug.Log($"ReadyStateChanged called: {oldReadyState} -> {newReadyState}");

        if (isLocalPlayer && uiInstance != null)
        {
            UIManager uiManager = uiInstance.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateReadyButton(newReadyState);
            }
        }

    }

    #endregion

    #region ClientRpc Methods

    //[ClientRpc]
    void RcpShowJoinGameIcon()
    {
        Debug.Log("RpcShowJoinGameIcon called.");

        // Ensure there is only one canvas
        if (canvasInstance == null)
        {
            // Instantiate the canvas prefab if it doesn't exist
            if (canvasPrefab != null)
            {
                canvasInstance = Instantiate(canvasPrefab);
            }
            else
            {
                Debug.LogError("canvasPrefab is not assigned!");
                return;
            }
        }

        // Null check for joinGameIcon before attempting to instantiate it
        if (joinGameIcon != null)
        {
            // Instantiate the join game icon and set its parent to the canvas
            GameObject indicator = Instantiate(joinGameIcon);
            indicator.transform.SetParent(canvasInstance.transform, false);
        }
        else
        {
            Debug.LogError("joinGameIcon is not assigned!");
        }
    }


    #endregion

    #region Optional UI

    //public override void OnGUI()
    //{
    //    base.OnGUI();
    //}

    #endregion
}

