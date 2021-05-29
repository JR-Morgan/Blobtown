using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelGenerateButton))]
public class LevelGenerateButtonEditor : Editor
{
    private VisualElement root;
    private LevelGenerateButton levelGen;


    private void OnEnable()
    {
        levelGen = (LevelGenerateButton)target;
        root = new VisualElement();
        
    }


    public override VisualElement CreateInspectorGUI()
    {
        root.Clear();

        {
            Button generate = new Button
            {
                text = "Regenerate Level"
            };

            generate.RegisterCallback<ClickEvent>(evt =>
            {
                levelGen.Generate();
            });

            root.Add(generate);
        }

        return root;
    }


}


