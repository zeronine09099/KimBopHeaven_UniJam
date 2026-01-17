using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 어묵 재료
    /// 트리거 시: 인접한 재료가 모두 해산물이면 점수 * 2
    /// variable01: 점수 배수 (기본값: 2)
    /// </summary>
    public class FishcakeIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.FishCake.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 인접한 재료가 모두 해산물인지 확인
            int currentIndex = args.MatchData.IndexOf(args.Tile);
            int adjacentCount = 0;
            if (currentIndex > 0 && args.MatchData[currentIndex - 1].CurrentIngredient != null) adjacentCount++;
            if (currentIndex < args.MatchData.Count - 1 && args.MatchData[currentIndex + 1].CurrentIngredient != null) adjacentCount++;
            
            int seafoodCount = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Seafood.Get());
            bool seafoodCheck = (adjacentCount > 0);

            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (seafoodCheck)
            {
                int kimbapScore = PlayerState.Current.GetKimbapScore();
                int bonusScore = (int)(kimbapScore * ((int)variable01 - 1) * PlayerState.Current.CurrentTempMultiplier);
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, 2, bonusScore);
            }
        }
    }
}


