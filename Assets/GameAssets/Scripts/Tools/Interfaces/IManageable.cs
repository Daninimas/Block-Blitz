using System.Collections;

namespace GameAssets.Scripts.Tools.Interfaces
{
    public interface IManageable
    {
        public bool initialized
        {
            set ;
            get;
        }

        IEnumerator LoadAssets();

        void UnloadData();
    }   
    
}