using System;
using UnityEngine;

namespace CellLogic
{
    public class Cell : MonoBehaviour
    {
        private Letter _cellLetter;
        [SerializeField] private GameObject _letterObject;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [NonSerialized] private bool _isShaking;
        
        public Letter CellLetter => _cellLetter;
        public GameObject LetterObject => _letterObject;
        public bool IsShaking => _isShaking;
        
        public void SetLetter(CellCharacteristic cellCharacteristic)
        {
            _spriteRenderer.sprite = cellCharacteristic.sprite;
            _cellLetter = cellCharacteristic.letter;
            _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, cellCharacteristic.rotation);
        }
        
        public void SetShaking(bool isShaking)
        {
            _isShaking = isShaking;
        }
    }
}