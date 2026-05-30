using System.Collections;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace GameAssets.Scripts.Managers.Audio
{
    public class AudioManager : Singleton<AudioManager>, IManageable
    {
        [SerializeField] private AudioSource soundAudioSource;
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioDirectory audioDirectory;
        
        private float _globalMusicVolume;
        
        public bool initialized { get; set; }

        #region IManageable implementation

        public IEnumerator LoadAssets()
        {
            _globalMusicVolume = audioDirectory.globalMusicVolume;
            
            initialized  = true;
            yield break;
        }

        public void UnloadData()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion

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

            soundAudioSource.outputAudioMixerGroup = soundData.mixerGroup;
            soundAudioSource.PlayOneShot(randomClip, volume * soundData.volume);
        }

        #endregion

        #region Music management

        public void PlayMusic(string musicId, float volume = 1f)
        {
            MusicData musicData = audioDirectory.GetMusicData(musicId);

            if (musicData == null)
            {
                Log.Error("AudioManager", $"Music with id {musicId} not found in audio directory.");
                return;
            }
            
            AudioClip musicClip = musicData.musicClip;

            musicAudioSource.clip = musicClip;
            musicAudioSource.volume = volume * musicData.volume * _globalMusicVolume;
            musicAudioSource.Play();
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }

        public void PauseMusic()
        {
            musicAudioSource.Pause();
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
    
    [System.Serializable]
    public class MusicData
    {
        public string musicId;
        [Range(0f, 1f)] public float volume;
        public AudioClip musicClip;
    }
}