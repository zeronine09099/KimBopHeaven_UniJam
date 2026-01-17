using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 연어 
    /// 한 턴에 다른 김밥이 완성되었다면 덱에 연어 추가
    /// </summary>
    public class SalmonIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.Salmon.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await base.OnTrigger(args);
            if(PuzzleManager.Instance.ThisTurnCompletedKimbapCount > 0)
            {
                PlayerState.Current.GameDeck.AddIngredient(this);
                PuzzleManager.Instance.AddIngredientToRetrieved(this, 1);
                DefaultTriggerEffect(args, 5, 0);
            }

         
        }
    }
}


