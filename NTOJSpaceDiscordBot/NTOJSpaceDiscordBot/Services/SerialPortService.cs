﻿using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace NTOJSpaceDiscordBot.Services
{
    /// <summary>
    /// Сервис для взаимодействия с устройствами по последовательному порту.
    /// </summary>
    public class SerialPortService : IDisposable    
    {
        // Константы для общения с другим устройством
        public const string FORWARD = "f";
        public const string BACK = "b";
        public const string LEFT = "l";
        public const string RIGHT = "r";
        public const string STOP = "s";
        public const string OK = "k";

        /// <summary>
        /// Объект последовательного порта.
        /// </summary>
        private readonly SerialPort _serialPort;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="programConfig">Параметры запуска программы.</param>
        public SerialPortService(ProgramConfig programConfig)
        {
            _serialPort = new SerialPort
            {
                PortName = programConfig.PortName,
                BaudRate = programConfig.BaudRate,
                ReadTimeout = programConfig.ReadTimeout,
                WriteTimeout = programConfig.WriteTimeout
            };
        }

        /// <summary>
        /// Асинхронная инициализация сервиса.
        /// </summary>
        /// <returns>Асинхронная операция.</returns>
        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                _serialPort?.Open();
            });
        }

        /// <summary>
        /// Закрытие порта и освобождение ресурсов.
        /// </summary>
        public void Dispose()
        {
            _serialPort?.Close();
        }

        /// <summary>
        /// Отправить команду движения вперёд.
        /// </summary>
        /// <param name="command">Команда на исполнение.</param>
        /// <returns>Результат выполнения команды.</returns>
        public async Task<bool> SendCommandAsync(string command)
        {
            // Проверяем, что порт создан и открыт
            if (_serialPort == null || !_serialPort.IsOpen)
                return false;

            // Параллельно оправляем команду и ждём результат
            var taskResult = await Task.Run(() => SendCommand(command));

            // Проверяем, что пришёл корректный результат
            return taskResult.Contains(OK);
        }

        /// <summary>
        /// Отправить команду на последовательный порт.
        /// </summary>
        /// <param name="command">Команда на исполнение.</param>
        /// <returns>Возвращаемый результат выполнения команды.</returns>
        private string SendCommand(string command)
        {
            var readBuff = "";

            _serialPort.Write(command);

            Thread.Sleep(500);

            while (_serialPort.BytesToRead > 0)
            {
                readBuff += _serialPort.ReadExisting();
                Thread.Sleep(100);
            }

            return readBuff;
        }
    }
}
