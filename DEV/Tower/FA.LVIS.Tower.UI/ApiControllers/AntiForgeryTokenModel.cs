using Newtonsoft.Json;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    public sealed class AntiForgeryTokenModel
    {
        [JsonProperty("antiForgeryToken")]
        public string AntiForgeryToken { get; set; }
    }
}