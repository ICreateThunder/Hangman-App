using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hangman_App.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int AccountId {  get; set; }
        public int WordlistId {  get; set; }
        public string attempted { get; set; }
        public int guesses { get; set; }
        
        public Word Word { get; set; }
        public Account Account { get; set; }
        public Wordlist Wordlist { get; set; }
    }
}
