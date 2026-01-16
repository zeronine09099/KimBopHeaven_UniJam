using System;
using Core;
using Database.Generated;
using Machamy.Attributes;
using Machamy.Utils;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    
    public class IngameUI : MonoBehaviour
    {
        [SerializeField,VisibleOnly(EditableIn.EditMode)] private ScoreUI scoreUI;
        [SerializeField,VisibleOnly(EditableIn.EditMode)] private DeckUI deckUI;
        
        
        [SerializeField] TextMeshProUGUI stageGoalScoreText;
        
        [SerializeField] Button menuButton;
        [SerializeField] Button deckButton;
        
        public ScoreUI ScoreUI => scoreUI;
        public DeckUI DeckUI => deckUI;
        
        private void Awake()
        {
            UIManager.Instance.InGameUI = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
       
        }

        private void OnDisable()
        {

        }
        
        public void InitializeForStage(StageInfo stageInfo)
        {
            stageGoalScoreText.text =((int)stageInfo.goalScore).ToString();
            
            ScoreUI.CurrentValue = 0;
            ScoreUI.TempValue = 0;
            ScoreUI.MaxValue = stageInfo.goalScore;
            ScoreUI.UpdateUIImmediate();
        }
        
        public void OnMenuClicked()
        {
            LogEx.Log("Ingame Menu Clicked");
            UIManager.Instance.SettingUI.Show();
        }
        
        public void OnDeckClicked()
        {
            LogEx.Log("Ingame Deck Clicked");
            DeckUI.ShowDeckForm();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
    
}