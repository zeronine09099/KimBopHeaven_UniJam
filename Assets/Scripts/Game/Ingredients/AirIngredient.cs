using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 공기 재료 (빈 타일)
    /// 트리거 시: 점수 없음 (비어있는 타일 표현)
    /// 설명: 비어있습니다. 재료가 채워지면 재료로 대체
    /// </summary>
    public class AirIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Etc.Air.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            await DefaultTriggerEffect(args, 1, 0);
        }
    }
}


