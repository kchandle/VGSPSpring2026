using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class SceneLoader : ScriptableObject
{
    static private Dictionary<string, SceneLoaderAsync> loadingScenes = new();
    static private Dictionary<string, SceneLoaderAsync> readyScenes = new();
    static private HashSet<string> loadedScenes = new();
    static private Scene? EmptyScene = null;

    static SceneLoader()
    {
        if(EmptyScene == null)
        {   
            EmptyScene = SceneManager.CreateScene("__RuntimeFallback__Internal__");

            SceneManager.sceneLoaded += (scene, mode) => { loadedScenes.Add(scene.name); };
            SceneManager.sceneUnloaded += (scene) => { loadedScenes.Remove(scene.name); };
        }

    }

    static private Scene GetScene(string scene)
    {   return SceneManager.GetSceneByName(scene);
    }

    static private Scene GetScene(int index)
    {   return SceneManager.GetSceneByBuildIndex(index);
    }

    static private Scene GetScene(Scene scene)
    {   return scene;
    }

    static private string GetSceneName(Scene scene)
    {   return scene.name;
    }

    static private string GetSceneName(int index)
    {   return System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
    }

    static private string GetSceneName(string scene)
    {   return scene;
    }

    static private IEnumerator _LoadSceneAsync(SceneLoaderAsync obj)
    {
        AsyncOperation asyncLoad;

        obj.IsWaiting = true;
        yield return new WaitForSeconds(obj.WaitTime);
        obj.IsWaiting = false;

        //catch any errors when initaiting ASYNC handling
        asyncLoad = SceneManager.LoadSceneAsync(obj.Name, LoadSceneMode.Additive);

        //catch any errors if the scene wasnt preloaded at all somehow
        if(asyncLoad == null)
        {
            obj.Destroy = true;
            obj.Used = true;
            Debug.LogError($"Failed to load scene {obj.Name}");
            yield break;
        }

        //automatically disable the scene to be reactivated later
        asyncLoad.allowSceneActivation = false;

        // max unity progress is 90 % for some reason
        float PRELOAD_MAX_PROGRESS = 0.9f;

        while(asyncLoad.progress < PRELOAD_MAX_PROGRESS && !asyncLoad.isDone && !obj.Destroy)
        {   
            obj.LoadProgress = asyncLoad.progress * 100;
            yield return null;
        }

        string previousScene;

        if(obj.Destroy)
        {   
            obj.LoadProgress = 100f;
            SceneLoader.Unload(obj.Name);
            yield break;
        }
        else if(obj.IsConsumed())
        {   
            asyncLoad.allowSceneActivation = true;
        }

        previousScene = SceneManager.GetActiveScene().name;

        while((asyncLoad.progress < 1f) && !asyncLoad.isDone)
        {
            obj.LoadProgress = asyncLoad.progress * 100;

            if(obj.IsConsumed())
            {   asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        obj.LoadProgress = 100f;
        obj.Used = true;

        asyncLoad.completed += (AsyncOperation op) => 
        { 
            SceneManager.UnloadSceneAsync(previousScene); 
        };

        SceneManager.SetActiveScene(GetScene(obj.Name));

    }

    static private IEnumerator _UnloadSceneAsync(SceneLoaderAsync obj)
    {
        AsyncOperation async = null;

        obj.Destroy = true;

        async = SceneManager.UnloadSceneAsync(obj.Name);

        if(async == null)
        {  
            Debug.LogError($"Could not unload any scene {obj.Name}");
            yield break;
        }
    }
    
    #region Public Interface
    static public SceneLoaderAsync PreLoad(string scene, float waitTime = 0f)
    {
        SceneLoaderAsync async = new();

        if(!readyScenes.ContainsKey(scene))
        {
            if(loadingScenes.ContainsKey(scene))
            {   return loadingScenes[scene];
            }
            else
            {
                async.Name = scene;
                async.WaitTime = waitTime;

                loadingScenes.Add(scene, async);
            }
        }
        else
        {   async = readyScenes[scene];
        }

        GlobalMono.Instance.StartCoroutine(_LoadSceneAsync(async));

        return async;
    }

    static public SceneLoaderAsync PreLoad(int buildIndex, float waitTime = 0f)
    {   return PreLoad(GetSceneName(buildIndex), waitTime);
    }

    static public SceneLoaderAsync PreLoad(Scene scene, float waitTime = 0f)
    {   return PreLoad(GetSceneName(scene), waitTime);
    }

    // Load a scene, this may fail if another thread consumes the SceneLoaderAsync token, you can check this with IsConsumed();
    //
    // RETURN: true on Success.
    // RETURN: false on Failure.
    static public bool Load(SceneLoaderAsync loader)
    {
        if(loader.IsConsumed())
        {   return false;
        }

        if(readyScenes.ContainsKey(loader.Name))
        {   loader.Used = true;
        }
        else if(loadingScenes.ContainsKey(loader.Name))
        {   loader.Used = true;
        }

        return true;
    }
    static public SceneLoaderAsync Unload(string scene, bool UnloadReadyOnlyScene = false)
    {   
        SceneLoaderAsync obj = null;

        if(loadedScenes.Contains(scene))
        {
            obj = new();
            obj.Name = scene;
        }

        if(readyScenes.ContainsKey(scene))
        {   obj = readyScenes[scene];
        }

        if(UnloadReadyOnlyScene)
        {   // this is supposed to do nothing.
        }
        else if(loadingScenes.ContainsKey(scene))
        {   obj = loadingScenes[scene];
        }

        if(obj != null)
        {   GlobalMono.Instance.StartCoroutine(_UnloadSceneAsync(obj));
        }

        return obj;
    }

    static public SceneLoaderAsync Unload(int buildIndex, bool UnloadReadyOnlyScene = false)
    {   return Unload(GetSceneName(buildIndex), UnloadReadyOnlyScene);
    }

    static public SceneLoaderAsync Unload(Scene scene, bool UnloadReadyOnlyScene = false)
    {   return Unload(GetSceneName(scene), UnloadReadyOnlyScene);
    }

    #endregion
}

public class SceneLoaderAsync
{
    internal bool Destroy = false;
    internal bool Used = false;
    internal string Name = null;
#region Public Interface
    public float LoadProgress { get; internal set; } = 0f;
    public bool IsWaiting { get; internal set; } = false;
    public float WaitTime { get; internal set; } = 0f;

    public bool IsConsumed()
    {   return !this.Used || this.Destroy;
    }

    public bool IsLoading()
    {   return LoadProgress >= 89.999f;
    }

    public bool IsReady()
    {   return !IsLoading();
    }

    public bool IsLoaded()
    {   return LoadProgress >= 100f || Destroy;
    }
#endregion
}