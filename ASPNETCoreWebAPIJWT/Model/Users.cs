using System.Text.Json.Serialization;

namespace ASPNETCoreWebAPIJWT.Model
{
    public class Users
    {
        [JsonConstructor]
        public Users(string id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        [JsonConstructor]
        public Users(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public Users() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Password
        {
            get; set;
        }
    }
}