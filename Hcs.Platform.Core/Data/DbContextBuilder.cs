using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace Hcs.Platform.Data
{
    public class DbContextBuilder : IDbModelCreatingConfig
    {
        static ConcurrentBag<Action<ModelBuilder>> configuraions = new ConcurrentBag<Action<ModelBuilder>>();
        static ConcurrentBag<Action<DbContext>> seeds = new ConcurrentBag<Action<DbContext>>();

        public IEnumerable<Action<ModelBuilder>> Configuraions => configuraions;

        public static void Config(Action<ModelBuilder> config)
        {
            configuraions.Add(config);
        }
        public static void Seed(Action<DbContext> seed)
        {
            seeds.Add(seed);

        }
        public static void RunSeeds(DbContext context)
        {
            foreach (var seed in seeds)
            {
                seed(context);
            }
        }
    }
}