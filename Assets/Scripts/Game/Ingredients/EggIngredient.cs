using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 계란 재료
    /// 트리거 시: 기본 점수만 제공 (잘 삶은 계란)
    /// </summary>
    public class EggIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Etc.Egg.Get();

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


