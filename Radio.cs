using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace GTAPilotsRadio
{
    public class Radio : Script
    {
        string[] BannedWords = { "FUCK", "ASS", "A$$", "ASSHOLE", "ASS HOLE", "SHIT", "DICK", "PENIS", "VAGINA", "PUSSY", "BITCH", "TITS", "WANKER", "DICKHEAD", "DICK HEAD" };
        Dictionary<string, string> RadioDictionary = new Dictionary<string, string>();
        
        public Radio()
        {
            //API.onChatCommand += OnChatCommand;
            API.onChatMessage += OnChatMessage;
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            RadioDictionary.Add("#rw", "Runway");
            RadioDictionary.Add("#tx1", "Taxiing");
            RadioDictionary.Add("#tx2", "Taxied");
            RadioDictionary.Add("#to1", "Taking Off");
            RadioDictionary.Add("#to2", "Taken Off");
            RadioDictionary.Add("#dp1", "Departing");
            RadioDictionary.Add("#dp2", "Departed");
            RadioDictionary.Add("#ln1", "Landing");
            RadioDictionary.Add("#ln2", "Landed");
            RadioDictionary.Add("#ac", "Active");
            RadioDictionary.Add("#al", "Altitude");
            RadioDictionary.Add("#fl", "Flight Level");
            RadioDictionary.Add("#hd", "Heading");
            RadioDictionary.Add("#sp", "Speed");
            RadioDictionary.Add("#lsia", "Los Santos International Airport");
            RadioDictionary.Add("#evwa", "East Vinewood Airfield");
            RadioDictionary.Add("#sandy", "Sandy Shores Airfield");
            RadioDictionary.Add("#fz", "Fort Zancudo Airport");
        }

        private void OnChatMessage(Client sender, string message, CancelEventArgs cancel)
        {
            
            bool playerInVehicle = API.isPlayerInAnyVehicle(sender);

            //string Tailnumber = "";
            
            string Uppercase = message.ToUpper();
            
            foreach (var NaughtyWord in BannedWords)
            {
                if (Uppercase.Contains(NaughtyWord))
                {
                    API.sendChatMessageToPlayer(sender, "Please keep the chat civilised!");
                    cancel.Cancel = true;

                    //bool anyData = API.hasEntityData(sender, "Warnings");

                    //if (anyData == true)
                    //{
                    //    int Warnings = API.getEntityData(sender, "Warnings");

                    //    int Warn = Warnings + 1;
                    //    API.setEntityData(sender, "Warnings", Warn);
                    //}
                    //else
                    //{
                    //    API.setEntityData(sender, "Warnings", 1);
                    //}
                }

                break;
            }

            if (message.StartsWith("+"))
            {                
                message = message.Replace("+", "(Global: ");
                
                if (message.Contains("#tn"))
                {
                    if (playerInVehicle == true)
                    {
                        NetHandle veh = API.getPlayerVehicle(sender);

                        bool hasVehicleType = API.hasEntitySyncedData(veh, "VehicleType");

                        if (hasVehicleType == true)
                        {
                            string vehicleType = API.getEntitySyncedData(veh, "VehicleType");

                            if (vehicleType == "Aircraft")
                            {
                                bool anyVehicleData = API.hasEntitySyncedData(veh, "Tailnumber");

                                if (anyVehicleData == true)
                                {
                                    string Tailnumber = API.getEntitySyncedData(veh, "Tailnumber");

                                    message = message.Replace("#tn", "G-" + Tailnumber);                                   
                                }
                            }
                            else
                            {
                                message = message.Replace("#tn", "");
                            }
                        }
                        else
                        {
                            message = message.Replace("#tn", "");
                        }
                    }
                    else
                    {
                        message = message.Replace("#tn", "");
                    }
                }

                foreach(var Shortcut in RadioDictionary)
                {
                    if (message.Contains(Shortcut.Key))
                    {
                        message = message.Replace(Shortcut.Key, Shortcut.Value);
                       
                    }
                }
                
                API.sendChatMessageToAll("~c~" + message + ")");
                cancel.Cancel = true;
            }
            else if (message.StartsWith("-"))
            {               
                message = message.Replace("-", "(Local: ");

                if (message.Contains("#tn"))
                {
                    if (playerInVehicle == true)
                    {
                        NetHandle veh = API.getPlayerVehicle(sender);

                        bool hasVehicleType = API.hasEntitySyncedData(veh, "VehicleType");

                        if (hasVehicleType == true)
                        {
                            string vehicleType = API.getEntitySyncedData(veh, "VehicleType");

                            if (vehicleType == "Aircraft")
                            {
                                bool anyVehicleData = API.hasEntitySyncedData(veh, "Tailnumber");

                                if (anyVehicleData == true)
                                {
                                    string Tailnumber = API.getEntitySyncedData(veh, "Tailnumber");

                                    message = message.Replace("#tn", "G-" + Tailnumber);
                                }
                            }
                            else
                            {
                                message = message.Replace("#tn", "");
                            }
                        }
                        else
                        {
                            message = message.Replace("#tn", "");
                        }
                    }
                    else
                    {
                        message = message.Replace("#tn", "");
                    }
                }

                foreach (var Shortcut in RadioDictionary)
                {
                    if (message.Contains(Shortcut.Key))
                    {
                        message = message.Replace(Shortcut.Key, Shortcut.Value);
                    }
                }

                List<Client> PlayersInRadius = API.getPlayersInRadiusOfPlayer(20, sender);
                cancel.Cancel = true;
                foreach (var Players in PlayersInRadius)
                {
                    API.sendChatMessageToPlayer(Players, "~c~" + message + ")");
                }               
            }
            else if (message.StartsWith("="))
            {
                message = message.Replace("=", "(Intercomm: ");

                if (message.Contains("#tn"))
                {
                    if (playerInVehicle == true)
                    {
                        NetHandle veh = API.getPlayerVehicle(sender);

                        bool hasVehicleType = API.hasEntitySyncedData(veh, "VehicleType");

                        if (hasVehicleType == true)
                        {
                            string vehicleType = API.getEntitySyncedData(veh, "VehicleType");

                            if (vehicleType == "Aircraft")
                            {
                                bool anyVehicleData = API.hasEntitySyncedData(veh, "Tailnumber");

                                if (anyVehicleData == true)
                                {
                                    string Tailnumber = API.getEntitySyncedData(veh, "Tailnumber");

                                    message = message.Replace("#tn", "G-" + Tailnumber);
                                }
                            }
                            else
                            {
                                message = message.Replace("#tn", "");
                            }
                        }
                        else
                        {
                            message = message.Replace("#tn", "");
                        }
                    }
                    else
                    {
                        message = message.Replace("#tn", "");
                    }
                }

                foreach (var Shortcut in RadioDictionary)
                {
                    if (message.Contains(Shortcut.Key))
                    {
                        message = message.Replace(Shortcut.Key, Shortcut.Value);
                    }
                }

                NetHandle Veh = API.getPlayerVehicle(sender);
                Client[] PlayersInVehicle = API.getVehicleOccupants(Veh);

                cancel.Cancel = true;
                foreach (var Player in PlayersInVehicle)
                {
                    API.sendChatMessageToPlayer(Player, "~c~" + message + ")");
                }
            }
            else if (message.StartsWith("!"))
            {
                NetHandle Veh = API.getPlayerVehicle(sender);

                Client[] PlayersInVehicle = API.getVehicleOccupants(Veh);

                foreach (var Players in PlayersInVehicle)
                {
                    if (Players.vehicleSeat == -1)
                    {
                        message = message.Replace("!", "(Captain: ");
                    }
                    else if (Players.vehicleSeat == 0){
                        message = message.Replace("!", "(First Officer: ");
                    }
                    else
                    {
                        message = null;
                        cancel.Cancel = true;
                    }

                    if (message.Contains("#tn"))
                    {
                        if (playerInVehicle == true)
                        {
                            NetHandle veh = API.getPlayerVehicle(sender);

                            bool hasVehicleType = API.hasEntitySyncedData(veh, "VehicleType");

                            if (hasVehicleType == true)
                            {
                                string vehicleType = API.getEntitySyncedData(veh, "VehicleType");

                                if (vehicleType == "Aircraft")
                                {
                                    bool anyVehicleData = API.hasEntitySyncedData(veh, "Tailnumber");

                                    if (anyVehicleData == true)
                                    {
                                        string Tailnumber = API.getEntitySyncedData(veh, "Tailnumber");

                                        message = message.Replace("#tn", "G-" + Tailnumber);
                                    }
                                }
                                else
                                {
                                    message = message.Replace("#tn", "");
                                }
                            }
                            else
                            {
                                message = message.Replace("#tn", "");
                            }
                        }
                        else
                        {
                            message = message.Replace("#tn", "");
                        }
                    }

                    foreach (var Shortcut in RadioDictionary)
                    {
                        if (message.Contains(Shortcut.Key))
                        {
                            message = message.Replace(Shortcut.Key, Shortcut.Value);
                        }
                    }
                    cancel.Cancel = true;
                    API.sendChatMessageToPlayer(Players, "~c~" + message + ")");
                }
            }
            else
            {

                bool anyData = sender.hasData("Class");
                if (anyData == true)
                {
                    string Class = sender.getData("Class");

                    cancel.Cancel = true;

                    if (Class == "Civil")
                    {
                        API.sendChatMessageToAll("~#79CE79~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Military")
                    {
                        API.sendChatMessageToAll("~#737373~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Stunt")
                    {
                        API.sendChatMessageToAll("~#ECF029~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Rescue")
                    {
                        API.sendChatMessageToAll("~#3776BD~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Repair")
                    {
                        API.sendChatMessageToAll("~#847232~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Medic")
                    {
                        API.sendChatMessageToAll("~#ffffff~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Fire")
                    {
                        API.sendChatMessageToAll("~#E13B3B~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Security")
                    {
                        API.sendChatMessageToAll("~#4C4276~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Host")
                    {
                        API.sendChatMessageToAll("~#F745A5~", sender.name + "~w~: " + message);
                    }
                    else if (Class == "Passenger")
                    {
                        API.sendChatMessageToAll("~#9ECDEB~", sender.name + "~w~: " + message);
                    }
                }
            }
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {

        }
    }
}
