using System;
using System.Threading.Tasks;
using NPoco;

namespace MatchMaker.Api.Databases.Queries
{
    public static class DatabaseExtensions
    {
        public static Task<T> QueryAsync<T>(this IDatabase self, IQuery<T> query)
        {
            return query.ExecuteAsync(self);
        }
    }
}