using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Threading.Tasks;
using System.Timers;

namespace GTAPilotsRadio
{
    public class TakeoffandLanding : Script
    {
        bool Takeoff;
        Timer TakeoffTimer;

        public TakeoffandLanding()
        {
            API.onResourceStart += OnResourceStart;
            API.onPlayerFinishedDownload += OnPlayerDownload;
        }

        public void OnResourceStart()
        {
            ColShape LsiaArea = API.createSphereColShape(new Vector3(-1336.026, -3044.2, 12.55686), 2000f);
            ColShape EvwaArea = API.createSphereColShape(new Vector3(1148.543, 124.9639, 81.10194), 2000f);
            ColShape SandyArea = API.createSphereColShape(new Vector3(1330.159, 3112.599, 40.00936), 2000f);
            ColShape ZancudoArea = API.createSphereColShape(new Vector3(-2239.216, 3190.27, 31.90979), 2000f);

            LsiaArea.onEntityEnterColShape += LsiaOnEnter;
            EvwaArea.onEntityEnterColShape += EvwaOnEnter;
            ZancudoArea.onEntityEnterColShape += ZancudoOnEnter;
            SandyArea.onEntityEnterColShape += SandyOnEnter;

            LsiaArea.onEntityExitColShape += LsiaOnExit;
            EvwaArea.onEntityExitColShape += EvwaOnExit;
            ZancudoArea.onEntityExitColShape += ZancudoOnExit;
            SandyArea.onEntityExitColShape += SandyOnExit;
        }

        public void OnPlayerDownload(Client Player)
        {
            API.setEntitySyncedData(Player, "Taking Off", false);
            API.setEntitySyncedData(Player, "Landing", false);
            API.setEntitySyncedData(Player, "ADF Status", false);
            API.setEntitySyncedData(Player, "DME Status", false);
            API.setEntitySyncedData(Player, "ILS Status", false);
        }

        public void LsiaOnEnter(ColShape shape, NetHandle entity)
        {
            Client Player = API.getEntityFromHandle<Client>(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntityData(Player, "Airport", "Lsia");
        }

        public void EvwaOnEnter(ColShape shape, NetHandle entity)
        {
            
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntitySyncedData(Player, "InsideAirportArea", "Evwa");
        }

        public void SandyOnEnter(ColShape shape, NetHandle entity)
        {
            
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntitySyncedData(Player, "InsideAirportArea", "Sandy");
        }

        public void ZancudoOnEnter(ColShape shape, NetHandle entity)
        {
            
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntitySyncedData(Player, "InsideAirportArea", "Zancudo");
        }

        public void LsiaOnExit(ColShape shape, NetHandle entity)
        {
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntitySyncedData(Player, "Taking Off", false);
            API.setEntitySyncedData(Player, "CommandCallerLsiaTO", false);
            API.setEntitySyncedData(Player, "TakeoffTimerLsia", false);
            API.setEntityData(Player, "Airport", "None");
        }

        public void EvwaOnExit(ColShape shape, NetHandle entity)
        {
            
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntitySyncedData(Player, "TakeoffTimer", false);
            API.setEntitySyncedData(Player, "LandingTimer", false);
            API.setEntitySyncedData(Player, "CommandCallerLND", false);
            API.setEntitySyncedData(Player, "CommandCallerTO", false);
            API.setEntitySyncedData(Player, "Landing", false);
            API.setEntitySyncedData(Player, "Taking Off", false);
        }

        public void SandyOnExit(ColShape shape, NetHandle entity)
        {
           
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }
            API.setEntitySyncedData(Player, "TakeoffTimer", false);
            API.setEntitySyncedData(Player, "LandingTimer", false);
            API.setEntitySyncedData(Player, "CommandCallerLND", false);
            API.setEntitySyncedData(Player, "CommandCallerTO", false);
            API.setEntitySyncedData(Player, "Landing", false);
            API.setEntitySyncedData(Player, "Taking Off", false);
        }

