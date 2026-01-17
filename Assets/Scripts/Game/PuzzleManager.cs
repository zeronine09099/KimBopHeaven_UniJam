using BandoWare.GameplayTags;
using Common.Singleton;
using Core;
using DG.Tweening;
using Game.Field;
using Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Attributes;
using Common.Extentions;
using Cysharp.Threading.Tasks;
using Machamy.Attributes;
using Machamy.Utils;
using Player;
using Sound;
using UnityEngine;


namespace Game
{
    public class PlayerInputData
    {
        public Tile firstTile;
        public Tile secondTile;
    }
    
    public class TriggerArguments
    {
        public Tile Tile;
        public IngredientObject Ingredient;
        public PuzzleManager.MatchData MatchData;
        public List<(Tile, Func<UniTask>)> AfterMatchActions = new ();
        
        public Tile PreviousTile => MatchData.IndexOf(Tile) > 0 ? MatchData[MatchData.IndexOf(Tile) - 1] : null;
        public Tile NextTile => MatchData.IndexOf(Tile) < MatchData.Count - 1 ? MatchData[MatchData.IndexOf(Tile) + 1] : null;
        
        public int CountExectTag(GameplayTag tag)
        {
            int count = 0;
            foreach (var tile in MatchData)
            {
                if (tile.CurrentIngredient != null && tile.CurrentIngredient.Data.Tag == tag)
                {
                    count++;
                }
            }
            return count;
        }
        
        public int CountHasTag(GameplayTag parentTag)
        {
            int count = 0;
            foreach (var tile in MatchData)
            {
                if (tile.CurrentIngredient != null && tile.CurrentIngredient.Data.Tag.IsChildOf(parentTag))
                {
                    count++;
                }
            }
            return count;
        }
        
        public int CountAdjacentHasTag(GameplayTag parentTag)
        {
            int count = 0;
            int currentIndex = MatchData.IndexOf(Tile);
            // 왼쪽
            if (currentIndex > 0)
            {
                var leftIngredient = MatchData[currentIndex - 1].CurrentIngredient;
                if (leftIngredient != null && leftIngredient.Data.Tag.IsChildOf(parentTag))
                {
                    count++;
                }
            }
            // 오른쪽
            if (currentIndex < MatchData.Count - 1)
            {
                var rightIngredient = MatchData[currentIndex + 1].CurrentIngredient;
                if (rightIngredient != null && rightIngredient.Data.Tag.IsChildOf(parentTag))
                {
                    count++;
                }
            }
            return count;
        }
        
        public int CountAdjacentExactTag(GameplayTag tag)
        {
            int count = 0;
            int currentIndex = MatchData.IndexOf(Tile);
            // 왼쪽
            if (currentIndex > 0)
            {
                var leftIngredient = MatchData[currentIndex - 1].CurrentIngredient;
                if (leftIngredient != null && leftIngredient.Data.Tag == tag)
                {
                    count++;
                }
            }
            // 오른쪽
            if (currentIndex < MatchData.Count - 1)
            {
                var rightIngredient = MatchData[currentIndex + 1].CurrentIngredient;
                if (rightIngredient != null && rightIngredient.Data.Tag == tag)
                {
                    count++;
                }
            }
            return count;
        }
    }
    
    public class PuzzleManager : Singleton<PuzzleManager>
    {
        protected override void AfterAwake()
        {
        }
        [Header("제한 설정")]
        [SerializeField, Label("초기 재료 섞기"), Tooltip("게임 시작 시 필드에 배치되는 재료들을 섞어서 배치합니다. 제한설정과 관련있습니다.")] 
        private bool shuffleInitials = true;
        [SerializeField, Label("소환된 재료 섞기"), Tooltip("낙하하는 재료들을 섞어서 배치합니다. 제한설정과 관련있습니다.")] 
        private bool shuffleSummons = true;
        [SerializeField, Label("최소 김 개수")] private int MinimumGimCount = 2;
        [SerializeField, Label("최소 밥 개수")] private int MinimumRiceCount = 1;
        [SerializeField,VisibleOnly] private int GimCount = 0;
        [SerializeField,VisibleOnly] private int RiceCount = 0;
        [Header("애니메이션 설정")]
        [Tooltip("타일이 교체되는 애니메이션 시간")]
        [SerializeField] private float swapDuration = 0.3f;

