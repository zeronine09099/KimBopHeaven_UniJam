using Core;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour
{
    PlayerState player;


    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;

    [SerializeField] private Button titleBtn;
    [SerializeField] private Button retryBtn;



    [Space(10)]
    [Header("Transition")]
    [SerializeField] private RectTransform[] papers;



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

    public void Show()
    {
        //player.CurrentStageInfo.Stage
        scoreText.text = $"최종 점수 : {player.TotalScore:N0}pt";
        highestScoreText.text = $"가장 효율적이었던 스와이프 : {player.BestScore:N0}pt";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}