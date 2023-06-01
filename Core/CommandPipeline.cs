using EasyTelegramBot.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core
{
	public class CommandPipeline
	{
		
		private LinkedList<Middleware> middlewares = new();
		private readonly Action<CommandPipeline> startMethod;
		public UpdateType UpdateType { get; private set; }

		public CommandPipeline(Action<CommandPipeline> method, UpdateType updateType)
		{
			startMethod = method;
			this.UpdateType = updateType;
		}

		//Start pipeline
		public async Task<Middleware> Start(Update update)
		{
			return await middlewares.First().InvokeAsync(update);
		}

		// Start method that contains all middlewires
		// On invoke it adds middlewares to pipeline
		public void Build()
		{
			startMethod.Invoke(this);
			BuildPipeline();
		}

		//Add middleware
		public void AddMiddleware(Middleware commandDelegate)
		{
			middlewares.AddLast(commandDelegate);
		}

		// Build pipeline with middlewares
		public Middleware? BuildPipeline()
		{
			Middleware? middleware = null;
			//middleware.CommandHandler = update => Task.CompletedTask;
			for (var i = middlewares.Count - 1; i >= 0; i--)
			{
				var ware = middlewares.ElementAt(i);
				ware.Next = middleware;
				middleware = ware;
			}
			return middleware;
		}

	}
}


