using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour
{
    private readonly List<IUpdating> updateSubscribers = new List<IUpdating>();
    private readonly List<IUpdating> newSubscribers = new List<IUpdating>();
    private readonly List<IUpdating> unsubscribers = new List<IUpdating>();

    private readonly List<IUpdating> objects = new List<IUpdating>();
    private readonly List<IUpdating> newObjects = new List<IUpdating>();

    private GameController controller;

    public static Main instance;

    public static void SubscribeToUpdates(IUpdating gameObject)
    {
        instance.newSubscribers.Add(gameObject);
    }
    public static void UnubscribeFromUpdates(IUpdating gameObject)
    {
        instance.unsubscribers.Add(gameObject);
    }

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        GameData data = new GameData();
        controller = new GameController(data);
        CreateObj<GameInput>(controller);
        Object[] initObjects = GameObject.FindObjectsOfType(typeof(Initializer));
        foreach(Object initObject in initObjects)
        {
            Initializer initializer = initObject as Initializer;
            initializer.setupObject.transform = initializer.gameObject.transform;
            newObjects.Add(initializer.setupObject);
            if (initializer.setupObject is HexGrid)
                initializer.setupObject.Init(data);
            Destroy(initializer);
        }
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        yield return new WaitForEndOfFrame();
        controller.StartGame();
    }

    private void StartNewObjects()
    {
        foreach (IUpdating gameObject in newObjects)
            gameObject.Awake();
        foreach (IUpdating gameObject in newObjects)
        {
            gameObject.Start();
            objects.Add(gameObject);
        }
        newObjects.Clear();
    }

    public void Update()
    {
        StartNewObjects();

        foreach (IUpdating gameObject in unsubscribers)
            updateSubscribers.Remove(gameObject);
        unsubscribers.Clear();
        foreach (IUpdating gameObject in newSubscribers)
            updateSubscribers.Add(gameObject);
        newSubscribers.Clear();

        float time = Time.deltaTime;
        foreach (IUpdating gameObject in updateSubscribers)
            gameObject.UpdateTime(time);
    }

    public static PrefabObject CreateObjectFromPrefab(GameObject prefab, PrefabObject gameObject, Transform transform, params object[] args)
    {
        PrefabObject newObject = (PrefabObject)ScriptableObject.CreateInstance(gameObject.GetType());
        newObject.transform = Instantiate(prefab, transform, false).transform;
        newObject.Init(args);
        instance.newObjects.Add(newObject);
        return newObject;
    }

    public static T CreateObjFromPrefab<T>(GameObject prefab, Transform transform, params object[] args) where T : UpdatingTransform
    {
        T newObject = System.Activator.CreateInstance<T>();
        newObject.transform = Instantiate(prefab, transform, false).transform;
        newObject.Init(args);
        instance.newObjects.Add(newObject);
        return newObject;
    }

    /*public static UpdatingObject CreateObject(UpdatingObject gameObject, params object[] args)
    {
        PrefabObject newObject = (PrefabObject)ScriptableObject.CreateInstance(gameObject.GetType());
        newObject.Init(args);
        instance.newObjects.Add(newObject);
        return newObject;
    }*/

    /*public static T CreateObject<T>(params object[] args) where T : UpdatingObject
    {
        T newObject = ScriptableObject.CreateInstance(typeof(T)) as T;
        newObject.Init(args);
        instance.newObjects.Add(newObject);
        return newObject;
    }*/

    public static T CreateObj<T>(params object[] args) where T : IUpdating
    {
        T newObject = System.Activator.CreateInstance<T>();
        newObject.Init(args);
        instance.newObjects.Add(newObject);
        return newObject;
    }

}
