using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.CharacterSelection
{
    public class SceneReferencer : MonoBehaviour
    {

        private CharacterData characterData;
        public GameObject characterSelectionObject;
        public GameObject sceneObjects;
        public GameObject cameraObject;

        private void Start()
        {
            characterData = CharacterData.characterDataSingleton;
            if (characterData == null)
            {
                Debug.Log("Add CharacterData prefab singleton into the scene.");
                return;
            }
        }

        public void ButtonCharacterSelection()
        {
            // server-only mode should not press this button
            //Debug.Log("ButtonCharacterSelection");
            cameraObject.SetActive(false);
            sceneObjects.SetActive(false);
            characterSelectionObject.SetActive(true);
            this.GetComponent<Canvas>().enabled = false;
        }

        public void CloseCharacterSelection()
        {
            //Debug.Log("CloseCharacterSelection");
            cameraObject.SetActive(true);
            characterSelectionObject.SetActive(false);
            sceneObjects.SetActive(true);
            this.GetComponent<Canvas>().enabled = true;
        }
    }
}