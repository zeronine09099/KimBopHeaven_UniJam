using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Core;

public class TransitionController : MonoBehaviour
{
    public static TransitionController Instance { get; private set; }

    [SerializeField] private RectTransform transitionPanel;

    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform stop;
    [SerializeField] private RectTransform end;

    [SerializeField] private Ease easeType = Ease.InOutQuad;
    [SerializeField] private int state = 0; // 0: hide, 1: show, 2: hide again


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public async UniTask PlayTransition(int state)
    {
        this.state = state;
        Debug.Log($"Transition State: {state}");

        switch (state)
        {
            case 0:
                await transitionPanel.DOAnchorPosY(start.anchoredPosition.y, 1f).SetEase(easeType).SetUpdate(true).ToUniTask();
                break;
            case 1:
                await transitionPanel.DOAnchorPosY(stop.anchoredPosition.y, 1f).SetEase(easeType).SetUpdate(true).ToUniTask();
                break;
            case 2:
                await transitionPanel.DOAnchorPosY(end.anchoredPosition.y, 1f).SetEase(easeType).SetUpdate(true).ToUniTask();
                break;
        }
    }

}
