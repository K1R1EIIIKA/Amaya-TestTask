using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelTask : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private GridSpawner _gridSpawner;

    private void Start()
    {
        GenerateTask();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _gridSpawner.RemoveGrid();
            GenerateTask();
        }
    
    }

    private void GenerateTask()
    {
        _gridSpawner.CreateGrid();
        string task = "Find the following cells:\n";
        
    }
}
