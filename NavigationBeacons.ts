/// <reference path ="\types-gt-mp\Definitions\index.d.ts" />

var resX = API.getScreenResolutionMaintainRatio().Width;
var resY = API.getScreenResolutionMaintainRatio().Height;

API.onServerEventTrigger.connect(function (Command, args) {

    var player = API.getLocalPlayer();
    var ADFStatus = null;
    var DMEStatus = null;
    var ADFID = "";
    var DMEID = "";

    switch (Command) {
        case "ADF LSIA":
            {
                API.onUpdate.connect(function () {
                    var Lsia = new Vector3(-1336.25, -3044.04, 12.94);
                    var veh = API.getPlayerVehicle(player);
                    var playerLocation = API.getEntityPosition(veh);
                    var vehRot = API.getEntityRotation(veh);

                    const normalHdgVector = {
                        x: API.returnNative('0x8BB4EF4214E0E6D5', 7, veh), // float ENTITY::GET_ENTITY_FORWARD_X
                        y: API.returnNative('0x866A4A5FAE349510', 7, veh) // float ENTITY::GET_ENTITY_FORWARD_Y
                    }

                    // ^ the above object is a normalized Vector2 (e.g. instead of -180 to +180, it's -1.0 to +1.0)
                    // this means we can do an atan2 then convert it's radians to degrees! (and slightly exploit it so it's easier correction)
                    let Compass = Math.round(Math.atan2(normalHdgVector.x, normalHdgVector.y) * 180 / Math.PI)

                    // hdg is now what an entity's Z rotation is when on flat ground! A little correction...
                    if (Compass < 0) {
                        Compass = Math.abs(Compass)
                    } else if (Compass > 0) {
                        Compass = 360 - Compass
                    }
                    // The value we have is mirrored, so this flips it.
                    Compass = 360 - Compass

                    var radian = Math.atan2((vehRot.Y - Lsia.Y), (vehRot.X - Lsia.X));
                    var angle = Math.round(radian * (180 / Math.PI));

                    var test4 = Math.round(Math.atan2(angle, Compass) * (180 / Math.PI))

                    var test1 = Math.round(angle + 180);

                    var test3 = Math.round(360 - test1)
                    
                    ADFStatus = true;
                    ADFID = "Los Santos International Airport Beacon";

                    API.setEntitySyncedData(player, "ADF", test3);
                    API.setEntitySyncedData(player, "ADF Status", ADFStatus);
                    API.setEntitySyncedData(player, "ADFID", ADFID);
                });

                break;
            }
        case "DME LSIA":
            {
                API.onUpdate.connect(function () {
                    var LSIA = new Vector3(-1336.25, -3044.04, 12.94);
                    var playerLocation = API.getEntityPosition(API.getLocalPlayer());

                    var distanceX = Math.round(playerLocation.X - LSIA.X);
                    var distanceY = Math.round(playerLocation.Y - LSIA.Y);
                    var distanceXsquared = Math.round(distanceX * distanceX);
                    var distanceYsquared = Math.round(distanceY * distanceY);
                    var distanceSqrt = Math.sqrt(distanceXsquared + distanceYsquared);
                    var distance = Math.round((distanceSqrt / 100) / 1852);

                    DMEID = "Los Santos International Airport Beacon";

                    API.setEntitySyncedData(player, "DME", distance);
                    API.setEntitySyncedData(player, "DME Status", true);
                    API.setEntitySyncedData(player, "DMEID", DMEID);
                });
                break;
            }
        case "VOR LSIA":
            {
                break;
            }
        case "ILS LSIA":
            {
                API.onUpdate.connect(function () {
                    let LSIAGlideslope: Vector3 = new Vector3(-1502.511, -2948.156, 13.94577);

                    let veh: LocalHandle = API.getPlayerVehicle(API.getLocalPlayer());
                    let VehPos: Vector3 = API.getEntityPosition(veh);
                    let Opposite: number = VehPos.Z;
                    let Adjacent: number = LSIAGlideslope.DistanceTo2D(VehPos);

                    let Tan: number = Opposite / Adjacent;

                    LsiaL(API.getLocalPlayer(), veh);

                    API.setEntitySyncedData(player, "LSIA Glideslope", Tan);
                    API.setEntitySyncedData(player, "ILS ID", "LSIA");
                    API.setEntitySyncedData(player, "ILS Status", true);
                });
                break;
            }
        case "ADF EVWA":
            {
                API.onUpdate.connect(function () {
                    var Lsl = new Vector3(1153.90381, 128.684952, 80.824646);
                    var playerLocation = API.getEntityPosition(player);

                    var radian = Math.atan2(Math.round(playerLocation.Y - Lsl.Y), Math.round(playerLocation.X - Lsl.X));
                    var angle = Math.round(radian * Math.round(180 / Math.PI));
                    var signLSL = Math.sign(angle);

                    if (signLSL == 1) {
                        var angleTrue = Math.round(360 - angle);
                    }

                    else if (signLSL == -1) {
                        var angleTrue = Math.round(Math.abs(angle));
                    }

                    ADFStatus = true;
                    ADFID = "East Vinewood Airfield";

                    API.setEntitySyncedData(player, "ADF", angleTrue);
                    API.setEntitySyncedData(player, "ADF Status", ADFStatus);
                    API.setEntitySyncedData(player, "ADFID", ADFID);
                });
                break;
            }
        case "DME EVWA":
            {
                API.onUpdate.connect(function () {
                    var LSL = new Vector3(1153.90381, 128.684952, 80.824646);
                    var playerLocation = API.getEntityPosition(player);

                    var distanceX = Math.round(playerLocation.X - LSL.X);
                    var distanceY = Math.round(playerLocation.Y - LSL.Y);
                    var distanceXsquared = Math.round(distanceX * distanceX);
                    var distanceYsquared = Math.round(distanceY * distanceY);
                    var distanceSqrt = Math.sqrt(distanceXsquared + distanceYsquared);
                    var distance = Math.round(distanceSqrt / 100);

                    DMEID = "Los Santos Local Airport";

                    API.setEntitySyncedData(player, "DME", distance);
                    API.setEntitySyncedData(player, "DME Status", true);
                    API.setEntitySyncedData(player, "DMEID", DMEID);
                });
                break;
            }
        case "VOR EVWA":
            {
                break;
            }
        case "ILS EVWA":
            {
                break;
            }
        case "ADF FZ":
            {
                API.onUpdate.connect(function () {
                    var Mb = new Vector3(-2196.0354, 3028.24268, 31.9);
                    var playerLocation = API.getEntityPosition(player);

                    var radian = Math.atan2(Math.round(playerLocation.Y - Mb.Y), Math.round(playerLocation.X - Mb.X));
                    var angle = Math.round(radian * Math.round(180 / Math.PI));
                    var convert = Math.abs(angle);

                    ADFStatus = true;
                    ADFID = "Military Base";

                    API.setEntitySyncedData(player, "ADF", angle);
                    API.setEntitySyncedData(player, "ADF Status", ADFStatus);
                    API.setEntitySyncedData(player, "ADFID", ADFID);
                });
                break;
            }
        case "DME FZ":
            {
                API.onUpdate.connect(function () {
                    var MB = new Vector3(-2196.0354, 3028.24268, 31.9);
                    var playerLocation = API.getEntityPosition(player);

                    var distanceX = Math.round(playerLocation.X - MB.X);
                    var distanceY = Math.round(playerLocation.Y - MB.Y);
                    var distanceXsquared = Math.round(distanceX * distanceX);
                    var distanceYsquared = Math.round(distanceY * distanceY);
                    var distanceSqrt = Math.sqrt(distanceXsquared + distanceYsquared);
                    var distance = Math.round(distanceSqrt / 100);


                    DMEID = "Military Base";

                    API.setEntitySyncedData(player, "DME", distance);
                    API.setEntitySyncedData(player, "DME Status", true);
                    API.setEntitySyncedData(player, "DMEID", DMEID);
                });
                break;
            }
        case "VOR FZ":
            {
                break;
            }
        case "ILS FZ":
            {
                break;
            }
        case "ADF Sandy":
            {
                break;
            }
        case "DME Sandy":
            {
                break;
            }
        case "VOR Sandy":
            {
                break;
            }
        case "ILS Sandy":
            {
                break;
            }
        case "ADF OFF":
            {
                API.onUpdate.connect(function () {

                    ADFStatus = false;
                    ADFID = "";
                    API.setEntitySyncedData(player, "ADF Status", ADFStatus);
                    API.setEntitySyncedData(player, "ADFID", ADFID);

                });
                break;
            }
        case "DME OFF":
            {
                DMEID = "";
                API.setEntitySyncedData(player, "DME Status", false);
                API.setEntitySyncedData(player, "DMEID", DMEID);
                break;
            }
        case "VOR OFF":
            {
                break;
            }
        case "ILS OFF":
            {
                API.setEntitySyncedData(API.getLocalPlayer(), "ILS ID", "");
                API.setEntitySyncedData(API.getLocalPlayer(), "ILS Status", false);
                API.sendChatMessage("ILS Disabled");
                break;
            }
    }
});

