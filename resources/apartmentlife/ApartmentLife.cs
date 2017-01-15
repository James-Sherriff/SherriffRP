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
      API.onEntityEnterColShape += OnEntityEnterColShapeHandler;
      Database.Init();
      ApartmentManager.Init();
    }
    
    private void OnEntityEnterColShapeHandler(ColShape shape, NetHandle entity)
    {
      Client player = API.getPlayerFromHandle(entity);        
      //Get the home for the player, spawn the right IPL, teleport them to it and change their dimension.
      //If the player doesn't have a home yet, notify them that they need to buy a home and show the command.
      //apa_v_mp_h_01_a
      API.requestIpl(API.getEntityData(player, "ApartmentType"));
      API.sendChatMessageToPlayer(player, "Welcome home.");
      //There should be a system where normalized names are linked to IPLs, get the apa1 vector for the used IPL
      API.setEntityPosition(player, new Vector3(-786.8663, 315.7642, 216.6385) + new Vector3(4, 0, 0));
      API.setEntityDimension(player, 1);
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
    public void apartments(Client player, string action, string parameter1 = "none")
    {
      if(action == "")
      {
        API.sendChatMessageToPlayer(player, "[ApartmentLife Help]");
        API.sendChatMessageToPlayer(player, "/apartments buy <apartmentName>");
        API.sendChatMessageToPlayer(player, "/apartments home");
        API.sendChatMessageToPlayer(player, "/apartments list");
      }
      else if(action == "buy")
      {
        string apartment = parameter1;
        int money = API.getEntityData(player, "Money");
        API.sendChatMessageToPlayer(player, "You have £" + money);
        API.sendChatMessageToPlayer(player, "You want to buy the apartment " + apartment);
        if(!String.IsNullOrEmpty(API.getEntityData(player, "ApartmentType")))    
        {
          API.sendChatMessageToPlayer(player, "You already have " + API.getEntityData(player, "ApartmentType") + " at " + API.getEntityData(player, "ApartmentLocation"));
        }
        else
        {
          API.setEntityData(player, "ApartmentType", apartment);
          API.setEntityData(player, "ApartmentLocation", API.getEntityPosition(player));
          LoadPlayerApartmentDoor(player);
        }
      }
      else if(action == "home")
      {
        //Get the home for the player, spawn the right IPL, teleport them to it and change their dimension.
        //If the player doesn't have a home yet, notify them that they need to buy a home and show the command.
        API.requestIpl(API.getEntityData(player, "ApartmentType"));
        API.sendChatMessageToPlayer(player, "Welcome home.");
        //There should be a system where normalized names are linked to IPLs, get the apa1 vector for the used IPL
        API.setEntityPosition(player, new Vector3(-786.8663, 315.7642, 216.6385) + new Vector3(4, 0, 0));
        API.setEntityDimension(player, 1);
      }
      else if(action == "back")
      {
        // There should be a door leave location set to an apartment. use this here
        API.setEntityPosition(player, API.getEntityData(player, "ApartmentLocation"));
      }
    }
    
    public void LoadPlayerApartmentDoor(Client sender)
    {
      if(!String.IsNullOrEmpty(API.getEntityData(player, "ApartmentType")))
      {
        var door = API.createSphereColShape(API.getEntityData(sender, "ApartmentLocation"), 1);
      }
    }
  }
}