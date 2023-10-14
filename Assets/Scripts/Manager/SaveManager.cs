using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : Singleton<SaveManager>
{
    string scene;
    string lastScene;
    Vector3 playerTransform;

    public string Scene     //返回上次场景
    {
        get
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/Scene", FileMode.Open);
            scene = (string)formatter.Deserialize(file);

            file.Close();
            return scene;
        }
    }

    public string LastScene     //返回上次场景
    {
        get
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/LastScene", FileMode.Open);
            lastScene = (string)formatter.Deserialize(file);

            file.Close();
            return lastScene;
        }
    }

    public Vector3 PlayerTransform   //获取玩家上次所处的位置
    {
        get
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/Transform", FileMode.Open);
            playerTransform = (Vector3)formatter.Deserialize(file);

            file.Close();
            return playerTransform;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void SaveTransform()
    {
        //首先需要判断是否存在保存数据的文件夹，没有则创建
        if(!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();  //用于进行二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/Transform");

        var json = JsonUtility.ToJson(GameManager.Instance.player.transform.position);
        formatter.Serialize(file, json);

        file.Close();
    }

    public void LoadTransform()
    {
        Vector2 point = new Vector2();
        BinaryFormatter formatter = new BinaryFormatter();

        if(File.Exists(Application.persistentDataPath + "/game_SaveGame/Transform"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveGame/Transform", FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)formatter.Deserialize(file), point);
            
            GameManager.Instance.player.transform.position = point;

            file.Close();
        }
    }

    public void Save()
    {

        if(GameManager.Instance.player != null)
        {
            SaveGame_Json(GameManager.Instance.player.characterStats.characterData);
        }
        SaveGame_Json(InventoryManager.Instance.UseableBagData);
        SaveGame_Json(InventoryManager.Instance.WeaponBagData);
        SaveGame_Json(InventoryManager.Instance.ActionBagData);
        SaveGame_Json(InventoryManager.Instance.ArmorBagData);
        SaveGame_Json(InventoryManager.Instance.EquipmentBagData);
        Debug.Log("Success");
    }

    public void Load()
    {
        if(GameManager.Instance.player != null)
        {
            LoadGame_Json(GameManager.Instance.player.characterStats.characterData);
        }
        LoadGame_Json(InventoryManager.Instance.UseableBagData);
        LoadGame_Json(InventoryManager.Instance.WeaponBagData);
        LoadGame_Json(InventoryManager.Instance.ActionBagData);
        LoadGame_Json(InventoryManager.Instance.ArmorBagData);
        LoadGame_Json(InventoryManager.Instance.EquipmentBagData);
    }

    public void SaveLastScene()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream sceneFile = File.Create(Application.persistentDataPath + "/game_SaveData/LastScene");

        formatter.Serialize(sceneFile, SceneManager.GetActiveScene().name);

        sceneFile.Close();
    }

    void SaveGame(Object data)  //保存数据
    {
        //首先需要判断是否存在保存数据的文件夹，没有则创建
        if(!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();  //用于进行二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/" + data.name);
        FileStream sceneFile = File.Create(Application.persistentDataPath + "/game_SaveData/Scene");

        formatter.Serialize(file, data);
        formatter.Serialize(sceneFile, SceneManager.GetActiveScene().name);

        file.Close();
        sceneFile.Close();
    }

    void SaveGame_Json(Object data)  //保存数据
    {
        //首先需要判断是否存在保存数据的文件夹，没有则创建
        if(!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();  //用于进行二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/" + data.name);
        FileStream sceneFile = File.Create(Application.persistentDataPath + "/game_SaveData/Scene");

        var json = JsonUtility.ToJson(data);
        formatter.Serialize(file, json);
        formatter.Serialize(sceneFile, SceneManager.GetActiveScene().name);

        file.Close();
        sceneFile.Close();
    }

    void LoadGame(Object data)  //加载数据
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if(File.Exists(Application.persistentDataPath + "/game_SaveData/" + data.name))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/" + data.name, FileMode.Open);

            data = (Object)formatter.Deserialize(file);
            
            
            file.Close();
        }
    }

    void LoadGame_Json(Object data)  //加载数据
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/game_SaveData/" + data.name))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/" + data.name, FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)formatter.Deserialize(file), data);
            
            file.Close();
        }
    }

    public bool GetIsSaveGame() //外部调用，判断是否已经保存游戏
    {
        if(Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            return true;
        }
        
        return false;
    }
}
