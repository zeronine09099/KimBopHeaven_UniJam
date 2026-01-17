using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 버섯 재료
    /// 구우면 맛있어짐
    /// </summary>
    public class MushroomIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Mushroom.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await base.OnTrigger(args);
            
            if(args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Etc.Fire.Get()) > 0)
            {
                IngredientSO grilledMushroom = IngredientLibrary.Instance.GetIngredientSO(AllGameplayTags.Ingredient.Vegetable.GrilledMushroom.Get());
                args.Tile.CurrentIngredient.Initialize(grilledMushroom);
                DefaultTriggerEffect(args, 2, 0);
            }
         
        }
    }
}


