using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private float _spawnTime = 1;
    [SerializeField] private Transform _pathPatrolling;
    [SerializeField] private int _maxObjects;

    private Transform _createdObjects;
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
        if (_createdObjects == null)
        {
            CreateSpawnedGroup();
        }

        if (_createdObjects.childCount >= _maxObjects)
        {
            return;
        }

        var currentObject = Instantiate(_objectToSpawn, _createdObjects);
        currentObject.transform.position = transform.position;

        if (currentObject.TryGetComponent<WayPointsMovement>(out WayPointsMovement objectWay) == false)
        {
            objectWay = currentObject.AddComponent<WayPointsMovement>();
        }
        objectWay.SetPath(_pathPatrolling);
    }

    private void CreateSpawnedGroup()
    {
        _createdObjects = new GameObject().transform;
        _createdObjects.name = "CreatedObjects";
        _createdObjects.parent = transform;
    }
}
