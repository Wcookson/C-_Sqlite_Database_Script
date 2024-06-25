using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class GamesDB : MonoBehaviour
{
    public bool resetDatabase;
    private string connectionString;

    void Start()
    {
        connectionString = "URI=file:GameDB.db";
        InitializeDB();
    }

    private void InitializeDB()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Create tables
                CreateTables(dbCmd);

                // Update the Settings table with the current state of resetDatabase
                dbCmd.CommandText = $"UPDATE 'Settings' SET ResetDatabase = {(resetDatabase ? 1 : 0)}";
                dbCmd.ExecuteNonQuery();
            }

            if (resetDatabase)
            {
                ResetDB();
            }

            dbConnection.Close();
        }
    }

    private void CreateTables(IDbCommand dbCmd)
    {
        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Settings' (" +
                            " 'ResetDatabase' BOOLEAN NOT NULL DEFAULT 0)";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Texts' (" +
                            " 'TextID' INTEGER PRIMARY KEY AUTOINCREMENT, " +
                            " 'TextData1' TEXT, " +
                            " 'TextData2' TEXT, " +
                            " 'TextData3' TEXT, " +
                            " 'TextData4' TEXT)";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Items' (" +
                           " 'ItemID' INTEGER PRIMARY KEY AUTOINCREMENT, " +
                           " 'Name' TEXT NOT NULL, " +
                           " 'ImageNum' INTEGER, " +
                           " 'Description' TEXT, " +
                           " 'EffectNum' INTEGER)";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Storage' (" +
                          " 'StorageID' INTEGER PRIMARY KEY AUTOINCREMENT, " +
                          " 'ItemID' INTEGER," +
                          "'ItemNum' INTEGER)";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Players' (" +
                           " 'PlayerID' INTEGER PRIMARY KEY AUTOINCREMENT, " +
                           " 'MonsterOwnerID' INTEGER NOT NULL, " +
                           " 'Name' TEXT NOT NULL, " +
                           " 'LocationX' INTEGER NOT NULL, " +
                           " 'LocationY' INTEGER NOT NULL, " +
                           " 'Scene' INTEGER NOT NULL)";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'NPCs' (" +
                            " 'NPCID' INTEGER PRIMARY KEY AUTOINCREMENT, " +
                            " 'MonsterOwnerID' INTEGER NOT NULL, " +
                            " 'TextID' INTEGER NOT NULL, " +
                            " 'Name' TEXT NOT NULL, " +
                            " 'Scene' INTEGER NOT NULL, " +
                            " 'Defeated' BOOLEAN NOT NULL)";
        dbCmd.ExecuteNonQuery();

        dbCmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Monsters' (" +
                            " 'MonsterID' INTEGER PRIMARY KEY AUTOINCREMENT, " +
                            " 'MonsterOwnerID' INTEGER NOT NULL, " +
                            " 'Name' TEXT NOT NULL, " +
                            " 'Level' INTEGER NOT NULL, " +
                            " 'SpriteID' INTEGER NOT NULL, " +
                            " 'HP' INTEGER NOT NULL, " +
                            " 'MaxHP' INTEGER NOT NULL, " +
                            " 'EXP' INTEGER NOT NULL, " +
                            " 'MaxEXP' INTEGER NOT NULL, " +
                            " 'AttackDamage' INTEGER NOT NULL, " +
                            " 'MagicDamage' INTEGER NOT NULL, " +
                            " 'MagicResist' INTEGER NOT NULL, " +
                            " 'Defence' INTEGER NOT NULL, " +
                            " 'Speed' INTEGER NOT NULL, " +
                            " 'IsEmpty' BOOLEAN NOT NULL, " +
                            " FOREIGN KEY(MonsterOwnerID) REFERENCES Players(PlayerID) ON DELETE CASCADE)";
        dbCmd.ExecuteNonQuery();
    }

    public void CreateDB()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Create tables
                CreateTables(dbCmd);
            }

            dbConnection.Close();
        }
    }

    public void ResetDB()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Drop tables if they exist
                dbCmd.CommandText = "DROP TABLE IF EXISTS 'Texts'";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS 'Items'";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS 'Storage'";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS 'Players'";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS 'NPCs'";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS 'Monsters'";
                dbCmd.ExecuteNonQuery();

                // Recreate tables
                CreateTables(dbCmd);
            }

            dbConnection.Close();
        }
    }
}
