using UnityEngine;

namespace GameAssets.Scripts.Tools
{
    public class CameraScaler : MonoBehaviour
    {
        private int _currentWidth;
        private int _currentHeight;

        public int defaultWidth;

        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            SetRes();
        }
        
        private void SetRes()
        {
            float currentAspect = _cam.aspect;
            _cam.orthographicSize = defaultWidth / currentAspect / 200f;
            
            _currentWidth = Screen.width;
            _currentHeight = Screen.height;
        }

        void Update()
        {
            if (Screen.width != _currentWidth || Screen.height != _currentHeight)
            {
                SetRes();
            }
        }
    }
}