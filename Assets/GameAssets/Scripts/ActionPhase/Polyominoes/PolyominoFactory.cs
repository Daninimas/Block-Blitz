using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class PolyominoFactory : MonoBehaviour
    {
        [SerializeField] private Polyomino polyominoPrefab;


        public Polyomino CreatePolyomino(int[,] polyominoShape, Transform parent)
        {
            var cellsColorData = ActionPhaseManager.Instance.blockColorsDirectory.GetRandomBlockColor();
            
            var newPolyomino = Instantiate(polyominoPrefab, parent);
            
            newPolyomino.SetUp(polyominoShape, cellsColorData);
                
            return newPolyomino;
        }
    }
}