using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using GTANetworkServer;
using GTANetworkShared;

namespace ApartmentLifeResource
{
  public static class ApartmentManager
  {
    public const string APARTMENTS_FOLDER = "apartments";
    public static void Init()
    {
      if(!Directory.Exists(APARTMENTS_FOLDER))
      {
        Directory.CreateDirectory(APARTMENTS_FOLDER);
      }
    }
  }
}