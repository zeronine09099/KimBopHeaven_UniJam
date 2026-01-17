using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 마늘 재료
    /// 트리거 시: 고기류가 인접해 있다면 점수 20 추가
    /// variable01: 추가 점수 (기본값: 20)
    /// </summary>
    public class GarlicIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Vegetable.Garlic.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 인접한 타일이 고기류인지 확인
            bool hasAdjacentMeat = args.CountAdjacentHasTag(AllGameplayTags.Ingredient.Meat.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);

            int count = 1;
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentMeat)
            {
                PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                await DefaultTriggerEffect(args, ++count, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
            
            if(args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Etc.Fire.Get()) > 0)
            {
                IngredientSO grilledGarlic = IngredientLibrary.Instance.GetIngredientSO(AllGameplayTags.Ingredient.Vegetable.GrilledGarlic.Get());
                args.Tile.CurrentIngredient.Initialize(grilledGarlic);
                AddIngredientToPlayer(grilledGarlic,1);
                await DefaultTriggerEffect(args, ++count, 0);
            }
        }
    }
}


