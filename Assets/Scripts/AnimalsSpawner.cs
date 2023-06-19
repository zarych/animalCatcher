using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsSpawner : MonoBehaviour
{
    private bool _spawningActive = true;
    private int _animalsStartCount;
    private ObjectPool _objectPool;
    private BoxCollider2D _boxCollider;
    private Vector3 _boundsMin;
    private Vector3 _boundsMax;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boundsMin = _boxCollider.bounds.min;
        _boundsMax = _boxCollider.bounds.max;

        _objectPool = ObjectPool.Instance;
        _animalsStartCount = Random.Range(3, 6);

        StartCoroutine(spawnInTime());
    }

    IEnumerator spawnInTime()
    {
        yield return new WaitForSeconds(.1f);

        while (_animalsStartCount > 0)
        {
            SpawnAnimal();
            _animalsStartCount--;
        }

        while (_spawningActive)
        {
            yield return new WaitForSeconds(Random.Range(3, 6));
            SpawnAnimal();
        }
    }

    void SpawnAnimal()
    {
        GameObject animal = _objectPool.GetObjectFromPool();

        if (animal != null)
        {
            animal.SetActive(true);
            animal.transform.position = new Vector3(Random.Range(_boundsMin.x, _boundsMax.x), Random.Range(_boundsMin.y, _boundsMax.y), 0);
            EventBus.OnAnimalSpawned?.Invoke(animal);
        }
    }
}
