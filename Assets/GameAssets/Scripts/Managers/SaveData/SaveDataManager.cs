using System.Collections;
using GameAssets.Scripts.Tools;
using GameAssets.Scripts.Tools.Interfaces;
using UnityEngine;

namespace GameAssets.Scripts.Managers.SaveData
{
    public class SaveDataManager : Singleton<SaveDataManager>, IManageable
    {
        private SaveData _saveData = new SaveData();
        
        public bool initialized { get; set; }

        #region IManageable
        
        public IEnumerator LoadAssets()
        {
            LoadAllData();
            
            initialized = true;
            yield break;
        }

        public void UnloadData()
        {
            
        }
        
        #endregion
        

        private void LoadAllData()
        {
            _saveData = new SaveData()
            {
                hiScore = PlayerPrefs.GetInt("HiScore", 0)
            };
        }

        public SaveData GetSaveData()
        {
            if(_saveData == null)
                LoadAllData();
            
            return _saveData;
        }

        public void SaveData(SaveData modifiedSaveData)
        {
            _saveData = modifiedSaveData;
            
            PlayerPrefs.SetInt("HiScore", modifiedSaveData.hiScore);
        }
    }
}