namespace Api.GRRInnovations.FeatureFlags.Models
{
    public class FeatureFlagConfig<T>
    {
        public List<EnabledForConfig<T>> EnabledFor { get; set; }
    }

    public class EnabledForConfig<T>
    {
        public ParametersConfig<T> Parameters { get; set; }
    }

    public class ParametersConfig<T>
    {
        public string[] Values { get; set; }
    }
}
