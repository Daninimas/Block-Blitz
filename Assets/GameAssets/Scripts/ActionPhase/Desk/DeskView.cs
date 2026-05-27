using GameAssets.Scripts.Tools;
using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class DeskView : MonoBehaviour
    {
        [SerializeField] private Transform[] usablePolyominoesPositions;
        
        public int UsablePolyominoesPositions => usablePolyominoesPositions.Length;


        public Polyomino InstantiatePolyominoInUsablePosition(Polyomino polyominoPrefab, int posIndex)
        {
            if (posIndex < 0 || posIndex >= usablePolyominoesPositions.Length)
            {
                Log.Warning("DeskView", $"Trying to instantiate polyomino in position {posIndex} but there " +
                                        $"are only {usablePolyominoesPositions.Length} usable positions.");
                return null;
            }
            
            return Instantiate(polyominoPrefab, usablePolyominoesPositions[posIndex]);
        }


        public void DestroyGameObject(GameObject gObject)
        {
            Destroy(gObject);
        }
    }
}