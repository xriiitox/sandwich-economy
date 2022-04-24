using Discord;
using Discord.Commands;


public class HelpModule : ModuleBase<SocketCommandContext> {
    [Command("help", true)]
    public async Task HelpAsync(string firstarg = "normal") {
        var embed = new EmbedBuilder(){};
        try {
            if (firstarg == "admin") {
                    embed.AddField("**Admin Commands**", "*removeuser - Removes a user from the database.\n" +
                    "*set - Sets a user's materials.\n" + 
                    "*setadmin - Sets a user's admin status.\n")
                    .WithColor(Color.Green)
                    .WithFooter($"Requested by {Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
            } else if (firstarg == "normal") {
                embed.AddField("**Commands**", "\n\n*createprofile - Creates a profile for the user.\n" +
                    "*help - Shows this message.\n" +
                    "*materials - Shows the amount of materials the user has.\n" +
                    "*harvest - get one peanut\n" +
                    "*bake - get one slice of bread\n" + 
                    "*mash - craft one peanut butter\n" +
                    "*makesandwich - make one sandwich (requires one peanut butter and two slices of bread)", true)
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