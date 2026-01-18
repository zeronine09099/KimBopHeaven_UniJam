using System;
using Core;
using Machamy.Utils;
using Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Title
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialPanel;


        private void Awake()
        {
            UIManager.Instance.TitleUI = this;

        }

        private void OnEnable()
        {
           
        }

        private void OnDisable()
        {

        }

        public void OnStartButtonClicked()
        {
            LogEx.Log("Start Button Clicked");
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
            GameManager.Instance.StartGame();
        }
        
        public void OnTutorialButtonClicked()
        {
            LogEx.Log("Tutorial Button Clicked");
            tutorialPanel.SetActive(true);
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
            // GameManager.Instance.StartTutorial();
        }

        public void OnCloseTutorialButtonClicked()
        {
            LogEx.Log("Close Tutorial Button Clicked");
            tutorialPanel.SetActive(false);
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
        }

        public void OnSettingsButtonClicked()
        {
            LogEx.Log("Settings Button Clicked");
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
            UIManager.Instance.SettingUI.Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}