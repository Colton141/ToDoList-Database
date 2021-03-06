using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    private int _id;
    private int _categoryId;

    public Item (string description, int categoryId, int id = 0)
    {
      _description = description;
      _categoryId = categoryId;
      _id = id;
    }

    public string GetDescription()
    {
      return _description;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetId(int newId)
    {
      _id = newId;
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        int itemCategoryId = rdr.GetInt32(2);
        Item newItem = new Item(itemDescription, itemCategoryId, itemId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }



    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //create and open a new database connection
      var cmd = conn.CreateCommand() as MySqlCommand;
      //We call DB.Connection() to create our conn object, representing our connection to the database, and then call Open() upon it to open the connection. Remember, the DB before the method name here refers to the DB class defined in Database.cs
      cmd.CommandText = @"DELETE FROM items;";
      //built in command that modifies data instead of querying and returning it
      cmd.ExecuteNonQuery();
      //close databases
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
         Item newItem = (Item) otherItem;
         bool idEquality = this.GetId() == newItem.GetId();
         bool descriptionEquality = this.GetDescription() == newItem.GetDescription();
         bool categoryEquality = this.GetCategoryId() == newItem.GetCategoryId();
         return (idEquality && descriptionEquality && categoryEquality);
       }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description, category_id) VALUES (@description, @category_id);";
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);
      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@category_id";
      categoryId.Value = this._categoryId;
      cmd.Parameters.Add(categoryId);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int itemId = 0;
      string itemName = "";
      int itemCategoryId = 0;
      while(rdr.Read())
      {
        itemId = rdr.GetInt32(0);
        itemName = rdr.GetString(1);
        itemCategoryId = rdr.GetInt32(2);
      }
      Item newItem = new Item(itemName, itemCategoryId, itemId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newItem;
    }

    public void Edit(string newDescription)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newDescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);
      cmd.ExecuteNonQuery();
      _description = newDescription;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteItem()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = _id;
      cmd.Parameters.Add(thisId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }







  }
}
