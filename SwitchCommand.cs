using CommandSystem;
using System;

namespace Fragility
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SwitchCommand : ICommand
    {
        public string Command => "fragility";
        public string[] Aliases => new[] { "fgl" };
        public string Description => "Enables or disables the fragile camera mechanics for SCP-079.";
        public string Permission { get; } = "Fragility079";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Usage: fragility <true/false>";
                return false;
            }

            if (bool.TryParse(arguments.At(0), out bool result))
            {
                Plugin.Instance.Config.IsEnabled = result;
                response = $"Fragile camera mechanics are now {(result ? "enabled" : "disabled")}.";
                return true;
            }

            response = $"Invalid value: {arguments.At(0)}. Use true or false.";
            return false;
        }
    }
}