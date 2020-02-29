using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilemapStyx : EditorWindow
{
    private readonly List<MapObject> _palette = new List<MapObject>();
    private readonly Vector2 _cellSize = new Vector2(1.0f, 1.0f);
    private readonly string[] layersSelection = new string[] { "Background", "Midground", "Foreground" };
    private readonly string[] collisionSelection = new string[] { "Collision", "No Collision" };
    private readonly string _rootPath = "Assets/Editor Default Resources";
    private readonly string _backgroundPath = "Assets/Editor Default Resources/Background";
    private readonly string _midgroundPath = "Assets/Editor Default Resources/Midground";
    private readonly int _paletteColumnSize = 3;
    private GameObject _tilemapStyxRoot;
    private GameObject _tilemapStyxRootPrefab;
    private Vector2 tileGridScrollPosition;
    private Color _cellColor;
    private int _paletteIndex;
    private int _layerIndex;
    private int _collisionIndex;
    private int _brushSize;
    private bool _paintMode;


    [MenuItem("Window/Tilemap Styx")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TilemapStyx));
    }

    void Update()
    {
        Repaint();
    }

    #region OnGui
    void OnGUI()
    {
        DisplayModeToggleSection();
        DisplayPaintSection();
        DisplayTileGrid();
    }

    private void DisplayModeToggleSection()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal("box");
        GUILayout.Toggle(_paintMode, "Palette", "Button", GUILayout.Height(40f));
        GUILayout.Toggle(_paintMode, "Rule Tiles", "Button", GUILayout.Height(40f));
        GUILayout.Toggle(_paintMode, "Shortcuts", "Button", GUILayout.Height(40f));
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayPaintSection()
    {
        EditorGUILayout.Space();
        if (_paintMode)
            _paintMode = GUILayout.Toggle(_paintMode, "Stop painting", "Button", GUILayout.Height(60f));
        else
            _paintMode = GUILayout.Toggle(_paintMode, "Start painting", "Button", GUILayout.Height(60f));

        _brushSize = EditorGUILayout.IntSlider("Brush Size:", _brushSize, 1, 10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayTileGrid()
    {
        EditorGUILayout.Space();
        GUILayout.FlexibleSpace();
        _collisionIndex = EditorGUILayout.Popup(_collisionIndex, collisionSelection);
        GUILayout.FlexibleSpace();
        _layerIndex = EditorGUILayout.Popup(_layerIndex, layersSelection);

        GUIStyle centerLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField(_palette[_paletteIndex].name, centerLabelStyle, GUILayout.ExpandWidth(true));
       
        GUILayout.BeginVertical();
        tileGridScrollPosition = GUILayout.BeginScrollView(tileGridScrollPosition, false, true, GUILayout.MinWidth(350), GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true), GUILayout.MinHeight(100), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true));
        List<GUIContent> paletteIcons = new List<GUIContent>();
        foreach (MapObject mapObject in _palette)
        {
            if (mapObject.GetComponent<SpriteRenderer>().sortingOrder == 1)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(mapObject.gameObject);
                paletteIcons.Add(new GUIContent(texture));
            }
        }
        _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), _paletteColumnSize, GUILayout.MinWidth(200), GUILayout.MaxWidth(300), GUILayout.ExpandWidth(true), GUILayout.MinHeight(1000), GUILayout.MaxHeight(1200), GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
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
        _palette.Clear();

        Directory.CreateDirectory(_rootPath);
        Directory.CreateDirectory(_backgroundPath); 

        string[] root = Directory.GetFiles(_rootPath, "*.prefab");
        foreach (string prefabFile in root)
        {
            _tilemapStyxRoot = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
        }

        string[] backgroundFiles = Directory.GetFiles(_backgroundPath, "*.prefab");
        foreach (string backgroundFile in backgroundFiles)
        {
            _palette.Add(AssetDatabase.LoadAssetAtPath(backgroundFile, typeof(MapObject)) as MapObject);
        }

        string[] midgroundFiles = Directory.GetFiles(_midgroundPath, "*.prefab");
        foreach (string midgroundFile in midgroundFiles)
        {
            _palette.Add(AssetDatabase.LoadAssetAtPath(midgroundFile, typeof(MapObject)) as MapObject);
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
        if (_paletteIndex < _palette.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            MapObject mapObject = _palette[_paletteIndex];
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
            if (_paletteIndex != _palette.Count - 1)
            {
                _paletteIndex++;
            }
            else
            {
                _paletteIndex = 0;
            }
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
        {
            if (_paletteIndex != 0)
            {
                _paletteIndex--;
            }
            else
            {
                _paletteIndex = _palette.Count - 1;
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

        Undo.RegisterCreatedObjectUndo(mapObject, "");
    }
    #endregion
}
