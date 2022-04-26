using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Discord;


public class Program {
    private DiscordSocketClient? _client;
    private CommandService? _commands;
    private CommandHandler? _handler;

    public static Task Main(string[] args) => new Program().MainAsync();

	public async Task MainAsync() {
        _client = new DiscordSocketClient(new DiscordSocketConfig {
            LogLevel = LogSeverity.Verbose,
            MessageCacheSize = 100,
            AlwaysDownloadUsers = true,
            LogGatewayIntentWarnings = false
        });
        _commands = new CommandService(new CommandServiceConfig {
            CaseSensitiveCommands = false,
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Verbose,
            SeparatorChar = ' '
        });
        _handler = new CommandHandler(_client, _commands);
        _client.Log += Log;
        await _handler.InstallCommandsAsync();
        string token;
        try {
            token = File.ReadAllText("token.txt");
        } catch (FileNotFoundException e) {
            Console.WriteLine("'token.txt' not found. Please create one with your bot token and place it in this folder.");
            Console.WriteLine(e.Message);
            Console.ReadKey();
            return;
        }
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await _client.SetActivityAsync(new Game("Selling Sandwiches"));

        await Task.Delay(-1);
        
	}
    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}