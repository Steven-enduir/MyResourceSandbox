using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MyResourceSandboxClient
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            //Spawn weapon on character
            RegisterCommand("weapon", new Action<int, List<object>, string>( (source, args, raw) =>
           {
               var model = "WEAPON_MICROSMG";
               if(args.Count > 0)
               {
                   model = args[0].ToString();
               }

               var hash = (uint)GetHashKey(model);
               //var hash = (uint)WeaponHash.MicroSMG;
               if(!IsWeaponValid(hash))
               {
                   TriggerEvent("chat:addMessage", new
                   {
                       color = new[] { 255, 0, 0 },
                       args = new[] { "[WeaponSpawner]", $"This is a invalid weapon, bro!" }
                   });
                   return;
               }

               //Give the player the weapon
               GiveWeaponToPed( PlayerPedId(), hash, 20, false, true);

               TriggerEvent("chat:addMessage", new
               {
                   color = new[] { 255, 0, 0 },
                   args = new[] { "[CarSpawner]", $"Enjoy your new weapon! :)" }
               });
           }),false);

            //Spawn car and enter the vehicle
            RegisterCommand("car", new Action<int, List<object>, string>( async(source, args, raw) =>
            {
                var model = "adder";
                if(args.Count > 0)
                {
                    model = args[0].ToString();
                }

                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {

                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[CarSpawner]", $"I wish I could spawn this {(args.Count > 0 ? $"{args[0]} or" : "")} adder but my owner was too lazy. :(" }
                    });
                    return;
                }


                // create the vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                //Set the player ped into the vehicle and driver seat
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                //Tell the player

                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner", $"Woohoo! Enjoy your new ^*{model}!" }
                });

            }), false);
        }
    }
}
