using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilemapStyx : EditorWindow
{
    private enum SORTING_LAYERS { Background, Midground, Foreground};
    private enum COLLISION_LAYERS { Collision, NoCollision };

    private readonly List<MapObject> _backgroundPalette = new List<MapObject>();
    private readonly List<MapObject> _midgroundPalette = new List<MapObject>();
    private readonly List<MapObject> _foregroundPalette = new List<MapObject>();
    private readonly Vector2 _cellSize = new Vector2(1.0f, 1.0f);
    private readonly string _rootPath = "Assets/Editor Default Resources";
    private readonly string _backgroundPath = "Assets/Editor Default Resources/Background";
    private readonly string _midgroundPath = "Assets/Editor Default Resources/Midground";
    private readonly string _foregroundPath = "Assets/Editor Default Resources/Foreground";
    private readonly int _paletteColumnSize = 3;
	private List<MapObject> _currentPalette;
	private GameObject _tilemapStyxRoot;
    private GameObject _tilemapStyxRootPrefab;
    private Vector2 tileGridScrollPosition;
    private Color _cellColor;
	private SORTING_LAYERS _sortingLayer;
	private COLLISION_LAYERS _collisionLayer;
	private int _paletteIndex;
    private int _brushSize;
	private bool _paletteToggle;
	private bool _ruleTileToggle;
	private bool _shortcutToggle;
    private bool _paintMode;
    private bool _ruleTileMode;
    private bool _holdBrush;
    private bool _isHoldingMouse;


    [MenuItem("Window/Tilemap Styx")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TilemapStyx));
    }

    void Awake()
    {
		_paletteToggle = true;
		_currentPalette = _backgroundPalette;
    }

    void Update()
    {
        Repaint();
    }

    #region OnGui
    void OnGUI()
    {
        DisplayModeToggleSection();
		DisplaySelectedToggleMode();
    }

    private void DisplayModeToggleSection()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal("box");
        _paletteToggle = GUILayout.Toggle(_paletteToggle, "Palettes", "Button", GUILayout.Height(40f));
        _ruleTileToggle = GUILayout.Toggle(_ruleTileToggle, "Rule Tiles", "Button", GUILayout.Height(40f));
        _shortcutToggle = GUILayout.Toggle(_shortcutToggle, "Shortcuts", "Button", GUILayout.Height(40f));
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

	private void DisplaySelectedToggleMode()
	{
		if (_paletteToggle)
		{
			DisplayPaintSection();
			DisplayTileGrid();
		}
		else if (_ruleTileToggle)
		{

		}
		else
		{

		}
	}

	#region Palette
	private void DisplayPaintSection()
    {
        EditorGUILayout.Space();
        if (_paintMode)
            _paintMode = GUILayout.Toggle(_paintMode, "Stop painting", "Button", GUILayout.Height(60f));
        else
            _paintMode = GUILayout.Toggle(_paintMode, "Start painting", "Button", GUILayout.Height(60f));

        _brushSize = EditorGUILayout.IntSlider("Brush Size:", _brushSize, 1, 10);
        _ruleTileMode = GUILayout.Toggle(_ruleTileMode, "Rule tile:", "Toggle", GUILayout.Height(20f));
        _holdBrush = GUILayout.Toggle(_holdBrush, "Hold brush:", "Toggle", GUILayout.Height(20f));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayTileGrid()
    {
        EditorGUILayout.Space();
        GUILayout.FlexibleSpace();
        _collisionLayer = (COLLISION_LAYERS)EditorGUILayout.EnumPopup("", _collisionLayer);
        GUILayout.FlexibleSpace();
        _sortingLayer = (SORTING_LAYERS)EditorGUILayout.EnumPopup("", _sortingLayer);

        GUIStyle centerLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
		if (_paletteIndex >= 0 && _paletteIndex < _currentPalette.Count -1) 
		{
			EditorGUILayout.LabelField(_currentPalette[_paletteIndex].name, centerLabelStyle, GUILayout.ExpandWidth(true));
		}
		GUILayout.BeginVertical();
        tileGridScrollPosition = GUILayout.BeginScrollView(tileGridScrollPosition, false, true, GUILayout.MinWidth(350), GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true), GUILayout.MinHeight(100), GUILayout.MaxHeight(800));
        List<GUIContent> paletteIcons = new List<GUIContent>();
        switch (_sortingLayer)
		{
            case SORTING_LAYERS.Background:
                _currentPalette = _backgroundPalette;
                break;
            case SORTING_LAYERS.Midground:
                _currentPalette = _midgroundPalette;
                break;
            case SORTING_LAYERS.Foreground:
                _currentPalette = _foregroundPalette;
                break;
        }
        foreach (MapObject mapObject in _currentPalette)
        {
            {
                Texture2D texture = AssetPreview.GetAssetPreview(mapObject.gameObject);
                GUIContent tileGuiContent = new GUIContent() { image = texture, tooltip = mapObject.name };
                paletteIcons.Add(tileGuiContent);
            }
        }
        _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), _paletteColumnSize, GUILayout.MinWidth(150), GUILayout.MaxWidth(200), GUILayout.ExpandWidth(true), GUILayout.MinHeight(600), GUILayout.MaxHeight(700));
		GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
	#endregion

	#region Rule Tile

	#endregion
	#region Shortcut

	#endregion

	#endregion

	#region Focus And Destroy
	void OnFocus()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
        RefreshPalette();
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void RefreshPalette()
    {
        _tilemapStyxRoot = null;
        _backgroundPalette.Clear();
        _midgroundPalette.Clear();
        _foregroundPalette.Clear();
        Directory.CreateDirectory(_rootPath);
        Directory.CreateDirectory(_backgroundPath);
        Directory.CreateDirectory(_midgroundPath);
        Directory.CreateDirectory(_foregroundPath);

        string[] root = Directory.GetFiles(_rootPath, "*.prefab");
        foreach (string prefabFile in root)
        {
            _tilemapStyxRoot = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
        }

        string[] backgroundFiles = Directory.GetFiles(_backgroundPath, "*.prefab");
        foreach (string backgroundFile in backgroundFiles)
        {
            _backgroundPalette.Add(AssetDatabase.LoadAssetAtPath(backgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] midgroundFiles = Directory.GetFiles(_midgroundPath, "*.prefab");
        foreach (string midgroundFile in midgroundFiles)
        {
            _midgroundPalette.Add(AssetDatabase.LoadAssetAtPath(midgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] foregroundFiles = Directory.GetFiles(_foregroundPath, "*.prefab");
        foreach (string foregroundFile in foregroundFiles)
        {
            _foregroundPalette.Add(AssetDatabase.LoadAssetAtPath(foregroundFile, typeof(MapObject)) as MapObject);
        }
    }
    #endregion


    #region OnSceneGUI
    private void OnSceneGUI(SceneView sceneView)
    {
        if (_paintMode)
        {
            Vector2 cellCenter = GetSelectedCell();
            ShowSpritePointer(cellCenter);
            HandleSceneViewInputs(cellCenter);
            sceneView.Repaint();
        }
    }

    private Vector2 GetSelectedCell()
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

        Vector2Int cell = new Vector2Int(Mathf.FloorToInt(mousePosition.x / _cellSize.x), Mathf.FloorToInt(mousePosition.y / _cellSize.y));
        Vector2 cellCenter = cell * _cellSize;
        return cellCenter;
    }

    private void ShowSpritePointer(Vector2 cellCenter)
    {
        Vector3 topLeft = cellCenter + Vector2.up * _cellSize;
        Vector3 topRight = cellCenter + Vector2.right * _cellSize + Vector2.up * _cellSize;
        Vector3 bottomLeft = cellCenter;
        Vector3 bottomRight = cellCenter + Vector2.right * _cellSize;

        Handles.color = _cellColor;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        ConsumeClickEvent();
        SwitchBetweenTiles();

        if (Event.current.control)
        {
            _cellColor = Color.red;
        }
        else
        {
            _cellColor = Color.white;
        }
        if (Event.current.type == EventType.MouseDown)
        {
            _isHoldingMouse = true;
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            _isHoldingMouse = false;
        }
        if (_paletteIndex < _currentPalette.Count && Event.current.button == 0 && Event.current.type == EventType.MouseDown || _paletteIndex < _currentPalette.Count && _holdBrush && _isHoldingMouse)
        {
            MapObject mapObject = _currentPalette[_paletteIndex];
            if (Event.current.control)
            {
                RemoveMapObject(cellCenter, mapObject.SortingLayerID);
            }
            else
            {
                AddMapObject(cellCenter, mapObject);
            }
        }
    }

    private void ConsumeClickEvent()
    {
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0);
        }
    }

    private void SwitchBetweenTiles()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D)
        {
			if (_paletteIndex % _paletteColumnSize - 2 == 0)
			{
				tileGridScrollPosition.y += 100;
			}

			if (_paletteIndex != _currentPalette.Count - 1)
            {
				_paletteIndex++;
            }
            else
            {
				tileGridScrollPosition.y = 0;
				_paletteIndex = 0;
            }
		}
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
        {
			if (_paletteIndex % _paletteColumnSize == 0)
			{
				tileGridScrollPosition.y -= 100;
			}

			if (_paletteIndex != 0)
            {
				_paletteIndex--;
			}
			else
            {
				tileGridScrollPosition.y = _currentPalette.Count * 10;
				_paletteIndex = _currentPalette.Count - 1;
            }
		}
    }

    private void RemoveMapObject(Vector2 cellCenter, int sortingLayerID)
    {
        MapObject previousObject = MapEditorModel.Get(cellCenter, sortingLayerID);
        if (previousObject)
        {
            Undo.DestroyObjectImmediate(previousObject.gameObject);
        }
    }

    private void AddMapObject(Vector2 cellCenter, MapObject prefab)
    {
        RemoveMapObject(cellCenter, prefab.SortingLayerID);

        MapObject mapObject = PrefabUtility.InstantiatePrefab(prefab) as MapObject;
        mapObject.transform.position = cellCenter + _cellSize * 0.5f;
        //if (_tilemapStyxRootPrefab == null)
        //{
        //    _tilemapStyxRootPrefab = PrefabUtility.InstantiatePrefab(_tilemapStyxRoot) as GameObject;
        //    PrefabUtility.UnpackPrefabInstance(_tilemapStyxRootPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        //}
        //mapObject.transform.SetParent(_tilemapStyxRootPrefab.transform);

        Undo.RegisterCreatedObjectUndo(mapObject, mapObject.name);
    }
    #endregion
}