        public void ZancudoOnExit(ColShape shape, NetHandle entity)
        {
            
            Client Player = API.getPlayerFromHandle(entity);

            if (Player == null)
            {
                return;
            }

            API.setEntitySyncedData(Player, "TakeoffTimer", false);
            API.setEntitySyncedData(Player, "LandingTimer", false);
            API.setEntitySyncedData(Player, "CommandCallerLND", false);
            API.setEntitySyncedData(Player, "CommandCallerTO", false);
            API.setEntitySyncedData(Player, "Landing", false);
            API.setEntitySyncedData(Player, "Taking Off", false);
        }

        public void TakeoffTimerFunction(Client Player)
        {
            Takeoff = true;

            TakeoffTimer  = API.startTimer(60000, true, () =>
            {
                Takeoff = false;
                API.setEntityData(Player, "TakeoffAllowed", false);
            });            
        }

        [Command("takeoff", Alias = "to", GreedyArg = true)]
        public void TakeoffCommand(Client Player)
        {
            bool inVeh = API.isPlayerInAnyVehicle(Player);
            if (inVeh == true)
            {
                List<Client> AllPlayers = API.getAllPlayers();
                foreach (Client Players in AllPlayers)
                {
                    bool anyAirportData = API.hasEntityData(Players, "Airport");
                    if (anyAirportData == true)
                    {
                        string Airport = API.getEntityData(Players, "Airport");                    
                        if (Airport == "Lsia")
                        {
                            bool anyData = API.hasEntitySyncedData(Players, "TakeoffAllowed");
                            if (anyData == true)
                            {
                                bool TakeoffAllowed = API.getEntitySyncedData(Players, "TakeoffAllowed");
                                if (TakeoffAllowed == true)
                                {
                                    API.setEntitySyncedData(Player, "TakeoffAllowed", false);
                                    API.triggerClientEvent(Player, "LsiaTOCommand", false);
                                }
                                else
                                {
                                    TakeoffTimerFunction(Player);
                                    API.setEntitySyncedData(Player, "TakeoffAllowed", true);
                                    API.triggerClientEvent(Player, "LsiaTOCommand", true);
                                }
                            }
                            else
                            {
                                API.setEntityData(Player, "TakeoffAllowed", false);
                            }
                            API.triggerClientEvent(Players, "LsiaTO");
                        }
                    }
                }
            }
        }

