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
using Common.Extentions;
using Cysharp.Threading.Tasks;
using Machamy.Utils;
using Player;
using UnityEngine;


namespace Game
{
    public class PlayerInputData
    {
        public Tile firstTile;
        public Tile secondTile;
    }
    
    public class PuzzleManager : Singleton<PuzzleManager>
    {
        protected override void AfterAwake()
        {
        }

        [Tooltip("타일이 교체되는 애니메이션 시간")]
        [SerializeField] private float swapDuration = 0.3f;

        [Tooltip("재료가 낙하하는 애니메이션 시간")]
        [SerializeField] private float fallDuration = 0.4f;
        
        [SerializeField] private float triggerInterval = 0.1f;
        [SerializeField] private float matchInterval = 0.2f;
        [SerializeField] private float explodeDelay = 0.2f;

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
            LogEx.Log($"Pressed Tile: {pressedTile}");
            if (!isPlayerTurn || isSwapping)
            {
                return;
            }
            if (FirstSelectedTile != null)
            {
                SecondSelectedTile = pressedTile;
                
                return;
            }
            FirstSelectedTile = pressedTile;
        }

        public void OnTileReleased(Tile releasedTile)
        {
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
            if(ingredientPrefab == null){
                Debug.LogWarning("puzzlemanager: 테스트재료나 프리팹이 비어있음");
                return;
            }

            for(int i = 0; i < field.Height ; i++)
            {
                for (int j = 0; j < field.Width ; j++) 
                {
                    Tile tile = field.GetTile(i, j);

                    IngredientSO randomData = GetNextIngredientData();
                    IngredientObject newIngredientObject = Instantiate(ingredientPrefab);
                    newIngredientObject.Initialize(randomData);
                    newIngredientObject.transform.SetParent(tile.transform);
                    newIngredientObject.transform.localPosition = Vector3.zero;
                    tile.SetIngredient(newIngredientObject);
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
        
        public void ShuffleRetrievedIngredients()
        {
            possibleIngredients.AddRange(retrivedIngredients);
            retrivedIngredients.Clear();
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
            List<Tile> toExplodeTiles = new List<Tile>();
            
            foreach (var match in matchGroups)
            {
                foreach (var tile in match)
                {
                    await tile.CurrentIngredient.Data.OnTrigger(tile).AttachExternalCancellation(cancellationToken);
                    await UniTask.Delay(TimeSpan.FromSeconds(triggerInterval), cancellationToken: cancellationToken);
                }
                toExplodeTiles.AddRange(match);
                await UniTask.Delay(TimeSpan.FromSeconds(matchInterval), cancellationToken: cancellationToken);
            }
            
            // 폭발
            foreach (var tile in toExplodeTiles)
            {
                await tile.CurrentIngredient.Data.OnExplode(tile).AttachExternalCancellation(cancellationToken);
                await UniTask.Delay(TimeSpan.FromSeconds(explodeDelay), cancellationToken: cancellationToken);
            }
            
        }

        
        /// <summary>
        /// 재료 생성하고 떨어트리기
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async UniTask WrapUpTurn(CancellationToken cancellationToken)
        {
            float tileDelta = - field.GetTile(0,0).transform.position.y + field.GetTile(1,0).transform.position.y;
            List<Tile> fallingIngredients = new ();
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
                               
                               fallingIngredients.Add(tile);
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
                            
                            // 애니메이션 처리
                            Sequence fallSequence = DOTween.Sequence();
                            fallSequence.Append(newIngredientObject.transform.DOLocalMove(Vector3.zero, fallDuration).SetEase(fallEase));
                            
                            fallingIngredients.Add(tile);
                          }
                   }
                   
               }

           }
           
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
        private List<MatchData> FindWrapperMatches()
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
                for(int i = 0; i< field.Height; i++)
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
                if (currentIngredient.Data.IsGim() == true)
                {
                    if (startGim == -1)
                    {
                        startGim = i;
                    }
                    else
                    {
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
            public List<Tile> MatchedTiles = new List<Tile>();


            public static MatchData FromList(List<Tile> tiles)
            {
                MatchData matchData = new MatchData();
                foreach (var tile in tiles)
                {
                    matchData.Add(tile);
                }
                return matchData;
            }
            
            public static MatchData FromListReversed(List<Tile> tiles)
            {
                MatchData matchData = new MatchData();
                for (int i = tiles.Count - 1; i >= 0; i--)
                {
                    matchData.Add(tiles[i]);
                }
                return matchData;
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
