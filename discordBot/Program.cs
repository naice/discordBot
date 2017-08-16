using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace discordTests
{
    class Program
    {
        public static void Main(string[] args)
        => new Program().MainAsync(args).GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private const string CONFIG_PATH = @"D:\discord.smite.bot.json";
        private static Config _config;
        private IDiscordMessageBot[] _messageBots;

        public async Task MainAsync(string[] args)
        {
            _config = Config.Load(args.Length > 0 ? args[0] : null ?? CONFIG_PATH);

            //var smite = new SmiteAPI.Smite(new SmiteAPI.Config(_config.SmiteDevId, _config.SmiteAuthKey) { SmiteEndpoint = _config.SmiteEndpoint });
            //var test = await smite.GetMatchHistory("Emmuss");

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            
            _messageBots = new IDiscordMessageBot[]
            {
                new SmiteBot(_config),
            };

            await _client.LoginAsync(TokenType.Bot, _config.BotToken);
            await _client.StartAsync();
            
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage arg)
        {
            await Task.WhenAll(_messageBots.Select((A) => A.ProcessMessage(arg)).ToArray());
        }

        private Task Log(LogMessage message)
        {
            var cc = Console.ForegroundColor;
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
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;
            
            return Task.CompletedTask;
        }
    }
}