using BandoWare.GameplayTags;
using Common.Singleton;
using Core;
using DG.Tweening;
using Game.Field;
using Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gimbab = System.Collections.Generic.List<Game.Field.Tile>;

namespace Game
{
    public class PuzzleManager : Singleton<PuzzleManager>
    {
        protected override void AfterAwake()
        {
        }

        [Tooltip("타일이 교체되는 애니메이션 시간")]
        [SerializeField] private float swapDuration = 0.3f;

        [Tooltip("재료가 낙하하는 애니메이션 시간")]
        [SerializeField] private float fallDuration = 0.4f;

        [Tooltip("매치 판정 후 파괴되기 전 대기 시간")]
        [SerializeField] private float matchDelay = 0.5f;

        [SerializeField] private Ease ease = Ease.InQuad;   // 애니메이션 효과 종류
        [SerializeField] private IngredientObject ingredientPrefab;
        [SerializeField] private List<IngredientSO> testIngredients;

        private Field.Field field;
        private Tile firstSelectedTile;
        private Tile secondSelectedTile;
        private bool isSwapping = false;

        public void Start()
        {
            Debug.Log("Start 진입");
            InteractionManager.Instance.OnTilePressed += OnTilePressed;
            InteractionManager.Instance.OnTileReleased += OnTileReleased;
        }

        public void Update()
        {
            // 스와이프 중이 아닐 때만 마우스 따라가기 설정
            if ( isSwapping == false && firstSelectedTile != null && firstSelectedTile.CurrentIngredient != null)
            {
                Vector3 targetPos = InteractionManager.Instance.CurrentWorldPosition;
                targetPos.z = 0; // 2d 평면 유지

                firstSelectedTile.CurrentIngredient.transform.position = targetPos;
            }
        }

        public void OnTilePressed(Tile pressedTile)
        {
            Debug.Log("pressed");
            if (firstSelectedTile != null)
            {
                secondSelectedTile = pressedTile;
                StartCoroutine(TrySwapAndProcess(firstSelectedTile, secondSelectedTile));
                return;
            }
            firstSelectedTile = pressedTile;
        }

