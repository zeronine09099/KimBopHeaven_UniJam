using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 고추 재료
    /// 트리거 시: 점수 0인 불을 덱에 2개 추가
    /// variable01: 불의 점수 (기본값: 0)
    /// variable02: 덱에 추가할 불의 개수 (기본값: 2)
    /// </summary>
    public class ChilipepperIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.ChiliPepper.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await base.OnTrigger(args);
            
            // 덱에 불 추가
            var fireIngredient = (IngredientSO)AllGameplayTags.Ingredient.Etc.Fire.Get();
            int fireCount = (int)variable02;
            AddIngredientToPlayer(fireIngredient, fireCount);
            PuzzleManager.Instance.AddIngredientToRetrieved(fireIngredient, fireCount);
         
        }
    }
}


