This C# script uses SQLITE to create a database file called GameDB.db it also creates some tables like Players and Monsters. 
It is super easy to edit change and make into whatever you need.
to retrieve data from the GameDB.db file you can use something like this below you can even have a button activate the LoadItemStorage method if you want. 

"
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class StorageManager : MonoBehaviour
{
    private string connectionString;

    // Make storageItems a serialized field so it can be seen in the Unity Inspector
    [SerializeField]
    public List<Item> storageItems;

    private void Awake()
    {
        connectionString = "URI=file:GameDB.db";
        storageItems = new List<Item>();
    }

    void Start()
    {
        LoadItemStorage();
    }

    void Update()
    {

    }

    public void LoadItemStorage()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // SQL query to select items that are in storage
                string query = "SELECT Items.ItemID, Items.Name, Items.ImageNum, Items.Description, Items.EffectNum " +
                               "FROM Items " +
                               "INNER JOIN Storage ON Items.ItemID = Storage.ItemID";

                dbCmd.CommandText = query;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int itemId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int imageNum = reader.GetInt32(2);
                        string description = reader.GetString(3);
                        int effectNum = reader.GetInt32(4);

                        Item item = new Item(itemId, name, imageNum, description, effectNum);
                        storageItems.Add(item);
                    }
                }
            }

            dbConnection.Close();
        }
    }
}

[System.Serializable]
public class Item
{
    public int ItemID;
    public string Name;
    public int ImageNum;
    public string Description;
    public int EffectNum;

    public Item(int itemId, string name, int imageNum, string description, int effectNum)
    {
        ItemID = itemId;
        Name = name;
        ImageNum = imageNum;
        Description = description;
        EffectNum = effectNum;
    }
}
"
