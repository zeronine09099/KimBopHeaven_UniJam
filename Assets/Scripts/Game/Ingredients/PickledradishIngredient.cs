using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 단무지 재료
    /// 트리거 시: 밥이 인접해 있으면 점수 두배
    /// variable01: 점수 배수 (기본값: 2)
    /// </summary>
    public class PickledradishIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Essential.PickledRadish.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 인접한 타일이 밥인지 확인
            bool hasAdjacentRice = args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Essential.Rice.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(baseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(baseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentRice)
            {
                int kimbapScore = PlayerState.Current.GetKimbapScore();
                int bonusScore = (int)(kimbapScore * ((int)variable01 - 1) * PlayerState.Current.CurrentTempMultiplier);
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, 2, bonusScore);
            }
        }
    }
}


