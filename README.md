# Бот для Discord для управления космическим кораблём на НТО Junior 2021.

Весь код обильно покрыт комментариями, чтобы легче было в нём разобраться.

## Структура папок
- ArduinoIRController - папка с кодом для платы Arduino, подключенной по USB к компьютеру, на котором запущена программа бота. По последовательному порту данная плата Arduino принимает команды от программы бота, расшифровывает их и управляет подключенным к её пинам инфракрасным светодиодом, размещённым сверху над столом. Через инфракрасный светодиод пересылаются сигналы на корабли на столе. Используемые библиотеки:
  - **IRremote v. 3.5.0** (последней версии) для передачи команд через инфракрасный светодиод (https://github.com/Arduino-IRremote/Arduino-IRremote)
- NTOJSpaceDiscordBot - папка с кодом проекта консольной программы бота на .net core 3.1, которая запускается на компьютере, принимает команды от бота и передаёт команды на плату Arduino по последовательному порту через USB-кабель. Используемые библиотеки: 
  - **Discord.Net v. 2.4.0** для подключения к боту в Discord, получения и обработки команд от него (https://github.com/discord-net/Discord.Net)
  - **System.IO.Ports v. 6.0.0** для взаимодействия с платой Arduino по последовательному порту через USB-кабель (https://docs.microsoft.com/ru-ru/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-6.0)
  - **Microsoft.Extensions.DependencyInjection v. 6.0.0** чтобы современно, чистый код, всё такое (https://docs.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection)
  - **CsvHelper v. 27.2.1** не используется. Было желание протоколировать все действия с ботом в CSV, чтобы потом в случае чего видеть историю, кто какие команды посылал и не было вопросов, что кто-то кому-то помешал и т.п., но времени на это не осталось. Сейчас весь лог выводится в консоль, этого было достаточно (https://joshclose.github.io/CsvHelper/)

## Настройка бота

## Настройка программы на Arduino
Программа принимает символ от консольной программы бота и посылает сигнал нажатия соответствующей кнопки ИК-пульта кораблю (имитируем нажатия на кнопки пульта). Коды сигналов пульта были предоставлены организаторами. Если всё прошло хорошо, программа возвращает консольной программе бота констунту OK_STR, чтобы та понимала, что команда успешно дошла до Arduino и передана кораблю.      

- `#define IR_SEND_PIN 13` - для подключения задали такой пин, чтобы удобнее было присоединить кабель.
- `#define ADDRESS 0x0102` - такой адрес был в примере к библиотеке, вроде, можно заменить на 0х0 и всё равно будет работать. У нас работало так, поэтому ничего не меняли. Документация к данной библиотеке этото вопрос покрывает слабо или мы не нашли.
- `// Символьные константы, ожидаемые от программы` - сейчас не используются, вначале хотели задать конкретный набор команд, но потом переделали на просто пересылку всего, что приходит от консольной программы бота.

![image](https://user-images.githubusercontent.com/1927563/145013123-6461f0be-dfbb-4ea0-8290-61b57dc612b2.png)
