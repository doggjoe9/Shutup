using Dalamud.ContextMenu;
using Dalamud.Game.Command;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using Dalamud.Plugin;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Shutup {
	public sealed class Shutup : IDalamudPlugin {
		#region Fields
		public string Name => "Auctioneer XIV";
		private const string CommandName = "/shutup";

		private bool enabled = true;

		private DalamudContextMenu contextMenu;
		private GameObjectContextMenuItem playerShutupContextMenuItem;
		private GameObjectContextMenuItem playerTalkContextMenuItem;
		#endregion

		public Shutup(DalamudPluginInterface pluginInterface) {
			#region Service Init
			Services.Initialize(pluginInterface);
			#endregion
			#region Command Init
			Services.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand) {
				// TODO Put some helpful message here.
				HelpMessage = "Opens the main interface for Shutup"
			});
			#endregion
			contextMenu = new DalamudContextMenu();
			playerShutupContextMenuItem = new GameObjectContextMenuItem("Shutup", OnGameObjectShutup, true);
			playerTalkContextMenuItem = new GameObjectContextMenuItem("Talk Now", OnGameObjectTalk, true);
			contextMenu.OnOpenGameObjectContextMenu += OnOpenContextMenuOverride;
			Services.ChatGui.CheckMessageHandled += OnChat;
		}

		public void Dispose() {
			contextMenu.OnOpenGameObjectContextMenu -= OnOpenContextMenuOverride;
			Services.ChatGui.CheckMessageHandled -= OnChat;
			contextMenu.Dispose();
		}


		private List<string> shutupNames = new List<string>();

		private void OnOpenContextMenuOverride(GameObjectContextMenuOpenArgs args) {
			// filter anything that isn't a player
			if (args.ObjectWorld == 0)
				return;

			string name = args.Text.TextValue;
			PluginLog.Debug($"Context Menu object name: {name}");

			if (shutupNames.Contains(name)) {
				args.AddCustomItem(playerTalkContextMenuItem);
			} else {
				args.AddCustomItem(playerShutupContextMenuItem);
			}
		}

		private void OnGameObjectShutup(GameObjectContextMenuItemSelectedArgs args) {
			shutupNames.Add(args.Text.TextValue);
		}

		private void OnGameObjectTalk(GameObjectContextMenuItemSelectedArgs args) {
			shutupNames.Remove(args.Text.TextValue);
		}

		private void OnChat(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled) {
			if (enabled) {
				string senderName = sender.TextValue;
				if (!char.IsLetter(senderName[0]))
					senderName = senderName.Substring(1);
				isHandled = shutupNames.Contains(senderName);
			}
		}

		#region Command
		/// <summary>
		/// Executes when the player enters the command.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="args"></param>
		private void OnCommand(string command, string args) {
			if (command == CommandName) {
				enabled = !enabled;
				string enabledMessage = enabled ? "Shutup is now enabled." : "Shutup is now disabled.";
				Services.ChatGui.Print(enabledMessage);
			}
		}
		#endregion
	}
}
