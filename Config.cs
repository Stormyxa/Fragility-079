using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Fragility
{
    public class Config : IConfig
    {
        [Description("Whether this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages should be shown in the console.")]
        public bool Debug { get; set; } = false;

        [Description("How long will it take for the camera to recover after being hit?")]
        public int CooldownDuration { get; set; } = 10;
    }
}