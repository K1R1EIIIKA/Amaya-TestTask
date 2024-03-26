using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClickHandler : MonoBehaviour
{
    [SerializeField] private LevelTask _levelTask;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit.collider != null)
            {
                var cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    if (cell.CellLetter == _levelTask.taskLetter)
                    {
                        _levelTask.CompleteTask();
                    }
                }
            }
        }
    }
}