        [Tooltip("재료가 낙하하는 애니메이션 시간")]
        [SerializeField] private float fallDuration = 0.4f;
        
        [SerializeField] private float triggerInterval = 0.1f;
        [SerializeField] private float matchInterval = 0.2f;
        [SerializeField] private float explodeDelay = 0.0f;

        [SerializeField] private Ease ease = Ease.InQuad;   // 애니메이션 효과 종류
        [SerializeField] private Ease fallEase = Ease.InSine;
        [SerializeField] private IngredientObject ingredientPrefab;
        
        private Field.Field field;
        private Tile firstSelectedTile;
        private Tile secondSelectedTile;
        


        private Tile FirstSelectedTile
        {
            get => firstSelectedTile;
            set
            {
                firstSelectedTile = value;
                if (firstSelectedTile != null)
                {
                    Debug.Log($"FirstSelectedTile set: {firstSelectedTile}");
                }
            }
        }

        private Tile SecondSelectedTile
        {
            get => secondSelectedTile;
            set
            {
                secondSelectedTile = value;
                if (secondSelectedTile != null)
                {
                    Debug.Log($"SecondSelectedTile set: {secondSelectedTile}");
                }
            }
        }

        public List<MatchData> CurrentMatches { get; set; } = new List<MatchData>();
        
        public int ThisTurnCompletedKimbapCount { get; set; }

        private bool isSwapping = false;

        public void Start()
        {
            Debug.Log("Start 진입");
            InteractionManager.Instance.OnTilePressed += OnTilePressed;
            InteractionManager.Instance.OnTileReleased += OnTileReleased;
        }

        public void Update()
        {
            // // 스와이프 중이 아닐 때만 마우스 따라가기 설정
            // if ( isSwapping == false && firstSelectedTile != null && firstSelectedTile.CurrentIngredient != null)
            // {
            //     Vector3 targetPos = InteractionManager.Instance.CurrentWorldPosition;
            //     targetPos.z = 0; // 2d 평면 유지
            //
            //     firstSelectedTile.CurrentIngredient.transform.position = targetPos;
            // }
            
            
        }

        public void SetPlayerTurn(bool isPlayerTurn)
        {
            this.isPlayerTurn = isPlayerTurn;
        }
        
        private bool isPlayerTurn = true;
        
        public void OnTilePressed(Tile pressedTile)
        {

            if (!isPlayerTurn || isSwapping)
            {
                return;
            }
            LogEx.Log($"Pressed Tile: {pressedTile}");
            if (FirstSelectedTile != null)
            {
                SecondSelectedTile = pressedTile;
                
                return;
            }
            FirstSelectedTile = pressedTile;
        }

        public void OnTileReleased(Tile releasedTile)
        {
            if (FirstSelectedTile == null)
            {
                return;
            }
            Debug.Log($"Released Tile: {releasedTile}");
            if(releasedTile == null)
            {
                // 필드 밖에서 놓았을때도 원래 위치로 복귀
                ResetFirstSelection();
                return; 
            }

            if(releasedTile != FirstSelectedTile)
            {
                if (!isPlayerTurn || isSwapping)
                {
                    ResetFirstSelection();
                    return;
                }
                SecondSelectedTile = releasedTile;
                
            }
            else
            {
                // 제자리에 놓았을때 스왑x, 위치 초기화
                ResetFirstSelection();
            }
        }

        /// <summary>
        /// 드래그 취소 혹은 완료 후 재료 위치 복귀 함수
        /// </summary>
        private void ResetFirstSelection()
        {
            if (FirstSelectedTile != null && FirstSelectedTile.CurrentIngredient != null)
            {
                FirstSelectedTile.CurrentIngredient.transform.localPosition = Vector3.zero;
            }
            FirstSelectedTile = null;
        }

        /// <summary>
        /// 퍼즐 매니저 초기화. 필드 정보를 받아옴
        /// </summary>
        /// <param name="field">게임 필드 참조</param>
        public void Initialize(Field.Field field)
        {
            this.field = field;
            InitializePossibleIngredients();
            SpawnInitialIngredients();
        }

