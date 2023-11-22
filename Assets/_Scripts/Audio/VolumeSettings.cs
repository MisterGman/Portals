using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.Audio
{
    public class VolumeSettings : MonoBehaviour
    {
        #region Variables

        [field:
            SerializeField,
            Tooltip("Audio mixer of the game")]
        private AudioMixer gameMixer;

        [Header("Sliders")]
        [field: SerializeField]        
        private Slider masterSlider;
        
        [field: SerializeField]        
        private Slider musicSlider;
        
        [field: SerializeField]        
        private Slider soundSlider;

        [Header("Value names")]
        [SerializeField]
        private string masterValueName;
        
        [SerializeField]
        private string musicValueName;

        [SerializeField]
        private string soundValueName;
        
        [Header("PlayerPref save values")]
        [SerializeField]
        private string masterSaveName;
        
        [SerializeField]
        private string musicSaveName;

        [SerializeField]
        private string soundSaveName;

        #endregion
        
        private void Start()
        {
            if (PlayerPrefs.HasKey(masterSaveName)) 
                LoadVolume();
            else
                SetAllSliderValues();
            
            masterSlider.onValueChanged.AddListener(value => SetSoundValue(value, masterValueName, masterSaveName));
            musicSlider.onValueChanged.AddListener(value => SetSoundValue(value, musicValueName, musicSaveName));
            soundSlider.onValueChanged.AddListener(value => SetSoundValue(value, soundValueName, soundSaveName));
        }
        
        /// <summary>
        /// Load all values from PlayerPref and set AudioMixer volume values to be the same as in sliders
        /// </summary>
        private void LoadVolume()
        {
            masterSlider.value = PlayerPrefs.GetFloat(masterSaveName);
            musicSlider.value = PlayerPrefs.GetFloat(musicSaveName);
            soundSlider.value = PlayerPrefs.GetFloat(soundSaveName);

            SetAllSliderValues();
        }

        /// <summary>
        /// Set sound value to match to slider value
        /// </summary>
        /// <param name="value">Current slider value</param>
        /// <param name="nameOfValue">Name of value in AudioMixer</param>
        /// <param name="prefSave">Name of value in PlayerPref</param>
        private void SetSoundValue(float value, string nameOfValue, string prefSave)
        {
            gameMixer.SetFloat(nameOfValue, Mathf.Log10(value) * 20f); 
            PlayerPrefs.SetFloat(prefSave, value);
        }
        
        /// <summary>
        /// All volume change calls so we dont have to rewrite same lines of code
        /// </summary>
        private void SetAllSliderValues()
        {
            SetSoundValue(masterSlider.value, masterValueName, masterSaveName);
            SetSoundValue(musicSlider.value, musicValueName, musicSaveName);
            SetSoundValue(soundSlider.value, soundValueName, soundSaveName);
        }
    }
}
