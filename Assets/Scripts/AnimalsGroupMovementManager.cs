using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimalsGroupMovementManager : MonoBehaviour
{
    [SerializeField] private float animalPatrolSpeed = 20f;
    [SerializeField] private Transform PlayerPosition;

    private BoxCollider2D _boxCollider;
    private Vector3 _boundsMin;
    private Vector3 _boundsMax;

    private Dictionary<Vector3, GameObject> _animalsInGroup = new Dictionary<Vector3, GameObject>();

    public ReactiveProperty<int> SavedAnimals = new ReactiveProperty<int>(0);

    public static AnimalsGroupMovementManager Instance;
    private int _animalsGroupSize;
    public int AnimalsGroupSize
    {
        get { return _animalsGroupSize; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boundsMin = _boxCollider.bounds.min;
        _boundsMax = _boxCollider.bounds.max;
    }

    private void OnEnable()
    {
        EventBus.OnAnimalSpawned += DoPatrolMovement;
        EventBus.OnPlayerTouchedAnimal += CreateGroup;
        EventBus.OnAnimalSaved += DeleteFromGroup;
        EventBus.OnPlayerMoved += MoveAnimalsWithPlayer;
    }

    private void OnDisable()
    {
        EventBus.OnAnimalSpawned -= DoPatrolMovement;
        EventBus.OnPlayerTouchedAnimal -= CreateGroup;
        EventBus.OnAnimalSaved -= DeleteFromGroup;
        EventBus.OnPlayerMoved -= MoveAnimalsWithPlayer;
    }

    private void CreateGroup(Vector3 positionDelta, GameObject animal)
    {
        _animalsInGroup.Add(positionDelta, animal);
        _animalsGroupSize = _animalsInGroup.Count;
    }

    private void DeleteFromGroup(Vector3 positionDelta, GameObject animal)
    {
        _animalsInGroup.Remove(positionDelta);
        _animalsGroupSize = _animalsInGroup.Count;

        SavedAnimals.Value++;
    }

    private void DoPatrolMovement(GameObject patrolingAnimal)
    {
        if (!patrolingAnimal.GetComponent<AnimalsCollisionManager>().IsInGroup)
        {
            Vector3 destination = new Vector3(Random.Range(_boundsMin.x, _boundsMax.x), Random.Range(_boundsMin.y, _boundsMax.y), 0);
            float speed = animalPatrolSpeed / Vector3.Distance(destination, transform.position);

            patrolingAnimal.transform.DOMove(destination, speed > 5f ? 5f : speed).SetEase(Ease.Linear).OnComplete(() => DoPatrolMovement(patrolingAnimal));
        }
    }

    private void MoveAnimalsWithPlayer(Vector3 destination, float movementSpeed)
    {
        foreach (var (key, value) in _animalsInGroup)
        {
            Transform animalTransform = value.transform;

            if (DOTween.IsTweening(animalTransform))
                animalTransform.DOPause();

            animalTransform.DOMove(destination + key, movementSpeed).SetEase(Ease.Linear);
        }
    }
}