        private void SpawnInitialIngredients()
        {
            Debug.Log("spawnInitial 진입");
            if (ingredientPrefab == null)
            {
                Debug.LogWarning("puzzlemanager: 테스트재료나 프리팹이 비어있음");
                return;
            }
            List<IngredientObject> allIngredients = new List<IngredientObject>();
            List<IngredientSO> allIngredientSOs = new List<IngredientSO>();
            GimCount = 0;
            RiceCount = 0;

            for (int i = 0; i < field.Height; i++)
            {
                for (int j = 0; j < field.Width; j++)
                {
                    Tile tile = field.GetTile(i, j);

                    IngredientSO randomData = GetNextIngredientData();
                    IngredientObject newIngredientObject = Instantiate(ingredientPrefab);
                    newIngredientObject.Initialize(randomData);
                    if (randomData.IsGim())
                    {
                        GimCount++;
                    }
                    else if (randomData.IsRice())
                    {
                        RiceCount++;
                    }

                    newIngredientObject.transform.SetParent(tile.transform);
                    newIngredientObject.transform.localPosition = Vector3.zero;
                    tile.SetIngredient(newIngredientObject);
                    
                    allIngredients.Add(newIngredientObject);
                    allIngredientSOs.Add(randomData);
                    
                }
            }
            
            if (shuffleInitials)
            {
                allIngredientSOs.Shuffle();
                for (int index = 0; index < allIngredients.Count; index++)
                {
                    IngredientObject ingredientObject = allIngredients[index];
                    IngredientSO ingredientSO = allIngredientSOs[index];
                    ingredientObject.Initialize(ingredientSO);
                }
            }
            int attempt = 0;
            while(FindWrapperMatches().Count > 0 || GimCount < MinimumGimCount || RiceCount < MinimumRiceCount)
            {
                InitializePossibleIngredients();
                LogEx.Log($"초기 매치 발견, 재배치 시도 {attempt + 1}회");
                attempt++;
                if (attempt > 100)
                {
                    LogEx.LogError("초기 매치 재배치 시도 100회 초과, 무한루프 방지 종료");
                    break;
                }
                GimCount = 0;
                RiceCount = 0;
                allIngredientSOs.Clear();

                for (int i = 0; i < field.Height; i++)
                {
                    for (int j = 0; j < field.Width; j++)
                    {
                        Tile tile = field.GetTile(i, j);
                        IngredientSO randomData = GetNextIngredientData();
                        IngredientObject ingredientObject = tile.CurrentIngredient;
                        allIngredientSOs.Add(randomData);
                        ingredientObject.Initialize(randomData);
                        if (randomData.IsGim())
                        {
                            GimCount++;
                        }
                        else if (randomData.IsRice())
                        {
                            RiceCount++;
                        }

                        ingredientObject.transform.SetParent(tile.transform);
                        ingredientObject.transform.localPosition = Vector3.zero;
                        tile.SetIngredient(ingredientObject);
                    }
                }

                if (shuffleInitials)
                {
                    allIngredientSOs.Shuffle();
                    for (int index = 0; index < allIngredients.Count; index++)
                    {
                        IngredientObject ingredientObject = allIngredients[index];
                        IngredientSO ingredientSO = allIngredientSOs[index];
                        ingredientObject.Initialize(ingredientSO);
                    }
                }
            }


        }

        private List<IngredientSO> possibleIngredients = new List<IngredientSO>();
        private List<IngredientSO> retrivedIngredients = new List<IngredientSO>();
        
        private void InitializePossibleIngredients()
        {
            possibleIngredients.Clear();
            retrivedIngredients.Clear();

            possibleIngredients.AddRange(PlayerState.Current.GameDeck.GetSOEnumerator());
            possibleIngredients.Shuffle();
        }
        
