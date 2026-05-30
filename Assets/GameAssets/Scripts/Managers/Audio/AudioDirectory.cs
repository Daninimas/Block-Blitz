using System.Linq;
using UnityEngine;

namespace GameAssets.Scripts.Managers.Audio
{
    [CreateAssetMenu(fileName = "AudioDirectory", menuName = "ScriptableObjects/AudioDirectory", order = 0)]
    public class AudioDirectory : ScriptableObject
    {
        [Header("Configuration")]
        public float globalMusicVolume = 1f;
        
        [Header("References")]
        public SoundData[] soundDataArray;
        public MusicData[] musicDataArray;
        
        public SoundData GetSoundData(string soundId)
        {
            SoundData soundData = soundDataArray.FirstOrDefault(data => data.soundId == soundId);
            return soundData;
        }
        
        public MusicData GetMusicData(string musicId)
        {
            MusicData musicData = musicDataArray.FirstOrDefault(data => data.musicId == musicId);
            return musicData;
        }
    }
}