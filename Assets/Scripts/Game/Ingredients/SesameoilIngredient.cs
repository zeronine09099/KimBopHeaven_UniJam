using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 참기름 재료
    /// 트리거 시: 인접한 밥이 있다면 추가 점수 +25
    /// variable01: 추가 점수 (기본값: 25)
    /// </summary>
    public class SesameoilIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Sauce.SesameOil.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // ?�접???�?�이 밥인지 ?�인
            bool hasAdjacentRice = args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Essential.Rice.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(baseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(baseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentRice)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


