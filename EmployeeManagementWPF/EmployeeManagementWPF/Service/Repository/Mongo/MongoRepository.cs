using EmployeeManagementWPF.Service.Database;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagementWPF.Service.Repository.Mongo;

public delegate void DatabaseChangeListener<T>(IEnumerable<T> changedDocuments);

internal abstract class MongoRepository<T, ID> : IRepositoryListener<T, ID, string> where T : class
{
    private readonly IMongoCollection<BsonDocument> _collection;
    private readonly Dictionary<string, DatabaseChangeListener<T>> _listeners = new();

    public MongoRepository(string collectionName)
    {
        _collection = MongoConnection.Database.GetCollection<BsonDocument>(collectionName);
        // No funciona, necesita "replica sets" y es un movidón
        //StartListening();
    }

    private async void StartListening()
    {

        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
            .Match("{ operationType: { $in: [ 'insert', 'update', 'replace', 'delete' ] } }");

        var options = new ChangeStreamOptions { FullDocument = ChangeStreamFullDocumentOption.UpdateLookup };

        var cursor = await _collection.WatchAsync(pipeline, options);

        while (await cursor.MoveNextAsync())
        {
            var batch = cursor.Current;

            foreach (var change in batch)
            {
                var document = change.FullDocument;
                NotifyListeners(new List<T> { DeserializeDocument<T>(document) }, document["idc"].AsString);
            }
        }
    }

    public async Task<IEnumerable<T>> FindAll(string idc)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("idc", idc);
        var documents = await _collection.Find(filter).ToListAsync();
        return DeserializeDocuments<T>(documents);
    }

    public async Task<T?> FindById(ID id, string idc)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("idc", idc),
            Builders<BsonDocument>.Filter.Eq("Id", id)
        );

        var document = await _collection.Find(filter).FirstOrDefaultAsync();
        return document != null ? DeserializeDocument<T>(document) : null;
    }

    public async Task<T?> Save(T t, string idc, ID id)
    {
        if (await ExistsById(id, idc))
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("idc", idc),
                Builders<BsonDocument>.Filter.Eq("Id", id)
            );

            var document = BsonDocument.Parse(JsonSerializer.Serialize(t));
            document.Add("idc", idc);
            await _collection.ReplaceOneAsync(filter, document);

        }
        else
        {
            var document = BsonDocument.Parse(JsonSerializer.Serialize(t));
            document.Add("idc", idc);
            await _collection.InsertOneAsync(document);

        }

        await ManualNotification(idc);

        return t;
    }

    public async Task DeleteById(ID id, string idc)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("idc", idc),
            Builders<BsonDocument>.Filter.Eq("Id", id)
        );

        await _collection.DeleteOneAsync(filter);

        await ManualNotification(idc);
    }

    public async Task DeleteAll(string idc)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("idc", idc);
        await _collection.DeleteManyAsync(filter);

        await ManualNotification(idc);
    }

    public async Task<bool> ExistsById(ID id, string idc) => await FindById(id, idc) != null;

    public async Task<bool> Exists(T t, string idc)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("idc", idc);
        var document = await _collection.Find(filter).FirstOrDefaultAsync();
        return document != null;
    }

    public async Task<long> Count(string idc)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("idc", idc);
        var count = await _collection.CountDocumentsAsync(filter);
        return count;
    }

    private T DeserializeDocument<T>(BsonDocument document)
    {
        // Excluir el campo '_id' durante la deserialización
        document.Remove("_id");
        document.Remove("idc");

        var json = document.ToJson();
        return JsonSerializer.Deserialize<T>(json);
    }

    private IEnumerable<T> DeserializeDocuments<T>(IEnumerable<BsonDocument> documents)
    {
        return documents.Select(document => DeserializeDocument<T>(document));
    }

    public void AddChangeListener(string idc, Action<IEnumerable<T>> action)
    {
        _listeners[idc] = new DatabaseChangeListener<T>(action);
    }

    public void RemoveChangeListener(string idc)
    {
        _listeners.Remove(idc);
    }

    private void NotifyListeners(IEnumerable<T> changedDocuments, string idc)
    {
        if (_listeners.ContainsKey(idc))
        {
            _listeners[idc].Invoke(changedDocuments);
        }
    }

    private async Task ManualNotification(string idc)
    {
        //Notificacion manual :(
        NotifyListeners(await FindAll(idc), idc);
    }
}
