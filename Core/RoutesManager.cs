using Telegram.Bot.Types.Enums;

namespace Core
{
    internal class RoutesManager
    {
        private Dictionary<string, CommandPipeline> RouteCommand = new();

        /// <summary>
        /// Add route
        /// </summary>
        /// <param name="route"></param>
        /// <param name="updateType"></param>
        /// <param name="commandDelegate"></param>
        public void AddRoute(string route, UpdateType updateType, Action commandDelegate)
        {
            var commandPipeline = new CommandPipeline(commandDelegate, updateType);
            RouteCommand.Add(route, commandPipeline);
            commandPipeline.Start();
        }

        /// <summary>
        /// Get specific route by path
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public CommandPipeline GetRoute(string route)
        {
            if (RouteCommand.ContainsKey(route))
                return RouteCommand[route];
            else
                return null;
        }

        /// <summary>
        /// Get all routes
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, CommandPipeline> GetRoutes()
        {
            return RouteCommand;
        }
        /// <summary>
        /// Get last added route
        /// </summary>
        /// <returns></returns>
        public CommandPipeline GetLastRoute()
        {
            return RouteCommand.Last().Value;
        }

    }
}
