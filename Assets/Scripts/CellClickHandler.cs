using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CellClickHandler : MonoBehaviour
{
    [SerializeField] private LevelTask _levelTask;
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private float _winTime = 2f;

    private Camera _camera;
    private bool _canClick = true;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canClick)
        {
            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                var cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    if (cell.CellLetter == _levelTask.TaskLetter)
                    {
                        // _levelTask.CompleteTask();
                        BounceCell(cell);
                        SpawnParticle(cell);
                        Win();
                    }
                    else
                    {
                        BounceCell(cell);
                    }
                }
            }
        }
    }

    private void SpawnParticle(Cell cell)
    {
        Transform cellTransform = cell.transform;
        GameObject particle = Instantiate(_particlePrefab, cellTransform.position - Vector3.forward, Quaternion.identity, cellTransform);
        
        Destroy(particle, _winTime);
    }

    private void Win()
    {
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator WinCoroutine()
    {
        _canClick = false;
        
        yield return new WaitForSeconds(_winTime);
        
        _canClick = true;
        _levelTask.CompleteTask();
    }

    private void BounceCell(Cell cell)
    {
        if (cell.IsShaking) return;

        cell.IsShaking = true;

        cell.LetterObject.transform.DOShakePosition(_shakeDuration, new Vector3(0.3f, 0, 0), 10, 0, false, true)
            .SetEase(Ease.InBounce)
            .OnComplete(() => FinishShake(cell))
            .SetLink(cell.gameObject);
    }

    private void FinishShake(Cell cell)
    {
        cell.IsShaking = false;
    }
}