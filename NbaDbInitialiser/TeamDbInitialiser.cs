using Microsoft.EntityFrameworkCore;
using NbaDb.Data;
using NbaDb.Models;
using System;
using static System.Linq.Enumerable;

namespace NbaDbInitialiser
{
    public class TeamDbInitialiser
    {
        private Random Rand { get; set; }
        private NbaDbContext Context { get; set; }
        public TeamDbInitialiser(NbaDbContext context)
        {
            this.Context = context;
            this.Rand = new Random();
        }

        public void AddToDb()
        {
            var range = Range(0, 100);
            var teams = range.Select(r => new Team
                {
                    Name = RandomString(),
                    Prediction = RandomDouble(),
                    HashCode = RandomInt()
                })
                .ToList();

            using (var transaction = Context.Database.BeginTransaction())
            {
                Context.AddRange(teams);
                Context.SaveChanges();
                transaction.Commit();
            }
        }

        private string RandomString()
        {
            var length = 10;
            const string chars = "abcdefghijklmnopqrstuvwxyz_0123456789";
            return new string(Repeat(chars, length)
              .Select(s => s[Rand.Next(s.Length)]).ToArray());
        }

        private double RandomDouble() => Rand.NextDouble() * 100;

        private int RandomInt() => Rand.Next();
    }
}