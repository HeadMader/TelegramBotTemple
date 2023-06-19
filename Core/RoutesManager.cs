using Telegram.Bot.Types.Enums;

namespace Core
{
	internal class RoutesManager
	{
		private Dictionary<string, CommandPipeline> RouteCommand = new();

		//Add Route
		public void AddPipeline(string route, CommandPipeline commandPipeline)
		{
			RouteCommand.Add(route, commandPipeline);
		}

		// Get specific route by path
		public CommandPipeline GetRoute(string route)
		{
			if (RouteCommand.ContainsKey(route))
				return RouteCommand[route];
			else
				return null;
		}
	}
}
