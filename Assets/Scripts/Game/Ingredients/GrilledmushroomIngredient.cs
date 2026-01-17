using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 구운버섯 재료
    /// 트리거 시: 잘 구운 버섯 하나 (기본 점수만 제공)
    /// </summary>
    public class GrilledmushroomIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.GrilledMushroom.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await base.OnTrigger(args);
         
        }
    }
}


