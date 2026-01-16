using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class SpinachIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Spinach.Get();

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

