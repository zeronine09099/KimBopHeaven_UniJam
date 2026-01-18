using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 새우 재료
    /// 고기 파괴 후 새우 추가
    /// </summary>
    public class ShrimpIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.Shrimp.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            // 인접한 타일이 단무지인지 확인
            bool hasAdjacentPickledRadish = args.CountAdjacentExactTag(AllGameplayTags.Ingredient.Essential.Rice.Get()) > 0;

            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            if (hasAdjacentPickledRadish)
            {
                args.Tile.BOOOOM = true;
                // PlayerState.Current.CurrentTempScore += (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                // await DefaultTriggerEffect(args, 2, (int)(variable01 * PlayerState.Current.CurrentTempMultiplier));
            }
            
            // async UniTask ConvertToShrimp(Tile tile)
            // {
            //     if (tile?.CurrentIngredient?.Data != null)
            //     {
            //         PlayerState.Current.GameDeck.RemoveIngredient(tile.CurrentIngredient.Data);
            //         tile.CurrentIngredient.Initialize(this);
            //         PlayerState.Current.GameDeck.AddIngredient(this);
            //     }
            // }
            //
            // bool IsMeat(Tile tile)
            // {
            //     if (tile?.CurrentIngredient?.Data != null)
            //     {
            //         var Data = tile.CurrentIngredient.Data;
            //         return Data.Tag.IsChildOf(AllGameplayTags.Ingredient.Meat.Get());
            //     }
            //     return false;
            // }
            //
            // bool triggerTwice = false;
            // if (IsMeat(args.PreviousTile)){
            //     triggerTwice = true;
            //     args.AfterMatchActions.Add((args.Tile, async () => await ConvertToShrimp(args.PreviousTile)));
            // }   
            // if (IsMeat(args.NextTile)){
            //     triggerTwice = true;
            //     args.AfterMatchActions.Add((args.Tile, async () => await ConvertToShrimp(args.NextTile)));
            // }
            //
            // if (triggerTwice)
            // {
            //     args.AfterMatchActions.Add((args.Tile, async () => await DefaultTriggerEffect(args, 2)));
            // }

        }
    }
}


