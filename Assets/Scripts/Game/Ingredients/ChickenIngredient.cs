using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class ChickenIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Meat.Chicken.Get();

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

