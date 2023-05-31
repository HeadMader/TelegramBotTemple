using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Core
{
    public class CommandPipeline
    {
        private List<Func<CommandDelegate, CommandDelegate>> middlewares = new();
        private readonly Action<CommandPipeline> startMethod;
        private CommandDelegate firstMiddleware;
        public UpdateType UpdateType { get; private set; }

        public CommandPipeline(Action<CommandPipeline> method, UpdateType updateType)
        {
            startMethod = method;
            this.UpdateType = updateType;
        }

        //Start pipeline
        public async Task Start(Update update)
        {
			await firstMiddleware.Invoke(update);
		}

        // Start method that contains all middlewires
        // On invoke it adds middlewares to pipeline
        public void Build()
        {
			startMethod.Invoke(this);
			firstMiddleware = BuildPipeline();
		}
        
        //Add middleware
        public void AddMiddleware(Func<CommandDelegate, CommandDelegate> commandDelegate)
        {
            middlewares.Add(commandDelegate);
        }

        // Build pipeline with middlewares
        public CommandDelegate BuildPipeline()
        {
			CommandDelegate middleware = update => { return Task.CompletedTask; };

            for (var i = middlewares.Count - 1; i >= 0; i--)
            {
                middleware = middlewares[i](middleware);
            }
            return middleware;
        }

    }
}


