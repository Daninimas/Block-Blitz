using System.Linq;
using UnityEngine;

namespace GameAssets.Scripts.Managers.Audio
{
    [CreateAssetMenu(fileName = "AudioDirectory", menuName = "ScriptableObjects/AudioDirectory", order = 0)]
    public class AudioDirectory : ScriptableObject
    {
        public SoundData[] soundDataArray;
        
        public SoundData GetSoundData(string soundId)
        {
            SoundData soundData = soundDataArray.FirstOrDefault(data => data.soundId == soundId);
            return soundData;
        }
    }
}