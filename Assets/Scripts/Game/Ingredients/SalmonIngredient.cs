using System.Collections;
using BandoWare.GameplayTags;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class SalmonIngredient : IngredientSO
    {
        public override GameplayTag Tag => GameplayTagManager.RequestTag("Ingredient.Seafood.Salmon");
        public override IEnumerator OnFall(Tile tile)
        {
            yield break;
        }

        public override IEnumerator OnTrigger(Tile tile)
        {
            yield break;
        }
    }
}

