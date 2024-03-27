using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LettersConfig", menuName = "LettersConfig")]
public class CellConfig : ScriptableObject
{
    [FormerlySerializedAs("cells")] [SerializeField]
    private CellCharacteristic[] _cells;
    public IEnumerable<CellCharacteristic> Cells => _cells;
}