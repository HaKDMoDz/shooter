using System.Linq;
using Shooter.Data;
using System.Collections.Generic;

namespace Shooter.Application
{
#if WINDOWS || XBOX
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            //ShooterDb shooterDb = new ShooterDb();
            //List<HighScore> list = shooterDb.HighScores.ToList();

            //foreach (var item in list)
            //{
            //    System.Console.WriteLine("Name: " + item.Name + "\nKills: " + item.Kills + "\n");
            //}

            using (var game = new ShooterGame())
            {
                game.Run();
            }
        }
    }
#endif
}