        private IngredientSO GetNextIngredientData()
        {
            if(possibleIngredients.Count == 0)
            {
               ShuffleRetrievedIngredients();
            }
            if(possibleIngredients.Count == 0)
            {
                return IngredientLibrary.Instance.GetIngredientSO(AllGameplayTags.Ingredient.Etc.Air.Get());
            }
            IngredientSO FindAndPop(GameplayTag tag)
            {
                for (int i = 0; i < possibleIngredients.Count; i++)
                {
                    if (possibleIngredients[i].Tag == tag)
                    {
                        IngredientSO ingredientSo = possibleIngredients[i];
                        possibleIngredients.RemoveAt(i);
                        return ingredientSo;
                    }
                }
                for (int i = 0; i < retrivedIngredients.Count; i++)
                {
                    if (retrivedIngredients[i].Tag == tag)
                    {
                        IngredientSO ingredientSo = retrivedIngredients[i];
                        retrivedIngredients.RemoveAt(i);
                        return ingredientSo;
                    }
                }
                return null;
            }
            
            // 최소 김 2개, 밥 1개 보장
            if (GimCount < MinimumGimCount)
            {
                IngredientSO gimSo = FindAndPop(AllGameplayTags.Ingredient.Essential.Gim.Get());
                if (gimSo != null)
                {
                    return gimSo;
                }
                else
                {
                    LogEx.LogWarning("김 재료가 부족합니다!");
                }
            }
            if (RiceCount < MinimumRiceCount)
            {
                IngredientSO riceSo = FindAndPop(AllGameplayTags.Ingredient.Essential.Rice.Get());
                if (riceSo != null)
                {
                    return riceSo;
                }
                else
                {
                    LogEx.LogWarning("밥 재료가 부족합니다!");
                }
            }
            
            var ingredientSo = possibleIngredients[possibleIngredients.Count - 1];
            possibleIngredients.RemoveAt(possibleIngredients.Count - 1);
            return ingredientSo;
        }
        
        public void ResetPossibleIngredients(List<IngredientSO> ingredientSOs)
        {
            possibleIngredients.Clear();
            possibleIngredients.AddRange(ingredientSOs);
            retrivedIngredients.Clear();
        }
        
        public void RetrieveIngredient(IngredientSO ingredientSO)
        {
            retrivedIngredients.Add(ingredientSO);
        }

        public void AddIngredientToRetrieved(IngredientSO ingredientSO, int count)
        {
            for (int i = 0; i < count; i++)
            {
                retrivedIngredients.Add(ingredientSO);
            }
        }

        /// <summary>
        /// possibleIngredients에 직접 재료를 추가합니다 (즉시 적용)
        /// </summary>
        public void AddToPossibleIngredients(IngredientSO ingredientSO, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                possibleIngredients.Add(ingredientSO);
            }
        }

        public void ShuffleRetrievedIngredients()
        {
            possibleIngredients.AddRange(retrivedIngredients);
            retrivedIngredients.Clear();
            possibleIngredients.Shuffle();
        }

        public async UniTask<PlayerInputData> GetPlayerInput(CancellationToken cancellationToken)
        {
            isPlayerTurn = true;
            FirstSelectedTile = null;
            SecondSelectedTile = null;

            // 플레이어가 두 타일을 선택할 때까지 대기
            await UniTask.WaitUntil(() => SecondSelectedTile != null, cancellationToken: cancellationToken);
            isPlayerTurn = false;
            return new PlayerInputData
            {
                firstTile = FirstSelectedTile,
                secondTile = SecondSelectedTile
            };
        }
        
        /// <summary>
        /// 입력처리
        /// </summary>
        /// <param name="tileA"></param>
        /// <param name="tileB"></param>
        /// <returns></returns>
        public async UniTask<List<MatchData>> ProcessInput(PlayerInputData input, CancellationToken cancellationToken)
        {
            isSwapping = true; 
            Tile tileA = input.firstTile;
            Tile tileB = input.secondTile;
            SoundManager.Instance.PlaySfx(SoundReference.SwipeSFX);
            await SwapTile(tileA, tileB, cancellationToken);
            List<MatchData> matchGroups = FindWrapperMatches();
            
            FirstSelectedTile = null;
            SecondSelectedTile = null;
            isSwapping = false;
            return matchGroups;
        }
        
        

