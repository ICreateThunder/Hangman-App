using System.ComponentModel.DataAnnotations;

namespace Hangman_App.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username {  get; set; }
        public string Password { get; set; }
        public bool StayLoggedIn {  get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
