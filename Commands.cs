using Discord.Commands;
using Discord;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!

// TODO: make a shop with powerups and collectables, with a shop command and buy command
// also money to actually buy stuff and sell sandwiches for
// add multipliers for each item

public class CreateProfileModule : ModuleBase<SocketCommandContext> {
    [Command("createprofile")]
    public async Task CreateProfileAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Create a new user with that ID
            User user = new User(id, username);
            // Check if the user already exists
            if (File.Exists( "users/" + id +".json" )){
                throw new Exception();
            };
            // Save the user
            User.Save(id, user);
            // Set embed data
            embed.AddField("**Profile Created**", $"\n\n{username}'s profile has been created.", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {username}")
            .WithCurrentTimestamp();
            // Send the embed
            await ReplyAsync(embed: embed.Build());
        } catch (Exception e) {
            embed.AddField("**Error**", $"\nUser already exists.", true);
            await ReplyAsync(embed: embed.Build());
        }
    }
}

public class MaterialsModule : ModuleBase<SocketCommandContext> {
    [Command("materials")]
    public async Task MaterialsAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // Make Embed
            embed.AddField("**Materials**", $"\n\nMaterials for {Context.User.Username}\n \uD83E\uDD6A: {user.sandwiches}\n \uD83E\uDD5C: {user.peanuts}\n \uD83C\uDF5E: {user.breadSlices}\n \uD83E\uDDC8: {user.peanutButterJars}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
            await ReplyAsync(embed: embed.Build());
        }

        

    }
}

public class HarvestModule : ModuleBase<SocketCommandContext> {
    [Command("harvest")]
    public async Task HarvestAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // If user has at least five sandwiches, apply peanut multiplier
            int peanutsHarvested = 0;
            if (user.sandwiches >= 5) {
                var multiplier = decimal.Divide(user.sandwiches, 5);
                peanutsHarvested = ((int)Math.Round(multiplier)+1) * 2;
                user.peanuts += (int)Math.Round(multiplier) * 2;
                // change this when powerups are added so that it reads from powerups
            } else {
                peanutsHarvested = 2;
                user.peanuts += 2;
            }
            // Save the user
            User.Save(id, user);
            // Set embed data
            embed.AddField("**Harvest**", $"\n\n{username} has harvested {peanutsHarvested} peanuts. \nYou now have {user.peanuts}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {username}")
            .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }
    }
}

public class BakeModule : ModuleBase<SocketCommandContext> {
    [Command("bake")]
    public async Task BakeAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // Add one slice of bread to the user's bread slice count
            user.breadSlices += 1;
            // Save the user data
            User.Save(id, user);
            // Set embed data
            embed.AddField("**Bake**", $"\n\n{username} has baked one slice of bread. \nYou now have {user.breadSlices} slices.", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {username}")
            .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }
    }
}

public class MashModule : ModuleBase<SocketCommandContext> {
    [Command("mash")]
    public async Task MashAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // Check if the user has enough peanuts
            if (user.peanuts >= 5) {
                // Add one peanut butter jar to the user's peanut butter jar count
                user.peanutButterJars += 1;
                // Remove five peanuts from the user's peanut count
                user.peanuts -= 5;
                // Save the user data
                User.Save(id, user);
                // Send a message to the channel with the number of peanut butter jars the user now has
                embed.AddField("**Mash**", $"\n\n{username} has mashed 5 peanuts to make one jar of peanut butter. \nYou now have {user.peanutButterJars} peanut butter jars, and {user.peanuts} peanuts.", true)
                .WithColor(Color.Green)
                .WithFooter($"Requested by {username}")
                .WithCurrentTimestamp();

                await ReplyAsync(embed: embed.Build());
            } else {
                // Send a message to the channel saying the user doesn't have enough peanuts
                embed.AddField("**Problem Encountered!**", $"\n\n{Context.User.Username} doesn't have enough peanuts to mash.", true)
                .WithColor(Color.Red)
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }

    }
}

public class MakeSandwichModule : ModuleBase<SocketCommandContext> {
    [Command("makesandwich")]
    public async Task MakeSandwichAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // Check if the user has enough bread slices and peanut butter jars
            if (user.breadSlices >= 2 && user.peanutButterJars >= 1) {
                // Subtract the amount of bread slices and peanut butter jars from the user's inventory
                user.breadSlices -= 2;
                user.peanutButterJars -= 1;
                // Add one sandwich to the user's inventory
                user.sandwiches += 1;
                // Save the user data
                User.Save(id, user);
                // Send a message to the channel confirming the creation of the new sandwich
                embed.AddField("**Make Sandwich**", $"\n\n{username} has made a sandwich. \nYou now have {user.sandwiches} sandwiches, {user.breadSlices} bread slices, and {user.peanutButterJars} peanut butter jars.", true)
                .WithColor(Color.Green)
                .WithFooter($"Requested by {username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            } else {
                // Send a message to the channel saying the user doesn't have enough materials
                embed.AddField("**Problem Encountered!**", $"\n\n{Context.User.Username} doesn't have enough bread slices and/or peanut butter jars to make a sandwich.", true)
                .WithColor(Color.Red)
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }
    }
}

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

public class RemoveUserModule : ModuleBase<SocketCommandContext> {
    [Command("removeuser")]
    public async Task RemoveUserAsync(string? id = null) {
        var embed = new EmbedBuilder(){};
        User UserRunningCommand = User.Load(Context.User.Id.ToString());
        if (id == null) {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} has not provided a user ID.", true)
            .WithColor(Color.Red)
            .WithFooter($"Requested by ${Context.User.Username}")
            .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
        } else {
            if (UserRunningCommand.isAdmin) {
                try {
                    string username = Context.User.Username;
                    // Load the user
                    User user = User.Load(id);
                    string deletedUsername = user.username;
                    // Delete the user's data
                    User.Remove(id);
                    // Send a message to the channel confirming the deletion of the user
                    embed.AddField("**Remove User**", $"\n\n{username} has removed {deletedUsername}'s profile.", true)
                    .WithColor(Color.Green)
                    .WithFooter($"Requested by {username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
                } catch (IOException e) {
                    embed.AddField("**ERROR**", $"\n\nUser with id {id} has not created a profile.", true)
                    .WithColor(Color.Red)
                    .WithFooter($"Requested by ${Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
                }
            } else {
                embed.AddField("**ERROR**", $"\n\n{Context.User.Username} is not an admin.", true)
                .WithColor(Color.Red)
                .WithFooter($"Requested by ${Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}

public class SetAdminModule : ModuleBase<SocketCommandContext> {
    [Command("setadmin", true)]
    public async Task SetAdminAsync(string id) {
        var embed = new EmbedBuilder(){};
        User UserRunningCommand = User.Load(Context.User.Id.ToString());
        if (UserRunningCommand.isAdmin) {
            try {
                string username = Context.User.Username;
                // Load the user
                User user = User.Load(id);
                // Set the user's admin status
                user.isAdmin = true;
                // Save the user data
                User.Save(id, user);
                // Send a message to the channel confirming the creation of the new sandwich
                embed.AddField("**Set Admin**", $"\n\n{username} has set {user.username} as an admin.", true);
            } catch (IOException e) {
                User user = User.Load(id);
                embed.AddField("**ERROR**", $"\n\n{user.username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
                await ReplyAsync(embed: embed.Build());
            }
        } else {
            embed.AddField("**ERROR**", $"\n\n{Context.User.Username} is not an admin.", true);
            await ReplyAsync(embed: embed.Build());
        }
    }
}