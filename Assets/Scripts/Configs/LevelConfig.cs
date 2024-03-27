using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int[] _gridSizes;
    
        public Vector2Int[] GridSizes => _gridSizes;
    }
}