        /// <summary>
        /// 매치 처리
        /// </summary>
        /// <param name="matchGroups"></param>
        public async UniTask ProcessMatches(List<MatchData> matchGroups, CancellationToken cancellationToken)
        {
            // 트리거
            HashSet<Tile> toExplodeTiles = new ();
            
            SoundManager.Instance.ResetScoreSfxPitchIndex();
            PlayerState.Current.CurrentTempMultiplier = 1;
            foreach (var match in matchGroups)
            {
                foreach (var tile in match)
                {
                    var args = new TriggerArguments
                    {
                        Tile = tile,
                        Ingredient = tile.CurrentIngredient,
                        MatchData = match
                    };
                    await tile.CurrentIngredient.Data.OnTrigger(args).AttachExternalCancellation(cancellationToken);
                    await UniTask.Delay(TimeSpan.FromSeconds(triggerInterval), cancellationToken: cancellationToken);
                    // 임시 점수를 실제 점수에 반영
                    CurrentMatches.Add(match);
                    ThisTurnCompletedKimbapCount++;
                    PlayerState.Current.CurrentStageScore = PlayerState.Current.CurrentTempScore;

                    foreach (var (t,f) in args.AfterMatchActions)
                    {
                        await f().AttachExternalCancellation(cancellationToken);
                    }


                    LogEx.Log($"Current Stage Score: {PlayerState.Current.CurrentStageScore}");
                }
                // toExplodeTiles.AddRange(match);
                foreach (var tile in match)
                {
                    toExplodeTiles.Add(tile);
                }
                // 점수 곱배수 적용
                PlayerState.Current.CurrentTempMultiplier *= 1.5f;
                await UniTask.Delay(TimeSpan.FromSeconds(matchInterval), cancellationToken: cancellationToken);
            }
            
            List<UniTask> explodeTasks = new(toExplodeTiles.Count);

            foreach (var tile in toExplodeTiles)
            {
                // 1. 참조 캐싱 (매우 중요)
                // tile.CurrentIngredient가 나중에 null이 되어도 이 변수는 객체를 가리킴
                var targetIngredient = tile.CurrentIngredient;

                if (targetIngredient == null) 
                    continue;

                // 2. 태스크 생성 및 캔슬레이션 연결
                // ContinueWith()가 특별한 로직 없이 사용되었다면 불필요하므로 제거
                var data = targetIngredient.Data;
                if (data.IsGim())
                {
                    GimCount--;
                }
                else if (data.IsRice())
                {
                    RiceCount--;
                }
                var task = data.OnExplode(tile)
                    .ContinueWith(() =>
                    {
                        var targetObj = targetIngredient.gameObject;
                        targetObj.SetActive(false);
                
                        Destroy(targetObj, 2f); 
                    })
                    .AttachExternalCancellation(cancellationToken);
                explodeTasks.Add(task);

                // 3. 데이터 로직 처리
                RetrieveIngredient(targetIngredient.Data);
                tile.SetIngredient(null); // 타일에서 논리적 연결 해제
                

            }

            await UniTask.WhenAll(explodeTasks);
            
            
        }

        
        /// <summary>
        /// 재료 생성하고 떨어트리기
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async UniTask WrapUpTurn(CancellationToken cancellationToken)
        {
            float tileDelta = - field.GetTile(0,0).transform.position.y + field.GetTile(1,0).transform.position.y;
            List<IngredientObject> summoningIngredients = new ();
            List<IngredientSO> newlyAddedIngredients = new ();
            List<Sequence> fallSequences = new ();
            int start = 0;
           for(int j = 0; j < field.Width; j++)
           {
               for (int i = 0; i < field.Height; i++)
               {
                   bool foundIngredient = false;
                   int upDelta = 1;
                   Tile tile = field.GetTile(i, j);
                   if (tile.CurrentIngredient == null)
                   {
                       // 빈 타일 발견, 위에서부터 재료를 찾아서 떨어뜨림
                       for (int k = 1; k <= field.Height; k++)
                       {
                           int sourceRow = i + k;
                           if (sourceRow >= field.Height)
                               break;

                           Tile sourceTile = field.GetTile(sourceRow, j);
                           if (sourceTile.CurrentIngredient != null)
                           {
                               // 재료 발견, 떨어뜨리기
                               foundIngredient = true;
                               IngredientObject fallingIngredient = sourceTile.CurrentIngredient;
                               sourceTile.SetIngredient(null);
                               tile.SetIngredient(fallingIngredient);
                               
                               // 애니메이션 처리
                               fallingIngredient.transform.SetParent(tile.transform);
                               Sequence fallSequence = DOTween.Sequence();
                               fallSequence.Append(fallingIngredient.transform.DOLocalMove(Vector3.zero, fallDuration).SetEase(fallEase));
                               fallSequences.Add(fallSequence);
                               break; // 다음 빈 타일로 이동
                           }
                       }
                          if (!foundIngredient)
                          {
                            // 위에 재료가 없는 경우 새로 생성
                            IngredientSO randomData = GetNextIngredientData();
                            
                            IngredientObject newIngredientObject = Instantiate(ingredientPrefab);
                            newIngredientObject.Initialize(randomData);
                            newIngredientObject.transform.SetParent(tile.transform);
                            newIngredientObject.transform.position = field.GetTile(field.Height - 1, j).transform.position + new Vector3(0, tileDelta * upDelta, 0);
                            upDelta++;
                            tile.SetIngredient(newIngredientObject);
                            
                            if (randomData.IsGim())
                            {
                                GimCount++;
                            }
                            else if (randomData.IsRice())
                            {
                                RiceCount++;
                            }
                            
                            // 애니메이션 처리
                            Sequence fallSequence = DOTween.Sequence();
                            fallSequence.Append(newIngredientObject.transform.DOLocalMove(Vector3.zero, fallDuration).SetEase(fallEase));
                            fallSequences.Add(fallSequence);
                            summoningIngredients.Add(newIngredientObject);
                            newlyAddedIngredients.Add(randomData);
                          }
                   }
                   
               }

           }

           if (shuffleSummons)
           {
               newlyAddedIngredients.Shuffle();
               if (newlyAddedIngredients.Count != summoningIngredients.Count)
               {
                   LogEx.LogError(("낙하 재료 수와 새로 추가된 재료 수가 일치하지 않음"));
               }
           
               for (int index = 0; index < summoningIngredients.Count; index++)
               {
                     IngredientObject ingredientObject = summoningIngredients[index];
                     IngredientSO ingredientSO = newlyAddedIngredients[index];
                     ingredientObject.Initialize(ingredientSO);
               }
           }
           
           
              // 모든 낙하 애니메이션 대기 
                await UniTask.WhenAll(fallSequences.ConvertAll(seq => seq.ToUniTask(cancellationToken: cancellationToken,
                    tweenCancelBehaviour: TweenCancelBehaviour.Complete)));
           
        }
        
