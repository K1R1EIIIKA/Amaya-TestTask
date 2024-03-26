using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Letter CellLetter;
    public string CellName;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public GameObject LetterObject;

    [NonSerialized] public bool IsShaking;
    
    public void SetLetter(CellCharacteristic cellCharacteristic)
    {
        _spriteRenderer.sprite = cellCharacteristic.sprite;
        CellLetter = cellCharacteristic.letter;
        CellName = cellCharacteristic.letterName;
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, cellCharacteristic.rotation);
    }
}