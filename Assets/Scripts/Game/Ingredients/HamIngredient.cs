using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 햄 재료
    /// 트리거 시: 단무지 옆에 있으면 점수 20 추가
    /// variable01: 추가 점수 (기본값: 20)
    /// </summary>
    public class HamIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Essential.Ham.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {

            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            
            // 인접한 타일이 단무지인지 확인
            bool hasAdjacentPickledRadish = args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Essential.Rice.Get()) > 0;

            if (hasAdjacentPickledRadish)
            {
                args.Tile.BOOOOM = true;
                // PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                // await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }

        public override async UniTask OnExplode(Tile tile)
        {
            // if (tile.BOOOOM)
            // {
            //     // 크기가 커지면서 fadeout. 중간에 await끝
            //     var Ingredient = tile.CurrentIngredient;
            //     var transform = Ingredient.transform;
            //     Sequence seq = DOTween.Sequence();
            //     seq.Append(transform.DOScale(transform.localScale * 1.5f, 0.3f));
            //     seq.Join(tile.CurrentIngredient.SpriteRenderer.DOFade(0, 0.3f));
            //
            //     await seq.ToUniTask();
            // }
            await base.OnExplode(tile);
        }
    }
}