        /// <summary>
        /// 두 타일의 데이터(ingredient)를 교환하고 애니메이션 시간만큼 대기
        /// </summary>
        /// <returns></returns>
        private async UniTask SwapTile(Tile tileA, Tile tileB, CancellationToken cancellationToken) 
        {
            Debug.Log("SwapTile 진입");

            IngredientObject tempIngredient = tileA.CurrentIngredient;
            tileA.SetIngredient(tileB.CurrentIngredient);
            tileB.SetIngredient(tempIngredient);

            
            Sequence swapSequence = DOTween.Sequence();
            // 데이터 교환 후 비중러 위치도 교환된 부모에 맞춰 정렬
            if (tileA.CurrentIngredient != null)
            {
                tileA.CurrentIngredient.transform.SetParent(tileA.transform);
                swapSequence.Append(tileA.CurrentIngredient.transform.DOLocalMove(Vector3.zero, swapDuration).SetEase(ease));
            }
            if (tileB.CurrentIngredient != null)
            {
                tileB.CurrentIngredient.transform.SetParent(tileB.transform);
                swapSequence.Join(tileB.CurrentIngredient.transform.DOLocalMove(Vector3.zero, swapDuration).SetEase(ease));
            }



            await swapSequence.ToUniTask(cancellationToken: cancellationToken,
                tweenCancelBehaviour: TweenCancelBehaviour.Complete);
        }
        

