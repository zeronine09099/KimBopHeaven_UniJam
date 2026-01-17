using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 당근 재료
    /// 트리거 시: 김 제외 겹치는 재료가 없으면 점수 30 추가
    /// variable01: 추가 점수 (기본값: 30)
    /// </summary>
    public class CarrotIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Carrot.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            var matchData = args.MatchData;
            var ingredientTags = new System.Collections.Generic.HashSet<GameplayTag>();
            
            // 김밥의 모든 재료 수집 (김 제외)
            foreach (var tile in matchData)
            {
                var ingredient = tile.CurrentIngredient?.Data;
                if (ingredient != null && !ingredient.IsGim())
                {
                    if (!ingredientTags.Add(ingredient.Tag))
                    {
                        // 중복 발견
                        PlayerState.Current.CurrentTempScore += (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
                        await DefaultTriggerEffect(args, 1, (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
                        return;
                    }
                }
            }
            
            // 겹치는 재료가 없음
            PlayerState.Current.CurrentTempScore += (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
        }
    }
}


