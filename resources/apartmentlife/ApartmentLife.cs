using System;
using GTANetworkServer;
using GTANetworkShared;

namespace ApartmentLifeResource
{
  public class ApartmentLife : Script
  {
    //The vector for apa1
    //Vector3 apa1 = new Vector3(-786.8663, 315.7642, 216.6385);
    //The IPL for apa1
    //String apa1ipl = "apa_v_mp_h_01_a"

    public ApartmentLife()
    {
      API.onResourceStart += apartmentlifeResourceStart;
      Database.Init();
    }

    public void apartmentlifeResourceStart()
    {
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
        //if(money > 10000){
        //  API.sendChatMessageToPlayer(player, "Bought " + parameter1); 
        //}
      }
      else if(action == "home"){
        //Get the home for the player, spawn the right IPL, teleport them to it and change their dimension.
        //If the player doesn't have a home yet, notify them that they need to buy a home and show the command.
        NetHandle entity = player;
        API.requestIpl("the ipl for the players home");
        API.setEntityDimension(entity, theIdForTheDimensionCouldBePlayerID);
        API.setEntityPosition(player, thePlayersApartmentVector + new Vector3(4, 0, 0));
        API.sendChatMessageToPlayer(player, "Welcome home.");
      }
    }
  }
}