function LsiaL(Player: LocalHandle, Vehicle: LocalHandle) {

    let VehPos: Vector3 = API.getEntityPosition(Vehicle);

    let LSIARight: Vector3 = new Vector3(-1538.133, -3009.956, 13.95026);
    let LSIACenterline: Vector3 = new Vector3(-1545.173, -3022.796, 13.95519);
    let LSIALeft: Vector3 = new Vector3(-1552.572, -3035.513, 13.94475);

    var distanceXL = Math.round(VehPos.X - LSIALeft.X);
    var distanceXR = Math.round(VehPos.X - LSIARight.X);

    var distanceYL = Math.round(VehPos.Y - LSIALeft.Y);
    var distanceYR = Math.round(VehPos.Y - LSIARight.Y);

    var distanceXLsquared = Math.round(distanceXL * distanceXL);
    var distanceYLsquared = Math.round(distanceYL * distanceYL);

    var distanceXRsquared = Math.round(distanceXR * distanceXR);
    var distanceYRsquared = Math.round(distanceYR * distanceYR);

    var distanceSqrtL = Math.sqrt(distanceXLsquared + distanceYLsquared);
    var distanceSqrtR = Math.sqrt(distanceXRsquared + distanceYRsquared);

    var distanceL = (distanceSqrtL / 100);
    var distanceR = (distanceSqrtR / 100);

    var DistanceTotal = distanceSqrtL - distanceSqrtR;

    API.setEntitySyncedData(Player, "LSIA Centerline", DistanceTotal);
}

//function LsiaR() {

//}