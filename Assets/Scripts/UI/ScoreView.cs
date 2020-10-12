﻿using System.Collections;
using UnityEngine;
using Game;
using Events;
using UnityEngine.UI;
namespace UI {

    public class ScoreView : MonoBehaviour {
    //Fields//
        [SerializeField] 
        private float _scoreCountDelay;
        
        [SerializeField] 
        private ScriptableIntValue _currentScoreAsset;
        
        private int _currentScore = 0;
        
        [SerializeField] 
        private EventListener _eventListener;

        [SerializeField] 
        private Text _scoreLabel;
        
    //Methods//
        private void Awake() {
            _eventListener.OnEventHappened += UpdateBehaviour;
        }

        private void UpdateBehaviour() {
            if (_currentScoreAsset.score > _currentScore) StartCoroutine(SetScoreCoroutine(_currentScoreAsset.score));
        }
    //Coroutine//
        private IEnumerator SetScoreCoroutine(int score) {
            while (_currentScore < score) {
                _currentScore++;
                _scoreLabel.text = $"{_currentScore}";
                yield return new WaitForSeconds(_scoreCountDelay);
            }
        }
    }
}

