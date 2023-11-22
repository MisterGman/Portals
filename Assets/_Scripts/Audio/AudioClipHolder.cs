using System;
using UnityEngine;

namespace _Scripts.Audio
{
    [Serializable]
    public class AudioClipHolder
    {
        /// <summary>
        /// Name of sound we want to get
        /// </summary>
        public AudioType audioType;
        
        /// <summary>
        /// Sound that will be played by calling it's name
        /// </summary>
        public AudioClip audioClip;
    }
}