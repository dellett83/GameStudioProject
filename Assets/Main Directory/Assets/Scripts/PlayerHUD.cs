using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : NetworkBehaviour
{
    public GameObject canvas;

    public ragdollPlayerMovement playerMovementScript;

    public Team teamScript;

    public int ammoCount;
    public TMP_Text ammoText;

    public float healthValue;
    public Slider healthSlider;

    public float BlueScore;
    public TMP_Text blueText;

    public float RedScore;
    public TMP_Text redText;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject joinTeamPanel;

    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) return;
        
        Instantiate(canvas);

        ammoText = GameObject.Find("AmmoCount").GetComponent<TMP_Text>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        blueText = GameObject.Find("ScoreR").GetComponent<TMP_Text>();
        redText = GameObject.Find("ScoreL").GetComponent<TMP_Text>();
        winPanel = GameObject.Find("Winner");
        winPanel.SetActive(false);
        losePanel = GameObject.Find("Loser");
        losePanel.SetActive(false);
        pausePanel = GameObject.Find("Pause");
        pausePanel.SetActive(false);
        joinTeamPanel = GameObject.Find("Join team");
        joinTeamPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if(teamScript.teamID == 0)
        {
            joinTeamPanel.SetActive(true);
        }
        else
        {
            joinTeamPanel.SetActive(false);
        }

        ammoText.text = ammoCount.ToString();
        
        healthSlider.value = (int)healthValue;    

        blueText.text = GameManager.Instance.blueScore.ToString();

        redText.text = GameManager.Instance.redScore.ToString();

        if (Input.GetKeyDown(KeyCode.Escape) && !pausePanel.activeInHierarchy && !gameOver)
        {
            pausePanel.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pausePanel.activeInHierarchy && !gameOver)
        {
            pausePanel.SetActive(false);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

        if (GameManager.Instance.blueWon && teamScript.teamID == 1 && !gameOver)
        {
            pausePanel.SetActive(false);
            gameOver = true;
            winPanel.SetActive(true);
            playerMovementScript.gameOver = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else if (GameManager.Instance.blueWon && teamScript.teamID == 2 && !gameOver)
        {
            pausePanel.SetActive(false);
            gameOver = true;
            losePanel.SetActive(true);
            playerMovementScript.gameOver = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else if (GameManager.Instance.redWon && teamScript.teamID == 1 && !gameOver)
        {
            pausePanel.SetActive(false);
            gameOver = true;
            losePanel.SetActive(true);
            playerMovementScript.gameOver = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else if (GameManager.Instance.redWon && teamScript.teamID == 2 && !gameOver)
        {
            pausePanel.SetActive(false);
            gameOver = true;
            winPanel.SetActive(true);
            playerMovementScript.gameOver = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }
}
