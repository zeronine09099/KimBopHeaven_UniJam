using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 시금치
    /// 주변 모두 채소면 리롤 제공
    /// variable01: 지급할 리롤 개수 (기본 1)
    /// </summary>
    public class SpinachIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Spinach.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await base.OnTrigger(args);
            
            // 인접 재료 채소 확인
            int currentIndex = args.MatchData.IndexOf(args.Tile);
            int adjacentCount = 0;
            if (currentIndex > 0 && args.MatchData[currentIndex - 1].CurrentIngredient != null) adjacentCount++;
            if (currentIndex < args.MatchData.Count - 1 && args.MatchData[currentIndex + 1].CurrentIngredient != null) adjacentCount++;
            
            int vegetableCount = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Vegetable.Get());
            bool allVegetable = (adjacentCount > 0 && vegetableCount == adjacentCount);
            
            if (allVegetable)
            {
                // 리롤 횟수 추가
                PlayerState.Current.CurrentRerollRemain += (int) variable01;
                DefaultTriggerEffect(args, 2);
            }
         
        }
    }
}


