// Project:         Vibrant Wind for Daggerfall Unity
// Web Site:        http://forums.dfworkshop.net/viewtopic.php?f=14&t=532
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/TheLacus/vibrantwind-du-mod
// Original Author: TheLacus
// Contributors:    

using System;
using UnityEngine;
using Wenzil.Console;
using DaggerfallWorkshop.Game.Weather;

namespace VibrantWind
{
    /// <summary>
    /// Console commands for Vibrant Wind.
    /// </summary>
    public static class VibrantWindConsoleCommands
    {
        const string noInstanceMessage = "Vibrant Wind instance not found.";

        public static void RegisterCommands()
        {
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(ToggleVibrantWind.name, ToggleVibrantWind.description, ToggleVibrantWind.usage, ToggleVibrantWind.Execute);
                ConsoleCommandsDatabase.RegisterCommand(VibrantWindDebug.name, VibrantWindDebug.description, VibrantWindDebug.usage, VibrantWindDebug.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering Vibrant Wind Console commands: {0}", e.Message));
            }
        }

        private static class ToggleVibrantWind
        {
            public static readonly string name = "vwind_toggle";
            public static readonly string description = "Enable/Disable automatic wind strength changes.";
            public static readonly string usage = "vwind_toggle";

            public static string Execute(params string[] _)
            {
                var vibrantWind = VibrantWind.Instance;
                if (vibrantWind == null)
                    return noInstanceMessage;

                vibrantWind.ToggleMod(true);
                return vibrantWind.ToString();
            }
        }

        private static class VibrantWindDebug
        {
            public static readonly string name = "vwind_debug";
            public static readonly string description = "Debug tools.";
            public static readonly string usage = Usage();

            public static string Execute(params string[] args)
            {
                var vibrantWind = VibrantWind.Instance;
                if (vibrantWind == null)
                    return noInstanceMessage;

                if (args.Length < 1 || !int.TryParse(args[0], out int mode))
                    return usage;

                switch (mode)
                {
                    case 0:
                        return string.Format("Terrain: {0}\nAmbient: {1}", vibrantWind.TerrainWindStrength, vibrantWind.AmbientWindStrength);

                    case 1:
                        try
                        {
                            vibrantWind.ForceWeather((WeatherType)int.Parse(args[1]));
                            return "weather set.";
                        }
                        catch
                        {
                            return usage;
                        }

                    case 2:
                        vibrantWind.StartTestWeathers();
                        return "Close console to start test.";
                }

                return usage;
            }

            private static string Usage()
            {
                string usage = "vwind_debug {mode}";
                usage += "\n0: Get current wind strength";
                usage += "\n1: Set weather for wind strength.";
                usage += "\n2: Test all weathers in succession.";
                return usage;
            }
        }
    }
}
