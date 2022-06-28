namespace Editor.Models
{

    /// <summary>
    /// A model for generating class code
    /// </summary>
    public class ClassModel
    {
        /// <summary>
        /// Class Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of each of property in the class
        /// </summary>
        public List<Property> Properties { get; set; }

        public ClassModel()
        {
            Name = "RootObj";
            Properties = new List<Property>();
        }

        /// <summary>
        /// Add a new property to the class model
        /// </summary>
        /// <param name="item"></param>
        /// <param name="type"></param>
        public void AddProperty(KeyValuePair<string, System.Text.Json.Nodes.JsonNode> item, string type)
        {
            var property = new Property() {
                Name = item.Key,
                Type = type
            };

            Properties.Add(property);
        }

    }

    public class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
