﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace NTOJSpaceDiscordBot.Services
{
    /// <summary>
    /// Сервис для логгирования события Дискорда.
    /// </summary>
    public class LoggingService
    {
        private DiscordSocketClient _client;

        /// <summary>
        /// Настраиваем логирования клиента и команд.
        /// </summary>
        /// <param name="client">Клиент Дискорда.</param>
        /// <param name="commandService">Сервис команд Дискордаю</param>
        public LoggingService(DiscordSocketClient client, CommandService commandService)
        {
            _client = client;

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += Client_MessageReceived;

            commandService.Log += LogAsync;
        }

        /// <summary>
        /// Инициализация логгера.
        /// </summary>
        /// <returns>Завершенная асинхронная операция.</returns>
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Логгировать сообщение от пользователя Дискорда.
        /// </summary>
        /// <param name="arg">Аргументы сообщения.</param>
        /// <returns>Завершенная асинхронная операция.</returns>
        private Task Client_MessageReceived(SocketMessage arg)
        {
            // Игнорируем сообщения от самого бота
            if (arg.Author.Id == _client.CurrentUser.Id)
                return Task.CompletedTask;

            Console.WriteLine($"{arg.Author}, {arg.CreatedAt}, {arg.Channel}, {arg.Content}");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Логгировать событие Дискорда.
        /// </summary>
        /// <param name="message">Объект сообщения Дискорда.</param>
        /// <returns>Завершенная асинхронная операция.</returns>
        private Task LogAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine(message.ToString());
            
            // Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");

            Console.ResetColor();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The Ready event indicates that the client has opened a
        /// connection and it is now safe to access the cache.
        /// </summary>
        /// <returns>Завершенная асинхронная операция.</returns>
        private Task ReadyAsync()
        {
            Console.WriteLine("The Bot is connected!");

            return Task.CompletedTask;
        }
    }
}
