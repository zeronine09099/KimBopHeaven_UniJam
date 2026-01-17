using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 삼겹살 재료
    /// 인접 채소당 추가점수
    /// </summary>
    public class PorkbellyIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Meat.PorkBelly.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            int vegetableCount = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Vegetable.Get());

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


