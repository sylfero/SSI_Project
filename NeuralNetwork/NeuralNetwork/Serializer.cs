using Newtonsoft.Json;

namespace NeuralNetwork
{
    public static class Serializer
    {
        public static string Serialize(this Network network)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
            };
            return JsonConvert.SerializeObject(network, settings);
        }

        public static Network Deserialize(this string json)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            return JsonConvert.DeserializeObject<Network>(json, settings);
        }
    }
}
