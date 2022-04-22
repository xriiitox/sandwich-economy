using Newtonsoft;
using Newtonsoft.Json;
public class User {
    string id;
    public int sandwiches, peanuts, breadSlices, peanutButterJars;
    public User(string userID) {
        id = userID;
        /*sandwiches = 0;
        peanuts = 0;
        breadSlices = 0;
        peanutButterJars = 0;*/
    }
    public static void Save(string userID, User user) {
        string json = JsonConvert.SerializeObject(user);
        File.WriteAllText(userID + ".json", json);
    }

    public static User Load(string userID) {
        string json = File.ReadAllText(userID + ".json");
        return JsonConvert.DeserializeObject(json, typeof(User)) as User;
    }
}