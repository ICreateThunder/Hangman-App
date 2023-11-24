using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hangman_App.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Value {  get; set; }
    }
}