        /// <summary>
        /// Field의 모든 행과 열을 검사하여 김밥을 만들 수있는 리스트를 찾는다
        /// </summary>
        /// <returns>존재하는 모든 김밥리스트의 리스트</returns>
        public List<MatchData> FindWrapperMatches()
        {
            List<MatchData> allMatches = new();

            //가로검사
            for (int i = 0; i < field.Height; i++)
            {
                List<Tile> row = new List<Tile>();
                for (int j = 0; j < field.Width; j++)
                {
                    row.Add(field.GetTile(i, j));
                }
                CheckLine(row, ref allMatches);
            }

            // 세로검사
            for (int j = 0; j < field.Width; j++ )
            {
                List<Tile> col = new List<Tile>(); 
                for(int i = field.Height - 1; i >= 0; i--)
                {
                    col.Add(field.GetTile(i, j));
                }
                CheckLine(col, ref allMatches);
            }

            return allMatches;
        }

        /// <summary>
        /// 필드에 존재하는 행과 열을 하나씩 받아서, 열 안에 존재하는 김밥리스를 찾는 함수
        /// </summary>
        /// <param name="line">행 혹은 열</param>
        /// <param name="matches">김밥 리스트를 저장할 리스트리스트</param>
        private void CheckLine(List<Tile> line, ref List<MatchData> matches)
        {
            int startGim = -1;

            bool firstRiceOk = false;
            int lastRiceIndex = -1;
            bool lastRiceOk = false;
            var candidateMatches = new List<Tile>();
            
            void ResetCandidate()
            {
                startGim = -1;
                firstRiceOk = false;
                lastRiceIndex = -1;
                lastRiceOk = false;
                candidateMatches.Clear();
            }
            
            for (int i = 0; i < line.Count; i++)
            {
                IngredientObject currentIngredient = line[i].CurrentIngredient;
                
                // 현재 타일의 재료가 김인 경우
                if (currentIngredient.Data.IsGim())
                {
                    if (startGim == -1)
                    {
                        startGim = i;
                    }
                    else
                    {
                        // 두번째 김임
                        
                        // 밥 유효성 확인
                        if(lastRiceIndex + 1 == i)
                        {
                            lastRiceOk = true;
                        }

                        if (firstRiceOk)
                        {
                            candidateMatches.Add(line[i]);
                            matches.Add(MatchData.FromList(candidateMatches));
                            ResetCandidate();
                            startGim = i;
                        }else if (lastRiceOk)
                        {
                            candidateMatches.Add(line[i]);
                            matches.Add(MatchData.FromListReversed(candidateMatches));
                            ResetCandidate();
                            startGim = i;
                        }
                        
                        // 유효한 김밥 조합이 아님
                        else
                        {
                            ResetCandidate();
                            startGim = i;
                        }
                    }
                }
                else if (currentIngredient.Data.IsRice())
                {
                    lastRiceIndex = i;
                    if (startGim != -1 && startGim + 1 == i)
                    {
                        firstRiceOk = true;
                    }
                }

                if (startGim != -1)
                {
                    candidateMatches.Add(line[i]);
                }
            }
        }


        public class MatchData : List<Tile>
        {
            public List<IngredientSO> MatchedIngredients = new List<IngredientSO>();
            
            

            public static MatchData FromList(List<Tile> tiles)
            {
                MatchData matchData = new MatchData();
                foreach (var tile in tiles)
                {
                    matchData.Add(tile);
                    matchData.MatchedIngredients.Add(tile.CurrentIngredient.Data);
                }
                return matchData;
            }
            
            public static MatchData FromListReversed(List<Tile> tiles)
            {
                MatchData matchData = new MatchData();
                for (int i = tiles.Count - 1; i >= 0; i--)
                {
                    matchData.Add(tiles[i]);
                    matchData.MatchedIngredients.Add(tiles[i].CurrentIngredient.Data);
                }
                return matchData;
            }

            public bool HasIngredientTag(GameplayTag get)
            {
                foreach (var ingredient in MatchedIngredients)
                {
                    if (ingredient.Tag == get)
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        public bool IsValidSwap(Tile inputDataFirstTile, Tile inputDataSecondTile)
        {
            TileVector a = inputDataFirstTile.Coordinate;
            TileVector b = inputDataSecondTile.Coordinate;
            if ((Math.Abs(a.i - b.i) == 1 && a.j == b.j) || (Math.Abs(a.j - b.j) == 1 && a.i == b.i))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
