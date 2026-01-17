using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 김치 재료
    /// 김밥 재료에 불 하나마다 점수 20 추가
    /// variable01: 불당 추가점수 (기본 20)
    /// </summary>
    public class KimchiIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Kimchi.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 김밥의 불 개수 카운트
            int fireCount = args.CountExectTag(AllGameplayTags.Ingredient.Etc.Fire.Get());

            PlayerState.Current.CurrentTempScore += (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(reinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            int count = 1;
            while (fireCount-- > 0)
            {
                int bonusScore = (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, ++count, bonusScore);
            }
        }
    }
}


