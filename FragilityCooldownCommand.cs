using CommandSystem;
using System;

namespace Fragility
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class FragilityCooldownCommand : ICommand
    {
        public string Command => "fragilitycooldown";
        public string[] Aliases => new[] { "fgcld" };
        public string Description => "Sets the camera cooldown time in seconds.";
        public string Permission { get; } = "Fragility079";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Usage: fgcld <seconds | perm | 0>\n- <seconds>: Cooldown in seconds (e.g., 15).\n- <perm>: Sets a permanent lock for the round.\n- <0>: Disables the cooldown entirely.";
                return false;
            }

            string input = arguments.At(0).ToLower();

            // Handle permanent lock
            if (input == "perm")
            {
                Plugin.Instance.Config.CooldownDuration = int.MaxValue;
                response = "Camera cooldown has been set to permanent.";
                return true;
            }

            // Handle numeric value (including 0)
            if (int.TryParse(input, out int seconds))
            {
                if (seconds < 0)
                {
                    response = "Error: Cooldown cannot be a negative number.";
                    return false;
                }

                Plugin.Instance.Config.CooldownDuration = seconds;

                if (seconds == 0)
                {
                    response = "Camera cooldown has been disabled (0 seconds).";
                }
                else
                {
                    response = $"Camera cooldown has been set to {seconds} seconds.";
                }
                return true;
            }

            response = $"Error: '{input}' is not a valid number or the 'perm' keyword.";
            return false;
        }
    }
}