using CommandSystem;
using System;
using System.Text;

namespace Fragility
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class HelpCommand : ICommand
    {
        public string Command => "fglist";
        public string[] Aliases => new[] { "fghelp" };
        public string Description => "Displays a list of all Fragility plugin commands.";
        public string Permission { get; } = "Fragility079";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Fragility Plugin Commands:");
            sb.AppendLine("- fragility (fgl) <true/false>: Enables or disables the plugin mechanics.");
            sb.AppendLine("- fgshowpos (fgsw) <player>: Displays live coordinates of a player and the active SCP-079 camera.");
            sb.AppendLine("- fgcooldown (fgcld) <seconds|perm>: Sets the camera disable cooldown for the current round.");

            response = sb.ToString();
            return true;
        }
    }
}