        [Command("land", Alias = "lnd", GreedyArg = true)]
        public void LandingCommand(Client Player)
        {
            bool inVeh = API.isPlayerInAnyVehicle(Player);

            if (inVeh == true)
            {
                bool AnyAirportData = API.hasEntityData(Player, "Airport");

                if (AnyAirportData == true)
                {
                    string AirportData = API.getEntityData(Player, "Airport");

                    if (AirportData == "Lsia")
                    {
                        API.setEntitySyncedData(Player, "Landing", true);
                        API.setEntitySyncedData(Player, "CommandCallerLsiaLND", true);
                        API.setEntitySyncedData(Player, "LandingTimerLsia", true);
                        API.triggerClientEvent(Player, "LsiaLND");

                        API.sendChatMessageToPlayer(Player, "Lsia Landing Requested");

                        //API.getPlayersInRadiusOfPosition(2000f, new Vector3(-1336.026, -3044.2, 12.55686));
                        var t1 = Task.Run(async delegate
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5));
                            API.setEntitySyncedData(Player, "Landing", false);
                            API.triggerClientEvent(Player, "LsiaLND");
                            return;
                        });

                        var t2 = Task.Run(async delegate
                        {
                            await Task.Delay(TimeSpan.FromMinutes(1));
                            API.setEntitySyncedData(Player, "CommandCallerLsiaLND", false);
                            API.setEntitySyncedData(Player, "LandingTimerLsia", false);
                        });
                    }
                    else if (AirportData == "None")
                    {
                        API.resetEntitySyncedData(Player, "Landing");
                        API.resetEntitySyncedData(Player, "CommandCallerLsiaLND");
                        API.resetEntitySyncedData(Player, "LandingTimerLsia");
                    }
                }
            }

            List<Client> AllthePlayers = API.getAllPlayers();

            foreach (var Pilot in AllthePlayers)
            {
                if (Pilot == Player)
                {
                    return;
                }

                //bool pilotAnyData1 = API.hasEntitySyncedData(Pilot, "CommandCallerLsiaTO");
                bool pilotAnyData2 = API.hasEntitySyncedData(Pilot, "LandingTimerLsia");

                if (pilotAnyData2 == true)
                {
                    bool pilotData2 = API.getEntitySyncedData(Pilot, "LandingTimerLsia");

                    if (pilotData2 == false)
                    {
                        API.setEntitySyncedData(Pilot, "CommandCallerLsiaLND", false);
                        API.triggerClientEvent(Pilot, "LsiaLND");
                    }
                }
                return;
            }
        }

        [Command("ADF", "~y~Usage: ~w~/ADF [LSIA, EVWA, SANDY, FZ]", Alias = "adf", GreedyArg = true)]
        public void ADFBeacon(Client player, string text)
        {
            var beaconADF = text.ToUpper();

            if (beaconADF == "LSIA")
            {
                API.triggerClientEvent(player, "ADF LSIA");
            }

            else if (beaconADF == "EVWA")
            {
                API.triggerClientEvent(player, "ADF EVWA");
            }

            else if (beaconADF == "SANDY")
            {
                API.triggerClientEvent(player, "ADF Sandy");
            }

            else if (beaconADF == "FZ")
            {
                API.triggerClientEvent(player, "ADF FZ");
            }

            else if (beaconADF == "OFF")
            {
                API.triggerClientEvent(player, "ADF OFF");
            }

            else if (beaconADF == "HELP")
            {
                API.sendChatMessageToPlayer(player, "/ADF [LSIA, LSL, MB, MNT, OFF, HELP]");
            }

            else
            {
                API.sendChatMessageToPlayer(player, "Please enter valid ADF Beacon Callsign!");
            }
        }

        [Command("DME", "~y~Usage: ~w~/DME [LSIA, EVWA, SANDY, FZ]", Alias = "dme", GreedyArg = true)]
        public void DMEBeacon(Client player, string text)
        {
            var beaconDME = text.ToUpper();

            if (beaconDME == "LSIA")
            {
                API.triggerClientEvent(player, "DME LSIA");
            }

            else if (beaconDME == "EVWA")
            {
                API.triggerClientEvent(player, "DME EVWA");
            }

            else if (beaconDME == "SANDY")
            {
                API.triggerClientEvent(player, "DME Sandy");
            }

            else if (beaconDME == "FZ")
            {
                API.triggerClientEvent(player, "DME FZ");
            }

            else if (beaconDME == "OFF")
            {
                API.triggerClientEvent(player, "DME OFF");
            }

            else if (beaconDME == "HELP")
            {
                API.sendChatMessageToPlayer(player, "/DME [LSIA, LSL, MB, MNT, OFF, HELP]");
            }

            else
            {
                API.sendChatMessageToPlayer(player, "Please enter valid DME Beacon Callsign!");
            }
        }

        [Command("ILS", "~y~Usage: ~w~/ILS [LSIA, EVWA, SANDY, FZ]", Alias = "ils", GreedyArg = true)]
        public void ILSBeacon(Client player, string text)
        {
            var beaconILS = text.ToUpper();

            if (beaconILS == "LSIA")
            {
                API.sendChatMessageToPlayer(player, text);
                API.triggerClientEvent(player, "ILS LSIA");
            }

            else if (beaconILS == "EVWA")
            {
                API.triggerClientEvent(player, "ILS EVWA");
            }

            else if (beaconILS == "SANDY")
            {
                API.triggerClientEvent(player, "ILS Sandy");
            }

            else if (beaconILS == "FZ")
            {
                API.triggerClientEvent(player, "ILS FZ");
            }

            else if (beaconILS == "OFF")
            {
                API.triggerClientEvent(player, "ILS OFF");
            }

            else if (beaconILS == "HELP")
            {
                API.sendChatMessageToPlayer(player, "/ILS [LSIA, EVWA, SANDY, FZ, OFF, HELP]");
            }

            else
            {
                API.sendChatMessageToPlayer(player, "Use /ILS HELP for a list of available beacons");
            }
        }

    }
}
