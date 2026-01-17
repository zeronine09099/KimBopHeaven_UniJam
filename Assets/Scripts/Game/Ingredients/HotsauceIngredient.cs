using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 핫소스 재료
    /// 트리거 시: 고기류가 인접해 있다면 점수 30 추가
    /// variable01: 추가 점수 (기본값: 30)
    /// </summary>
    public class HotsauceIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Sauce.HotSauce.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 인접한 타일이 고기류인지 확인
            bool hasAdjacentMeat = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Meat.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentMeat)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


