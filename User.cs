using Newtonsoft;
using Newtonsoft.Json;
using Discord;
public class User {
    string id;
    public string username;
    public long sandwiches, peanuts, breadSlices, peanutButterJars, money;
    public bool isAdmin;
    public User(string userID, string username) {
        id = userID;
        this.username = username;
        isAdmin = false;
        money = 0;
        /*sandwiches = 0;
        peanuts = 0;
        breadSlices = 0;
        peanutButterJars = 0;*/
    }
    public static void Save(string userID, User user) {
        string json = JsonConvert.SerializeObject(user);
        File.WriteAllText("users/" + userID + ".json", json);
    }

    public static User Load(string userID) {
        string json = File.ReadAllText("users/" + userID + ".json");
        return JsonConvert.DeserializeObject(json, typeof(User)) as User;
    }

    public static void Remove(string userID) {
        File.Delete("users/" + userID + ".json");
    }
}