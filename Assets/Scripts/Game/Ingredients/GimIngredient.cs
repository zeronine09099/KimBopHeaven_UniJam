using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 김 재료
    /// 트리거 시: 기본 점수만 제공 (김밥의 필수 재료)
    /// </summary>
    public class GimIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Essential.Gim.Get();

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


