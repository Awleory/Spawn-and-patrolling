using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private float _spawnTime = 1;
    [SerializeField] private Transform _pathPatrolling;

    private float _passedTime;

    private void Update()
    {
        if (_objectToSpawn == null || _spawnTime == 0)
        {
            return;
        }

        _passedTime += Time.deltaTime;

        if (_passedTime >= _spawnTime)
        {
            _passedTime -= _spawnTime;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        Instantiate(_objectToSpawn, gameObject.transform);
        if (_objectToSpawn.TryGetComponent<WayPointsMovement>(out WayPointsMovement objectsWay))
        {
            objectsWay.SetPath(_pathPatrolling);
        }
    }
}
