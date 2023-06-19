using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core
{
	public delegate Task CommandDelegate(Update update);

	public class BotBuilder
	{
		private RoutesManager routesManager = new();
		private ITelegramBotClient botClient;

		public BotBuilder(ITelegramBotClient bot)
		{
			botClient = bot;
		}

		/// <summary>
		/// Start bot
		/// </summary>
		public void Start()
		{
			UpdateHandler updateHandler = new(routesManager);

			var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = { },
			};

			botClient.StartReceiving(
			updateHandler.HandleUpdateAsync,
			updateHandler.HandleErrorAsync,
			receiverOptions,
			cancellationToken);
		}

		// Create new command pipeline
		public void AddRoute(string route, UpdateType updateType, Action<CommandPipeline> commandDelegate)
		{
			var commandPipeline = new CommandPipeline(commandDelegate, updateType);
			commandPipeline.Build();
			routesManager.AddPipeline(route, commandPipeline);
		}
	}
}
