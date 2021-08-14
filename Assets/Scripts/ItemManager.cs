﻿using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemList allItems;

    Inventory playerInventory;

    static private ItemManager instance;

    static public ItemManager GetInstance() { return instance; }

    string savePath = "SaveFile.json";

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
#if UNITY_EDITOR
        for (int i = 0; i < allItems.List.Count; i++)
        {
            allItems.List[i].id = i;
        }
#endif 
    }

    void Start()
    {
        LoadJson();
    }
    public int GetRandomItemID()
    {
        return Random.Range(0, allItems.List.Count);
    }

    public int GetRandomAmmountOfItem(int id)
    {
        return Random.Range(1, allItems.List[id].maxStack);
    }

    public Item GetItemFromID(int id)
    {
        return allItems.List[id];
    }

    public void SetPlayerItemsSlots(List<Slot> slots)
    {
        playerInventory.SetNewInventory(slots);
    }

    public void SaveJson()
    {
        string json = "";
        for (int i = 0; i < playerInventory.GetInventoryList().Count; i++)
        {
            json += JsonUtility.ToJson(playerInventory.GetInventoryList()[i]);
        }

        FileStream fs;

        if (!File.Exists(savePath))
            fs = File.Create(savePath);
        else
            fs = File.Open(savePath, FileMode.Truncate);

        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(json);
        fs.Close();
        bw.Close();
    }

    public void LoadJson()
    {
        string savedData;
        if (File.Exists(savePath))
        {
            FileStream fs;
            fs = File.Open(savePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            savedData = br.ReadString();
            fs.Close();
            br.Close();
        }
        else
        {
            return;
        }
        List<Slot> newList = new List<Slot>();
        for (int i = 0; i < savedData.Length; i++)
        {
            if (savedData[i] == '{')
            {
                string slotString = "";
                int aux = 0;
                while (savedData[i + aux] != '}')
                {
                    slotString += savedData[i + aux];
                    aux++;
                }
                slotString += '}';
                Slot newSlot = JsonUtility.FromJson<Slot>(slotString);
                newList.Add(newSlot);
            }
        }
        playerInventory.SetNewInventory(newList);
    }

    void OnDestroy()
    {
        //SaveJson();
    }
}