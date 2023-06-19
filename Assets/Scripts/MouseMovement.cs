using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float movementSpeed = 1f;

    private Vector3 _target;
    private Tweener _moveTweener;

    private void OnMouseDown()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = transform.position.z;

        _moveTweener?.Kill();
        _moveTweener = player.DOMove(_target, movementSpeed).SetEase(Ease.Linear);
    }
}
