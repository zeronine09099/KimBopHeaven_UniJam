using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;

namespace Game.Ingredients
{
    public class ShrimpIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.Shrimp.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(Tile tile)
        {
            await base.OnTrigger(tile);
         
        }
    }
}

