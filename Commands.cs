using Discord.Commands;
using Discord;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!

// TODO: actually add items to the shop, make it so that you can buy items from the shop

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
            embed.AddField("**Profile Created**", $"{Environment.NewLine}{Environment.NewLine}{username}'s profile has been created.", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {username}")
            .WithCurrentTimestamp();
            // Send the embed
            await ReplyAsync(embed: embed.Build());
        } catch (Exception e) {
            embed.AddField("**Error**", $"{Environment.NewLine}User already exists.", true);
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
            embed.AddField("**Materials**", $"{Environment.NewLine}{Environment.NewLine}Materials for {Context.User.Username}{Environment.NewLine} \uD83E\uDD6A: {user.sandwiches}{Environment.NewLine} \uD83D\uDCB5: {user.money}{Environment.NewLine} \uD83E\uDD5C: {user.peanuts}{Environment.NewLine} \uD83C\uDF5E: {user.breadSlices}{Environment.NewLine} \uD83E\uDDC8: {user.peanutButterJars}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true);
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
                peanutsHarvested = ((int)Math.Round(multiplier)) * 2;
                user.peanuts += (int)Math.Round(multiplier) * 2;
                // change this when powerups are added so that it reads from powerups
            } else {
                peanutsHarvested = 2;
                user.peanuts += 2;
            }
            // Save the user
            User.Save(id, user);
            // Set embed data
            embed.AddField("**Harvest**", $"{Environment.NewLine}{Environment.NewLine} +{peanutsHarvested} \uD83E\uDD5C {Environment.NewLine}Current \uD83E\uDD5C: {user.peanuts}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {username}")
            .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true)
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
            embed.AddField("**Bake**", $"{Environment.NewLine}{Environment.NewLine}+1 \uD83C\uDF5E {Environment.NewLine}Current \uD83C\uDF5E: {user.breadSlices}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {username}")
            .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true)
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
                embed.AddField("**Mash**", $"{Environment.NewLine}{Environment.NewLine}-5 \uD83E\uDD5C {Environment.NewLine}+1 \uD83E\uDDC8 {Environment.NewLine}Current \uD83E\uDDC8: {user.peanutButterJars} {Environment.NewLine}Current \uD83E\uDD5C: {user.peanuts}", true)
                .WithColor(Color.Green)
                .WithFooter($"Requested by {username}")
                .WithCurrentTimestamp();

                await ReplyAsync(embed: embed.Build());
            } else {
                // Send a message to the channel saying the user doesn't have enough peanuts
                embed.AddField("**Problem Encountered!**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} doesn't have enough peanuts to mash.", true)
                .WithColor(Color.Red)
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }

    }
}

public class MakeSandwichModule : ModuleBase<SocketCommandContext> {
    [Command("make")]
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
                embed.AddField("**Make Sandwich**", $"{Environment.NewLine}{Environment.NewLine}+1 \uD83E\uDD6A. {Environment.NewLine}Current \uD83E\uDD6A: {user.sandwiches} {Environment.NewLine}Current \uD83C\uDF5E: {user.breadSlices} {Environment.NewLine}Current \uD83E\uDDC8: {user.peanutButterJars}", true)
                .WithColor(Color.Green)
                .WithFooter($"Requested by {username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            } else {
                // Send a message to the channel saying the user doesn't have enough materials
                embed.AddField("**Problem Encountered!**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} doesn't have enough bread slices and/or peanut butter jars to make a sandwich.", true)
                .WithColor(Color.Red)
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } catch (IOException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
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
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not provided a user ID.", true)
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
                    embed.AddField("**Remove User**", $"{Environment.NewLine}{Environment.NewLine}{username} has removed {deletedUsername}'s profile.", true)
                    .WithColor(Color.Green)
                    .WithFooter($"Requested by {username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
                } catch (IOException e) {
                    embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}User with id {id} has not created a profile.", true)
                    .WithColor(Color.Red)
                    .WithFooter($"Requested by ${Context.User.Username}")
                    .WithCurrentTimestamp();
                    await ReplyAsync(embed: embed.Build());
                }
            } else {
                embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} is not an admin.", true)
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
                embed.AddField("**Set Admin**", $"{Environment.NewLine}{Environment.NewLine}{username} has set {user.username} as an admin.", true);
            } catch (IOException e) {
                User user = User.Load(id);
                embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{user.username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true);
                await ReplyAsync(embed: embed.Build());
            }
        } else {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} is not an admin.", true);
            await ReplyAsync(embed: embed.Build());
        }
    }
}

