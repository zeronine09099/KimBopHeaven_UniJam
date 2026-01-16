using System;
using System.Collections.Generic;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class DeckUI : MonoBehaviour
    {
        
        private List<DeckUIElement> deckUIElements = new List<DeckUIElement>();
        
        [Header("Setting")]
        [SerializeField] private DeckUIElement deckUIElementPrefab;
        [SerializeField] private Vector2 originalPosition;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform contentTransform;
        
        
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        private void Awake()
        {
            UIManager.Instance.DeckUI = this;
            originalPosition = transform.position;
            Initialize();
            gameObject.SetActive(false);
        }

        private void Initialize()
        {
            foreach (var element in deckUIElements)
            {
                Destroy(element.gameObject);
            }
            deckUIElements.Clear();
            
            foreach (var ingredient in IngredientLibrary.Instance.AllIngredientList)
            {
                var element = Instantiate(deckUIElementPrefab, contentTransform);
                element.Initialize(ingredient);
                deckUIElements.Add(element);
            }
        }
        
        public void ShowDeckForm()
        {
            scrollRect.verticalNormalizedPosition = 1f;
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            foreach (var element in deckUIElements)
            {
                element.SetDeckForm();
                element.Refresh();
            }
            ShowTask(cancellationTokenSource.Token).Forget();
        }
        
        public void ShowDeleteForm()
        {
            scrollRect.verticalNormalizedPosition = 1f;
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
           foreach (var element in deckUIElements)
           {
               element.SetDeleteForm();
               element.Refresh();
           }
           ShowTask(cancellationTokenSource.Token).Forget();
        }
        
        public async UniTask ShowTask(CancellationToken cancellationToken = default)
        {
            try
            {
                gameObject.SetActive(true);
           
                await UniTask.Yield(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // 취소된 경우 처리
            }

        }
        
        
        public void Hide()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            gameObject.SetActive(false);
        }
        
    }
}