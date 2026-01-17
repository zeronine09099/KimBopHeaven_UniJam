using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 두부 재료
    /// 소스있으면 2배
    /// variable01: 점수 배수 (기본값 2)
    /// </summary>
    public class TofuIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Etc.Tofu.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            bool hasAdjacentSauce = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Sauce.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(baseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(baseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentSauce)
            {
                int kimbapScore = PlayerState.Current.GetKimbapScore();
                int bonusScore = (int)(kimbapScore * ((int)variable01 - 1) * PlayerState.Current.CurrentTempMultiplier);
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, 2, bonusScore);
            }
        }
    }
}


