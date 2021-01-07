using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactBookAPI.Service
{
    public interface ICosmosDBService<T> where T : class
    {
        Task<Document> CreateItemAsync(T item);
        Task DeleteItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(string sQuery);
        Task<Document> UpdateItemAsync(string id, T item);
        Task<T> GetItemAsync(string sQuery);
    }
}
