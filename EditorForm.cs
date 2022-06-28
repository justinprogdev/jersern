using System.Text.Json;
using System.Text.Json.Nodes;
using Editor.Models;

namespace Editor
{
    public partial class MainForm : Form
    {
        private LintingService _lintingService;
        private List<ClassModel> _classModels;

        public MainForm()
        {
            InitializeComponent();
            _classModels = new List<ClassModel>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtInput.TextChanged += TxtJson_TextChanged;
            _lintingService = new LintingService();
        }

        /// <summary>
        /// Someone copies or pastes model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtJson_TextChanged(object? sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtInput.Text))
                return;

            var jsonObject = _lintingService.ParseText(txtInput.Text);
            if (jsonObject == null)
            {
                txtOutput.Text = "Paste Json, not spaghetti";
                return;
            }

            if(_classModels!= null)
            {
                _classModels.Clear();
            }
            BuildClass(jsonObject, "RootObj");

            //Reset any state
            txtOutput.Text = String.Empty;
           

            foreach (var classModel in _classModels)
            {
                //Class Definition
                txtOutput.Text += $"public class {classModel.Name}\r\n";

                //Start Class Scope
                txtOutput.Text += "{\r\n";

                //Define class properties
                foreach (var prop in classModel.Properties)
                {
                    txtOutput.Text += "        " +
                         $"public {prop.Type} {prop.Name} {{get; set;}}\r\n";
                }
                //Class end scope
                txtOutput.Text += "}\r\n";

            }




        }

        private void BuildClass(JsonObject? jsonObject, string name = "")
        {

            var classModel = new ClassModel();
            classModel.Name = name;

            var nodeNames = new List<string>();

            //Build list of properties before classes. 
            foreach (var node in jsonObject)
            {
                var json = new JsonObject();

                //I have parsedJsonobject
                var type = _lintingService.ParseType(node.Value);
                bool isProperty = _lintingService.IsJustProperty(type);

                //Single level properties
                if (isProperty)
                {
                    var typeName = _lintingService.GetCSharpType(type.Name);
                    nodeNames.Add(node.Key);
                    classModel.AddProperty(node, typeName);
                }
                //Get more complex properties
                else if (node.Value.GetType() == typeof(JsonArray))
                {
                    classModel.AddProperty(node, $"List<{node.Key}>");
                }
                //If it is a json object we have to make a class later
                else if (node.Value.GetType() == typeof(JsonObject))
                {
                    classModel.AddProperty(node, $"{node.Key}");
                }
            }

            _classModels.Add(classModel);

            //Now build arrays and subtypes
            foreach (var node2 in jsonObject)
            {
                //Don't add em again
                if (nodeNames.Contains(node2.Key))
                    continue;

                //Add other classes
                if (node2.Value.GetType() == typeof(JsonArray))
                {
                    try
                    {
                        var obj = node2.Value[0].AsObject();
                        var jsonT = JsonSerializer.Serialize(obj);
                        var json = _lintingService.ParseText(jsonT);
                        BuildClass(json, node2.Key);
                    }
                    catch (Exception)
                    {

                    }
                }
                //If it is a json object we have to make a class later
                else if (node2.Value.GetType() == typeof(JsonObject))
                {
                    var jsonT = JsonSerializer.Serialize(node2.Value);
                    var json = _lintingService.ParseText(jsonT);
                    BuildClass(json, node2.Key);
                }
            }

        }
    }
}