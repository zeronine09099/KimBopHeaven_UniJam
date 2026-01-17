using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 참치 재료
    /// 트리거 시: 기본 점수 + 인접한 마요네즈가 있다면 추가 점수 제공
    /// variable01: 추가 점수 (기본값: 30)
    /// </summary>
    public class TunaIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.Tuna.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            
            bool hasAdjacentMayonnaise = args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Sauce.Mayonnaise.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(baseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(baseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentMayonnaise)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


