using Core;
using EasyTelegramBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotHelper.Core
{
	public static class PipelineExtantions
	{
		//Add middleware
		public static CommandPipeline Use(this CommandPipeline pipeline, Func<Update, Task> command)
		{
			Middleware middleware = new();
			middleware.Command = update => command(update);
			pipeline.AddMiddleware(middleware);
			return pipeline;
		}
	}
}
