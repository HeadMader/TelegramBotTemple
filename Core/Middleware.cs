using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace EasyTelegramBot.Core
{
	public class Middleware
	{
		public Middleware Next = null;

		public CommandDelegate CommandHandler;

		public Middleware Invoke(Update update)
		{
			CommandHandler.Invoke(update);
			return Next;
		}
		public async Task<Middleware> InvokeAsync(Update update)
		{
			await CommandHandler.Invoke(update);
			return Next;
		}

	}
}
