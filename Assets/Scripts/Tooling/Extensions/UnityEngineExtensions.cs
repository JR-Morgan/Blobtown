using UnityEngine;

/// <summary>
/// This class contains several extension methods for <see cref="UnityEngine"/> classes.
/// </summary>
internal static class UnityEngineExtensions
{
    #region Destroy GameObjects
    /// <summary>
    /// Destroys all children associated with the <paramref name="transform"/>.<br/>
    /// Will call <see cref="Object.Destroy(Object)"/> at play time or if in editor; performs a delayed call to <see cref="Object.DestroyImmediate(Object)"/>
    /// </summary>
    /// <param name="transform">The <see cref="Transform"/> whose children are to be destroyed.</param>
    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (child != null) UnityEngine.Object.DestroyImmediate(child.gameObject);
            };
#else
        GameObject.Destroy(child.gameObject);
#endif
        }
    }
    public static void DestroyChildren(this GameObject gameObject) => DestroyChildren(gameObject.transform);
    #endregion

    #region Layers
    public static int ToLayerNumber(this LayerMask mask) => Mathf.RoundToInt(Mathf.Log(mask.value, 2));
    #endregion

    #region Try Get Component
    private const bool DEFAULT_INCLUDE_INTERACTIVE = false;
    public static bool TryGetComponentInParents<T>(this Component source, out T component, bool includeInactive = DEFAULT_INCLUDE_INTERACTIVE) where T : Component => TryGetComponentInParents(source.gameObject, out component, includeInactive);
    public static bool TryGetComponentInParents<T>(this GameObject gameObject, out T component, bool includeInactive = DEFAULT_INCLUDE_INTERACTIVE) where T : Component
    {
        component = gameObject.GetComponentInParent<T>(includeInactive);
        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this Component source, out T component, bool includeInactive = DEFAULT_INCLUDE_INTERACTIVE) where T : Component => TryGetComponentInChildren(source.gameObject, out component, includeInactive);
    public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeInactive = DEFAULT_INCLUDE_INTERACTIVE) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>(includeInactive);
        return component != null;
    }
    #endregion

    #region Require Get
    /// <summary>
    /// Finds the first occurrence of <typeparamref name="T"/> in <paramref name="source"/> or children of.
    /// Asserts if no <typeparamref name="T"/> were found.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Component"/></typeparam>
    /// <param name="source"></param>
    /// <param name="component">The component to be assigned</param>
    /// <param name="includeInactive">Whether to include inactive objects</param>
    /// <returns><c>true</c> if <paramref name="component"/> was assigned</returns>
    public static bool RequireComponentInChildren<T>(this Component source, out T component, bool includeInactive = false) where T : Component
    {
        bool found = TryGetComponentInChildren<T>(source, out component, includeInactive);
        Debug.Assert(found, $"{source.GetType()} could not find any {typeof(T)} {nameof(component)} in self or children!", source);
        return found;
    }

    /// <summary>
    /// Finds the first occurrence of <typeparamref name="T"/> in <paramref name="source"/> or parents.
    /// Asserts if no <typeparamref name="T"/> were found.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Component"/></typeparam>
    /// <param name="source"></param>
    /// <param name="component">The component to be assigned</param>
    /// <param name="includeInactive">Whether to include inactive objects</param>
    /// <returns><c>true</c> if <paramref name="component"/> was assigned</returns>
    public static bool RequireComponentInParents<T>(this Component source, out T component, bool includeInactive = false) where T : Component
    {
        bool found = TryGetComponentInParents<T>(source, out component, includeInactive);
        Debug.Assert(found, $"{source.GetType()} could not find any {typeof(T)} {nameof(component)} in self or parents!", source);
        return found;
    }

    #endregion

    #region Vector
    public static Vector2 RotateAround(this Vector2 pos, Vector2 origin, float radians) => RotateAround(pos, origin.x, origin.y, radians);
    public static Vector2 RotateAround(this Vector2 pos, float originX, float originY, float radians)
    {
        float sine = Mathf.Sin(radians);
        float cosine = Mathf.Cos(radians);

        float x = pos.x;
        float y = pos.y;

        x -= originX;
        y -= originY;

        float newX = x * cosine - y * sine;
        float newY = x * sine + y * cosine;

        pos.x = newX + originX;
        pos.y = newY + originY;
        return pos;
    }
    #endregion

}
