using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables

        public static AudioManager Instance { get; private set; }

        [field: 
            SerializeField, 
            Tooltip("Music source for playing background music")]
        private AudioSource musicSource;
        
        [field: 
            SerializeField, 
            Tooltip("Music source for playing sound effects")]
        private AudioSource soundEffectSource;

        [field: SerializeField,
                Tooltip("All audio clips that will be used in game. " +
                        "List is a temp holder so that we can add them through editor")]
        private List<AudioClipHolder> audioClipsList = new();

        public readonly Dictionary<AudioType, AudioClip> audioClipDictionary = new();

        #endregion
       
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            ConvertClipsToDictionary();
            musicSource.clip = audioClipDictionary[AudioType.BgdMusicOne];
            musicSource.Play();
        }

        /// <summary>
        /// Convert list of clips to dictionary so we can easily get clip by type.
        /// </summary>
        private void ConvertClipsToDictionary()
        {
            foreach (var clip in audioClipsList)
            {
                audioClipDictionary.Add(clip.audioType, clip.audioClip);
            }
        }

        /// <summary>
        /// Play any sound from anywhere by calling this
        /// </summary>
        /// <param name="audioType">Set type of sound to be played</param>
        public void PlaySoundOnce(AudioType audioType)
        {
            soundEffectSource.PlayOneShot(audioClipDictionary[audioType]);
        }
        
        /// <summary>
        /// Play any sound which needs to be looped by calling this
        /// </summary>
        /// <param name="audioType">Set type of sound to be played</param>
        /// <param name="isLoop">Set sound as loop or stop playing by false</param>
        public void PlaySoundLoop(AudioType audioType, bool isLoop)
        {
            if(isLoop)
            {
                soundEffectSource.clip = audioClipDictionary[audioType];
                soundEffectSource.Play();
            }
            else
                soundEffectSource.Stop();

            soundEffectSource.loop = isLoop;
        }

        public void SwitchSong(AudioType audioType)
        {
            musicSource.clip = audioClipDictionary[audioType];
            musicSource.Play();
        }
    }
}
