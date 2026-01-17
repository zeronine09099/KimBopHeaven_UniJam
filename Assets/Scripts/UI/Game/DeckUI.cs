using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
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


        [Space(10)]
        [Header("Description")]
        [SerializeField] private DeckUIElement selectedElement = null;
        [SerializeField] private GameObject descriptionObject;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Ease descriptionEase = Ease.OutBack;



        [SerializeField] private Button closeButton;


        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        private void Awake()
        {
            UIManager.Instance.DeckUI = this;
            originalPosition = transform.position;

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Sound.SoundManager.Instance.PlaySfx(Sound.SoundReference.ButtonClickSFX);
                Hide();
            });

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

        /// <summary>
        /// 흑종원 인카운터를 위해서 마이너스 버튼을 활성화시키는 함수
        /// </summary>
        public void ActivateRemoveMode()
        {
            scrollRect.verticalNormalizedPosition = 1f;
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            foreach (var element in deckUIElements)
            {
                element.ActivateMinusButton();
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


        public void SwitchDesc(DeckUIElement element)
        {
            if (selectedElement == element)
            {
                selectedElement = null;
                descriptionText.text = null;
                descriptionObject.transform.DOScale(Vector3.zero, 0.3f).From(Vector3.one).SetEase(descriptionEase).OnComplete(() =>
                {
                    descriptionObject.SetActive(false);
                });
                return;
            }

            selectedElement = element;
            descriptionText.text = element.ingredient.description;
            descriptionObject.transform.localScale = Vector3.zero;
            descriptionObject.SetActive(true);
            descriptionObject.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(descriptionEase);
        }

    }
}