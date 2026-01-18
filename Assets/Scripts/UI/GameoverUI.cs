using System;
using System.Collections.Generic;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using TMPro;
using UI.GameOver;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

[System.Serializable]
public class PSElementViewData
{    
    public string KeyString;
    public PlayerState.VariableKey Key;
    public string DisplayName;
    
    public int Value => PlayerState.Current.Variables.Items[KeyString].IntValue;
    public float FloatValue => PlayerState.Current.Variables.Items[KeyString].FloatValue;
}

public class GameoverUI : MonoBehaviour
{
    PlayerState player;


    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;

    [Space(10)] 
    [Header("Player State Elements")] 
    [SerializeField] private PSElementUI psElementViewPrefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private List<PSElementViewData> psElementViews;
    [SerializeField] private List<PSElementUI> psElements;

    [SerializeField] private Button titleBtn;
    [SerializeField] private Button retryBtn;



    [Space(10)]
    [Header("Transition")]
    [SerializeField] private RectTransform[] papers;

    [Header("Score Animation")]
    [SerializeField] private float scoreFadeInDuration = 0.5f;

    [SerializeField] private float scoreIntervalDuration = 0.3f;


    private void Awake()
    {
        UIManager.Instance.GameoverUI = this;
        gameObject.SetActive(false);

        player = GameManager.Instance.PlayerStatus;
        // player.BestScore = 0;
        //player.CurrentStageInfo

        titleBtn.onClick.RemoveAllListeners();
        titleBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.CancelGameAndReturnToTitle();
        });

        retryBtn.onClick.RemoveAllListeners();
        retryBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.StartGame();
        });
    }

    // public void Show()
    // {
    //     //player.CurrentStageInfo.Stage
    //     scoreText.text = $"최종 점수 : {player.TotalScore:N0}pt";
    //     // highestScoreText.text = $"가장 효율적이었던 스와이프 : {player.BestSingleSwipeScore:N0}pt";
    //     gameObject.SetActive(true);
    // }
    List<Tween> tweens = new List<Tween>();
    public async UniTask ShowAsync(CancellationToken cancellationToken = default)
    {
        // //player.CurrentStageInfo.Stage
        // scoreText.text = $"최종 점수 : {player.TotalScore:N0}pt";
        // highestScoreText.text = $"최고 점수 : {player.BestScore:N0}pt";
        //스크롤뷰 맨위로
        scrollView. verticalNormalizedPosition = 1f;



        
        
        tweens.Clear();
        // 초기화
        for(int i = 0; i < psElements.Count; i++)
        {
            var element = psElementViews[i];
            var psElementView = psElements[i];
            var canvasGroup = psElementView.CanvasGroup;
            psElementView.PSName.text = element.DisplayName;
            psElementView.PSValue.text = element.Value.ToString("N0");

            psElementView.gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
            
        }       
        gameObject.SetActive(true);
        
        // 종이 트랜지션

        
        
        
        
        for(int i = 0; i < psElements.Count; i++)
        {
            var psElementView = psElements[i];
            var canvasGroup = psElementView.CanvasGroup;
            psElementView.gameObject.SetActive(true);
            var tween = canvasGroup.DOFade(1f, scoreFadeInDuration);
            tweens.Add(tween);

            scrollView.verticalNormalizedPosition = 1f - (float)(i + 1) / psElements.Count;
            await UniTask.Delay(TimeSpan.FromSeconds(scoreIntervalDuration), cancellationToken: cancellationToken);
        }
        await UniTask.WhenAll(tweens.Select(t => t.ToUniTask(cancellationToken: cancellationToken)));
    }

    public async UniTask WaitForHide(CancellationToken cancellationToken = default)
    {       
        while (gameObject.activeSelf)
        {
            await UniTask.Yield(cancellationToken);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    [ContextMenu("Initialize PSElementViews")]
    public void InitializePSElementViews()
    {
        psElementViews = new List<PSElementViewData>();
        foreach (PlayerState.VariableKey key in Enum.GetValues(typeof(PlayerState.VariableKey)))
        {
            if (key != PlayerState.VariableKey.None)
            {
                var element = new PSElementViewData
                {
                    Key = key,
                    KeyString = key.ToString(),
                    DisplayName = key.ToString()
                };
                psElementViews.Add(element);
            }
        }
    }
    
    #if UNITY_EDITOR

    [ContextMenu("Create PSElementUI")]
    public void CreatePSElementUI()
    {
        psElements = new List<PSElementUI>();
        for (int j = parentTransform.childCount - 1; j >= 0; j--)
        {
            DestroyImmediate(parentTransform.GetChild(j).gameObject);
        }
        foreach (var viewData in psElementViews)
        {
            var psElementView = UnityEditor.PrefabUtility.InstantiatePrefab(psElementViewPrefab, parentTransform) as PSElementUI;
            psElementView.PSName.text = viewData.DisplayName;
            psElementView.PSValue.text = "00";
            psElements.Add(psElementView);
        }
    }
    #endif

    // private void OnValidate()
    // {
    //     for(int i = 0; i < psElementViews.Count; i++)
    //     {
    //         var element = psElementViews[i];
    //         if (element.Key != PlayerState.VariableKey.None)
    //         {
    //             element.KeyString = element.Key.ToString();
    //             element.Key = element.Key;
    //             if (string.IsNullOrEmpty(element.DisplayName))
    //             {
    //                 element.DisplayName = element.Key.ToString();
    //             }
    //         }
    //         
    //         if (i < psElements.Count)
    //         {
    //             var psElementView = psElements[i];
    //             psElementView.PSName.text = element.DisplayName;
    //             psElementView.PSValue.text = "000";
    //         }
    //         else
    //         {
    //             psElements = new List<PSElementUI>();
    //             for (int j = parentTransform.childCount - 1; j >= 0; j--)
    //             {
    //                 DestroyImmediate(parentTransform.GetChild(j).gameObject);
    //             }
    //             foreach (var viewData in psElementViews)
    //             {
    //                 var psElementView = Instantiate(psElementViewPrefab, parentTransform);
    //                 psElementView.PSName.text = viewData.DisplayName;
    //                 psElementView.PSValue.text = viewData.Value.ToString("N0");
    //                 psElements.Add(psElementView);
    //             }
    //             break;
    //         }
    //         
    //         
    // }
    // }
}