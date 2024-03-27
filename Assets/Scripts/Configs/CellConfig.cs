using System.Collections.Generic;
using CellLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "LettersConfig", menuName = "LettersConfig")]
    public class CellConfig : ScriptableObject
    {
        [FormerlySerializedAs("cells")] [SerializeField]
        private CellCharacteristic[] _cells;
        public IEnumerable<CellCharacteristic> Cells => _cells;
    }
}