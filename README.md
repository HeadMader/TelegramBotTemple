# EasyTelegramBot
## Description
Helps to easily create conversation flow, and step by step conversation 

# .NET CLI
```
dotnet add package Telegram.Bot --version 19.0.0
```
# Package Manager
```
NuGet\Install-Package Telegram.Bot -Version 19.0.0
```

## Simple usage example
  
```c#
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

static string BotToken = "YourBotToken";
public static ITelegramBotClient Bot = new TelegramBotClient(BotToken);

BotBuilder botBuilder = new BotBuilder(Bot);

botBuilder.AddRoute("/start", UpdateType.Message, (pipeline) =>
{
	pipeline.Use(async (update) =>
	{
		await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Write Your Name");
	});
		pipeline.Use(async (update) =>
	{
		string name = update?.Message?.Text;
		await Bot.SendTextMessageAsync(update.Message.Chat.Id, $"Hello {name}");
	});
});

botBuilder.Start();
```
