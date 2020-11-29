﻿using System.Collections.Generic;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game {

    public class EnemySpawner : MonoBehaviour {

        [SerializeField]
        private EventListener _updateEventListener;

        [SerializeField]
        private EventListener _carCollisionListener;

        [ValidateInput(nameof(ListHasDuplicates), "List has duplicates")]
        [SerializeField]
        private List<GameObject> _carPrefabs = new List<GameObject>();

        [SerializeField]
        private float _distanceToPlayerToSpawn;

        [SerializeField]
        private float _distanceToPlayerToDestroy;

        [SerializeField]
        private ScriptableFloatValue _playerPositionZ;

        [SerializeField]
        private ScriptableFloatValue _roadWidth;
        
        [SerializeField] 
        private ScriptableBoolValue _isHardScriptableBoolValue;
        
        private float _currentTimer;
        private float _spawnCooldown;
        private List<GameObject> _cars = new List<GameObject>();

        private void OnEnable() {
            SubscribeToEvents();
            SetDifficulty();
        }

        private void OnDisable() {
            UnsubscribeToEvents();
        }

        private void SetDifficulty() {
            if (_isHardScriptableBoolValue.value) {
                _spawnCooldown = 2f;
            }
            else {
                _spawnCooldown = 5f;
            }
        }
        
        private void SubscribeToEvents() {
            _updateEventListener.OnEventHappened += UpdateBehaviour;
            _carCollisionListener.OnEventHappened += OnCarCollision;
        }

        private void UnsubscribeToEvents() {
            _updateEventListener.OnEventHappened -= UpdateBehaviour;
            _carCollisionListener.OnEventHappened -= OnCarCollision;
        }

        private void OnCarCollision() {
            UnsubscribeToEvents();
        }

        private void UpdateBehaviour() {
            HandleCarsBehindPlayer();

            _currentTimer += Time.deltaTime;
            if (_currentTimer < _spawnCooldown) {
                return;
            }
            _currentTimer = 0f;

            SpawnCar();
        }

        private void SpawnCar() {
            var randomRoad = Random.Range(-1, 2);
            var randomCar = Random.Range(0, _carPrefabs.Count);
            var _carPrefab = _carPrefabs[randomCar];
            var position = new Vector3(1f * randomRoad * _roadWidth.value, 0f, _playerPositionZ.value + _distanceToPlayerToSpawn);
            var car = Instantiate(_carPrefab, position, Quaternion.Euler(0f, 180f, 0f));
            _cars.Add(car);
        }

        private void HandleCarsBehindPlayer() {
            for (int i = _cars.Count - 1; i > -1; i--) {
                if (_playerPositionZ.value - _cars[i].transform.position.z > _distanceToPlayerToDestroy) {
                    Destroy(_cars[i]);
                    _cars.RemoveAt(i);
                }
            }
        }

        private bool ListHasDuplicates(List<GameObject> list) {
            var hasDuplicates = false;

            var set = new HashSet<GameObject>();
            foreach (var car in list) {
                if (!set.Add(car)) {
                    hasDuplicates = true;
                    break;
                }
            }
            
            return !hasDuplicates;
        }
    }
}