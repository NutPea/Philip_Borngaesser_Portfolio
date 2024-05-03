#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";
    public AllDialogueCollectionHolder _allDialogueCollectionHolder;
    public DialogueCollection currentDialogueCollection;
    private DialogueCollectionSelector dialogueCollectionSel;

    private List<string> allDialogueCollectionNames = new List<string>();
    private int selectedDialogeCollectionIndex = 0;
    Toolbar toolbar;
    private TextField fileNameTextField;
    private Blackboard blackboard;
    private BlackboardSection blackboardSection;

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstractGraphView();
        GenerateToolbar();
        _graphView.dialogueGraph = this;
    }

    private void GenerateBlackBoard()
    {

        _graphView._allDialogueCollectionHolder = _allDialogueCollectionHolder;

        blackboard = new Blackboard(_graphView);
        blackboardSection = new BlackboardSection { title = "Variables" };
        blackboard.Add(blackboardSection);


        blackboard.addItemRequested = _blackboard =>
        {
             _graphView.AddPropertyToBlackBoard(new ExposedProperty());
        };

        blackboard.editTextRequested = (blackboard1, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if (currentDialogueCollection.exposedProperties.Any(x => x.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already Exists please chose another one", "Ok");
                return;
            }

            var _propertyIndex = currentDialogueCollection.exposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
            currentDialogueCollection.exposedProperties[_propertyIndex].PropertyName = newValue;

            ((BlackboardField)element).text = newValue;

        };

        blackboard.SetPosition(new Rect(10, 30, 200, 300));
        _graphView.Add(blackboard);
        _graphView.blackboard = blackboard;

        currentDialogueCollection.exposedProperties.RemoveAll(item => item == null);
        foreach (ExposedProperty prop in currentDialogueCollection.exposedProperties)
        {
            _graphView.DrawPropertyToBlackBoard(prop);
        }

    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void GenerateToolbar()
    {
        toolbar = new Toolbar();


        if (_allDialogueCollectionHolder == null)
        {
            //_allDialogueCollectionHolder = Resources.Load<AllDialogueCollectionHolder>("Assets/Editor/Recources/AllDialogueCollectionHolder");
            _allDialogueCollectionHolder = (AllDialogueCollectionHolder)AssetDatabase.LoadAssetAtPath("Assets/Editor/Recources/AllDialogueCollectionHolder.asset", typeof(AllDialogueCollectionHolder));
        }
        RefreshDialogueCollectionNames();
        fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt =>
        {
            _fileName = evt.newValue;
            currentDialogueCollection.RenameCollection(_fileName);
            EditorUtility.SetDirty(currentDialogueCollection);
            AssetDatabase.SaveAssets();
        }

        );


        toolbar.Add(new Button(() => CreateDialogueData()) { text = "Create new Dialogue" });
        toolbar.Add(new Button(() => LoadDialogueCollection()) { text = "Load Data" });
        var nodeCreateButton = new Button(() =>
        {
            _graphView.CreateNode("Dialogue Node", currentDialogueCollection);
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(fileNameTextField);
        toolbar.Add(nodeCreateButton);
        toolbar.Add(new Label("_____________________"));
        toolbar.Add(new Button(() => DeleteCurrentCollection()) { text = "Delete Current Collection" });



        rootVisualElement.Add(toolbar);
    }

    private void RefreshDialogueCollectionNames()
    {
        allDialogueCollectionNames.Clear();
        foreach (DialogueCollection dialogueCollection in _allDialogueCollectionHolder.dialogueCollections)
        {
            allDialogueCollectionNames.Add(dialogueCollection.name);
        }
    }

    private void DeleteCurrentCollection()
    {
        int option = EditorUtility.DisplayDialogComplex("Delete Dialogue",
            "Do you want to delete the current Dialoge?",
            "Delete",
            "Cancel",
            "Don't Delete");

        switch (option)
        {
            // Save.
            case 0:
                int indexOfCollection = _allDialogueCollectionHolder.dialogueCollections.IndexOf(currentDialogueCollection);
                _allDialogueCollectionHolder.dialogueCollections.RemoveAt(indexOfCollection);
                if (indexOfCollection > 0)
                {
                    currentDialogueCollection.DeleteThisAsset();
                    if(indexOfCollection >= 1)
                    {
                        currentDialogueCollection = _allDialogueCollectionHolder.dialogueCollections[indexOfCollection-1];
                        LoadData();
                    }
                }
                break;

            case 1:
                break;

            case 2:
                break;

            default:
                Debug.LogError("Unrecognized option.");
                break;
        }
        RefreshDialogueCollectionNames();
    }

    private void CreateDialogueData()
    {
        DialogueCollection dialogueCollection = new DialogueCollection();
        AssetDatabase.CreateAsset(dialogueCollection, "Assets/Dialoge/" + "Dialogue " + _allDialogueCollectionHolder.dialogueCollections.Count+".asset");
        AssetDatabase.SaveAssets();
        //dialogueCollection.RenameCollection("Dialogue " + _allDialogueCollectionHolder.dialogueCollections.Count);
        _allDialogueCollectionHolder.dialogueCollections.Add(dialogueCollection);
        _allDialogueCollectionHolder.SaveCollectionHolder();
        currentDialogueCollection = dialogueCollection;
        LoadData();
        RefreshDialogueCollectionNames();
    }

    private void LoadDialogueCollection()
    {
        RefreshDialogueCollectionNames();
        dialogueCollectionSel = ScriptableObject.CreateInstance(typeof(DialogueCollectionSelector)) as DialogueCollectionSelector;
        dialogueCollectionSel.dialogueGraph = this;

        List<string> replacedDialogueCollectionName = new List<string>();
        foreach(String dialogeName in allDialogueCollectionNames)
        {
            replacedDialogueCollectionName.Add(dialogeName.Replace("_", "/"));
        }
        dialogueCollectionSel.SetDialogueCollectionNames(replacedDialogueCollectionName);
        dialogueCollectionSel.Show();
    }

    public void LoadData()
    {
        _graphView.RemoveGraphViewChange();

        _graphView.RemoveAllDialogueNodes();
        _graphView.RemoveAllEdges();
        if(dialogueCollectionSel != null)
        {

            dialogueCollectionSel.Close();
        }
        fileNameTextField.SetValueWithoutNotify(currentDialogueCollection.name);
        if(currentDialogueCollection.startNodeData == null)
        {
             _graphView.CreateStartNode("StartNode", currentDialogueCollection);
        }
        else
        {
            _graphView.LoadNode("StartNode", currentDialogueCollection.startNodeData,false);
        }

        for(int index = 0; index < currentDialogueCollection.dialogueNodeDatas.Count; index++)
        {
            _graphView.LoadNode("DialogueNode" + index, currentDialogueCollection.dialogueNodeDatas[index]);
        }

        _graphView.LoadAllChoisePorts();
        _graphView.LoadAllEdges();
        _graphView.dialogueCollection = currentDialogueCollection;
        _graphView.AddGraphViewChange();
        _graphView.RefreshGraphView();

        GenerateBlackBoard();
    }


    private void ConstractGraphView()
    {

        _graphView = new DialogueGraphView
        {
            name = "Graph View"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }
}
#endif
