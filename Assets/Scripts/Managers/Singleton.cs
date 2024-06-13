using Sirenix.OdinInspector;
using UnityEngine;

// A generic Singleton class for SerializedMonoBehaviours.
public abstract class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    private static T instance;

    // Singleton instance property.
    public static T Instance
    {
        get
        {
            // If instance is not assigned, try to find it in the scene.
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                // If instance is still not found, log an error.
                if (instance == null)
                {
                    Debug.LogError($"The GameObject of type {typeof(T)} is not present in the scene, " +
                                   $"yet its method is being called. Please add {typeof(T)} to the scene.");
                }
                else
                {
                    #if ! UNITY_EDITOR
                   // If instance is found, ensure it persists across scene changes.
                                DontDestroyOnLoad(instance);             
                    #endif
                    
                }
            }
            return instance;
        }
    }

    // Called when the instance is first created.
    protected virtual void Awake()
    {
        // If instance is not assigned, assign this instance to it and make it persistent.
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            DoAtAwake(); // Customizable method called at Awake.
        }
        else
        {
            // If instance is already assigned, destroy this instance.
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Customizable method that can be overridden in derived classes.
    protected virtual void DoAtAwake()
    {
        // Implement any specific behavior at Awake if needed.
    }

    public static bool HasInstance()
    {
        return instance != null;
    }
}