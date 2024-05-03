using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Mirror.Examples.CharacterSelection.NetworkManagerCharacterSelection;

namespace Mirror.Examples.CharacterSelection
{
    public class PlayerSelector : MonoBehaviour
    {
        // Make sure to attach these Buttons in the Inspector
        public Button buttonNextCharacter, buttonPreviousCharacter, buttonGo;
        public Text textTitle, textHealth, textSpeed, textAttack;

        public Transform podiumPosition;
        private int currentlySelectedCharacter = 1;
        private CharacterData characterData;
        private GameObject currentInstantiatedCharacter;
        private CharacterSelection characterSelection;
        public SceneReferencer sceneReferencer;
        public Camera cameraObj;

        public GameObject PanelPlayer;

        private void Start()
        {
            characterData = CharacterData.characterDataSingleton;
            if (characterData == null)
            {
                Debug.Log("Add CharacterData prefab singleton into the scene.");
                return;
            }

            buttonNextCharacter.onClick.AddListener(ButtonNextCharacter);
            buttonPreviousCharacter.onClick.AddListener(ButtonPreviousCharacter);
            buttonGo.onClick.AddListener(ButtonGo);

            LoadData();
            SetupCharacters();
        }

        public void ButtonGo()
        {
            //Debug.Log("ButtonGo");

            // presumes we're already in-game
            if (sceneReferencer && NetworkClient.active)
            {
                //Debug.Log("&^$%^%$&^%%^%$&^%&^%^%$^%$&^%$^&^^%^%$^$");
                // You could check if prefab (character number) has not changed, and if so just update the sync vars and hooks of current prefab, this would call a command from your player.
                // this is not fully setup for this example, but provides a minor template to follow if needed
                //NetworkClient.localPlayer.GetComponent<CharacterSelection>();

                CreateCharacterMessage _characterMessage = new CreateCharacterMessage
                {
                    characterNumber = StaticVariables.characterNumber
                };

                ReplaceCharacterMessage replaceCharacterMessage = new ReplaceCharacterMessage
                {
                    createCharacterMessage = _characterMessage
                };
                NetworkManagerCharacterSelection.singleton.ReplaceCharacter(replaceCharacterMessage);
                sceneReferencer.CloseCharacterSelection();
            }
            else
            {
                // not in-game
                SceneManager.LoadScene("MirrorCharacterSelection");
                Debug.Log($"sceneReferencer is {(sceneReferencer == null ? "null" : "not null")}");
                Debug.Log("NetworkClient.active: " + NetworkClient.active);
                Debug.Log("Attempting to connect to server at address: " + NetworkManager.singleton.networkAddress);
            }

            PanelPlayer.SetActive(false);
        }

        public void ButtonNextCharacter()
        {
            //Debug.Log("ButtonNextCharacter");

            currentlySelectedCharacter += 1;
            if (currentlySelectedCharacter == 3)
            {
                currentlySelectedCharacter = 3;
            }
            SetupCharacters();

            StaticVariables.characterNumber = currentlySelectedCharacter;
        }

        public void ButtonPreviousCharacter()
        {
            //Debug.Log("ButtonNextCharacter");

            currentlySelectedCharacter -= 1;
            if (currentlySelectedCharacter == 2)
            {
                currentlySelectedCharacter = 2;
            }
            SetupCharacters();

            StaticVariables.characterNumber = currentlySelectedCharacter;
        }


        private void SetupCharacters()
        {
            textTitle.text = "" + characterData.characterTitles[currentlySelectedCharacter];
            textHealth.text = "Health: " + characterData.characterHealths[currentlySelectedCharacter];
            textSpeed.text = "Speed: " + characterData.characterSpeeds[currentlySelectedCharacter];
            textAttack.text = "Attack: " + characterData.characterAttack[currentlySelectedCharacter];

            if (currentInstantiatedCharacter)
            {
                Destroy(currentInstantiatedCharacter);
            }
            currentInstantiatedCharacter = Instantiate(characterData.characterPrefabs[currentlySelectedCharacter]);
            currentInstantiatedCharacter.transform.position = podiumPosition.position;
            currentInstantiatedCharacter.transform.rotation = podiumPosition.rotation;
            characterSelection = currentInstantiatedCharacter.GetComponent<CharacterSelection>();
            currentInstantiatedCharacter.transform.SetParent(this.transform.root);

            SetupPlayerName();

            if (cameraObj)
            {
                characterSelection.floatingInfo.forward = cameraObj.transform.forward;
            }
        }


        public void SetupPlayerName()
        {
            //Debug.Log("SetupPlayerName");
            if (characterSelection)
            {
                characterSelection.playerName = StaticVariables.playerName;
                characterSelection.AssignName();
            }
        }

        public void LoadData()
        {
            // check that prefab is set, or exists for saved character number data
            if (StaticVariables.characterNumber > 0 && StaticVariables.characterNumber < characterData.characterPrefabs.Length)
            {
                currentlySelectedCharacter = StaticVariables.characterNumber;
            }
            else
            {
                StaticVariables.characterNumber = currentlySelectedCharacter;
            }
        }
    }
}