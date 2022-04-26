using Discord;
using Discord.Commands;


public class HelpModule : ModuleBase<SocketCommandContext> {
    [Command("help", true)]
    public async Task HelpAsync(string firstarg = "normal") {
        var embed = new EmbedBuilder(){};
        try {
            if (firstarg == "admin") {
                    embed.AddField("**Admin Commands**", $"*removeuser - Removes a user from the database.{Environment.NewLine}" +
                    $"*set - Sets a user's materials.{Environment.NewLine}" + 
                    $"*setadmin - Sets a user's admin status.{Environment.NewLine}" +
                    "*addshopitem - add new item to shop", true)
                    .WithColor(Color.Green)
                    .WithFooter($"Requested by {Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
            } else if (firstarg == "normal") {
                embed.AddField("**Commands**", $"{Environment.NewLine}{Environment.NewLine}*createprofile - Creates a profile for the user.{Environment.NewLine}" +
                    $"*help - Shows this message.{Environment.NewLine}" +
                    $"*materials - Shows the amount of materials the user has.{Environment.NewLine}" +
                    $"*harvest - get one peanut{Environment.NewLine}" +
                    $"*bake - get one slice of bread{Environment.NewLine}" + 
                    $"*mash - craft one peanut butter{Environment.NewLine}" +
                    $"*makesandwich - make one sandwich (requires one peanut butter and two slices of bread){Environment.NewLine}" + 
                    $"*shop - view item listings{Environment.NewLine}" +
                    "*sell - sell sandwiches", true)
                    .WithColor(Color.Green)
                    .WithFooter($"Requested by {Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
            }
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            await ReplyAsync("An error occured. Please try again later.");
        }
    }
}