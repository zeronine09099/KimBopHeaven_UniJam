using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 게맛살 재료
    /// 트리거 시: 김밥에 포함된 다른 해산물 재료 개수만큼 추가 점수 제공
    /// variable01: 해산물 재료당 추가 점수 (기본값: 10)
    /// </summary>
    public class CrapstickIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.CrabStick.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 김밥의 다른 해산물 재료 개수 확인
            int seafoodCount = args.CountHasTag(AllGameplayTags.Ingredient.Seafood.Get()) - 1;

            PlayerState.Current.CurrentTempScore += (int) baseScore;
            await DefaultTriggerEffect(args, 1, (int)baseScore);
            int count = 1;
            while (seafoodCount-- > 0)
            {
                int bonusScore = (int)variable01;
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, ++count, bonusScore);
            }
        }
    }
}


