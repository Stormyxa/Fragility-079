using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Fragility
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ShowPositionCommand : ICommand
    {
        public string Command => "fgshowpos";
        public string[] Aliases => new[] { "fgsw" };
        public string Description => "Shows the coordinates of the player and SCP-079's active camera.";
        public string Permission { get; } = "Fragility079";

        private static readonly Dictionary<Player, CoroutineHandle> TrackedPlayers = new Dictionary<Player, CoroutineHandle>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Usage: fgshowpos <player's name or ID>";
                return false;
            }

            if (!Player.TryGet(arguments.At(0), out Player target))
            {
                response = $"Player '{arguments.At(0)}' not found.";
                return false;
            }

            if (TrackedPlayers.TryGetValue(target, out CoroutineHandle handle))
            {
                Timing.KillCoroutines(handle);
                TrackedPlayers.Remove(target);
                target.ClearBroadcasts();
                response = $"Coordinate display for player {target.Nickname} has been disabled.";
                return true;
            }

            CoroutineHandle newHandle = Timing.RunCoroutine(DisplayHintCoroutine(target));
            TrackedPlayers.Add(target, newHandle);
            response = $"Now displaying coordinates for player {target.Nickname}.";
            return true;
        }

        private IEnumerator<float> DisplayHintCoroutine(Player target)
        {
            try
            {
                while (target != null && target.IsConnected)
                {
                    StringBuilder hintText = new StringBuilder();
                    Vector3 playerPos = target.Position;
                    hintText.AppendLine($"Player: X: {playerPos.x:F2} Y: {playerPos.y:F2} Z: {playerPos.z:F2}");

                    if (Plugin.Scp079Player != null && Plugin.Scp079Player.Role is Scp079Role scp079Role)
                    {
                        var activeCamera = scp079Role.Camera;
                        if (activeCamera != null)
                        {
                            Vector3 cameraPos = activeCamera.Position;
                            hintText.AppendLine($"Camera ({activeCamera.Id}): X: {cameraPos.x:F2} Y: {cameraPos.y:F2} Z: {cameraPos.z:F2}");
                            float distance = Vector3.Distance(playerPos, cameraPos);
                            hintText.AppendLine($"Distance: {distance:F1}m");
                        }
                        else { hintText.AppendLine("Active Camera: Not Found (Camera is null)"); }
                    }
                    else { hintText.AppendLine("Active Camera: Not Found (No SCP-079)"); }

                    target.ShowHint(hintText.ToString(), 1.1f);
                    yield return Timing.WaitForSeconds(1f);
                }
            }
            finally
            {
                if (TrackedPlayers.ContainsKey(target))
                    TrackedPlayers.Remove(target);
            }
        }
    }
}