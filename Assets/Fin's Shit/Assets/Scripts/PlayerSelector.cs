using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{

    public Button buttonPlayer1, buttonPlayer2, buttonPlayer3;

    public GameObject player1, player2, player3;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("Player 1");
    }

    public void ButtonPlayer2()
    {
        Debug.Log("Player 2");
    }

    public void ButtonPlayer3()
    {
        Debug.Log("Player 3");
    }
}
