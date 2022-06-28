namespace Editor.Models
{

    internal class ClassModel
    {
        internal string Name { get; set; }
        internal List<Property> Properties { get; set; }

        public ClassModel()
        {
            Name = "RootObj";
            Properties = new List<Property>();
        }

        public void AddProperty(KeyValuePair<string, System.Text.Json.Nodes.JsonNode> item, string type)
        {
            var property = new Property() {
                Name = item.Key,
                Type = type
            };

            Properties.Add(property);
        }

    }

    internal class Property
    {
        internal string Name { get; set; }
        internal string Type { get; set; }
    }
}
