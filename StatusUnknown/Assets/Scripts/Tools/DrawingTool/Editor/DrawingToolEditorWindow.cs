namespace Tools.DrawingTool.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Inventory;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public class DrawingToolEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField]
        private VisualTreeAsset slotSquareAsset;

        //UI references
        private Vector2IntField gridSizeOption;
        private Button confirmGridSizeButton;
        private Slider squaresSizeOption;
        private ScrollView slotsScrollView;
    
        //Export references
        private TextField fileFolderPath;
        private TextField fileName;
        private Button exportButton;
        private Button loadButton;
        private Button exportAsBrushButton;
        private Button exportToSelectedItemButton;
        private Button importFromSelectedItemButton;
    
        //Drawing references
        private ToolbarButton paintBrushButton;
        private ToolbarMenu brushSelector;
        private ToolbarButton anchorSetterButton;
        private TextField drawStateDisplay;
    
        //Symmetry references
        private EnumField symmetryAxisChoice;
        private SliderInt symmetryXAxisSlider;
        private SliderInt symmetryYAxisSlider;
        private VisualElement symmetryXAxisLine;
        private VisualElement symmetryYAxisLine;
        private Button symmetrySetToAnchorButton;
    
        //Grid content management references
        private Button addLineLeftButton;
        private Button addLineRightButton;
        private Button addLineUpButton;
        private Button addLineDownButton;
    
        private Button removeLineLeftButton;
        private Button removeLineRightButton;
        private Button removeLineUpButton;
        private Button removeLineDownButton;
    
        private Button moveContentLeftButton;
        private Button moveContentRightButton;
        private Button moveContentDownButton;
        private Button moveContentUpButton;

        //Data
        private string brushesPath => this.fileFolderPath.value + "/Brushes";
        private readonly List<VisualElement> horizontalSlotParents = new ();
        private SquareInfos[] squaresData;
        private AreaPattern[] brushPatterns;
        private AreaPattern currentBrushPattern;
        private Vector2Int anchorPosition;
        private E_DrawState drawState;
        private E_DrawTool selectedTool;
        private Vector2Int gridSize;
        private List<Vector2Int> currentHighlightedSquares = new List<Vector2Int>();
        private Vector2 symmetryAxis;
        private IShaped selectedItemScriptable;

        [MenuItem("Status/DrawingTool")]
        public static void Display()
        {
            DrawingToolEditorWindow wnd = GetWindow<DrawingToolEditorWindow>();
            wnd.titleContent = new GUIContent("DrawingToolEditorWindow");
        }

        #region UNITY_FLOW
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = this.rootVisualElement;

            // Instantiate UXML
            VisualElement treeAsset = this.m_VisualTreeAsset.Instantiate();
            root.Add(treeAsset);
    
            //Grid management
            this.gridSizeOption = treeAsset.Q<Vector2IntField>("gridSizeOption");
            this.confirmGridSizeButton = treeAsset.Q<Button>("confirmNewSize");
            this.confirmGridSizeButton.clicked += () => this.ChangeGridSize(this.gridSizeOption.value);

            this.squaresSizeOption = treeAsset.Q<Slider>("squareSizeOption");
            this.squaresSizeOption.RegisterValueChangedCallback(value => this.ResizeAllSquares(value.newValue));
        
            this.slotsScrollView = treeAsset.Q<ScrollView>("drawingZone");
            this.slotsScrollView.RegisterCallback<MouseLeaveEvent>((x) => { this.OnMouseLeaveDrawingArea();});
            this.slotsScrollView.RegisterCallback<MouseUpEvent>((x) => { this.ChangeDrawState(E_DrawState.NONE);});
        
            //File management
            this.fileFolderPath = treeAsset.Q<TextField>("fileFolderPath");
            this.fileName = treeAsset.Q<TextField>("fileName");
        
            this.exportButton = treeAsset.Q<Button>("exportButton");
            this.exportButton.clicked += this.OnExportButtonClicked;

            this.exportAsBrushButton = treeAsset.Q<Button>("exportAsBrushButton");
            this.exportAsBrushButton.clicked += this.OnExportBrushButtonClicked;

            this.exportToSelectedItemButton = treeAsset.Q<Button>("exportToSelectedButton");
            this.exportToSelectedItemButton.clicked += this.OnExportToSelectedItemButtonClicked;

            this.importFromSelectedItemButton = treeAsset.Q<Button>("importFromSelectedButton");
            this.importFromSelectedItemButton.clicked += this.OnImportFromSelectedItemButtonClicked;

            this.loadButton = treeAsset.Q<Button>("loadButton");
            this.loadButton.clicked += this.OnLoadButtonClicked;

            //Tools management
            this.drawStateDisplay = treeAsset.Q<TextField>("drawState");

            this.paintBrushButton = treeAsset.Q<ToolbarButton>("drawerButton");
            this.paintBrushButton.clicked += () => this.ChangeTool(E_DrawTool.DRAWER);

            this.brushSelector = treeAsset.Q<ToolbarMenu>("brushSelector");

            this.anchorSetterButton = treeAsset.Q<ToolbarButton>("anchorSetterButton");
            this.anchorSetterButton.clicked += () => this.ChangeTool(E_DrawTool.ANCHOR_SELECTOR);

            //Symmetry management
            this.symmetryAxisChoice = treeAsset.Q<EnumField>("symmetryAxis");
            this.symmetryAxisChoice.RegisterValueChangedCallback((x) => { this.OnSymmetryToolStateChanged((E_Axis)x.newValue); });
        
            this.symmetryXAxisSlider = treeAsset.Q<SliderInt>("symmetryXAxisSlider");
            this.symmetryYAxisSlider = treeAsset.Q<SliderInt>("symmetryYAxisSlider");
            this.symmetryXAxisSlider.RegisterValueChangedCallback((x) => { this.OnSymmetryAxisValueChanged(new Vector2Int(x.newValue, this.symmetryYAxisSlider.value)); });
            this.symmetryYAxisSlider.RegisterValueChangedCallback((x) => { this.OnSymmetryAxisValueChanged(new Vector2Int(this.symmetryXAxisSlider.value, x.newValue)); });
        
            this.symmetryXAxisLine = treeAsset.Q<VisualElement>("symmetryXAxisLine");
            this.symmetryYAxisLine = treeAsset.Q<VisualElement>("symmetryYAxisLine");

            this.symmetrySetToAnchorButton = treeAsset.Q<Button>("setSymmetryAxisToAnchorButton");
            this.symmetrySetToAnchorButton.clicked += this.SetSymmetryAxisToAnchor;
        
            //Grid content management
            this.moveContentLeftButton = treeAsset.Q<Button>("moveContentLeftButton");
            this.moveContentLeftButton.clicked += () => this.MoveContent(Vector2Int.left);
            this.moveContentRightButton = treeAsset.Q<Button>("moveContentRightButton");
            this.moveContentRightButton.clicked += () => this.MoveContent(Vector2Int.right);
            this.moveContentUpButton = treeAsset.Q<Button>("moveContentUpButton");
            this.moveContentUpButton.clicked += () => this.MoveContent(Vector2Int.up);
            this.moveContentDownButton = treeAsset.Q<Button>("moveContentDownButton");
            this.moveContentDownButton.clicked += () => this.MoveContent(Vector2Int.down);

            this.addLineLeftButton = treeAsset.Q<Button>("addLineLeftButton");
            this.addLineLeftButton.clicked += () => this.AddEdgeLine(E_Direction.LEFT);
            this.addLineRightButton = treeAsset.Q<Button>("addLineRightButton");
            this.addLineRightButton.clicked += () => this.AddEdgeLine(E_Direction.RIGHT);
            this.addLineUpButton = treeAsset.Q<Button>("addLineUpButton");
            this.addLineUpButton.clicked += () => this.AddEdgeLine(E_Direction.UP);
            this.addLineDownButton = treeAsset.Q<Button>("addLineDownButton");
            this.addLineDownButton.clicked += () => this.AddEdgeLine(E_Direction.DOWN);
        
            this.removeLineLeftButton = treeAsset.Q<Button>("removeLineLeftButton");
            this.removeLineLeftButton.clicked += () => this.RemoveEdgeLine(E_Direction.LEFT);
            this.removeLineRightButton = treeAsset.Q<Button>("removeLineRightButton");
            this.removeLineRightButton.clicked += () => this.RemoveEdgeLine(E_Direction.RIGHT);
            this.removeLineUpButton = treeAsset.Q<Button>("removeLineUpButton");
            this.removeLineUpButton.clicked += () => this.RemoveEdgeLine(E_Direction.UP);
            this.removeLineDownButton = treeAsset.Q<Button>("removeLineDownButton");
            this.removeLineDownButton.clicked += () => this.RemoveEdgeLine(E_Direction.DOWN);

            this.Setup();
        }

        private void Setup()
        {
            //Get brushes from assets
            this.RefreshBrushesList();

            //Instantiate initial rows
            this.squaresData = new SquareInfos[]{};
            this.ChangeGridSize(new Vector2Int(10,10));
        
            this.ResetAnchor();
        
            this.SetNewSymmetryAxisValue(Vector2.zero);
        
            this.OnSymmetryToolStateChanged(E_Axis.NONE);
            
            this.CheckSelectedItem();
        }

        private void OnSelectionChange()
        {
            this.CheckSelectedItem();
        }

        #endregion // UNITY_FLOW

        #region GRID_MANAGEMENT
    
        /// <summary>
        /// Add necessary rows and columns to fit the new dimension.
        /// </summary>
        private void ChangeGridSize(Vector2Int dimensions)
        {
            if (dimensions.x != this.gridSize.x)
            {
                if (dimensions.x < 1)
                    dimensions.x = 1;
                this.RefreshGridWidth(dimensions.x);
            }
        
            if (dimensions.y != this.gridSize.y)
            {
                if (dimensions.y < 1)
                    dimensions.y = 1;
                this.RefreshGridHeight(dimensions.y, dimensions.x);
            }
        
            this.RefreshGridSizeValues();
        }
    
        /// <summary>
        /// Clear grid and resize.
        /// </summary>
        private void ForceNewGrid(Vector2Int dimensions)
        {
            this.ClearGridContent();
            this.ChangeGridSize(dimensions);
        }

        private void ClearGridContent()
        {
            foreach (SquareInfos infos in this.squaresData)
                this.ChangeSquareState(infos, E_PointState.EMPTY);
        }

        private void RefreshGridSizeValues()
        {
            this.gridSizeOption.SetValueWithoutNotify(this.gridSize);
            this.RefreshSymmetryMaxAxis(this.gridSize);
        }

        //Width management
        private void RefreshGridWidth(int newWidth)
        {
            int difference = newWidth - this.gridSize.x;

            for (int i = 0; i < Mathf.Abs(difference); i++)
            {
                if(difference < 0)
                    this.RemoveColumnAt(this.gridSize.x - 1);
                else
                    this.AddColumnAt(this.gridSize.x);
            }
        }

        /// <summary>
        /// Insert column in grid at specific index.
        /// </summary>
        private void AddColumnAt(int newColumnIndex)
        {
            List<TemplateContainer> instantiatedSquares = new List<TemplateContainer>();

            for (int i = 0; i < this.horizontalSlotParents.Count(); i++)
                instantiatedSquares.Add(this.InstantiateSquare(new Vector2Int(newColumnIndex,i)));

            this.DataAddColumn(newColumnIndex, instantiatedSquares.ToArray());
        }

        /// <summary>
        /// Remove column in grid at specific index.
        /// </summary>
        private void RemoveColumnAt(int index)
        {
            int gridHeight = this.gridSize.y;
        
            for (int i = 0; i < gridHeight; i++)
                this.RemoveSquare(new Vector2Int(index,i));

            this.DataRemoveColumn(index);
        }
    
        //Height management
        private void RefreshGridHeight(int newHeight, int rowWidth)
        {
            int difference = newHeight - this.horizontalSlotParents.Count;

            if (difference < 0)
            {
                for (int i = 0; i < Mathf.Abs(difference); i++)
                    this.RemoveRowAt(this.horizontalSlotParents.Count - 1);
            }
            else
            {
                for (int i = 0; i < difference; i++)
                    this.AddRowAt(this.horizontalSlotParents.Count, rowWidth);
            }
        }
    
        /// <summary>
        /// Insert row in grid at specific index.
        /// </summary>
        private void AddRowAt(int newRowIndex, int initialSquaresCount)
        {
            VisualElement row = new ();
            row.AddToClassList("horizontalSlotStyle");
        
            this.slotsScrollView.Insert(newRowIndex, row);
            this.horizontalSlotParents.Insert(newRowIndex, row);

            List<TemplateContainer> instantiatedSquares = new List<TemplateContainer>();
            for (int i = 0; i < initialSquaresCount; i++)
                instantiatedSquares.Add(this.InstantiateSquare(new Vector2Int(i, newRowIndex)));
        
            this.DataAddRow(newRowIndex, instantiatedSquares.ToArray());
        }
    
        /// <summary>
        /// Remove row in grid at specific index.
        /// </summary>
        private void RemoveRowAt(int index)
        {
            VisualElement slotsParent = this.horizontalSlotParents[index];
            int childCount = slotsParent.childCount;

            this.slotsScrollView.Remove(this.horizontalSlotParents[index]);
            this.horizontalSlotParents.RemoveAt(index);
        
            for (int i = 0; i < childCount; i++)
            {
                Vector2Int square = new Vector2Int(i, index);

                if (this.currentHighlightedSquares.Count > 0 && this.currentHighlightedSquares.Contains(square))
                    this.currentHighlightedSquares.Remove(square);
            }
        
            if(index == this.anchorPosition.y)
                this.ResetAnchor();
        
            this.DataRemoveRow(index);
        
        }

        //Square management
        /// <summary>
        /// Resize grid square.
        /// </summary>
        private void ResizeSquare(IStyle style, float newSize)
        {
            style.width = newSize;
            style.height = newSize;
        }
    
        private void ResizeAllSquares(float newSize)
        {
            foreach (SquareInfos squareInfos in this.squaresData)
                this.ResizeSquare(squareInfos.SquareScaler.style, newSize);
        }
    
        private TemplateContainer InstantiateSquare(Vector2Int coordinate)
        {
            TemplateContainer newSquare = this.slotSquareAsset.Instantiate();
        
            this.horizontalSlotParents[coordinate.y].Insert(coordinate.x, newSquare);
        
            this.ResizeSquare(newSquare.Q<VisualElement>("squareScaler").style, this.squaresSizeOption.value);

            return newSquare;
        }
    
        private void RemoveSquare(Vector2Int coordinate)
        {
            if (coordinate == this.anchorPosition)
                this.ResetAnchor();

            if (this.currentHighlightedSquares.Count > 0 && this.currentHighlightedSquares.Contains(coordinate))
                this.currentHighlightedSquares.Remove(coordinate);

            VisualElement squareToRemove = this.squaresData[this.GridPositionToIndex(coordinate)].SquareTemplate;
            this.horizontalSlotParents[coordinate.y].Remove(squareToRemove);
        }
        #endregion //GRID_MANAGEMENT

        #region INPUTS_MANAGEMENT
        private void OnStartClickSquare(Vector2Int coordinates, int button)
        {
            if (button == 0)
                this.ChangeDrawState(E_DrawState.DRAWING);
            else if(button == 1)
                this.ChangeDrawState(E_DrawState.ERASING);
            else
                return;
        
            this.ApplyActionOnSquare(coordinates);
        }

        private void OnMouseLeaveDrawingArea()
        {
            this.ChangeDrawState(E_DrawState.NONE);
            this.ClearHighlightedSquares();
        }
        #endregion //INPUTS_MANAGEMENT
    
        #region TOOLS_METHODS
        /// <summary>
        /// Apply action when click on square.
        /// The action depends on the current selected tool and draw state.
        /// </summary>
        private void ApplyActionOnSquare(Vector2Int squareCoordinates)
        {
            switch (this.drawState)
            {
                case E_DrawState.NONE:
                    break;
                case E_DrawState.DRAWING:
                    ApplyDrawOnSquare();
                    break;
                case E_DrawState.ERASING:
                    ApplyEraseOnSquare();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ApplyHoverFeedback();

            void ApplyHoverFeedback()
            {
                switch (this.selectedTool)
                {
                    case E_DrawTool.DRAWER:
                        this.HighlightSquares(this.GetAllAffectedTiles(squareCoordinates));
                        break;
                    case E_DrawTool.ANCHOR_SELECTOR:
                        this.HighlightSquares(new Vector2Int[]{squareCoordinates});
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        
            void ApplyDrawOnSquare()
            {
                switch (this.selectedTool)
                {
                    case E_DrawTool.DRAWER:
                        this.ApplyBrush(squareCoordinates, E_PointState.FILL);
                        break;
                    case E_DrawTool.ANCHOR_SELECTOR:
                        this.SetNewAnchor(squareCoordinates);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        
            void ApplyEraseOnSquare()
            {
                switch (this.selectedTool)
                {
                    case E_DrawTool.DRAWER:
                        this.ApplyBrush(squareCoordinates, E_PointState.EMPTY);
                        break;
                    case E_DrawTool.ANCHOR_SELECTOR:
                        if(squareCoordinates == this.anchorPosition) this.ResetAnchor();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ApplyBrush(Vector2Int origin, E_PointState newState)
        {
            foreach (Vector2Int coordinates in this.GetAllAffectedTiles(origin))
                this.ChangeSquareState(this.squaresData[this.GridPositionToIndex(coordinates)], newState);
        }
    
        /// <summary>
        /// Get all affected tiles from context.
        /// </summary>
        private Vector2Int[] GetAllAffectedTiles(Vector2Int origin)
        {
            List<Vector2Int> result = this.GetBrushAreaCoordinates(origin).ToList();

            switch (this.symmetryAxisChoice.value)
            {
                case E_Axis.NONE :
                    break;
                case E_Axis.VERTICAL :
                    ProcessVertical();
                    break;
                case E_Axis.HORIZONTAL :
                    ProcessHorizontal();
                    break;
                case E_Axis.BOTH_HORIZONTAL_VERTICAL :
                    ProcessVertical();
                    ProcessHorizontal();
                    break;
            }

            void ProcessVertical()
            {
                int count = result.Count;

                for (int i = 0; i < count; i++)
                {
                    Vector2Int newPosition = new Vector2Int((int)(this.symmetryAxis.x * 2) - result[i].x - 1, result[i].y);
                    if(this.IsInGrid(newPosition))
                        result.Add(newPosition);
                }
            }

            void ProcessHorizontal()
            {
                int count = result.Count;

                for (int i = 0; i < count; i++)
                {
                    Vector2Int newPosition = new Vector2Int(result[i].x, (int)(this.symmetryAxis.y * 2) - result[i].y - 1);
                    if(this.IsInGrid(newPosition))
                        result.Add(newPosition);
                }
            }
        
            return result.ToArray();
        }
    
        private Vector2Int[] GetBrushAreaCoordinates(Vector2Int origin)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            if (this.currentBrushPattern is { IsEmpty: false })
            {
                foreach (KeyValuePair<Vector2Int, E_PointState> point in this.currentBrushPattern.GetActivePointsRelativeToAnchor())
                {
                    Vector2Int newCoordinates = origin + point.Key;
                    if(this.IsInGrid(newCoordinates))
                        result.Add(newCoordinates);
                }
            }
            else
            {
                result.Add(origin);
            }
            return result.ToArray();
        }
    
        private void ChangeSquareState(SquareInfos squareInfos, E_PointState newState)
        {
            if (squareInfos.PointState == newState)
                return;
        
            squareInfos.PointState = newState;
        
            VisualElement square = squareInfos.SquareSlot;
            square.AddToClassList(newState == E_PointState.EMPTY ? "empty" : "toggle");
            square.RemoveFromClassList(newState == E_PointState.EMPTY ? "toggle" : "empty");
        }
    
        //Highlight
        private void HighlightSquares(Vector2Int[] squares)
        {
            this.ClearHighlightedSquares();
        
            foreach (Vector2Int coordinates in squares)
                this.squaresData[this.GridPositionToIndex(coordinates)].SquareSlot.AddToClassList("highlight");

            this.currentHighlightedSquares = squares.ToList();
        }

        private void ClearHighlightedSquares()
        {
            foreach (Vector2Int coordinates in this.currentHighlightedSquares)
                this.squaresData[this.GridPositionToIndex(coordinates)].SquareSlot.RemoveFromClassList("highlight");
        
            this.currentHighlightedSquares.Clear();
        }

        //Anchor
        /// <summary>
        /// Set new position for the anchor.
        /// </summary>
        private void SetNewAnchor(Vector2Int newAnchorPosition)
        {
            if (!this.IsInGrid(newAnchorPosition))
                return;
        
            if(this.IsInGrid(this.anchorPosition))this.squaresData[this.GridPositionToIndex(this.anchorPosition)].SquareSlot.RemoveFromClassList("anchor");
        
            this.squaresData[this.GridPositionToIndex(newAnchorPosition)].SquareSlot.AddToClassList("anchor");
            this.anchorPosition = newAnchorPosition;
        }

        /// <summary>
        /// Reset anchors position to (0,0).
        /// </summary>
        private void ResetAnchor()
        {
            this.SetNewAnchor(new Vector2Int(0,0));
        }
    
        //Symmetry
        private void OnSymmetryToolStateChanged(E_Axis axis)
        {
            this.symmetryXAxisLine.visible = axis is E_Axis.VERTICAL or E_Axis.BOTH_HORIZONTAL_VERTICAL;
            this.symmetryYAxisLine.visible = axis is E_Axis.HORIZONTAL or E_Axis.BOTH_HORIZONTAL_VERTICAL;
        
            this.RefreshSymmetryAxis();
        }
    
        private void RefreshSymmetryMaxAxis(Vector2Int maxCoordinates)
        {
            this.symmetryXAxisSlider.highValue = maxCoordinates.x * 2;
            this.symmetryYAxisSlider.highValue = maxCoordinates.y * 2;
        
            this.RefreshSymmetryAxis();

            //Vector2Int newCoordinates = new Vector2Int(Mathf.Min(this.symmetryXAxisSlider.value, maxCoordinates.x), Mathf.Min(this.symmetryYAxisSlider.value, maxCoordinates.y));
        }

        /// <summary>
        /// Refresh symmetry axis visuals.
        /// </summary>
        private void RefreshSymmetryAxis()
        {
            if (this.horizontalSlotParents.Count == 0)
                return;

            float squareSize = this.squaresSizeOption.value;
        
            this.symmetryXAxisLine.PlaceInFront(this.horizontalSlotParents[^1]);
            this.symmetryXAxisLine.transform.position = new Vector2(squareSize * this.symmetryAxis.x, 0);
        
            this.symmetryYAxisLine.PlaceInFront(this.horizontalSlotParents[^1]);
            this.symmetryYAxisLine.transform.position = new Vector2(0, -squareSize * this.symmetryAxis.y); //Negative bc I inverted the squares order in vertical (0,0 is bottom left instead of top left)
        }
    
        /// <summary>
        /// Change symmetry axis point.
        /// </summary>
        private void SetNewSymmetryAxisValue(Vector2 newAxis)
        {
            this.symmetryAxis = newAxis;
            this.symmetryXAxisSlider.SetValueWithoutNotify((int)(newAxis.x * 2));
            this.symmetryYAxisSlider.SetValueWithoutNotify((int)(newAxis.y * 2));
            this.RefreshSymmetryAxis();
        }

        private void OnSymmetryAxisValueChanged(Vector2Int newAxis)
        {
            this.SetNewSymmetryAxisValue(new Vector2(newAxis.x / 2f, newAxis.y / 2f));
        }

        /// <summary>
        /// Change axis point to anchor's position.
        /// </summary>
        private void SetSymmetryAxisToAnchor()
        {
            this.SetNewSymmetryAxisValue(this.anchorPosition + Vector2.one * 0.5f);
        }

        //Drawing
        /// <summary>
        /// Set new draw state.
        /// </summary>
        /// <param name="newState"></param>
        private void ChangeDrawState(E_DrawState newState)
        {
            this.drawState = newState;
            this.drawStateDisplay.SetValueWithoutNotify(newState.ToString());
        }

        /// <summary>
        /// Select new tool.
        /// </summary>
        /// <param name="newTool"></param>
        private void ChangeTool(E_DrawTool newTool)
        {
            this.selectedTool = newTool;
        }
    
        private void SelectNewBrush(int index)
        {
            this.currentBrushPattern = this.brushPatterns[index];
            this.ChangeTool(E_DrawTool.DRAWER);
        }
    
        //Row & Column actions
        /// <summary>
        /// Move every squares states and anchor toward direction.
        /// </summary>
        private void MoveContent(Vector2Int direction) //[TODO] Absolutely not optimised
        {
            E_PointState[] newPositions = new E_PointState[this.squaresData.Length];

            for (int i = 0; i < this.squaresData.Length; i++)
            {
                Vector2Int coordinates = this.IndexToGridPosition(i);
                Vector2Int dataRefCoordinates = coordinates - direction;
                newPositions[i] = this.IsInGrid(dataRefCoordinates) ? this.squaresData[this.GridPositionToIndex(dataRefCoordinates)].PointState : E_PointState.EMPTY;
            }

            this.ClearGridContent();

            for (int i = 0; i < newPositions.Length; i++)
            {
                this.ChangeSquareState(this.squaresData[i], newPositions[i]);
            }
            
        
            if(this.IsInGrid(this.anchorPosition + direction))
                this.SetNewAnchor(this.anchorPosition + direction);
        }

        /// <summary>
        /// Remove a line at the extremity of the grid, according to the selected direction.
        /// </summary>
        /// <param name="direction">Edge position.</param>
        private void RemoveEdgeLine(E_Direction direction)
        {
            Vector2Int directionVector = this.DirectionToVector(direction);
            Vector2Int newSize = this.gridSize - new Vector2Int(Mathf.Abs(directionVector.x), Mathf.Abs(directionVector.y));

            if (newSize is { x: >= 1, y: >= 1 })
            {
                switch (direction)
                {
                    case E_Direction.LEFT:
                        this.RemoveColumnAt(0);
                        break;
                    case E_Direction.RIGHT:
                        this.RemoveColumnAt(this.gridSize.x - 1);
                        break;
                    case E_Direction.UP:
                        this.RemoveRowAt(this.gridSize.y - 1);
                        break;
                    case E_Direction.DOWN:
                        this.RemoveRowAt(0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
                this.RefreshGridSizeValues();
            }
        }

        /// <summary>
        /// Add a line at the extremity of the grid, according to the selected direction.
        /// </summary>
        /// <param name="direction">Edge position.</param>
        private void AddEdgeLine(E_Direction direction)
        {
            Vector2Int directionVector = this.DirectionToVector(direction);
            Vector2Int newSize = this.gridSize + new Vector2Int(Mathf.Abs(directionVector.x), Mathf.Abs(directionVector.y));

            if (newSize is { x: >= 1, y: >= 1 })
            {
                switch (direction)
                {
                    case E_Direction.LEFT:
                        this.AddColumnAt(0);
                        break;
                    case E_Direction.RIGHT:
                        this.AddColumnAt(this.gridSize.x);
                        break;
                    case E_Direction.UP:
                        this.AddRowAt(this.gridSize.y, this.gridSize.x);
                        break;
                    case E_Direction.DOWN:
                        this.AddRowAt(0, this.gridSize.x);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
                this.RefreshGridSizeValues();
            }
        }
    
        #endregion //TOOLS_METHODS

        #region EXPORT_DATA
        private void OnExportButtonClicked()
        {
            this.ExportData(this.fileFolderPath.value, this.fileName.value);
        }

        private void OnExportBrushButtonClicked()
        {
            this.ExportData(this.brushesPath, this.fileName.value);
            this.RefreshBrushesList();
        }

        private void OnExportToSelectedItemButtonClicked()
        {
            if(this.selectedItemScriptable == null)
                return;

            (Vector2Int, Vector2Int) contentBounds = this.GetGridContentBounds();
            Vector2Int newSize = (contentBounds.Item2 + Vector2Int.one) - contentBounds.Item1;
            Debug.Log($"grid size = {newSize}. Min : {contentBounds.Item1}. Max : {contentBounds.Item2}.");
            
            List<bool> exportedShape = new List<bool>();

            for (int y = 0; y < newSize.y; y++) //Inverted because the grid is visually inverted so we need to save data in good order
            {
                for (int x = 0; x < newSize.x; x++)
                {
                    Vector2Int relativePos = new Vector2Int(x, y) + contentBounds.Item1;
                    bool value = this.squaresData[GridPositionToIndex(relativePos)].PointState > 0;
                    exportedShape.Add(value);
                }
            }
            this.selectedItemScriptable.Shape = new Shape(newSize, this.anchorPosition, exportedShape.ToArray());
            EditorUtility.SetDirty((Object)this.selectedItemScriptable);
        }

        private void OnImportFromSelectedItemButtonClicked()
        {
            if (this.selectedItemScriptable == null)
                return;

            Shape loadedShape = this.selectedItemScriptable.Shape;
            
            this.ForceNewGrid(loadedShape.shapeSize);
            this.SetNewAnchor(loadedShape.anchor);

            for (int i = 0; i < loadedShape.shapeContent.Length; i++)
                this.ChangeSquareState(this.squaresData[i], loadedShape.shapeContent[i] == false ? E_PointState.EMPTY : E_PointState.FILL);
        }

        private void OnLoadButtonClicked()
        {
            this.LoadData(this.fileFolderPath.value, this.fileName.value);
        }

        /// <summary>
        /// Export all grid data in a .ptrn text file in the selected folder.
        /// </summary>
        /// <param name="location">Folder path</param>
        /// <param name="nameFile">File name</param>
        private void ExportData(string location, string nameFile)
        {
            string path = $"{location}/{nameFile}.ptrn";

            if (!File.Exists(path))
                File.Create(path).Close();

            using (StreamWriter streamWriter = new (path))
            {
                foreach (string text in this.SerializeData())
                    streamWriter.WriteLine(text);
            }

            Debug.Log($"Exported drawer data to : {path}");
        }
    
        /// <summary>
        /// Load data from a .ptrn text file and override current grid with it.
        /// </summary>
        /// <param name="location">Folder path</param>
        /// <param name="nameFile">File name</param>
        private void LoadData(string location, string nameFile)
        {
            string path = $"{location}/{nameFile}.ptrn";
            AreaPattern pattern = new (path);

            if (!pattern.IsEmpty)
            {
                Debug.Log($"Load grid size : {pattern.Size} and anchor at {pattern.Anchor}.");
                this.ForceNewGrid(pattern.Size);
                this.SetNewAnchor(pattern.Anchor);
            
                foreach (KeyValuePair<Vector2Int, E_PointState> info in pattern.PointsData)
                    this.ChangeSquareState(this.squaresData[this.GridPositionToIndex(info.Key)], info.Value);

                Debug.Log($"Grid loaded successfully from : {path}");
            }
            else
            {
                Debug.Log($"Load failed. No data exists at path : {path}");
            }
        }

        /// <summary>
        /// Translate current data into array of strings in order to write it in a file.
        /// </summary>
        private string[] SerializeData()
        {
            (Vector2Int, Vector2Int) contentBounds = this.GetGridContentBounds();

            Vector2Int newSize = contentBounds.Item2 - contentBounds.Item1;
            Vector2Int newGridSize = newSize + Vector2Int.one;
        
            string[] result = new string[newGridSize.y + 1];
        
            //Grid dimensions
            result[0] = $"[{newGridSize.x};{newGridSize.y}]";

            //Anchor position
            Vector2Int newAnchorPosition = this.anchorPosition - contentBounds.Item1;
            result[0] += $"[{newAnchorPosition.x};{newAnchorPosition.y}]";
        
            //Squares data
            bool isFirstValue = true;

            for (int i = 0; i < this.squaresData.Length; i++)
            {
                Vector2Int newCoordinates = this.IndexToGridPosition(i) - contentBounds.Item1;

                if (newCoordinates is { x: >= 0, y: >= 0 } && newCoordinates.x <= newSize.x && newCoordinates.y <= newSize.y)
                {
                    result[newCoordinates.y + 1] += $"[{newCoordinates.x};{newCoordinates.y}:{(int)this.squaresData[i].PointState}]";
                    if (isFirstValue)
                        isFirstValue = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Extract coordinates of non-empty squares in current grid.
        /// </summary>
        private Vector2Int[] GetGridContentOnly()
        {
            List<Vector2Int> result = new List<Vector2Int>();

            for (int i = 0; i < this.squaresData.Length; i++)
            {
                if(this.squaresData[i].PointState != E_PointState.EMPTY)
                    result.Add(this.IndexToGridPosition(i));
            }

            return result.ToArray();
        }

        private void CheckSelectedItem()
        {
            this.selectedItemScriptable = Selection.activeObject as IShaped;

            this.exportToSelectedItemButton.SetEnabled(this.selectedItemScriptable != null);
            this.importFromSelectedItemButton.SetEnabled(this.selectedItemScriptable != null);
        }
        #endregion //EXPORT_DATA
    
        #region UTILITIES
        /// <summary>
        /// Find every brushes from brushes folder and add them to the list.
        /// </summary>
        private void RefreshBrushesList()
        {
            List<AreaPattern> areaPatterns = new ();

            DirectoryInfo dirInfo = new (this.brushesPath);
            FileInfo[] filesInfo = dirInfo.GetFiles();

            foreach (FileInfo info in filesInfo)
            {
                if (info.Extension == ".ptrn")
                    areaPatterns.Add(new AreaPattern(info.FullName));
            }

            this.brushPatterns = areaPatterns.ToArray();

            this.brushSelector.menu.MenuItems().Clear();

            for (int i = 0; i < areaPatterns.Count; i++)
            {
                int index = i;
                this.brushSelector.menu.AppendAction(areaPatterns[i].PatternName, x => this.SelectNewBrush(index));
            }
        }

        /// <summary>
        /// Check if the current coordinates are inside the bounds of the grid.
        /// </summary>
        private bool IsInGrid(Vector2Int coordinates)
        {
            return coordinates.x >= 0 && coordinates.x < this.gridSize.x && coordinates.y >= 0 && coordinates.y < this.gridSize.y;
        }

        /// <summary>
        /// Translate E_Direction enum to corresponding vector.
        /// </summary>
        private Vector2Int DirectionToVector(E_Direction direction)
        {
            Vector2Int result;
            switch (direction)
            {
                case E_Direction.LEFT:
                    result = Vector2Int.left;
                    break;
                case E_Direction.RIGHT:
                    result = Vector2Int.right;
                    break;
                case E_Direction.UP:
                    result = Vector2Int.up;
                    break;
                case E_Direction.DOWN:
                    result = Vector2Int.down;
                    break;
                default:
                    result = Vector2Int.zero;
                    break;
            }

            return result;
        }
        
        private (Vector2Int lowerBound, Vector2Int upperBound) GetGridContentBounds()
        {
            Vector2Int[] content = this.GetGridContentOnly();
            Vector2Int lowerPosition = this.anchorPosition;
            Vector2Int upperPosition = this.anchorPosition;
        
            foreach (Vector2Int coordinate in content)
            {
                if (coordinate.x < lowerPosition.x)
                    lowerPosition.x = coordinate.x;
                if (coordinate.y < lowerPosition.y)
                    lowerPosition.y = coordinate.y;
            
                if (coordinate.x > upperPosition.x)
                    upperPosition.x = coordinate.x;
                if (coordinate.y > upperPosition.y)
                    upperPosition.y = coordinate.y;
            }

            return (lowerPosition, upperPosition);
        }
        #endregion //UTILITIES
    
        #region GRID_DATA
    
        private int GridPositionToIndex(Vector2Int coordinates)
        {
            return this.GridPositionToIndex(coordinates, this.gridSize.x);
        }
    
        /// <summary>
        /// Translate a 2D position to a 1D array index.
        /// </summary>
        private int GridPositionToIndex(Vector2Int coordinates, int width)
        {
            return coordinates.y * width + coordinates.x;
        }

        private Vector2Int IndexToGridPosition(int index)
        {
            return this.IndexToGridPosition(index, this.gridSize.x);
        }
    
        /// <summary>
        /// Translate an array index to a 2D position.
        /// </summary>
        private Vector2Int IndexToGridPosition(int index, int width)
        {
            return new Vector2Int(index % width, Mathf.FloorToInt((float)index / width));
        }

        /// <summary>
        /// Register that a row has been added at specific index and update all data.
        /// </summary>
        private void DataAddRow(int index, TemplateContainer[] squareRefs)
        {
            SquareInfos[] newData = new SquareInfos[this.squaresData.Length + this.gridSize.x];

            for (int i = 0; i < this.squaresData.Length; i++)
            {
                Vector2Int pos = this.IndexToGridPosition(i);
                newData[i + (pos.y >= index ? this.gridSize.x : 0)] = this.squaresData[i];
            }

            for (int i = 0; i < this.gridSize.x; i++)
                newData[this.GridPositionToIndex(new Vector2Int(i, index))] = new SquareInfos(squareRefs[i], new Vector2Int(i, index), this.ApplyActionOnSquare, this.OnStartClickSquare);

            this.squaresData = newData;
            this.gridSize.y++;
            this.RefreshSquaresCoordinates();
        
            if(index <= this.anchorPosition.y)
                this.anchorPosition.y++;
        
            if (index <= this.symmetryAxis.y)
                this.SetNewSymmetryAxisValue(this.symmetryAxis + Vector2Int.up);
        }
    
        /// <summary>
        /// Register that a row has been removed at specific index and update all data.
        /// </summary>
        private void DataRemoveRow(int index)
        {
            SquareInfos[] newData = new SquareInfos[this.squaresData.Length - this.gridSize.x];

            for (int i = 0; i < newData.Length; i++)
            {
                Vector2Int pos = this.IndexToGridPosition(i);
                newData[i] = this.squaresData[i + (pos.y >= index ? this.gridSize.x : 0)];
            }
        
            this.squaresData = newData;
            this.gridSize.y--;
            this.RefreshSquaresCoordinates();
        
        
            if(index < this.anchorPosition.y)
                this.anchorPosition.y--;
            else if(index == this.anchorPosition.y)
                this.ResetAnchor();
        
            if (index < this.symmetryAxis.y && this.symmetryAxis.y >= 1)
                this.SetNewSymmetryAxisValue(this.symmetryAxis + Vector2Int.down);
        }
    
        /// <summary>
        /// Register that a column has been added at specific index and update all data.
        /// </summary>
        private void DataAddColumn(int newColumnIndex, TemplateContainer[] squareRefs)
        {
            SquareInfos[] newData = new SquareInfos[this.squaresData.Length + this.gridSize.y];

            int squaresRefNextIndex = 0;
            int squaresDataNextIndex = 0;
        
            for (int i = 0; i < newData.Length; i++)
            {
                Vector2Int coordinates = this.IndexToGridPosition(i, this.gridSize.x + 1);
                if ( coordinates.x == newColumnIndex)
                {
                    newData[i] = new SquareInfos(squareRefs[squaresRefNextIndex],new Vector2Int(newColumnIndex, coordinates.y), this.ApplyActionOnSquare, this.OnStartClickSquare);
                    squaresRefNextIndex++;
                }
                else
                {
                    newData[i] = this.squaresData[squaresDataNextIndex];
                    squaresDataNextIndex++;
                }
            }
        
            this.squaresData = newData;
            this.gridSize.x++;
            this.RefreshSquaresCoordinates();
        
            if(newColumnIndex <= this.anchorPosition.x)
                this.anchorPosition.x++;
        
            if (newColumnIndex <= this.symmetryAxis.x)
                this.SetNewSymmetryAxisValue(this.symmetryAxis + Vector2Int.right);
        }

        /// <summary>
        /// Register that a column has been added at specific index and update all data.
        /// </summary>
        private void DataRemoveColumn(int index)
        {
            List<SquareInfos> newData = new List<SquareInfos>();

            for (int i = 0; i < this.squaresData.Length; i++)
            {
                if((i % this.gridSize.x) != index)
                    newData.Add(this.squaresData[i]);
            }
        
            this.squaresData = newData.ToArray();
            this.gridSize.x--;
            this.RefreshSquaresCoordinates();
        
            if(index < this.anchorPosition.x)
                this.anchorPosition.x--;
            if(index == this.anchorPosition.x)
                this.ResetAnchor();
        
            if (index <= this.symmetryAxis.y && this.symmetryAxis.x >= 1)
                this.SetNewSymmetryAxisValue(this.symmetryAxis + Vector2Int.left);
        }
    
        /// <summary>
        /// Update all squares intern coordinates to match squaresData info.
        /// (could be optimised)
        /// </summary>
        private void RefreshSquaresCoordinates()
        {
            for (int x = 0; x < this.gridSize.x; x++)
            {
                for (int y = 0; y < this.gridSize.y; y++)
                {
                    Vector2Int coord = new Vector2Int(x, y);
                    this.squaresData[this.GridPositionToIndex(coord)].SetCoordinates(coord);
                }
            }
        }
        #endregion //GRID_DATA
    }

    public enum E_DrawState
    {
        NONE,
        DRAWING,
        ERASING,
    }

    public enum E_DrawTool
    {
        DRAWER,
        ANCHOR_SELECTOR,
    }

    public enum E_Axis
    {
        NONE,
        HORIZONTAL,
        VERTICAL,
        BOTH_HORIZONTAL_VERTICAL,
    }

    public enum E_Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public class SquareInfos
    {
        public Vector2Int Coordinates { get; private set; }

        public void SetCoordinates(Vector2Int value)
        {
            this.SquareTemplate.name = $"Square {value}";
            this.Coordinates = value;
        }

        public readonly TemplateContainer SquareTemplate;
    
        public E_PointState PointState;
        public VisualElement SquareSlot => this.SquareTemplate.Q<VisualElement>("squareSlot");
        public VisualElement SquareScaler => this.SquareTemplate.Q<VisualElement>("squareScaler");

        private Action<Vector2Int> onHoverMethod;
        private Action<Vector2Int, int> onClickedMethod;

        public SquareInfos(TemplateContainer squareTemplate, Vector2Int coordinates, Action<Vector2Int> onHover, Action<Vector2Int, int> onClicked)
        {
            this.SquareTemplate = squareTemplate;
            this.PointState = E_PointState.EMPTY;

            this.SetCoordinates(coordinates);
        
            this.onHoverMethod = onHover;
            this.onClickedMethod = onClicked;

            squareTemplate.RegisterCallback<MouseOverEvent>((x) => { this.onHoverMethod.Invoke(this.Coordinates); });
            squareTemplate.RegisterCallback<MouseDownEvent>((x) => { this.onClickedMethod.Invoke(this.Coordinates, x.button);});
        }
    }
}