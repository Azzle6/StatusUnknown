using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Sirenix.OdinInspector;

// [CustomEditor(typeof(GameplayManager))]    
public class GameplayManagerEditor : Editor
{
    public VisualTreeAsset _UXML; // goes through all the tags and instantiate them 

    /* public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        _UXML.CloneTree(root);


        // init list view
        var mcListView = root.Q<MultiColumnListView>();
        var playManager = target as GameplayManager;
        mcListView.itemsSource = playManager._EnemyCharacters; 

        // set make cell

        var cols = mcListView.columns;
        cols["name"].makeCell = () => new Label();
        cols["starting-health"].makeCell = () => new Label();
        cols["current-health"].makeCell = () => new ProgressBar() { lowValue = 0, highValue = 120 };

        // set bind cell
        cols["name"].bindCell = (VisualElement e, int index) =>
        {
            (e as Label).text = "Enemy " + (index + 1);
        };

        cols["starting-health"].bindCell = (VisualElement e, int index) =>
        {
            var l = e as Label;
            l.bindingPath = "_EnemyCharacters.Array.data[" + index + "]._StartingHealth"; 
            l.Bind(serializedObject);
        };

        cols["current-health"].bindCell = (VisualElement e, int index) =>
        {
            var pb = e as ProgressBar;
            var pbLabel = pb.Q<Label>(className: ProgressBar.titleUssClassName);

            var tankInstance = playManager._EnemyCharacters[index]._Instance;
            if (tankInstance == null || tankInstance.GetComponent<Health>() == null)  // if not in play mode
            {
                var bpath = "_EnemyCharacters.Array.data[" + index + "]._StartingHealth";
                pb.bindingPath = bpath;
                pbLabel.bindingPath = bpath;
                pb.Bind(serializedObject);
            }
            else // in play mode, rebind to the enemy instance
            {
                // Health.cs must be created
                
                pb.bindingPath = nameof(Health._CurrentHealth);
                pbLabel.bindingPath = nameof(Health._CurrentHealth);
                pb.Bind(new SerializedObject(tankInstance.GetComponent<Health>())); 
                
            
        };

        // set unbindcell
        cols["starting-health"].unbindCell = (VisualElement e, int i) => e.Unbind();
        cols["current-health"].unbindCell = (VisualElement e, int i) => e.Unbind(); 

        // respond to game state change
        var gameStateEnumField = root.Q<EnumField>("game-state");
        // gameStateEnumField.RegisterValueChangedCallback((e) => mcListView.RefreshItems()); 
        gameStateEnumField.SetEnabled(true); 

        // draw default inspector
        var foldout = new Foldout() { viewDataKey = "CharacterManagerFullInspectorFoldout", text = "Full Inspector" }; 
        InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
        root.Add(foldout);  
        return root;   
    } */
}
