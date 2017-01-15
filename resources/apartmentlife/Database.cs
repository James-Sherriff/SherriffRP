using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using GTANetworkServer;

namespace ApartmentLifeResource
{
  public static class Database
  {
    public const string PLAYERS_FOLDER = "players";
    public static void Init()
    {
      if (!Directory.Exists(PLAYERS_FOLDER))
      {
        Directory.CreateDirectory(PLAYERS_FOLDER);
      }
      API.shared.consoleOutput("Database initialized!");
    }
    
    public static bool DoesAccountExist(string name)
    {
      var path = Path.Combine(PLAYERS_FOLDER, name);
      return Directory.Exists(path);
    }
    
    public static bool IsPlayerLoggedIn(Client player)
    {
      return API.shared.getEntityData(player, "LOGGED_IN") == true;
    }
    
    public static void CreatePlayerAccount(Client player, string password)
    {
      var path = Path.Combine(PLAYERS_FOLDER, player.socialClubName);
      var data = new PlayerData()
      {
        socialClubName = player.socialClubName,
        Password = API.shared.getHashSHA256(password),
      };
      var ser = API.shared.toJson(data);
      File.WriteAllText(path,ser);
    }
    
    public static bool TryLoginPlayer(Client player, string password)
    {
      var path = Path.Combine(PLAYERS_FOLDER, player.socialClubName);
      
      var txt = File.ReadAllText(path);
      
      PlayerData playerObj = API.shared.fromJson(txt).ToObject<PlayerData>();
      
      return API.shared.getHashSHA256(password) == playerObj.Password;
    }
    
    public static void LoadPlayerAccount(Client player)
    {
      var path = Path.Combine(PLAYERS_FOLDER, player.socialClubName);
      
      var txt = File.ReadAllText(path);
      
      PlayerData playerObj = API.shared.fromJson(txt).ToObject<PlayerData>();
      
      API.shared.setEntityData(player, "LOGGED_IN", true);
      
      foreach (var property in typeof(PlayerData).GetProperties())
      {
        if (property.GetCustomAttributes(typeof (XmlIgnoreAttribute), false).Length > 0) continue;
        
        API.shared.setEntityData(player, property.Name, property.GetValue(playerObj, null));
      }
    }
    
    public static void SavePlayerAccount(Client player)
    {
      var path = Path.Combine(PLAYERS_FOLDER, player.socialClubName);
      
      if(!File.Exists(path)) return;
      
      var old = API.shared.fromJson(File.ReadAllText(path));
      
      var data = new PlayerData()
      {
        socialClubName = player.socialClubName,
        Password = old.Password,
      };
      
      foreach (var property in typeof(PlayerData).GetProperties())
      {
        if (property.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length > 0) continue;
        
        if(API.shared.hasEntityData(player, property.Name))
        {
          property.SetValue(data, API.shared.getEntityData(player, property.Name), null);
        }
      }
      var ser = API.shared.toJson(data);
      File.WriteAllText(path, ser);
    }
  }
  public class PlayerData
  {
    [XmlIgnore]
    public string socialClubName { get; set; }
    [XmlIgnore]
    public string Password { get; set; }

    public int Level { get; set; }
    public int WantedLevel { get; set; }
    public int Money { get; set; }
    public string Apartment { get; set; }
  }
}