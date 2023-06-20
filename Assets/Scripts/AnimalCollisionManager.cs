using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class AnimalsCollisionManager : MonoBehaviour
{
    private GameObject _obj;
    private Vector3 _positionDelta;
    private Vector3 _playerDestination;
    private float _playerSpeed;

    private const string _player = "Player";
    private int _playerLayer;

    private const string _safeZone = "SafeZone";
    private int _safeZoneLayer;

    public bool IsInGroup;

    private void Start()
    {
        _playerLayer = LayerMask.NameToLayer(_player);
        _safeZoneLayer = LayerMask.NameToLayer(_safeZone);
    }

    private void OnEnable()
    {
        EventBus.OnPlayerMoved += MoveAnimalsWithPlayer;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerMoved -= MoveAnimalsWithPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsInGroup && collision.gameObject.layer == _playerLayer && AnimalsGroupMovementManager.Instance.AnimalsGroupSize < 5)
        {
            IsInGroup = true;
            _obj = collision.gameObject;
            _positionDelta = transform.position - _obj.transform.position;

            EventBus.OnPlayerTouchedAnimal?.Invoke(_positionDelta, this.gameObject);

            if (DOTween.IsTweening(transform))
                transform.DOPause();

            transform.DOMove(_playerDestination + _positionDelta, _playerSpeed / 1.5f).SetEase(Ease.Linear);
        }

        if (IsInGroup && collision.gameObject.layer == _safeZoneLayer)
        {
            IsInGroup = false;
            gameObject.SetActive(false);

            EventBus.OnAnimalSaved?.Invoke(_positionDelta, this.gameObject);
        }
    }

    private void MoveAnimalsWithPlayer(Vector3 destination, float movementSpeed)
    {
        _playerDestination = destination;
        _playerSpeed = movementSpeed;
    }
}