        public void OnTileReleased(Tile releasedTile)
        {
            Debug.Log("Released");
            if(releasedTile == null)
            {
                // 필드 밖에서 놓았을때도 원래 위치로 복귀
                ResetFirstSelection();
                return; 
            }

            if(releasedTile != firstSelectedTile)
            {
                secondSelectedTile = releasedTile;
                StartCoroutine(TrySwapAndProcess(firstSelectedTile, secondSelectedTile));
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
            if (firstSelectedTile != null && firstSelectedTile.CurrentIngredient != null)
            {
                firstSelectedTile.CurrentIngredient.transform.localPosition = Vector3.zero;
            }
            firstSelectedTile = null;
        }

        /// <summary>
        /// 퍼즐 매니저 초기화. 필드 정보를 받아옴
        /// </summary>
        /// <param name="field">게임 필드 참조</param>
        public void Initialize(Field.Field field)
        {
            this.field = field;

            SpawnInitialIngredients();
        }

        private void SpawnInitialIngredients()
        {
            Debug.Log("spawnInitial 진입");
            if(ingredientPrefab == null || testIngredients == null || testIngredients.Count == 0)
            {
                Debug.LogWarning("puzzlemanager: 테스트재료나 프리팹이 비어있음");
                return;
            }

            for(int i = 0; i < field.Height ; i++)
            {
                for (int j = 0; j < field.Width ; j++) 
                {
                    Tile tile = field.GetTile(i, j);

                    IngredientSO randomData = testIngredients[UnityEngine.Random.Range(0, testIngredients.Count)];
                    IngredientObject newIngredientObject = Instantiate(ingredientPrefab);
                    newIngredientObject.Initialize(randomData);
                    newIngredientObject.transform.SetParent(tile.transform);
                    newIngredientObject.transform.localPosition = Vector3.zero;
                    tile.SetIngredient(newIngredientObject);
                }
            }
        }

        private IngredientObject CreateRandomIngredientAt(Tile tile)
        {
            IngredientSO randomData = testIngredients[UnityEngine.Random.Range(0, testIngredients.Count)];
            IngredientObject newIngredientObject = Instantiate(ingredientPrefab);
            newIngredientObject.Initialize(randomData);
            newIngredientObject.transform.SetParent(tile.transform);
            newIngredientObject.transform.localPosition = Vector3.zero;
            tile.SetIngredient(newIngredientObject);
            return newIngredientObject;
        }

        /// <summary>
        /// 해당 재료가 김인지를 확인한다 
        /// </summary>
        /// <param name="ingredient">IngredientObjetct.Data를 이용해서 SO를 받는다</param>
        /// <returns></returns>
        private bool IsWrapper(IngredientSO ingredient)
        {
            if(ingredient.Tag == GameplayTagManager.RequestTag("Ingredient.Essential.Gim"))
            {
                return true;
            }

            return false;
        }

        public IEnumerator TrySwapAndProcess(Tile tileA, Tile tileB)
        {
            Debug.Log("tryswapandprocess 진입");
            isSwapping = true; 
            yield return StartCoroutine(SwapTile(tileA, tileB));

            List<Gimbab> matchGroups = FindWrapperMatches();

            // 매치된 김밥이 있으면 점수계산 및 파괴처리
            if (matchGroups.Count > 0)
            {
                yield return StartCoroutine(ProcessMatches(matchGroups));
            }

            firstSelectedTile = null;
            secondSelectedTile = null;
            isSwapping = false; 
        }

        /// <summary>
        /// 두 타일의 데이터(ingredient)를 교환하고 애니메이션 시간만큼 대기
        /// </summary>
        /// <returns></returns>
        private IEnumerator SwapTile(Tile tileA, Tile tileB)
        {
            Debug.Log("SwapTile 진입");

            IngredientObject tempIngredient = tileA.CurrentIngredient;
            tileA.SetIngredient(tileB.CurrentIngredient);
            tileB.SetIngredient(tempIngredient);

            // 데이터 교환 후 비중러 위치도 교환된 부모에 맞춰 정렬
            if (tileA.CurrentIngredient != null)
            {
                tileA.CurrentIngredient.transform.SetParent(tileA.transform);
                tileA.CurrentIngredient.transform.localPosition = Vector3.zero;
            }
            if (tileB.CurrentIngredient != null)
            {
                tileB.CurrentIngredient.transform.SetParent(tileB.transform);
                tileB.CurrentIngredient.transform.localPosition = Vector3.zero;
            }

            // TODO: 여기에 두트윈으로 타일 이동 애니메이션 추가
            yield return new WaitForSeconds(swapDuration);
        }

        private IEnumerator ProcessMatches(List<Gimbab> matchGroups)
        {
            // 파괴 전 사용자가 인지할 수 있도록 대기
            yield return new WaitForSeconds(matchDelay);

            HashSet<Tile> tilesToDeleteingredient = new HashSet<Tile>();
            List<Coroutine> explodeRoutines = new List<Coroutine>();

            // 각 김밥들의 점수를 계산하여 전달
            foreach (var group in matchGroups)
            {
                float groupScore = 0f;

                foreach(var tile in group)
                {
                    if(tile.CurrentIngredient != null)
                    {
                        groupScore += tile.CurrentIngredient.Data.baseScore;
                        tilesToDeleteingredient.Add(tile);
                    }
                }
                // 점수 매니저에게 전달 ( 혹은 점수 반영)
            }

            // 파괴 연출 실행
            foreach (var tile in tilesToDeleteingredient)
            {
                if ( tile.CurrentIngredient != null)
                {
                    //각 제료의 onexplode 호출
                    explodeRoutines.Add(StartCoroutine(tile.CurrentIngredient.Data.OnExplode(tile)));
                }
            }

            // 모든 삭제 연출 대기
            foreach (var routine in explodeRoutines)
            {
                yield return routine;
            }

            // 실제 데이터 삭제 
            foreach (var tile in tilesToDeleteingredient)
            {
                if (tile.CurrentIngredient != null)
                {
                    Destroy(tile.CurrentIngredient.gameObject);
                }

                tile.ClearIngredient();
            }

            yield return StartCoroutine(ApplyGravity());
            yield return StartCoroutine(FillEmptyGradients());
            // 낙하 및 리필 로직 호출
        }

        private IEnumerator ApplyGravity()
        {
            Debug.Log("ApplyGravity 진입");
            List<Coroutine> moveCoroutines = new List<Coroutine>();

            for (int x = 0; x < field.Width; x++)
            {
                // writeRow: 재료를 쌓을 바닥 위치 (0부터 차곡차곡 쌓음)
                int writeRow = 0;

                // 1. 바닥부터 위로 훑으면서 재료가 있다면 writeRow 위치로 당겨옴
                for (int readRow = 0; readRow < field.Height; readRow++)
                {
                    Tile readTile = field.GetTile(readRow, x);

                    if (readTile.CurrentIngredient != null)
                    {
                        // 제자리가 아니라면 이동
                        if (writeRow != readRow)
                        {
                            Tile targetTile = field.GetTile(writeRow, x);
                            IngredientObject movingIng = readTile.CurrentIngredient;

                            // 데이터 이동
                            targetTile.SetIngredient(movingIng);
                            readTile.ClearIngredient();

                            // 시각적 이동 (애니메이션)
                            if (movingIng != null)
                            {
                                movingIng.transform.SetParent(targetTile.transform);
                                moveCoroutines.Add(StartCoroutine(AnimateMove(movingIng.transform, Vector3.zero, fallDuration)));
                            }
                        }
                        writeRow++;
                    }
                }
            }

            // 모든 이동이 끝날 때까지 대기
            foreach (var co in moveCoroutines)
            {
                yield return co;
            }
        }

        private IEnumerator FillEmptyGradients()
        {
            Debug.Log("FillEmptyGradients 진입");
            List<Coroutine> moveCoroutines = new List<Coroutine>();

            for (int x = 0; x < field.Width; x++)
            {
                // 1. 위에서부터 빈칸인지 확인하는 것이 아니라, 
                //    이미 Gravity가 적용되었으므로 위쪽의 빈칸들만 채우면 됨.
                //    (하지만 안전하게 전체 검사해도 무방)
                for (int y = 0; y < field.Height; y++)
                {
                    Tile tile = field.GetTile(y, x);

                    if (tile.CurrentIngredient == null)
                    {
                        // 랜덤 생성
                        IngredientObject newObj = CreateRandomIngredientAt(tile);

                        // 생성 연출: 타일 위쪽에서 떨어지는 느낌
                        newObj.transform.position = tile.transform.position + Vector3.up * 2f;

                        // 생성 후 제자리로 이동하는 애니메이션
                        moveCoroutines.Add(StartCoroutine(AnimateMove(newObj.transform, Vector3.zero, fallDuration)));

                        // 너무 빨리 생성되면 어색하므로 아주 짧은 딜레이 (선택사항)
                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }

            // 생성 애니메이션 대기
            foreach (var co in moveCoroutines)
            {
                yield return co;
            }
        }

        private IEnumerator AnimateMove(Transform target, Vector3 localTargetPos, float duration)
        {
            Vector3 startPos = target.localPosition;
            float elapsed = 0f;

            while(elapsed <duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                target.localPosition = Vector3.Lerp(startPos, localTargetPos, t);
                yield return null;
            }

            target.localPosition = localTargetPos;
        }

        /// <summary>
        /// Field의 모든 행과 열을 검사하여 김밥을 만들 수있는 리스트를 찾는다
        /// </summary>
        /// <returns>존재하는 모든 김밥리스트의 리스트</returns>
        private List<Gimbab> FindWrapperMatches()
        {
            List<Gimbab> allMatches = new List<Gimbab>();

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
        private void CheckLine(List<Tile> line, ref List<Gimbab> matches)
        {
            int startWrapperIndex = -1;

            for (int i = 0; i < line.Count; i++)
            {
                IngredientObject currentIngredient = line[i].CurrentIngredient;

                // 중간에 빈칸이 존재하는 경우 => 김밥이 끊긴 것
                if (currentIngredient == null)
                {
                    startWrapperIndex = -1;
                    continue;
                }

                // 현재 타일의 재료가 김인 경우
                if (IsWrapper(currentIngredient.Data) == true)
                {
                    // 두번째로 만나는 김인 경우
                    if (startWrapperIndex != -1)
                    {
                        // 첫 김과 1칸 이상 떨어져 잇는 경우
                        if (i > startWrapperIndex)
                        {
                            // 시작 인덱스부터 현재 인덱스까지 김밥을 만들어 김밥리스트에 저장
                            List<Tile> gimbap = new List<Tile>();
                            for (int k = startWrapperIndex; k <= i; k++)
                            {
                                gimbap.Add(line[k]);
                            }
                            matches.Add(gimbap);
                        }

                        // 김밥김밥김 과같은 경우를 처리하기 위해 해당 위치부터 다시 탐색
                        startWrapperIndex = i;
                    }
                    else // 첫번째로 만나는 김인 경우
                    {
                        startWrapperIndex = i;
                    }
                }
            }
        }


    }
}
