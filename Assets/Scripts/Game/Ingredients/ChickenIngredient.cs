using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 치킨 재료
    /// 트리거 시: 다른 종류의 고기류가 없다면 점수 +30
    /// variable01: 추가 점수 (기본값: 30)
    /// </summary>
    public class ChickenIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Meat.Chicken.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 다른 고기류가 있는지 확인 (자기 자신 제외)
            int meatCount = args.CountHasTag(AllGameplayTags.Ingredient.Meat.Get());
            bool hasOtherMeat = meatCount > 1;

            PlayerState.Current.CurrentTempScore += (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (!hasOtherMeat)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


