using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class PlayerSelector : MonoBehaviour
{

    public Image m_Image;
    public Button buttonPlayer1, buttonPlayer2, buttonPlayer3;
    public Sprite player1Image, player2Image, player3Image;
    public GameObject player1, player2, player3;
    public NetworkRoomManager networkRoomManager;

    // Start is called before the first frame update
    void Start()
    {
        //m_Image = GetComponent<Image>();

        buttonPlayer1.onClick.AddListener(ButtonPlayer1);
        buttonPlayer2.onClick.AddListener(ButtonPlayer2);
        buttonPlayer3.onClick.AddListener(ButtonPlayer3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPlayer1()
    {
        // Debug.Log("Player 1");
        // //networkRoomManager.roomPlayerPrefab = player1;
        // NetworkRoomPlayer networkRoomPlayer = player1.GetComponent<NetworkRoomPlayer>();
        // if (networkRoomPlayer != null){
        //     networkRoomManager.roomPlayerPrefab = networkRoomPlayer;
        // }
        // else{
        //     Debug.LogError("The selected GameObject does not have a NetworkRoomPlayer component.");
        // }

        Debug.Log("Player 1 selected");
        if (player1 != null) {
            networkRoomManager.playerPrefab = player1.gameObject;  // Direct assignment without casting
            Debug.Log("Player prefab set to Player 1");
            m_Image.sprite = player1Image;
        } else {
            Debug.LogError("Player 1 prefab is not assigned!");
        }
    }

    public void ButtonPlayer2()
    {
        Debug.Log("Player 2 selected");
        if (player2 != null) {
            networkRoomManager.playerPrefab = player2.gameObject;  // Direct assignment without casting
            Debug.Log("Player prefab set to Player 2");
            m_Image.sprite = player2Image;
        } else {
            Debug.LogError("Player 2 prefab is not assigned!");
        }
    }

    public void ButtonPlayer3()
    {
        Debug.Log("Player 3 selected");
        if (player3 != null) {
            networkRoomManager.playerPrefab = player3.gameObject;  // Direct assignment without casting
            Debug.Log("Player prefab set to Player 3");
            m_Image.sprite = player3Image;
        } else {
            Debug.LogError("Player 3 prefab is not assigned!");
        }
    }
}
