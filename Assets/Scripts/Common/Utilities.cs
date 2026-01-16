namespace Common
{
    public static class Utilities
    {
        
        
        public static int WeightedRandomIndex(params float[] weights)
        {
            float totalWeight = 0f;
            foreach (float weight in weights)
            {
                totalWeight += weight;
            }

            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;

            for (int i = 0; i < weights.Length; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue <= cumulativeWeight)
                {
                    return i;
                }
            }

            return weights.Length - 1; // Fallback
        }
    }
}