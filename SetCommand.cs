using Discord;
using Discord.Commands;


public class SetModule : ModuleBase<SocketCommandContext> {
    [Command("set")]
    public async Task SetMaterialsAsync(string? id = null, string? material = null, string? amount = null) {
        var embed = new EmbedBuilder(){};
        User UserRunningCommand = User.Load(Context.User.Id.ToString());
        User user;
        try {
            long ID = Convert.ToInt64(id);
        } catch (Exception e) {
            embed.AddField("**ERROR**", $"\n\nInvalid ID.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
            return;
        }
        if (UserRunningCommand.isAdmin) {
            if (id != null && material != null && amount != null) {
                try {
                    bool invalidMaterial = false;
                    if (id.Length == 18) {
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
                                embed.AddField("**ERROR**", "\n\nInvalid material.", true)
                                .WithColor(Color.Red)
                                .WithFooter($"Requested by {UserRunningCommand.username}")
                                .WithCurrentTimestamp();
                                break;
                        }
                        User.Save(id, user);
                    } else {
                        invalidMaterial = true;
                        embed.AddField("**ERROR**", "\n\nInvalid ID.", true)
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
                        embed.AddField("**Set**", $"\n\n{UserRunningCommand.username} has set {user.username}'s ``{material}`` to {amount}.", true)
                        .WithColor(Color.Green)
                        .WithFooter($"Requested by {UserRunningCommand.username}")
                        .WithCurrentTimestamp();
                        await ReplyAsync(embed: embed.Build());
                    }
                } catch (Exception e) {
                    user = User.Load(id);
                    embed.AddField("**Something has gone wrong!**", $"\n\nMost likely, you inputted an invalid user ID, or they have not created a profile yet. \nThe amount of material to be added may have been invalid as well.", true)
                    .WithColor(Color.Red)
                    .WithFooter($"Requested by {Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
                }
            } else {
                embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not provided all the required arguments. \nPlease use the command in the following format: ```*set <user id> <material> <amount>```", true)
                .WithColor(Color.Red)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } else {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} is not an admin.", true);
            await ReplyAsync(embed: embed.Build());
        }
    }
}