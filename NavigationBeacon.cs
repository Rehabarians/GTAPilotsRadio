using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAPilotsRadio
{
    class NavigationBeacon : Script
    {
        public NavigationBeacon()
        {
            API.onResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            throw new NotImplementedException();
        }
    }
}
