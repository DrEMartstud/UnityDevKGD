﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    
    public class UIManager : MonoBehaviour {

        [SerializeField] 
        private GameObject _menuScreen;
        
        [SerializeField] 
        private GameObject _gameScreen;
        
        [SerializeField] 
        private GameObject _leaderboardsScreen;
        
        public static UIManager Instance;
        
        [SerializeField]
        private Fader _fader;
        
        private string _currentSceneName = "Gameplay";

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Start() {
           
        }
        private void OnSceneFadeIn() {
            StartCoroutine(FadeOutAndLoadGameplay());
        }
        private IEnumerator FadeOutAndLoadGameplay() {
            yield return new WaitForSeconds(3f);
            _fader.OnFadeOut += LoadGameplayScene;
            _fader.FadeOut();
        }
        private void LoadGameplayScene() {
            _fader.OnFadeOut -= LoadGameplayScene;
            StartCoroutine(LoadSceneCoroutine(_currentSceneName));
            _currentSceneName = _currentSceneName == "Gameplay" ? "Menu" : "Gameplay";
        }
        private IEnumerator LoadSceneCoroutine(string sceneName) {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncOperation.isDone) {
                yield return null;
            }
            yield return new WaitForSeconds(3f);
            _fader.FadeIn();
        }

        public void ShowMenuScreen() {
            HideAllScreens();
            _menuScreen.SetActive(true);
        }
        
        public void ShowGameScreen() {
            HideAllScreens();
            _gameScreen.SetActive(true);
        }
        
        public void ShowLeaderboardsScreen() {
            HideAllScreens();
            _leaderboardsScreen.SetActive(true);
        }
        
        public void HideAllScreens() {
            _menuScreen.SetActive(false);
            _gameScreen.SetActive(false);
            _leaderboardsScreen.SetActive(false);
        }
    }
}