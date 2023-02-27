using System.Text.Json.Serialization;

namespace ASPNETCoreWebAPIJWT.Model
{
    public class User
    {
        [JsonConstructor]
        public User(string id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        [JsonConstructor]
        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public User() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Password
        {
            get; set;
        }
    }
}