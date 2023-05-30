using Core;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotHelper
{
	internal class Program
	{
		static string BotToken = "Your Bot Token";

		public static ITelegramBotClient Bot = new TelegramBotClient(BotToken);

		static void Main(string[] args)
		{
			BotBuilder botBuilder = new BotBuilder(Bot);

			botBuilder.AddRoute("/start", UpdateType.Message, () =>
			{
				botBuilder.End(async (update) =>
				{
					await Bot.SendTextMessageAsync(update.Message.Chat.Id,"Hello You");
				});
			});

			botBuilder.Start();
			Console.ReadLine();
		}
	}
}