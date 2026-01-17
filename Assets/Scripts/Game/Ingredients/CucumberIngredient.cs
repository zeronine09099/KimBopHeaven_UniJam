using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 오이 재료
    /// 트리거 시: 김밥에 다른 채소류가 없다면 점수 30 추가
    /// variable01: 추가 점수 (기본값: 30)
    /// </summary>
    public class CucumberIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Cucumber.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 다른 채소류가 있는지 확인 (자기 자신 제외)
            int vegetableCount = args.CountHasTag(AllGameplayTags.Ingredient.Vegetable.Get());
            bool hasOtherVegetable = vegetableCount > 1;

            PlayerState.Current.CurrentTempScore += (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (!hasOtherVegetable)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


