using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core
{
	internal class UpdateHandler
	{
		private Dictionary<long, CommandDelegate> userActions = new();

		private RoutesManager routesManager;

		internal UpdateHandler(RoutesManager routesManager)
		{
			this.routesManager = routesManager;
		}

		public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

			Message? message = update.Message;

			var chatId = GetChatId(update);

			CommandPipeline? route = null;

			string[]? complexCallback = null;

			// Get path
			if (update.Type == UpdateType.Message)
			{
				complexCallback = message?.Text?.Split(" ")!;
			}
			else if (update.Type == UpdateType.CallbackQuery)
			{
				CallbackQuery callbackQuery = update.CallbackQuery!;
				complexCallback = callbackQuery?.Data?.Split('/')!;
			}

			//Get route by path
			if (complexCallback != null)
				route = routesManager.GetRoute(complexCallback[0]);

			//if we entered into new route
			if (route != null)
			{
				if (route.UpdateType != update.Type)
				{
                    await Console.Out.WriteLineAsync($"Pipeline by path = {complexCallback[0]} has not vaild UpdateType");
                }
				await route.Start(update);

			}
			//continue current messages pipeline
			else
			{
				if (userActions.Keys.Contains(chatId))
				{
					await userActions[chatId].Invoke(update);
				}
			}
		}
		public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			await Console.Out.WriteLineAsync(exception.Message + "\n" + exception.StackTrace);
		}

		public long GetChatId(Update update)
		{
			long chatId = 0;

			if (update?.Message?.Chat.Id != null)
				return update.Message.Chat.Id;
			else if (update?.CallbackQuery?.Message?.Chat.Id != null)
				return update.CallbackQuery.Message.Chat.Id;
			else return chatId;
		}
	}
}
