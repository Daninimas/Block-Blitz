using System.Collections;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace GameAssets.Scripts.Managers.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>, IManageable
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioDirectory audioDirectory;
        
        public bool initialized { get; set; }
        
        public IEnumerator LoadAssets()
        {
            _audioSource = GetComponent<AudioSource>();
            
            initialized  = true;
            yield break;
        }

        public void UnloadData()
        {
            throw new System.NotImplementedException();
        }


        #region Sound management
        public void PlaySound(string soundId, float volume = 1f)
        {
            SoundData soundData = audioDirectory.GetSoundData(soundId);

            if (soundData == null)
            {
                Log.Error("AudioManager", $"Sound with id {soundId} not found in audio directory.");
                return;
            }
            
            AudioClip[] clips = soundData.sounds;
            AudioClip randomClip = clips[Random.Range(0, clips.Length)];

            _audioSource.outputAudioMixerGroup = soundData.mixerGroup;
            _audioSource.PlayOneShot(randomClip, volume * soundData.volume);
        }

        #endregion
    }
    
    [System.Serializable]
    public class SoundData
    {
        public string soundId;
        [Range(0f, 1f)] public float volume;
        public AudioMixerGroup mixerGroup;
        public AudioClip[] sounds;
    }
}