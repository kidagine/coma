#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

/**
 * This class is on the /Scripts part of our project, but it should actually only exist while in Editor.
 * 
 * Just to make sure we are not doing anything wrong, we wrap is with #if UNITY_EDITOR.
 */
public static class MapEditorModel
{

    /**
     * This simple Dictionary is the core of our model.
     */
    private readonly static Dictionary<MapObjectKey, MapObject> map = new Dictionary<MapObjectKey, MapObject>();

    /**
     * Adding a game object via its attached MapObject.
     */
    public static void Register(MapObject mapObject)
    {
        map.Add(GetKey(mapObject), mapObject);
    }

    /**
     * Removing a game object via its attached MapObject.
     */
    public static void Remove(MapObject mapObject)
    {
        map.Remove(GetKey(mapObject));
    }

    /**
     * Removing a game object via its cell and its sorting layer ID.
     */
    public static void Remove(Vector2 position, int sortingLayerID)
    {
        map.Remove(new MapObjectKey()
        {
            cell = GetCell(position),
            sortingLayerID = sortingLayerID
        });
    }

    /**
     * Retrieve a game object via its position and its sorting layer ID.
     * 
     * Will return null if there are none.
     */
    public static MapObject Get(Vector2 position, int sortingLayerID)
    {
        MapObject mapObject = null;
        MapObjectKey key = new MapObjectKey()
        {
            cell = GetCell(position),
            sortingLayerID = sortingLayerID
        };

        map.TryGetValue(key,
                        out mapObject);

        return mapObject;
    }

    /**
     * Creates the corresponding key for a map object.
     */
    private static MapObjectKey GetKey(MapObject mapObject)
    {
        return new MapObjectKey()
        {
            cell = GetCell(mapObject.transform.position),
            sortingLayerID = mapObject.SortingLayerID
        };
    }

    /**
     * This should replace the cellSize defined in the Window Editor script.
     * 
     * Defines the size of a cell for our map. Could be moved in a Scriptable Object.
     */
    public readonly static Vector2 cellSize = new Vector2(1f, 1f);

    /**
     * This should replace the corresponding defined in the Window Editor script.
     * 
     * Returns the coordinates of the pointed cell.
     */
    public static Vector2Int GetCell(Vector3 coords)
    {
        return new Vector2Int(Mathf.FloorToInt(coords.x / cellSize.x), Mathf.FloorToInt(coords.y / cellSize.y));
    }
}
#endif