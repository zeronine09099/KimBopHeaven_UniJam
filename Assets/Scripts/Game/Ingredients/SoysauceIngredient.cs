using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 간장 재료
    /// 트리거 시: 밥이 근처에 있고 다른 소스가 없다면 추가 점수 제공
    /// variable01: 밥 근처 & 다른 소스 없음 시 추가 점수(기본값 30)
    /// </summary>
    public class SoysauceIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Sauce.SoySauce.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            bool hasAdjacentRice = args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Essential.Rice.Get()) > 0;
            
            int sauceCount = args.CountHasTag(AllGameplayTags.Ingredient.Sauce.Get());
            bool hasOtherSauce = sauceCount > 1;

            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentRice && !hasOtherSauce)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


