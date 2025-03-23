using Microsoft.FeatureManagement;

namespace Api.GRRInnovations.FeatureFlags.Services
{
    [FilterAlias("Random")]
    public class RandomFilter : IFeatureFilter
    {
        private readonly Random _random;

        public RandomFilter()
        {
            _random = new Random();
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            int percentage = context.Parameters.GetSection("Percentage").Get<int>();

            int randomNumber = _random.Next(100);

            return Task.FromResult(randomNumber <= percentage);
        }
    }
}
