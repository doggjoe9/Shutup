using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Shutup {
	internal class Services {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		#region Injected Services
		[PluginService] internal static CommandManager CommandManager { get; private set; }
		[PluginService] internal static ChatGui ChatGui { get; private set; }
		#endregion

		#region Dalamud Services
		internal static DalamudPluginInterface PluginInterface { get; private set; }
		#endregion

		#region DalamudPluginTemplate1 Services
		internal static Configuration Configuration { get; private set; }
		#endregion
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		internal static void Initialize(DalamudPluginInterface pluginInterface) {
			pluginInterface.Create<Services>();
			PluginInterface = pluginInterface;

			Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
		}
	}
}
