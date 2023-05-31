using Core;
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

		// Add middleware
		public static CommandPipeline Use(this CommandPipeline pipeline, Func<Update, Func<Task>, Task> middleware)
		{
			pipeline.AddMiddleware(next =>
			{
				return update =>
				{
					Func<Task> simpleNext = () => next(update);
					return middleware(update, simpleNext);
				};
			});
			return pipeline;
		}

		// Add middleware
		public static CommandPipeline Use(this CommandPipeline pipeline, Func<Update, CommandDelegate, Task> command)
		{
			pipeline.AddMiddleware(next => update => command(update, next));
			return pipeline;
		}

		// Add End middleware
		public static CommandPipeline End(this CommandPipeline pipeline, Func<Update, Task> command)
		{
			pipeline.AddMiddleware(next => update => command(update));
			return pipeline;
		}
	}
}
