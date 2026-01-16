using System;
using Core;
using Machamy.Attributes;
using Machamy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    
    public class IngameUI : MonoBehaviour
    {
        [SerializeField,VisibleOnly(EditableIn.EditMode)] private ScoreUI scoreUI;
        [SerializeField,VisibleOnly(EditableIn.EditMode)] private DeckUI deckUI;
        
        
        
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
        
        public void OnMenuClicked()
        {
            LogEx.Log("Ingame Menu Clicked");
            
        }
        
        public void OnDeckClicked()
        {
            LogEx.Log("Ingame Deck Clicked");
            
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