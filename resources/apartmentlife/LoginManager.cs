using GTANetworkServer;
using GTANetworkShared;

namespace ApartmentLifeResource 
{
  public class LoginManager : Script
  {
    public LoginManager()
    {
      API.onResourceStop += onResourceStop;
    }
    
    [Command]
    public void Login(Client sender, string password)
    {
      if (Database.IsPlayerLoggedIn(sender))
      {
        API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
        return;
      }
      
      if(!Database.TryLoginPlayer(sender, password))
      {
        API.sendChatMessageToPlayer(sender, "~r~ERROR:~w~ Either your password is wrong or you haven't registered!");
      }
      
      else
      {
        Database.LoadPlayerAccount(sender);
        API.call("ApartmentLife", "LoadPlayerApartmentDoor", sender);
        API.sendChatMessageToPlayer(sender, "~g~ Logged in!");
      }
    }
    
    [Command] 
    public void Register(Client sender, string password)
    {
      if(Database.IsPlayerLoggedIn(sender))
      {
        API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
        return;
      }
      
      if(Database.DoesAccountExist(sender.socialClubName))
      {
        API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~An account linked to this Social Club handle already exists!");
        return;
      }
      
      Database.CreatePlayerAccount(sender, password);
      API.sendChatMessageToPlayer(sender, "~g~Account created! ~w~Now log in with ~y~/login [password]");
    }
    public void onResourceStop()
    {
      foreach (var client in API.getAllPlayers())
      {
        foreach (var data in API.getAllEntityData(client))
        {
          API.resetEntityData(client, data);
        }
      }
    }
  }
}