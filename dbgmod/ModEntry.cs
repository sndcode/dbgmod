using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//
using StardewValley.Menus;
using System.Reflection;

namespace dbgmod
{

    class ModConfig
    {
        public   bool skipintro { get; set; } = true;
        public   bool playsound { get; set; } = true;
    }

    public class ModEntry : Mod
    {
        internal static ModConfig Config;
        public bool isdebugmodeon;
        public string modversion = "0.0.1";

        public void worker()
        {
            ControlEvents.KeyPressed += this.ReceiveKeyPress;
        }

        private void skipIntro()
        {
            MenuEvents.MenuChanged += (sender, e) =>
            {
                try
                {   
                    //Credits : Pathoschild
                    //https://github.com/Pathoschild/StardewValley.SkipIntro
                    TitleMenu menu = e.NewMenu as TitleMenu;
                    if (menu == null)
                        return;
                    menu.skipToTitleButtons();
                    FieldInfo logoTimer = menu.GetType().GetField("chuckleFishTimer", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (logoTimer == null)
                        throw new InvalidOperationException("The 'chuckleFishTimer' field doesn't exist.");
                    logoTimer.SetValue(menu, 0);
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"Couldn't skip the menu: {ex}", LogLevel.Error);
                }
            };
        }

        private void HandleDebugHelp(object sender, EventArgsCommand e)
        {
            this.Monitor.Log("=========================================", LogLevel.Info);
            this.Monitor.Log("How to use the dbgmod version " + modversion , LogLevel.Info);
            this.Monitor.Log("Get ingame and press DELETE key once. Then you can use the debug hotkeys listed below. ", LogLevel.Info);
            this.Monitor.Log("=========================================", LogLevel.Info);
            this.Monitor.Log("DBGMOD by sandaasu - hotkey list - ", LogLevel.Info);
            this.Monitor.Log("=========================================", LogLevel.Info);
            this.Monitor.Log("T adds one hour to the clock.", LogLevel.Info);
            this.Monitor.Log("SHIFT + T subtract 10 minutes from the clock.", LogLevel.Info);
            this.Monitor.Log("1 warps the mountain(facing Robins house).", LogLevel.Info);
            this.Monitor.Log("2 warps the town(on the path between the town and community center).", LogLevel.Info);
            this.Monitor.Log("3 warps the farm(at your farmhouse door).", LogLevel.Info);
            this.Monitor.Log("4 warps the forest(near the traveling cart).", LogLevel.Info);
            this.Monitor.Log("5 warps the beach(left of Elliotts house).", LogLevel.Info);
            this.Monitor.Log("6 warps the mine(at the inside entrance).", LogLevel.Info);
            this.Monitor.Log("7 warps the desert(in Sandys shop).", LogLevel.Info);
            this.Monitor.Log("K moves down one mine level.If not currently in the mine, warp to it.", LogLevel.Info);
            this.Monitor.Log("F5 toggles the player.", LogLevel.Info);
            this.Monitor.Log("F7 draws a tile grid.", LogLevel.Info);
            this.Monitor.Log("B shifts the toolbar to show the next higher inventory row.", LogLevel.Info);
            this.Monitor.Log("N shifts the toolbar to show the next lower inventory row.", LogLevel.Info);
            this.Monitor.Log("=========================================", LogLevel.Info);
        }

        private void ReceiveKeyPress(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.Delete)
            {
                Game1.debugMode = !Game1.debugMode;
                if (Config.playsound) { Game1.playSound("achievement"); }
                this.Monitor.Log("Debug mode was toggled :)", LogLevel.Info);
            }
        }

        public override void Entry(IModHelper helper)
        {
            Command.RegisterCommand("help_dbgmod", "Shows debugmode hotkeys infos | debughelp").CommandFired += this.HandleDebugHelp;
            Config = helper.ReadConfig<ModConfig>();
            if (Config.skipintro) { skipIntro(); }
            worker();
        }
    }
}
