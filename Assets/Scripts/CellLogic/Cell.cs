using System;
using UnityEngine;

namespace CellLogic
{
    public class Cell : MonoBehaviour
    {
        public Letter CellLetter;
        public GameObject LetterObject;
    
        [NonSerialized] public bool IsShaking;
    
        [SerializeField] private SpriteRenderer _spriteRenderer;
    
        public void SetLetter(CellCharacteristic cellCharacteristic)
        {
            _spriteRenderer.sprite = cellCharacteristic.sprite;
            CellLetter = cellCharacteristic.letter;
            _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, cellCharacteristic.rotation);
        }
    }
}