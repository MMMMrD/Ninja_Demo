using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public GameObject playerPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        Debug.Log(SceneManager.GetSceneByName(sceneName));
        //TODO：保存数据
        SaveManager.Instance.Save();
        if(SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);

            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);

            //TODO:读取数据
            SaveManager.Instance.Load();
            SaveManager.Instance.Save();

            ObjectPool.Instance.ClearObjectPool();
            EventManager.Instance?.InvokeEvent<UI>(UI.UpdateMoney);
            InventoryManager.Instance.RefreshAllUI();
            yield break;
        }
        else
        {
            GameManager.Instance.player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            yield return null;
        }
    }



    public TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if(item.destinationTag == destinationTag)
            {
                return item;
            }
        }

        return null;
    }

    public TransitionDestination GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if(item.destinationTag == TransitionDestination.DestinationTag.Enter)
            {
                return item;
            }
        }

        return null;
    }

    //加载主场景
    public void TransitionToStartMenu()
    {
        StartCoroutine(LoadStarMenu());
    }

    //加载新游戏
    public void TransitionToNewGame()
    {
        StartCoroutine(LoadLevel("Level1"));
        SaveManager.Instance.Save();
        Debug.Log(SaveManager.Instance.Scene);
    }

    //继续游戏
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.Scene));
        SaveManager.Instance.Load();
        InventoryManager.Instance.RefreshAllUI();
        SaveManager.Instance.Save();
    }

    IEnumerator LoadStarMenu()  //加载主菜单协程
    {
        //TODO:
        yield return SceneManager.LoadSceneAsync("Level0");

        yield break;
    }

    IEnumerator LoadLevel(string scene) //加载场景协程
    {
        if(scene != "")
        {

            yield return SceneManager.LoadSceneAsync(scene);
            Debug.Log(scene);
            
            yield return Instantiate(playerPrefab, GetEntrance().transform.position, GetEntrance().transform.rotation);

            yield break;
        }
    }
}
