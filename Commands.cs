using Discord.Commands;
using Discord;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!

// TODO: make a shop with powerups and collectables, with a shop command and buy command
// also money to actually buy stuff and sell sandwiches for
// add multipliers for each item

public class HelpModule : ModuleBase<SocketCommandContext> {
    [Command("help")]
    public async Task HelpAsync() {
        var embed = new EmbedBuilder(){};

        embed.AddField("**Commands**", "\n*createprofile - Creates a profile for the user.\n" +
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
}

public class CreateProfileModule : ModuleBase<SocketCommandContext> {
    [Command("createprofile")]
    public async Task CreateProfileAsync() {
        var embed = new EmbedBuilder(){};
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Create a new user with that ID
            User user = new User(id);
            // Check if the user already exists
            if (File.Exists( "users/" + id +".json" )){
                throw new Exception();
            };
            // Save the user
            User.Save(id, user);
            // Set embed data
            embed.AddField("**Profile Created**", $"\n{username}'s profile has been created.", true)
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
            embed.AddField("**Materials**", $"\nMaterials for {Context.User.Username}\n \uD83E\uDD6A: {user.sandwiches}\n \uD83E\uDD5C: {user.peanuts}\n \uD83C\uDF5E: {user.breadSlices}\n \uD83E\uDDC8: {user.peanutButterJars}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
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
            embed.AddField("**Harvest**", $"\n{username} has harvested {peanutsHarvested} peanuts. \nYou now have {user.peanuts}", true);

            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
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
            embed.AddField("**Bake**", $"\n{username} has baked one slice of bread. \nYou now have {user.breadSlices} slices.", true);

            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
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
                embed.AddField("**Mash**", $"\n{username} has mashed 5 peanuts to make one jar of peanut butter. \nYou now have {user.peanutButterJars} peanut butter jars, and {user.peanuts} peanuts.", true);
                await ReplyAsync(embed: embed.Build());
            } else {
                // Send a message to the channel saying the user doesn't have enough peanuts
                embed.AddField("**Problem Encountered!**", $"\n{Context.User.Username} doesn't have enough peanuts to mash.", true);
                await ReplyAsync(embed: embed.Build());
            }
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
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
                embed.AddField("**Make Sandwich**", $"\n{username} has made a sandwich. \nYou now have {user.sandwiches} sandwiches, {user.breadSlices} bread slices, and {user.peanutButterJars} peanut butter jars.", true);
                await ReplyAsync(embed: embed.Build());
            } else {
                // Send a message to the channel saying the user doesn't have enough materials
                embed.AddField("**Problem Encountered!**", $"\n{Context.User.Username} doesn't have enough bread slices and/or peanut butter jars to make a sandwich.", true);
                await ReplyAsync(embed: embed.Build());
            }
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"\n{Context.User.Username} has not created a profile yet. \nPlease create a profile with the command 'createprofile'.", true);
            await ReplyAsync(embed: embed.Build());
        }
    }
}