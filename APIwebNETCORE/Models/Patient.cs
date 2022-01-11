namespace APIwebNETCORE.Models
{
    public class Patient
    {
        public string Num { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Adress { get; set; }

        public Boolean Malade { get; set; }
        public int Score { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string id { get; set; }
    }
}
