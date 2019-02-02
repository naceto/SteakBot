﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SteakBot.Core.EventHandlers.Abstraction;
using SteakBot.Core.EventHandlers.CustomMessageHandlers;

namespace SteakBot.Core.EventHandlers
{
	internal class MessageEventHandler : IMessageEventHandler
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IEnumerable<ICustomMessageHandler> _customMessageHandlers;

		internal MessageEventHandler(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_customMessageHandlers = LoadMessageHandlers();
		}

		public async Task HandleMessageReceivedAsync(SocketMessage messageParam)
		{
			// Don't process the command if it was a System Message, the sender is a bot or this is not a command.
			if (!(messageParam is SocketUserMessage message) || message.Source == MessageSource.Bot)
			{
				return;
			}

			Parallel.ForEach(_customMessageHandlers, customMessageHandler =>
			{
				if (customMessageHandler.CanHandle(message))
				{
					customMessageHandler.Invoke(message);
				}
			});
		}

		#region Private methods

		private IEnumerable<ICustomMessageHandler> LoadMessageHandlers()
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(x => x.GetTypes().Where(y => !y.IsAbstract && y.GetInterfaces().Contains(typeof(ICustomMessageHandler))))
				.Select(x => (ICustomMessageHandler)Activator.CreateInstance(x, _serviceProvider));
		}

		#endregion
	}
}
