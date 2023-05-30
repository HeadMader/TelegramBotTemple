# EasyTelegramBot

## Simple usage example
  
```c#
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

static string BotToken = "YourBotToken";
public static ITelegramBotClient Bot = new TelegramBotClient(BotToken);

BotBuilder botBuilder = new BotBuilder(Bot);

botBuilder.AddRoute("/start", UpdateType.Message, () =>
{
	botBuilder.End(async (update) =>
	{
		await Bot.SendTextMessageAsync(update.Message.Chat.Id,"Hello You");
	});
});
      
botBuilder.Start();
```
