#if UNITY_EDITOR
using UnityEngine;

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

/**
 * Put this component on the Map Editor prefabs.
 */
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class MapObject : MonoBehaviour
{
    [Tooltip("Fill this if the SpriteRenderer is in a child, leave empty otherwise.")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Sorting layer of our SpriteRenderer, can be called why we are a prefab, with no prior initialization
    public int SortingLayerID
    {
        get
        {
            return (spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>()).sortingLayerID;
        }
    }

    /**
     * Called when placing the order in the scene, after entering playmode and after leaving playmode.
     */
    private void Start()
    {
        MapEditorModel.Register(this);
    }


    /**
     * Counterpart of Start, called when removing from the scene, before entering playmode and before leaving playmode.
     */
    private void OnDestroy()
    {
        MapEditorModel.Remove(this);
    }
}


/**
 * Called when compiling, will remove all MapObject components from our scenes.
 */
public class MapObjectProcess : IProcessSceneWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
    {
        // Get all root objects in the scene.
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            // Get all MapObject components for each root object
            foreach (MapObject mapObject in go.GetComponentsInChildren<MapObject>())
                GameObject.DestroyImmediate(mapObject); // Remove it
        }
    }
}
#endif