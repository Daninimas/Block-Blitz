using UnityEngine;

namespace GameAssets.Scripts.ActionPhase
{
    public class CellFactory : MonoBehaviour
    {
        public Cell cellPrefab;

        public Cell CreateCell(Transform parent, Cell.CellColorData cellColorData)
        {
            Cell cell = Instantiate(cellPrefab, parent);
            cell.SetCellColors(cellColorData);
            return cell;
        }
    }
}