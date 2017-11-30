using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> database = new List<Item>();
    private JsonData itemData;
    public static Item nullItem = new Item(-1);

    void Awake()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + @"\items.json"));
        ConstructDatabase();

        foreach (Item item in database)
        {
            Debug.Log(item.Title);
        }
        //Debug.Log(database[2].Title);
    }

    private void ConstructDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            Item item = new Item();
            item.ID = (int)itemData[i]["id"];
            item.Title = itemData[i]["title"].ToString();
            item.Value = (int)itemData[i]["value"];
            item.Description = itemData[i]["description"].ToString();
            item.Stackable = bool.Parse(itemData[i]["stackable"].ToString());
            item.ItemType = (int)itemData[i]["itemType"];
            item.Defence = (int)itemData[i]["defence"];
            item.Power = (int)itemData[i]["power"];
            item.IconId = (int)itemData[i]["iconId"];

            AddItemInOrder(item);
        }
    }

    private void AddItemInOrder(Item item)
    {
        database.Add(item);

        if (database.Count <= 1)
        {
            return;
        }

        for (int i = database.Count - 1; i > 0; i--)
        {
            if (database[i].ID < database[i - 1].ID)
            {
                Item temp = database[i - 1];
                database[i - 1] = database[i];
                database[i] = temp;
            }
            else
            {
                break;
            }
        }
    }

    public static Item GetItem(int id)
    {
        int min = 0;
        int max = database.Count - 1;

        if (database[min].ID == id)
        {
            return database[min];
        }
        else if(database[max].ID == id)
        {
            return database[max];
        }

        while (min <= max)
        {
            int mid = (min + max) / 2;

            if (database[mid].ID == id)
            {
                return database[mid];
            }

            if (id > database[mid].ID)
            {
                min = mid + 1;
            }
            else
            {
                max = mid - 1;
            }
        }

        Debug.LogError("Item: " + id + " not found in database");
        return nullItem;
    }
}

public struct Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int ItemType { get; set; }
    public int Defence { get; set; }
    public int Power { get; set; }
    public int IconId { get; set; }
    public int count;

    public Item(int id)
    {
        ID = -1;
        Title = "Null Item";
        Value = 0;
        Description = "Null Item This Should Never Appear.";
        Stackable = false;
        ItemType = -1;
        Defence = 0;
        Power = 0;
        IconId = -1;
        count = -1;
    }
}

public class ItemTypes
{
    public static int weapon = 0;
    public static int armour = 1;
    public static int food = 3;
    public static int block = 2;
    public static int tool = 4;


}
