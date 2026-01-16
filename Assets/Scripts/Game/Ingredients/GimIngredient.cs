using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class GimIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Essential.Gim.Get();

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

