using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : NetworkBehaviour
{
    public GameObject canvas;

    public int ammoCount;
    public TMP_Text ammoText;

    public float healthValue;
    public Slider healthSlider;

    public float BlueScore;
    public TMP_Text blueText;

    public float RedScore;
    public TMP_Text redText;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer) return;
        
        Instantiate(canvas);

        ammoText = GameObject.Find("AmmoCount").GetComponent<TMP_Text>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        blueText = GameObject.Find("ScoreR").GetComponent<TMP_Text>();
        redText = GameObject.Find("ScoreL").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        ammoText.text = ammoCount.ToString();
        
        healthSlider.value = (int)healthValue;    

        blueText.text = GameManager.Instance.blueScore.ToString();

        redText.text = GameManager.Instance.redScore.ToString();
    }
}
