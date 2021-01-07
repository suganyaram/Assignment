using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactBookAPI.Service
{
    public class ContactGroupDBService<T> : IContactGroupDBService<T> where T : class
    {
        private readonly DocumentClient _client;
        private readonly string _cosmosDbDatabaseName;
        private readonly string _cosmosDbCollectionId1;
        private readonly FeedOptions queryFeedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

        public ContactGroupDBService(string CosmosDbEndPoint, string CosmosDbKey, string CosmosDbDatabaseName, string CosmosDbCollectionId1)
        {
            _cosmosDbDatabaseName = CosmosDbDatabaseName;
            _cosmosDbCollectionId1 = CosmosDbCollectionId1;
            _client = new DocumentClient(new Uri(CosmosDbEndPoint), CosmosDbKey);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<T> GetItemAsync(string sQuery)
        {
            try
            {
                List<dynamic> dynamicsList;
                var documentQuery = _client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(_cosmosDbDatabaseName, _cosmosDbCollectionId1)
                                    , sQuery, queryFeedOptions)
                                    .AsDocumentQuery();
                while (documentQuery.HasMoreResults)
                {
                    var docs = await documentQuery.ExecuteNextAsync();
                    dynamicsList = docs.ToList();
                    if (dynamicsList.Count > 0)
                        return dynamicsList[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Query Error" + ex);
                return null;
            }
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_cosmosDbDatabaseName, _cosmosDbCollectionId1), item);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string sQuery)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_cosmosDbDatabaseName, _cosmosDbCollectionId1)
                , sQuery
                , queryFeedOptions)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>().ConfigureAwait(true));
            }

            return results;
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            try
            {
                // Update a document
                return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_cosmosDbDatabaseName, _cosmosDbCollectionId1, id), item, new RequestOptions { PartitionKey = new PartitionKey(id) });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteItemAsync(string id)
        {
            try
            {
                await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_cosmosDbDatabaseName, _cosmosDbCollectionId1, id), new RequestOptions { PartitionKey = new PartitionKey(id) });
            }
            catch (Exception ex)
            {
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_cosmosDbDatabaseName));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Microsoft.Azure.Documents.Database { Id = _cosmosDbDatabaseName });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_cosmosDbDatabaseName, _cosmosDbCollectionId1));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_cosmosDbDatabaseName),
                        new DocumentCollection { Id = _cosmosDbCollectionId1 },
                        new Microsoft.Azure.Documents.Client.RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
