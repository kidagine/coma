
using System;
using UnityEngine;

public struct MapObjectKey : IEquatable<MapObjectKey>
{
    public int sortingLayerID; // SpriteRenderer.sortingLayerID
    public Vector2Int cell; // Coordinates on the virtual grid

    override public bool Equals(object obj)
    {
        return obj is MapObjectKey && Equals((MapObjectKey)obj);
    }

    public bool Equals(MapObjectKey other)
    {
        return sortingLayerID == other.sortingLayerID && cell == other.cell;
    }

    static public bool operator ==(MapObjectKey object1, MapObjectKey object2)
    {
        return object1.sortingLayerID == object2.sortingLayerID && object1.cell == object2.cell;
    }

    static public bool operator !=(MapObjectKey object1, MapObjectKey object2)
    {
        return object1.sortingLayerID != object2.sortingLayerID || object1.cell != object2.cell;
    }

    public override int GetHashCode()
    {
        return sortingLayerID + cell.GetHashCode();
    }
}