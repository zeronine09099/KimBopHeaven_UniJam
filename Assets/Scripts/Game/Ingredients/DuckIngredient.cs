using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 오리고기 재료
    /// 트리거 시: 인접한 재료가 모두 채소류라면 점수 +40
    /// variable01: 추가 점수 (기본값: 40)
    /// </summary>
    public class DuckIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Meat.Duck.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 인접한 재료가 모두 채소류인지 확인
            int currentIndex = args.MatchData.IndexOf(args.Tile);
            int adjacentCount = 0;
            if (currentIndex > 0 && args.MatchData[currentIndex - 1].CurrentIngredient != null) adjacentCount++;
            if (currentIndex < args.MatchData.Count - 1 && args.MatchData[currentIndex + 1].CurrentIngredient != null) adjacentCount++;
            
            int vegetableCount = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Vegetable.Get());
            bool allVegetable = (adjacentCount > 0 && vegetableCount == adjacentCount);

            PlayerState.Current.CurrentTempScore += (int)(baseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(baseScore * PlayerState.Current.CurrentTempMultiplier));
            if (allVegetable)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
        }
    }
}


