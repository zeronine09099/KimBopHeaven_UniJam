using System;
using Core;
using Machamy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Awake()
        {
            UIManager.Instance.TitleUI = this;
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnEnable()
        {
           
        }

        private void OnDisable()
        {

        }

        private void OnStartButtonClicked()
        {
            LogEx.Log("Start Button Clicked");

            GameManager.Instance.StartGame();
        }

    }
}