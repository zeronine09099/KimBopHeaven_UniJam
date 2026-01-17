using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 우엉 재료
    /// 트리거 시: 김밥의 채소마다 점수 10 추가
    /// variable01: 채소당 추가 점수 (기본값: 10)
    /// </summary>
    public class BurdockIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Burdock.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 김밥의 채소 개수 세기 (자기 자신 제외)
            int vegetableCount = args.CountHasTag(AllGameplayTags.Ingredient.Vegetable.Get()) - 1;

            PlayerState.Current.CurrentTempScore += (int)(baseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(baseScore * PlayerState.Current.CurrentTempMultiplier));
            int count = 1;
            while (vegetableCount-- > 0)
            {
                int bonusScore = (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, ++count, bonusScore);
            }
        }
    }
}

