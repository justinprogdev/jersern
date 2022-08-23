using System.Text.Json;
using System.Text.Json.Nodes;
using Editor.Models;

namespace Editor
{
    public partial class MainForm : Form
    {
        
        private Linter _linter;
        private List<ClassModel> _classModels;

        public MainForm()
        {
            //Todo add DI to ctor
            InitializeComponent();
            _classModels = new List<ClassModel>();
            _linter = new Linter();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtInput.TextChanged += TxtJson_TextChanged;
        }

        /// <summary>
        /// Left Pane Text Changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtJson_TextChanged(object? sender, EventArgs e)
        {
            //Exit for obvious
            if (string.IsNullOrEmpty(txtInput.Text))
                return;

            //Get the json object from the pasted text
            var jsonObject = _linter.ParseText(txtInput.Text);
            if (jsonObject == null)
            {
                txtOutput.Text = "Paste Json, not spaghetti";
                return;
            }

            //clear to start fresh
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
            //Start constructing the class code
            var classModel = new ClassModel();
            classModel.Name = name;

            var nodeNames = new List<string>();

            //Build list of properties before classes. 
            foreach (var node in jsonObject)
            {
                var json = new JsonObject();

                //I have parsedJsonobject
                var type = _linter.ParseType(node.Value);
                bool isProperty = _linter.IsJustProperty(type);

                //Single level properties
                if (isProperty)
                {
                    var typeName = _linter.GetCSharpType(type.Name);
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
                        var json = _linter.ParseText(jsonT);
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
                    var json = _linter.ParseText(jsonT);

                    BuildClass(json, node2.Key);
                }
            }
        }
    }
}