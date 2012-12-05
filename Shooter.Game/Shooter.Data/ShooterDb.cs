using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Shooter.Data
{
    public class ShooterDb : DbContext
    {
        public ShooterDb()
        {
        }

        public DbSet<HighScore> HighScores { get; set; }
        public void RefreshTable()
        {

            var toDelete = this.HighScores.OrderByDescending(x => x.Kills).Skip(10);

            foreach (var item in toDelete.ToList())
            {
                this.HighScores.Remove(item);
            }

            this.SaveChanges();
        }
        public IEnumerable<HighScore> TopTenHighScores
        {
            get
            {
                return this.HighScores.OrderByDescending(x => x.Kills);
            }
        }
        public void AddEntry(HighScore highScore) {
            this.HighScores.Add(highScore);
            this.SaveChanges();
            this.RefreshTable();
    }

        }
    public class ShooterDbInitializer : DropCreateDatabaseIfModelChanges<ShooterDb>
    {
        protected override void Seed(ShooterDb context)
        {
            {
                var highscore = new HighScore();
                highscore.Kills = 9001;
                highscore.Name = "Leroy Jenkins";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 21;
                highscore.Name = "Bob";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 9;
                highscore.Name = "Matt";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 7;
                highscore.Name = "Richard1";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 6;
                highscore.Name = "Richard2";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 5;
                highscore.Name = "Richard3";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 4;
                highscore.Name = "Richard4";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 3;
                highscore.Name = "Richard5";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 2;
                highscore.Name = "Richard6";
                context.HighScores.Add(highscore);
            }

            {
                var highscore = new HighScore();
                highscore.Kills = 0;
                highscore.Name = "Noob";
                context.HighScores.Add(highscore);
            }

            base.Seed(context);
        }
    }

   
}
