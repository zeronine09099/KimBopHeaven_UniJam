using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 치즈 재료
    /// 트리거 시: 덱에 치즈 추가, 이전 김밥에 치즈가 없었다면 점수 25 추가
    /// variable01: 추가 점수 (기본값: 20)
    /// </summary>
    public class CheeseIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Etc.Cheese.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await base.OnTrigger(args);
            
            // 덱에 치즈 추가
            AddIngredientToPlayer(this, 1);
            PuzzleManager.Instance.AddIngredientToRetrieved(this, 1);
            DefaultTriggerEffect(args, 2);
            
            
            bool previousKimbapHadCheese = PuzzleManager.Instance.CurrentMatches[^1].HasIngredientTag(AllGameplayTags.Ingredient.Etc.Cheese.Get());
            if (previousKimbapHadCheese)
            {
                PlayerState.Current.CurrentTempScore +=(int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                DefaultTriggerEffect(args, 3,25);
            }
        }
    }
}


