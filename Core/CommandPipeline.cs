using Telegram.Bot.Types.Enums;

namespace Core
{
    internal class CommandPipeline
    {
        private List<Func<CommandDelegate, CommandDelegate>> middlewares = new();
        private readonly Action startMeethod;
        public UpdateType UpdateType { get; private set; }

        internal CommandPipeline(Action method, UpdateType updateType)
        {
            startMeethod = method;
            this.UpdateType = updateType;
        }

        public void Start()
        {
            startMeethod.Invoke();
        }

        public void AddMiddleware(Func<CommandDelegate, CommandDelegate> commandDelegate)
        {
            middlewares.Add(commandDelegate);
        }

        /// <summary>
        /// Builds pipeline
        /// </summary>
        /// <returns></returns>
        public CommandDelegate Build()
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


