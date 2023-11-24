using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hangman_App.Models
{
    public class Wordlist
    {
        public int Id {  get; set; }
        public int gameId { get; set; }
        public int wordId { get; set; }
        public Game Game {  get; set; }
        public Word Word { get; set; }
    }
}
