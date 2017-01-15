using System;
using GTANetworkServer;
using GTANetworkShared;

namespace ApartmentLifeResource
{
  public class ApartmentLife : Script
  {
    //The vector for apa1
    Vector3 apa1 = new Vector3(-786.8663, 315.7642, 216.6385);

    //The IPL for apa1
    //String apa1ipl = "apa_v_mp_h_01_a"
    public ApartmentLife()
    {
      API.onResourceStart += apartmentlifeResourceStart;
      Database.Init();
      
      var door = API.createSphereColShape(new Vector3(780.74f,1274,361.28f), 1);
      
      // Triggered when a player, vehicle or object exit the mine's range
      door.onEntityEnterColShape += (s, ent) =>
      {
        Client player = API.getPlayerFromHandle(ent);        
        //Get the home for the player, spawn the right IPL, teleport them to it and change their dimension.
        //If the player doesn't have a home yet, notify them that they need to buy a home and show the command.
        API.requestIpl("apa_v_mp_h_01_a");
        API.sendChatMessageToPlayer(player, "Welcome home.");
        API.setEntityPosition(player, apa1 + new Vector3(4, 0, 0));
        API.setEntityDimension(ent, 1);
      };
    }

    public void apartmentlifeResourceStart()
    {
    }
    
    [Command("getmoney")]
    public void getmoney(Client player, int amount)
    {
      API.setEntityData(player, "Money", Convert.ToInt32(API.getEntityData(player, "Money")) + amount);
      API.sendChatMessageToPlayer(player, "You now have £" + API.getEntityData(player, "Money"));
    }
    
    [Command("whereami")]
    public void whereami(Client player)
    {
      API.sendChatMessageToPlayer(player, Convert.ToString(API.getEntityPosition(player)));
    }
    [Command("apartments")]
    public void apartments(Client player, string action, string parameter1)
    {
      if(action == ""){
        API.sendChatMessageToPlayer(player, "[ApartmentLife Help]");
        API.sendChatMessageToPlayer(player, "/apartments buy <apartmentName>");
        API.sendChatMessageToPlayer(player, "/apartments home");
        API.sendChatMessageToPlayer(player, "/apartments list");
      }
      else if(action == "buy"){
        string apartment = parameter1;
        int money = API.getEntityData(player, "Money");
        API.sendChatMessageToPlayer(player, "You have £" + money);
        API.sendChatMessageToPlayer(player, "You want to buy the apartment " + apartment);
        if(!String.IsNullOrEmpty(API.getEntityData(player, "Apartment")))    
        {
          API.sendChatMessageToPlayer(player, "You already have " + API.getEntityData(player, "Apartment"));
        }
        else
        {
          API.setEntityData(player, "Apartment", apartment);
        }
        //if(money > 10000){
        //  API.sendChatMessageToPlayer(player, "Bought " + parameter1); 
        //}
      }
      else if(action == "home"){
        //Get the home for the player, spawn the right IPL, teleport them to it and change their dimension.
        //If the player doesn't have a home yet, notify them that they need to buy a home and show the command.
        NetHandle entity = player;
        API.requestIpl("the ipl for the p home");
        API.setEntityDimension(entity, 1);
        API.setEntityPosition(player, apa1 + new Vector3(4, 0, 0));
        API.sendChatMessageToPlayer(player, "Welcome home.");
      }
    }
  }
}