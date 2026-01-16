using System.Collections;
using BandoWare.GameplayTags;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{
    public class FireIngredient : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Etc.Fire.Get();
        public override IEnumerator OnFall(Tile tile)
        {
            yield break;
        }

        public override IEnumerator OnExplode(Tile tile)
        {
            yield break;
        }
    }
}

