using System;
using _Scripts.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = _Scripts.Audio.AudioType;

namespace _Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsMenu;

        [field: SerializeField,
                Tooltip("Which scene to unlock on pressing NewGame button")]
        private int sceneToLoad;

        private void Awake()
        {
            AudioManager.Instance.SwitchSong(AudioType.BgdMusicOne);
        }

        public void PlayGame()
        {
            SceneManager.LoadSceneAsync(sceneBuildIndex: sceneToLoad);
            AudioManager.Instance.PlaySoundOnce(AudioType.ButtonClick);
        }
        
        public void SettingsOpen(bool isOpen)
        {
            settingsMenu.SetActive(isOpen);
            AudioManager.Instance.PlaySoundOnce(AudioType.ButtonClick);
        }
        
        public void QuitGame()
        {
            Application.Quit();
            AudioManager.Instance.PlaySoundOnce(AudioType.ButtonClick);
        }
    }
}
