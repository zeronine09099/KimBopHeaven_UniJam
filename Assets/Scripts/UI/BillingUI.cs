using System.Threading;
using System.Threading.Tasks;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BillingUI : MonoBehaviour
    {
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private float showDuration = 0.5f;

        [SerializeField] private RectTransform startRect;
        [SerializeField] private RectTransform endRect;

        [Space(10)]
        [Header("실패")]
        [SerializeField] private CanvasGroup failPanel;
        [Header("성공")]
        [SerializeField] private CanvasGroup successPanel;
        [SerializeField] private CanvasGroup successBackground;

        [SerializeField] private CanvasGroup billingContent;
        [SerializeField] private SimpleNumberText targetScore;
        [SerializeField] private SimpleNumberText achievedScore;
        [SerializeField] private SimpleNumberText remainedScore;
        
        
        
        private void Awake()
        {
            UIManager.Instance.BillingUI = this;
            gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
        }
        
        public async UniTask ShowFailAsync(CancellationToken cancellationToken = default)
        {
            failPanel.gameObject.SetActive(true);
            successPanel.gameObject.SetActive(false);
            successBackground. gameObject.SetActive(false);
            billingContent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            
            // 중앙에서 확대되면서 나타나는 애니메이션
            Sequence seq = DOTween.Sequence();
            successBackground.transform.localScale = Vector3.zero;
            failPanel.transform.position = startRect.position;
            seq.Append(failPanel.GetComponent<RectTransform>().DOAnchorPosX(endRect.anchoredPosition.x, showDuration).SetEase(showEase));
            seq.Append(successBackground.transform.DOScale(Vector3.one, showDuration).SetEase(showEase));
            await seq.Play().ToUniTask(cancellationToken: cancellationToken,tweenCancelBehaviour: TweenCancelBehaviour.Kill);
            
        }

        private CancellationTokenSource _skipCts = new CancellationTokenSource();
        public async UniTask ShowSuccessAsync(CancellationToken cancellationToken = default)
        {
            _skipCts.Cancel();
            _skipCts.Dispose();
            _skipCts = new CancellationTokenSource();
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _skipCts.Token).Token;
            failPanel.gameObject.SetActive(false);
            successPanel.gameObject.SetActive(true);
            successBackground. gameObject.SetActive(true);
            billingContent.gameObject.SetActive(true);
            gameObject.SetActive(true);

            _isSkipRequested = false;
            successPanel.alpha = 1f;
            successBackground.alpha = 0f;
            billingContent.alpha = 0f;
            
            RectTransform billingRect = billingContent.GetComponent<RectTransform>();
            
            // 중앙에서 확대되면서 나타나는 애니메이션
            Sequence seq = DOTween.Sequence();
            //successPanel.transform.localScale = Vector3.zero;
            successPanel.transform.position = startRect.position;   
            seq.Append(successPanel.GetComponent<RectTransform>().DOAnchorPosX(endRect.anchoredPosition.x, showDuration).SetEase(showEase));
            seq.AppendInterval(1f);

            //seq.Append(successPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            seq.Append(successPanel.DOFade(0f, 0.3f));
            seq.Join(successBackground.DOFade(1f, 0.75f));
            // succesPanel 끝난후, (0.3초 뒤) 빌링 컨텐츠 아래에서 올라옴
            seq.AppendInterval(0.4f);
            billingRect.anchoredPosition = new Vector2(0, -Screen.height*1.5f);
            seq.AppendCallback(() =>
            {
                Sound.SoundManager.Instance.PlaySfx(Sound.SoundReference.BillSFX);
                billingContent.alpha = 1f;
            });
            seq.Append(billingRect.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutCubic));
            // 이후 점수 증가
            seq.AppendInterval(0.2f);
            int target = PlayerState.Current.CurrentStageInfo.goalScore;
            int achieved = PlayerState.Current.CurrentStageScore;
            int remained = Mathf.Max(0, achieved - target);
            seq.Append(targetScore.CountTo(0, target, 1f));
            seq.Append(achievedScore.CountTo(0, achieved, 1f));
            seq.Append(remainedScore.CountTo(0, remained, 1f));
            await seq.Play().ToUniTask(cancellationToken: cancellationToken,tweenCancelBehaviour: TweenCancelBehaviour.Complete);
            
        }
        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            // 아래로 사라지는 애니메이션
            RectTransform billingRect = billingContent.GetComponent<RectTransform>();
            Sequence seq = DOTween.Sequence();
            seq.Append(billingRect.DOAnchorPosY(-Screen.height*1.5f, 0.5f).SetEase(Ease.InCubic));
            seq.Append(successBackground.DOFade(0f, 0.5f));
            await seq.Play().ToUniTask(cancellationToken: cancellationToken,tweenCancelBehaviour: TweenCancelBehaviour.Kill);
            gameObject.SetActive(false);
        }

        private bool _isSkipRequested = false;
        
        public void RequestSkip()
        {
            if (_skipCts.IsCancellationRequested)
            {
                _isSkipRequested = true;
            }
            _skipCts.Cancel();
        }
        
        public async UniTask WaitForSkip(CancellationToken cancellationToken = default)
        {
            while (!_isSkipRequested)
            {
                await UniTask.Yield(cancellationToken);
            }
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }


    }
}