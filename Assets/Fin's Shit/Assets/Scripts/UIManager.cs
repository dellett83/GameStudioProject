using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text statusText;
    public Button readyButton;
    public Text readyButtonText;

    private NetworkRoomPlayerScript player;

    void Awake()
    {
        // Ensure that there's only one instance of UIManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep this UIManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize button click listener
        readyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    public void SetPlayer(NetworkRoomPlayerScript player)
    {
        this.player = player;
    }

    public void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = status;
        }
    }

    public void UpdateReadyButton(bool isReady)
    {
        if (readyButtonText != null)
        {
            readyButtonText.text = isReady ? "Unready" : "Ready";
        }
    }

    private void OnReadyButtonClicked()
    {
        if (player != null)
        {
            if (player.readyToBegin)
            {
                player.CmdChangeReadyState(false);
            }
            else
            {
                player.CmdChangeReadyState(true);
            }
        }
    }
}
