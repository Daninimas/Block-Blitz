using UnityEngine;

namespace GameAssets.Scripts.Managers.Audio
{
    public class SoundTrigger : MonoBehaviour
    {
        public void PlaySound(string soundId)
        {
            AudioManager.Instance.PlaySound(soundId);
        }
    }
}