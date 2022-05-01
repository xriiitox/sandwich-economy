using Newtonsoft;
using Newtonsoft.Json;
using Discord;
public class User {
    public string username;
    public long sandwiches, peanuts, breadSlices, peanutButterJars, money;
    public bool isAdmin;
    public User(string username) {
        this.username = username;
        isAdmin = false;
        money = 0;
        /*sandwiches = 0;
        peanuts = 0;
        breadSlices = 0;
        peanutButterJars = 0;*/
    }
    public static void Save(ulong userID, User user) {
        string json = JsonConvert.SerializeObject(user);
        File.WriteAllText("users/" + userID.ToString() + ".json", json);
    }

    public static User Load(ulong userID) {
        string json = File.ReadAllText("users/" + userID.ToString() + ".json");
        return JsonConvert.DeserializeObject(json, typeof(User)) as User;
    }

    public static void Remove(ulong userID) {
        File.Delete("users/" + userID.ToString() + ".json");
    }
}