public class DisplayShopModule : ModuleBase<SocketCommandContext> {
    [Command("shop", true)]
    public async Task DisplayShopAsync() {
        var embed = new EmbedBuilder(){};
        string shopItems = "";
        string shopDescriptions = "";
        string prices = "";
        try {
            foreach (string line in File.ReadLines("shop.txt")) {
                shopItems = shopItems + line + Environment.NewLine;
            }
            foreach (string line in File.ReadLines("shopDescriptions.txt")) {
                shopDescriptions = shopDescriptions + line + Environment.NewLine;
            }
            foreach (string line in File.ReadLines("prices.txt")) {
                prices = prices + line + Environment.NewLine;
            }
            embed.AddField("**Item**", $"{Environment.NewLine}{Environment.NewLine}{shopItems}", true)
            .AddField("**Description**", $"{Environment.NewLine}{Environment.NewLine}{shopDescriptions}", true)
            .AddField("**Price**", $"{Environment.NewLine}{Environment.NewLine}{prices}", true)
            .WithColor(Color.Green)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        } catch (Exception e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}The Shop listing could not be found. Please contact the person running this bot.", true)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
            Console.WriteLine(e.Message);
            await ReplyAsync(embed: embed.Build());
        }
    }
}

public class SellSandwichesModule : ModuleBase<SocketCommandContext> {
    [Command("sell")]
    public async Task SellSandwichesAsync(string amountToSell = "1") {
        var embed = new EmbedBuilder(){};
        try {
            User user = User.Load(Context.User.Id.ToString());
            if (user.sandwiches > 0) {
                user.sandwiches -= Convert.ToInt64(amountToSell);
                user.money += Convert.ToInt64(amountToSell) * 5;
                var moneyEarned = (Convert.ToInt64(amountToSell) * 5).ToString();
                User.Save(Context.User.Id.ToString(), user);
                embed.AddField("**Sell Sandwiches**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has sold sandwiches and earned {moneyEarned} dollars.", true)
                .WithColor(Color.Green)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            } else {
                embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} doesn't have any sandwiches to sell.", true)
                .WithColor(Color.Red)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } catch (FileNotFoundException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true)
            .WithColor(Color.Red)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        } catch (FormatException e) {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}You have inputted an invalid value of sandwiches to sell.{Environment.NewLine}Try again with a valid value.", true)
            .WithColor(Color.Red)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }
    }
}

public class AddItemToShopModule : ModuleBase<SocketCommandContext> {
    [Command("addshopitem")]
    public async Task AddItemToShopAsync(string itemName, string itemDescription, string itemPrice) {
        var embed = new EmbedBuilder(){};
        User UserRunningCommand = User.Load(Context.User.Id.ToString());
        if (UserRunningCommand.isAdmin) {
            try {
                string username = Context.User.Username;
                // Load the user
                User user = User.Load(Context.User.Id.ToString());
                // Add item and description to shop
                File.AppendAllText("shop.txt", $"{itemName}{Environment.NewLine}");
                File.AppendAllText("shopDescriptions.txt", $"{itemDescription}{Environment.NewLine}");
                File.AppendAllText("prices.txt", $"{itemPrice}{Environment.NewLine}");
                // Send a message to the channel confirming the creation of the new item
                embed.AddField("**Add Item To Shop**", $"{Environment.NewLine}{Environment.NewLine}{username} has added {itemName} to the shop.", true)
                .WithColor(Color.Green)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            } catch (IOException e) {
                embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} has not created a profile yet. {Environment.NewLine}Please create a profile with the command 'createprofile'.", true)
                .WithColor(Color.Red)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: embed.Build());
            }
        } else {
            embed.AddField("**ERROR**", $"{Environment.NewLine}{Environment.NewLine}{Context.User.Username} is not an admin.", true)
            .WithColor(Color.Red)
            .WithFooter($"Requested by {Context.User.Username}")
            .WithCurrentTimestamp();
            await ReplyAsync(embed: embed.Build());
        }
    }
}