using UnityEngine;

public class LevelConfig : ScriptableObject
{
    [SerializeField] private Vector2Int[] _gridSizes;
    
    public Vector2Int[] GridSizes => _gridSizes;
}