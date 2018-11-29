using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hcs.Extensions.EntityFrameworkCore
{
    public static class UpdateSetExtensions
    {
        public static void UpdateSet<T>(this DbContext context, IEnumerable<T> newData, Expression<Func<T, bool>> scope, Func<T, T, bool> compaire, Action<T, T> update = null, Action<T> create = null) where T : class
        {
            var set = context.Set<T>();
            var olds = set.Where(scope).ToList();
            var newLines = newData.ToList();
            var total = newLines.Count;
            var updates = new List<T>();
            var deletes = new List<T>();
            //delete and add
            foreach (var old in olds.ToArray())
            {
                var newLine = newLines.FirstOrDefault(n => compaire(old, n));
                if (newLine == null)
                {
                    deletes.Add(old);
                }
                else
                {
                    update?.Invoke(old, newLine);
                    updates.Add(newLine);
                    newLines.Remove(newLine);
                }
            }
            if ((updates.Count + newLines.Count) != total)
            {
                throw new Exception($"input {total} item but update {updates.Count} and create {newLines.Count}");
            }
            set.RemoveRange(deletes);
            //add
            if (create != null)
            {
                foreach (var c in newLines)
                {
                    create(c);
                }
            }
            set.AddRange(newLines);
        }
    }
}
