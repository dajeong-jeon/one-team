using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.VisualScripting;

namespace Dunward.Capricorn
{
    public class CapricornEditorWindow : EditorWindow
    {
        public StyleSheet graphStyle;

        private GraphView graphView;

        private string filePath = null;

        [MenuItem("Leman/Node Manager")]
        public static void ShowExample()
        {
            var window = GetWindow<CapricornEditorWindow>();
            window.titleContent = new GUIContent("Node Manager");
        }

        [MenuItem("Leman/Asset Manager", priority = -10)]
        public static void OpenAMS()
        {
            Application.OpenURL("http://leman.genevagamestudio.com/character-generator");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var imguiContainer = new IMGUIContainer(CreateToolbar);

            root.Add(imguiContainer);

            AddGraphView();
        }

        public void CreateToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.Space(5);

            if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            {
                if (EditorUtility.OpenFilePanel("Load Graph", "", "json") is string path)
                {
                    graphView.Load(path);
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            {
                graphView.Save();
            }

            GUILayout.Space(5);
            
            if (GUILayout.Button("Save As...", EditorStyles.toolbarButton))
            {
                graphView.SaveAs();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Create New Graph", EditorStyles.toolbarButton))
            {
                // dialog to confirm
                if (EditorUtility.DisplayDialog("Clear Graph", "Are you sure you want to clear the graph?", "Yes", "No"))
                {
                    graphView.ClearGraphView();
                }
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Export CSV(Beta)", EditorStyles.toolbarButton))
            {
                System.IO.File.WriteAllText($"{Application.dataPath}/export.csv", graphView.ExportOnlyActionToCSV());
            }

            if (GUILayout.Button("Import CSV(Beta)", EditorStyles.toolbarButton))
            {
                if (EditorUtility.OpenFilePanel("Load CSV", "", "csv") is string path)
                {
                    graphView.ImportOnlyActionFromCSV(path);
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public void AddNodesFromJson(string json)
        {
            graphView.DeserializeNodesOnly(json);
        }

        public void ReplaceNodesFromJson(string json)
        {
            graphView.ReplaceNodesFromJson(json);
        }
        
        public (Vector2, float, int) GetGraphViewData()
        {
            return graphView.GetGraphViewData();
        }

        public string GetGraphSerializedData()
        {
            return graphView.SerializeGraph();
        }

        public string GetSelectionGraphSerializedData()
        {
            return graphView.SerializeSelectionGraph();
        }

        private void AddGraphView()
        {
            var content = new VisualElement();
            graphView = new GraphView(filePath);

            graphView.onChangeFilePath += (path) =>
            {
                filePath = path;
            };

            content.styleSheets.Add(graphStyle);
            content.name = "content";
            content.Add(graphView);
            rootVisualElement.Add(content);
        }
    }
}
