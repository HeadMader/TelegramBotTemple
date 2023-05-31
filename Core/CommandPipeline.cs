using EasyTelegramBot.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core
{
	public class CommandPipeline
	{
		private List<Middleware> middlewares = new();
		private readonly Action<CommandPipeline> startMethod;
		private Middleware firstMiddleware;
		public UpdateType UpdateType { get; private set; }

		public CommandPipeline(Action<CommandPipeline> method, UpdateType updateType)
		{
			startMethod = method;
			this.UpdateType = updateType;
		}

		//Start pipeline
		public async Task<Middleware> Start(Update update)
		{
			return await firstMiddleware.InvokeAsync(update);
		}

		// Start method that contains all middlewires
		// On invoke it adds middlewares to pipeline
		public void Build()
		{
			startMethod.Invoke(this);
			firstMiddleware = BuildPipeline();
		}

		//Add middleware
		public void AddMiddleware(Middleware commandDelegate)
		{
			middlewares.Add(commandDelegate);
		}

		// Build pipeline with middlewares
		public Middleware BuildPipeline()
		{
			Middleware middleware = new();
			middleware.Command = update => Task.CompletedTask;
			for (var i = middlewares.Count - 1; i >= 0; i--)
			{
				middlewares[i].Next = middleware;
				middleware = middlewares[i];
			}
			return middleware;
		}

	}
}


