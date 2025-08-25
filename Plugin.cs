using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Scp079 = Exiled.Events.Handlers.Scp079;

namespace Fragility
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }
        public override string Name => "Fragility";
        public override string Author => "kitten";
        public override Version Version => new Version(1, 0, 0); // The Furry Edition

        private readonly Dictionary<ushort, DateTime> _disabledCameraTimestamps = new Dictionary<ushort, DateTime>();
        public static Exiled.API.Features.Player Scp079Player { get; private set; }

        public override void OnEnabled() { Instance = this; RegisterEvents(); base.OnEnabled(); }
        public override void OnDisabled() { UnregisterEvents(); Cleanup(); base.OnDisabled(); }

        private void RegisterEvents()
        {
            Player.ChangingRole += OnChangingRole;
            Scp079.ChangingCamera += OnChangingCamera;
            Player.Shooting += OnShooting;
            Server.RoundEnded += OnRoundEnded;
        }

        private void UnregisterEvents()
        {
            Player.ChangingRole -= OnChangingRole;
            Scp079.ChangingCamera -= OnChangingCamera;
            Player.Shooting -= OnShooting;
            Server.RoundEnded -= OnRoundEnded;
        }

        private void OnChangingCamera(ChangingCameraEventArgs ev)
        {
            if (_disabledCameraTimestamps.TryGetValue(ev.Camera.Id, out DateTime disabledAt))
            {
                if (Instance.Config.CooldownDuration == int.MaxValue)
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHint("This camera is permanently disabled.", 2);
                    return;
                }

                TimeSpan cooldownDuration = TimeSpan.FromSeconds(Instance.Config.CooldownDuration);
                if (DateTime.UtcNow < disabledAt + cooldownDuration)
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHint("This camera is temporarily disabled.", 2);
                }
                else
                {
                    _disabledCameraTimestamps.Remove(ev.Camera.Id);
                }
            }
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == Scp079Player && ev.NewRole != RoleTypeId.Scp079) Scp079Player = null;
            if (ev.NewRole == RoleTypeId.Scp079) Scp079Player = ev.Player;
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (!Config.IsEnabled || Scp079Player == null) return;

            if (Physics.Raycast(ev.Player.CameraTransform.position, ev.Player.CameraTransform.forward, out RaycastHit hit, 100f))
            {
                var baseHitCamera = hit.collider.GetComponentInParent<Scp079Camera>();
                if (baseHitCamera == null) return;

                var hitCamera = Exiled.API.Features.Camera.Get(baseHitCamera);
                var scp079Role = Scp079Player.Role.As<Exiled.API.Features.Roles.Scp079Role>();
                var currentCamera = scp079Role.Camera;

                if (hitCamera != null && hitCamera.Id == currentCamera.Id)
                {
                    var fallbackCamera = FindBestFallbackCamera(scp079Role);
                    if (fallbackCamera != null)
                    {
                        scp079Role.Camera = fallbackCamera;

                        if (Instance.Config.CooldownDuration > 0)
                        {
                            _disabledCameraTimestamps[currentCamera.Id] = DateTime.UtcNow;
                        }

                        ev.Player.ShowHitMarker(1f);
                    }
                }
            }
        }

        private Exiled.API.Features.Camera FindBestFallbackCamera(Exiled.API.Features.Roles.Scp079Role scp079)
        {
            var currentCamera = scp079.Camera;

            var sameZoneCamera = Exiled.API.Features.Camera.List
                .Where(cam => cam.Zone == currentCamera.Zone && cam.Id != currentCamera.Id && !_disabledCameraTimestamps.ContainsKey(cam.Id))
                .OrderBy(cam => scp079.GetSwitchCost(cam))
                .FirstOrDefault();

            if (sameZoneCamera != null) return sameZoneCamera;

            List<ZoneType> fallbackZones = new List<ZoneType>();
            switch (currentCamera.Zone)
            {
                case ZoneType.Surface: fallbackZones.Add(ZoneType.Entrance); break;
                case ZoneType.Entrance: fallbackZones.Add(ZoneType.HeavyContainment); fallbackZones.Add(ZoneType.Surface); break;
                case ZoneType.HeavyContainment: fallbackZones.Add(ZoneType.Entrance); fallbackZones.Add(ZoneType.LightContainment); break;
                case ZoneType.LightContainment: fallbackZones.Add(ZoneType.HeavyContainment); break;
            }

            foreach (var zone in fallbackZones)
            {
                var crossZoneCamera = Exiled.API.Features.Camera.List
                    .Where(cam => cam.Zone == zone && !_disabledCameraTimestamps.ContainsKey(cam.Id))
                    .OrderBy(cam => scp079.GetSwitchCost(cam))
                    .FirstOrDefault();

                if (crossZoneCamera != null) return crossZoneCamera;
            }
            return null;
        }

        private void OnRoundEnded(RoundEndedEventArgs ev) => Cleanup();
        private void Cleanup() { _disabledCameraTimestamps.Clear(); Scp079Player = null; }
    }
}