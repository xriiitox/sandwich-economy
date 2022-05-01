using Discord;
using Discord.Commands;
using Discord.WebSocket;


public class SetModule : ModuleBase<SocketCommandContext> {
    [Command("set")]
    public async Task SetMaterialsAsync(SocketGuildUser guildUser, string? material = null, string? amount = null) {
        var embed = new EmbedBuilder(){};

        User UserRunningCommand = User.Load(Context.User.Id);
        User user;
        ulong id = guildUser.Id;
        if (UserRunningCommand.isAdmin) {
            if (guildUser != null && material != null && amount != null && guildUser != null) {
                try {
                    bool invalidMaterial = false;
                    if (id.ToString().Length == 18) {
                        // Load the user
                        user = User.Load(id);
                    
                        // Set materials
                        switch (material) {
                            case "bread":
                                user.breadSlices = int.Parse(amount);
                                break;
                            case "butter":
                                user.peanutButterJars = int.Parse(amount);
                                break;
                            case "peanuts":
                                user.peanuts = int.Parse(amount);
                                break;
                            case "sandwiches":
                                user.sandwiches = int.Parse(amount);
                                break;
                            default:
                                invalidMaterial = true;
                                embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}Invalid material.", true)
                                .WithColor(Color.Red)
                                .WithFooter($"Requested by {UserRunningCommand.username}")
                                .WithCurrentTimestamp();
                                break;
                        }
                        User.Save(id, user);
                    } else {
                        invalidMaterial = true;
                        embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}Invalid ID.", true)
                        .WithColor(Color.Red)
                        .WithFooter($"Requested by {UserRunningCommand.username}")
                        .WithCurrentTimestamp();
                    }
                    if (invalidMaterial) {
                        await ReplyAsync(embed: embed.Build());
                    } else {
                        user = User.Load(id);
                        // Save the user data
                        User.Save(id, user);
                        // Send a message to the channel confirming the setting of the materials
                        embed.AddField("**Set**", $"{Environment.NewLine}{Environment.NewLine}{UserRunningCommand.username} has set {user.username}'s ``{material}`` to {amount}.", true)
                        .WithColor(Color.Green)
                        .WithFooter($"Requested by {UserRunningCommand.username}")
                        .WithCurrentTimestamp();
                        await ReplyAsync(embed: embed.Build());
                    }
                } catch (Exception e) {
                    user = User.Load(id);
                    embed.AddField("**Something has gone wrong!**", $"{Environment.NewLine}{Environment.NewLine}Most likely, you inputted an invalid user ID, or they have not created a profile yet. {Environment.NewLine}The amount of material to be added may have been invalid as well.", true)
                    .WithColor(Color.Red)
                    .WithFooter($"Requested by {Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
                }
            } else {
                embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not provided all the required arguments. {Environment.NewLine}Please use the command in the following format: ```*set <user id> <material> <amount>```", true)
                .WithColor(Color.Red)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } else {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} is not an admin.", true);
            await ReplyAsync(embed: embed.Build());
        }
    }
}