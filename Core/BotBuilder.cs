using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core
{
	public delegate Task CommandDelegate(Update update);

	public class BotBuilder
	{
		private RoutesManager routesManager;
		private UpdateHandler updateHandler;
		private ITelegramBotClient botClient;

		public BotBuilder(ITelegramBotClient bot) {
			routesManager = new RoutesManager();
			updateHandler = new UpdateHandler(routesManager);
			botClient = bot;
		}

		/// <summary>
		/// Starts bot
		/// </summary>
		public void Start()
		{
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

		/// <summary>
		/// Create new command ppieline
		/// </summary>
		/// <param name="route"></param>
		/// <param name="updateType"></param>
		/// <param name="commandDelegate"></param>
		public void AddRoute(string route, UpdateType updateType, Action commandDelegate)
		{
			routesManager.AddRoute(route, updateType, commandDelegate);
		}

		/// <summary>
		/// Add middleware
		/// </summary>
		/// <param name="middleware"></param>
		/// <returns></returns>
		public BotBuilder Use(Func<Update, Func<Task>, Task> middleware)
		{
			routesManager.GetLastRoute().AddMiddleware(next =>
			{
				return update =>
				{
					Func<Task> simpleNext = () => next(update);
					return middleware(update, simpleNext);
				};
			});
			return this;
		}

		/// <summary>
		/// Add middleware
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public BotBuilder Use(Func<Update, CommandDelegate, Task> command)
		{
			routesManager.GetLastRoute().AddMiddleware(next => update => command(update, next));
			return this;
		}

		/// <summary>
		/// Add last middleware
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public BotBuilder End(Func<Update, Task> command)
		{
			routesManager.GetLastRoute().AddMiddleware(next => update => command(update));
			return this;
		}
	}
}
