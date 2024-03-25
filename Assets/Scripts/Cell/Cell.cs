using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void SetLetter(CellCharacteristic cellCharacteristic)
    {
        _spriteRenderer.sprite = cellCharacteristic.sprite;
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, cellCharacteristic.rotation);
        
    }
}