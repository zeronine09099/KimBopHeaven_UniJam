using System.Collections;
using BandoWare.GameplayTags;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class GrilledgarlicIngredient : IngredientSO
    {
        public override GameplayTag Tag => GameplayTagManager.RequestTag("Ingredient.Vegetable.GrilledGarlic");
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

