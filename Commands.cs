using Discord.Commands;

// Keep in mind your module **must** be public and inherit ModuleBase.
// If it isn't, it will not be discovered by AddModulesAsync!

// TODO: Make ALL commands send in Embeds

public class HelpModule : ModuleBase<SocketCommandContext> {
    [Command("help")]
    public async Task HelpAsync() {
        await ReplyAsync("```" +
            "*createprofile - Creates a profile for the user.\n" +
            "*help - Shows this message.\n" +
            "*materials - Shows the amount of materials the user has.\n" +
            "*harvest - get one peanut\n" +
            "*bake - get one slice of bread\n" + 
            "*mash - craft one peanut butter\n" +
            "*makesandwich - make one sandwich (requires one peanut butter and two slices of bread)" + "```");
    }
}

public class CreateProfileModule : ModuleBase<SocketCommandContext> {
    [Command("createprofile")]
    public async Task CreateProfileAsync() {
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Create a new user with that ID
            User user = new User(id);
            // Save the user
            User.Save(id, user);
            // Send a message to the channel confirming the creation of the user
            await ReplyAsync($"User {id} created.\n Username: {username}");
        } catch (IOException e) {
            await ReplyAsync($"User {Context.User.Id} already exists.");
        }
    }
}

public class MaterialsModule : ModuleBase<SocketCommandContext> {
    [Command("materials")]
    public async Task MaterialsAsync() {
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // Get the number of sandwiches
            int sandwiches = user.sandwiches;
            // Get the number of peanuts
            int peanuts = user.peanuts;
            // Get the number of bread slices
            int breadSlices = user.breadSlices;
            // Get the number of peanut butter jars
            int peanutButterJars = user.peanutButterJars;
            // Send a message to the channel with the number of sandwiches the user has
            await ReplyAsync($"{username} has {sandwiches} sandwiches, {peanuts} peanuts, {peanutButterJars} jars of peanut butter, and {breadSlices} bread slices.");
        } catch (IOException e) {
            await ReplyAsync($"User {Context.User.Username} does not exist. Please create a profile with the command `createprofile`.");
        }
    }
}

public class HarvestModule : ModuleBase<SocketCommandContext> {
    [Command("harvest")]
    public async Task HarvestAsync() {
        try {
            // Get the user's ID
            string id = Context.User.Id.ToString();
            string username = Context.User.Username;
            // Load the user
            User user = User.Load(id);
            // Add one peanut to the user's peanut count
            user.peanuts += 1;
            // Save the user
            User.Save(id, user);
            // Send a message to the channel with the number of peanuts the user has
            await ReplyAsync($"{username} now has {user.peanuts} peanuts.");
        } catch (IOException e) {
            await ReplyAsync($"User {Context.User.Username} does not exist. Please create a profile with the command 'createprofile'.");
        }
    }
}

public class BakeModule : ModuleBase<SocketCommandContext> {
    [Command("bake")]
    public async Task BakeAsync() {
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
            // Send a message to the channel with the number of bread slices the user has
            await ReplyAsync($"{username} now has {user.breadSlices} bread slices.");
        } catch (IOException e) {
            await ReplyAsync($"User {Context.User.Username} does not exist. Please create a profile with the command 'createprofile'.");
        }
    }
}

public class MashModule : ModuleBase<SocketCommandContext> {
    [Command("mash")]
    public async Task MashAsync() {
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
                // Send a message to the channel with the number of peanut butter jars the user has
                await ReplyAsync($"{username} now has {user.peanutButterJars} peanut butter jars, and {user.peanuts} peanuts.");
            } else {
                // Send a message to the channel saying the user doesn't have enough peanuts
                await ReplyAsync($"{username} doesn't have enough peanuts.");
            }
        } catch (IOException e) {
            await ReplyAsync($"User {Context.User.Username} does not exist. Please create a profile with the command 'createprofile'.");
        }

    }
}

public class MakeSandwichModule : ModuleBase<SocketCommandContext> {
    [Command("makesandwich")]
    public async Task MakeSandwichAsync() {
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
                await ReplyAsync($"{username} now has {user.sandwiches} sandwiches, {user.peanuts} peanuts, {user.peanutButterJars} jars of peanut butter, and {user.breadSlices} bread slices.");
            } else {
                // Send a message to the channel saying the user doesn't have enough materials
                await ReplyAsync($"{username} doesn't have enough materials.");
            }
        } catch (IOException e) {
            await ReplyAsync($"User {Context.User.Username} does not exist. Please create a profile with the command 'createprofile'.");
        }
    }
}