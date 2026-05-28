using GameAssets.Scripts.ActionPhase.Score;
using GameAssets.Scripts.Managers.ScreenManager;
using UnityEngine;

namespace GameAssets.Scripts.UI.Screens
{
    public class HUDScreen : ScreenBase
    {
        [Space(10)]
        [Header("Score configuration")]
        [SerializeField] public ScoreView scoreView;
        
    }
}