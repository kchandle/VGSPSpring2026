using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//If you dont want to handle PRE-LOADING use SceneManager.SetScene()
public class SceneLoader : MonoBehaviour 
{
    //dictionary of preloaded scenes
    private static Dictionary<string, AsyncOperation> preloadedScenes = new Dictionary<string, AsyncOperation>();
    //hashSet (List but with no order) of loaded scenes currently
    private static HashSet<string> loadedScenes = new HashSet<string>();
    //static instance singleoton
    public static SceneLoader Instance;

    void Awake()
    {
        //singleton setup
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //adds the open scenes to the open scene hashset at the start
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene s = SceneManager.GetSceneAt(i);
            loadedScenes.Add(s.name);
        }

        //scene manager event listeners for loading and unloading
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { loadedScenes.Add(scene.name); };
        SceneManager.sceneUnloaded += (Scene scene) => { loadedScenes.Remove(scene.name); };
    }

    //adds a scene to preloaded scene dictionary, called after the scene is set to preload
    private void AddScene(string sceneName, AsyncOperation op)
    {
        // make sure we have no duplicates.
        if(!this.IsSceneLoading(sceneName))
        {   
            preloadedScenes.Add(sceneName, op);
        }
    }

    //removes a scene from the preload dictionary after it is set to unload
    private void RemoveScene(string sceneName)
    {
        if(this.IsSceneLoading(sceneName))
        {   
            preloadedScenes.Remove(sceneName);
        }
    }

    //returns a bool for whether or not the scene is in the preloaded scene dictionary
    private bool IsSceneLoading(string sceneName)
    {   
        if (!preloadedScenes.ContainsKey(sceneName)) 
        {
            return false;
        }

        return preloadedScenes[sceneName] != null;
    }

    //returns a bool for if the scene is in the dictionary and if it is finshed preloading
    private bool IsSceneReady(string sceneName)
    {
        if (!preloadedScenes.ContainsKey(sceneName)) 
        {   
            return false;
        }

        //max unity progress is 90 % for some reason
        float PRELOAD_MAX_PROGRESS = 0.9f;
        //checks if the progress for loading is above 90% because unity stops tracking progress further than that (its essentially finished) or if its doen
        return preloadedScenes[sceneName].progress >= PRELOAD_MAX_PROGRESS || preloadedScenes[sceneName].isDone;
    }

    //returns the AsyncOperation of a given scene in the dictionary if one exists
    private AsyncOperation GetSceneOperation(string sceneName)
    {
        if (!preloadedScenes.ContainsKey(sceneName)) 
        {  
            return null;
        }
        return preloadedScenes[sceneName];
    }

    //activates the scene on pretty much if the scene exists in the proload dictionary 
    private void ActivateScene(string sceneName)
    {
        if (!preloadedScenes.ContainsKey(sceneName)) 
        {   
            return;
        }

        preloadedScenes[sceneName].allowSceneActivation = true;
    }

    //returns a bool for if the active scene is the same scene as the argument
    private bool IsSceneActive(string sceneName)
    {   
        return SceneManager.GetActiveScene().name == sceneName;
    }

    /* The coroutine that does all of the main scene preloading and loading
    * If always called from a different method in this class either Load() or SwitchScene()
    * Allows for additive preloading, switching scenes entirely and having a custom wait time between changing scenes
    * Scene if scene name for loading, mode is additive or single,
    * setscene means that it is actually changing the scene and waitTime is waitTime from the start of the coroutine
    */
    private IEnumerator LoadSceneAsync(String scene, LoadSceneMode mode, bool setScene = false, float waitTime = 0)
    {
        Debug.AssertFormat(scene != null, "While trying to load a scene, encoutered a NULL scene object.");

        //waits for the wait time we set before moving onward
        yield return new WaitForSeconds(waitTime);

        AsyncOperation asyncLoad;

        //if the scene is in the preload dictionary then gets the asyncLoad from the Dictionary through GetSceneOperation()
        if(IsSceneLoading(scene))
        { 
            asyncLoad = GetSceneOperation(scene);
        }
        //if the scene isnt in the dictionary then it preloads it now 
        else
        {
             //catch any errors when initaiting ASYNC handling
            asyncLoad = SceneManager.LoadSceneAsync(scene, mode);

            //catch any errors if the scene wasnt preloaded at all somehow
            if(asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene {scene}");
                yield break;
            }

            //automatically disable the scene to be reactivated later
            asyncLoad.allowSceneActivation = false;
            //adds to preload dictionary
            AddScene(scene, asyncLoad);
        }

        //make sure scene is in preload dictionary and the scene is done loading before continuing
        while (IsSceneLoading(scene) && !this.IsSceneReady(scene))
        { 
            yield return null;
        }

        //if its not on the preload somehow magically then whenever its done loading it will unload and it breaks out of this coroutine
        if(!IsSceneLoading(scene))
        {
            asyncLoad.completed += (AsyncOperation op) => { this.Unload(scene); };
            yield break;
        }

        //if the scene is set scene then it will set the active scene to the scene argument 
        if(setScene)
        {   
            //if mode is Single then when the async is done it will Unload every scene other than the scene argument 
            if(mode == LoadSceneMode.Single)
            {
                asyncLoad.completed += (AsyncOperation op) =>
                {
                    foreach(string i in loadedScenes)
                    {   
                        if(i != scene)
                        {
                            this.Unload(i);
                            Debug.LogError($" removed {i}");
                        }
                    }
                };
            }

            ActivateScene(scene);
        }
    }

    //unloads the passed scene
    private IEnumerator UnloadScene(Scene scene)
    {
        //checks if the scene isnt loaded already
        if(!scene.isLoaded)
        {
            //gets the old scene operation  
            AsyncOperation oldLoad = GetSceneOperation(scene.name);

            //if its in the dictionary then remove it since its not loaded 
            if (oldLoad != null)
            {   
                RemoveScene(scene.name);
            }

            yield break;
        }

        AsyncOperation asyncLoad;

        //unloads the scene in async
        asyncLoad = SceneManager.UnloadSceneAsync(scene);
        if(asyncLoad == null)
        {   
            Debug.LogError($"Could not unload any scene {scene}");
            yield break;
        }
    }

    //gets the scene name from the scene index
    private static String GetScene(int index)
    {   
        return System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
    }


    #region External Interface
    //preloads a scene using the scene name 
    public void Load(String name, float wait_seconds = 0) => StartCoroutine(this.LoadSceneAsync(name, LoadSceneMode.Additive, false, wait_seconds));
    //overload for an int index rather than a string
    public void Load(int index, float wait_seconds = 0) => StartCoroutine(this.LoadSceneAsync(SceneLoader.GetScene(index), LoadSceneMode.Additive, false, wait_seconds));
    
    //unload a scene with a scene reference
    public void Unload(Scene scene) => StartCoroutine(this.UnloadScene(scene));
    //unloading overload for scene name instead
    public void Unload(String name) => StartCoroutine(this.UnloadScene(SceneManager.GetSceneByName(name)));
    //unloading overload for a scene index too
    public void Unload(int index) => StartCoroutine(this.UnloadScene(SceneManager.GetSceneByBuildIndex(index)));


    //closes all current loaded scenes and loads a scene using the name
    public void SwitchScene(String name)
    {
        StartCoroutine(this.LoadSceneAsync(name, LoadSceneMode.Single, true));
    }
    //switch scene overload but int index instead of string
    public void SwitchScene(int index)
    {
        StartCoroutine(this.LoadSceneAsync(SceneLoader.GetScene(index), LoadSceneMode.Single, true));
    }
    
    //adds a scene ontop of the current scene without unloading the current scene
    public void AppendScene(String name) => StartCoroutine(this.LoadSceneAsync(name, LoadSceneMode.Additive, true));
    //overload with index
    public void AppendScene(int index) => StartCoroutine(this.LoadSceneAsync(SceneLoader.GetScene(index), LoadSceneMode.Additive, true));
    #endregion
}
