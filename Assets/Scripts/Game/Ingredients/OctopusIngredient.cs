using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;
using Game;
using Player;

namespace Game.Ingredients
{
    /// <summary>
    /// 문어 재료
    /// 인접한 문어가 있다면 점수 +10, 점수 계산 이후 양 옆 재료를 파괴 후 덱에 문어 추가
    /// variable01: 근처 문어당 추가 점수 (기본값: 10)
    /// </summary>
    public class OctopusIngredientSO : IngredientSO
    {
        public override GameplayTag Tag => AllGameplayTags.Ingredient.Seafood.Octopus.Get();

        public override async UniTask OnFall(Tile tile)
        { 
            await base.OnFall(tile);
          
        }

        public override async UniTask OnTrigger(TriggerArguments args)
        {
            // 인접한 문어 수 계산
            int adjacentOctopusCount = args.CountAdjacentExactTag(Tag);

            PlayerState.Current.CurrentTempScore += (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier);
            await DefaultTriggerEffect(args, 1, (int)(ReinforcedBaseScore * PlayerState.Current.CurrentTempMultiplier));
            int count = 1;
            while (adjacentOctopusCount-- > 0)
            {
                int bonusScore = (int)(variable01 * PlayerState.Current.CurrentTempMultiplier);
                PlayerState.Current.CurrentTempScore += bonusScore;
                await DefaultTriggerEffect(args, ++count, bonusScore);
            }

            // 양 옆 재료를 문어로 변환
            async UniTask ConvertToOctopus(Tile tile)
            {
                if (tile?.CurrentIngredient?.Data != null)
                {
                    PlayerState.Current.GameDeck.RemoveIngredient(tile.CurrentIngredient.Data);
                    tile.CurrentIngredient.Initialize(this);
                    PlayerState.Current.GameDeck.AddIngredient(this);
                }
            }

            bool IsGimOrRice(Tile tile)
            {
                if (tile?.CurrentIngredient?.Data != null)
                {
                    var Data = tile.CurrentIngredient.Data;
                    return Data.IsRice() || Data.IsGim();
                }
                return false;
            }

            bool hasConverted = false;
            if (!IsGimOrRice(args.PreviousTile))
            {
                hasConverted = true;
                args.AfterMatchActions.Add((args.Tile, async () => await ConvertToOctopus(args.PreviousTile)));
            }
            
            if (!IsGimOrRice(args.NextTile))
            {
                hasConverted = true;
                args.AfterMatchActions.Add((args.Tile, async () => await ConvertToOctopus(args.NextTile)));
            }

            if (hasConverted)
            {
                args.AfterMatchActions.Add((args.Tile, async () => await DefaultTriggerEffect(args, count + 1)));
            }
        }
    }
}


