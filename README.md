# EasyTelegramBot

## Simple usage example
  
```c#
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

static string BotToken = "YourBotToken";
public static ITelegramBotClient Bot = new TelegramBotClient(BotToken);

BotBuilder botBuilder = new BotBuilder(Bot);

string name = "";
botBuilder.AddRoute("/start", UpdateType.Message, (pipeline) =>
{
	pipeline.Use(async (update) =>
	{
		await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Write Your Name");
	});
	pipeline.Use(async (update) =>
	{
		await Bot.SendTextMessageAsync(update.Message.Chat.Id, $"Hello {name}");
	});
});

botBuilder.Start();
```
