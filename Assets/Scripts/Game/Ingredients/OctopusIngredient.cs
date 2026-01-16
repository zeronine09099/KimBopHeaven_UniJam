using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class OctopusIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.Octopus.Get();

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

