
using Core;
using Database.Generated;
using Machamy.Attributes;
using Machamy.Utils;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    
    public class IngameUI : MonoBehaviour
    {
        [SerializeField,VisibleOnly(EditableIn.EditMode)] private ScoreUI scoreUI;
        [SerializeField,VisibleOnly(EditableIn.EditMode)] private DeckUI deckUI;
        
        [SerializeField] TextMeshProUGUI stageIdText;
        [SerializeField] TextMeshProUGUI remainingMovesText;
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
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(OnMenuClicked);

            deckButton.onClick.RemoveAllListeners();
            deckButton.onClick.AddListener(OnDeckClicked);
        }

        private void OnDisable()
        {

        }
        
        public void InitializeForStage(StageInfo stageInfo)
        {
            stageGoalScoreText.text = $"{stageInfo.goalScore:N0}pt";
            
            ScoreUI.CurrentValue = 0;
            ScoreUI.TempValue = 0;
            ScoreUI.MaxValue = stageInfo.goalScore;
            ScoreUI.UpdateUIImmediate();
            
            stageIdText.text = $"{stageInfo.Stage}";
        }
        
        public void OnRemainingMovesChanged(int remainingMoves)
        {
            remainingMovesText.text = $"{remainingMoves}";
        }
        
        
        public void OnMenuClicked()
        {
            LogEx.Log("Ingame Menu Clicked");
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
            UIManager.Instance.SettingUI.Show();
        }
        
        public void OnDeckClicked()
        {
            LogEx.Log("Ingame Deck Clicked");
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
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