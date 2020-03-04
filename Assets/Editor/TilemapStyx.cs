using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilemapStyx : EditorWindow
{
    private enum SORTING_LAYERS { Background, Midground, Foreground};
    private enum COLLISION_LAYERS { Collision, NoCollision };

    private readonly List<MapObject> _backgroundNoCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _midgroundNoCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _foregroundNoCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _backgroundCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _midgroundCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _foregroundCollisionPalette = new List<MapObject>();
    private readonly Vector2 _cellSize = new Vector2(1.0f, 1.0f);
    private readonly string _rootPath = "Assets/Editor Default Resources";
    private readonly string _backgroundNoCollisionPath = "Assets/Editor Default Resources/NoCollision/Background";
    private readonly string _midgroundNoCollisionPath = "Assets/Editor Default Resources/NoCollision/Midground";
    private readonly string _foregroundNoCollisionPath = "Assets/Editor Default Resources/NoCollision/Foreground";
    private readonly string _backgroundCollisionPath = "Assets/Editor Default Resources/Collision/Background";
    private readonly string _midgroundCollisionPath = "Assets/Editor Default Resources/Collision/Midground";
    private readonly string _foregroundCollisionPath = "Assets/Editor Default Resources/Collision/Foreground";
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
    private bool _isPaletteTogglePressed;
    private bool _isRuleTileTogglePressed;
    private bool _isShortcutTogglePressed;


    [MenuItem("Window/Tilemap Styx")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TilemapStyx));
    }

    void Awake()
    {
		_paletteToggle = true;
		_currentPalette = _backgroundNoCollisionPalette;
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
        DisallowUntoggling();
        if (_paletteToggle && !_isPaletteTogglePressed)
        {
            _isPaletteTogglePressed = true;
            _isRuleTileTogglePressed = false;
            _isShortcutTogglePressed = false;
            _ruleTileToggle = false;
            _shortcutToggle = false;
        }
        if (_ruleTileToggle && !_isRuleTileTogglePressed)
        {
            _isRuleTileTogglePressed = true;
            _isPaletteTogglePressed = false;
            _isShortcutTogglePressed = false;
            _paletteToggle = false;
            _shortcutToggle = false;
        }
        if (_shortcutToggle && !_isShortcutTogglePressed)
		{
            _isShortcutTogglePressed = true;
            _isPaletteTogglePressed = false;
            _isRuleTileTogglePressed = false;
            _paletteToggle = false;
            _ruleTileToggle = false;
        }

        if (_paletteToggle)
        {
            DisplayPaintSection();
            DisplayTileGrid();
        }
        if (_ruleTileToggle)
        {
            DisplayRuleTile();
        }
        if (_shortcutToggle)
        {
            DisplayShortcut();
        }
    }

    private void DisallowUntoggling()
    {
        if (_isPaletteTogglePressed)
        {
            _paletteToggle = true;
        }
        if (_isRuleTileTogglePressed)
        {
            _ruleTileToggle = true;
        }
        if (_isShortcutTogglePressed)
        {
            _shortcutToggle = true;
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
		if (_paletteIndex >= 0 && _paletteIndex <= _currentPalette.Count -1) 
		{
			EditorGUILayout.LabelField(_currentPalette[_paletteIndex].name, centerLabelStyle, GUILayout.ExpandWidth(true));
		}

        if (_currentPalette.Count > 0)
        {
            GUILayout.BeginVertical();
            tileGridScrollPosition = GUILayout.BeginScrollView(tileGridScrollPosition, false, true, GUILayout.MinWidth(350), GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true), GUILayout.MinHeight(100), GUILayout.MaxHeight(800));
            List<GUIContent> paletteIcons = new List<GUIContent>();
            if (_collisionLayer == COLLISION_LAYERS.NoCollision)
            {
                switch (_sortingLayer)
                {
                    case SORTING_LAYERS.Background:
                        _currentPalette = _backgroundNoCollisionPalette;
                        break;
                    case SORTING_LAYERS.Midground:
                        _currentPalette = _midgroundNoCollisionPalette;
                        break;
                    case SORTING_LAYERS.Foreground:
                        _currentPalette = _foregroundNoCollisionPalette;
                        break;
                }
            }
            else
            {
                switch (_sortingLayer)
                {
                    case SORTING_LAYERS.Background:
                        _currentPalette = _backgroundCollisionPalette;
                        break;
                    case SORTING_LAYERS.Midground:
                        _currentPalette = _midgroundCollisionPalette;
                        break;
                    case SORTING_LAYERS.Foreground:
                        _currentPalette = _foregroundCollisionPalette;
                        break;
                }
            }

            foreach (MapObject mapObject in _currentPalette)
            {
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(mapObject.gameObject);
                    GUIContent tileGuiContent = new GUIContent() { image = texture, tooltip = mapObject.name };
                    paletteIcons.Add(tileGuiContent);
                }
            }
            _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), _paletteColumnSize);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical("box", GUILayout.MinWidth(350), GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true), GUILayout.MinHeight(100), GUILayout.MaxHeight(800));
            EditorGUILayout.LabelField("The current palette is empty", centerLabelStyle, GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
        }
    }
	#endregion

	#region Rule Tile
    private void DisplayRuleTile()
    {
        EditorGUILayout.LabelField("In works.", GUILayout.ExpandWidth(true));
    }
    #endregion
    #region Shortcut
    private void DisplayShortcut()
    {
        EditorGUILayout.LabelField("CTRL: Delete", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("A: Switch one tile back", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("D: Switch one tile ahead", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Q: Decrease brush size by one", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("E: Increase brush size by one", GUILayout.ExpandWidth(true));
    }
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
        _backgroundNoCollisionPalette.Clear();
        _midgroundNoCollisionPalette.Clear();
        _foregroundNoCollisionPalette.Clear();
        _backgroundCollisionPalette.Clear();
        _midgroundCollisionPalette.Clear();
        _foregroundCollisionPalette.Clear();
        Directory.CreateDirectory(_rootPath);
        Directory.CreateDirectory(_backgroundNoCollisionPath);
        Directory.CreateDirectory(_midgroundNoCollisionPath);
        Directory.CreateDirectory(_foregroundNoCollisionPath);
        Directory.CreateDirectory(_backgroundCollisionPath);
        Directory.CreateDirectory(_midgroundCollisionPath);
        Directory.CreateDirectory(_foregroundCollisionPath);

        string[] root = Directory.GetFiles(_rootPath, "*.prefab");
        foreach (string prefabFile in root)
        {
            _tilemapStyxRoot = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
        }

        string[] backgroundNoCollisionFiles = Directory.GetFiles(_backgroundNoCollisionPath, "*.prefab");
        foreach (string backgroundFile in backgroundNoCollisionFiles)
        {
            _backgroundNoCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(backgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] midgroundNoCollisionFiles = Directory.GetFiles(_midgroundNoCollisionPath, "*.prefab");
        foreach (string midgroundFile in midgroundNoCollisionFiles)
        {
            _midgroundNoCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(midgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] foregroundNoCollisionFiles = Directory.GetFiles(_foregroundNoCollisionPath, "*.prefab");
        foreach (string foregroundFile in foregroundNoCollisionFiles)
        {
            _foregroundNoCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(foregroundFile, typeof(MapObject)) as MapObject);
        }

        string[] backgroundCollisionFiles = Directory.GetFiles(_backgroundCollisionPath, "*.prefab");
        foreach (string backgroundFile in backgroundCollisionFiles)
        {
            _backgroundCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(backgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] midgroundCollisionFiles = Directory.GetFiles(_midgroundCollisionPath, "*.prefab");
        foreach (string midgroundFile in midgroundCollisionFiles)
        {
            _midgroundCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(midgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] foregroundCollisionFiles = Directory.GetFiles(_foregroundCollisionPath, "*.prefab");
        foreach (string foregroundFile in foregroundCollisionFiles)
        {
            _foregroundCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(foregroundFile, typeof(MapObject)) as MapObject);
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
				tileGridScrollPosition.y = _currentPalette.Count * 100;
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