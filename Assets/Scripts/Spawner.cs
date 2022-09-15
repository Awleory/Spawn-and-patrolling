using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _objectToSpawn;
    [SerializeField] private float _spawnTime = 1;
    [SerializeField] private Transform _pathPatrolling;
    [SerializeField] private int _maxObjects;

    private Transform _createdObjects;
    private Coroutine _currentCoroutine;

    private void OnEnable()
    {
        CreateSpawnedGroup();
    }

    private void Update()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_spawnTime);

        while (_createdObjects != null && _createdObjects.childCount < _maxObjects)
        {
            var currentObject = Instantiate(_objectToSpawn, _createdObjects);
            currentObject.transform.position = transform.position;

            if (currentObject.TryGetComponent<WayPointsMovement>(out WayPointsMovement objectWay) == false)
            {
                objectWay = currentObject.AddComponent<WayPointsMovement>();
            }
            objectWay.SetPath(_pathPatrolling);

            yield return new WaitForSeconds(_spawnTime);
        }

        _currentCoroutine = null;
    }

    private void CreateSpawnedGroup()
    {
        _createdObjects = new GameObject().transform;
        _createdObjects.name = "CreatedObjects";
        _createdObjects.parent = transform;
    }
}
