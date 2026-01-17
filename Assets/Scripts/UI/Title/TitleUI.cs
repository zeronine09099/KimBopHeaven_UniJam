using System;
using Core;
using Machamy.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Title
{
    public class TitleUI : MonoBehaviour
    {

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

            GameManager.Instance.StartGame();
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