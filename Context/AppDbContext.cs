using Hangman_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Hangman_App.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts {  get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Wordlist> WordList { get; set; }
        public DbSet<Word> words { get; set; }
    }
}
