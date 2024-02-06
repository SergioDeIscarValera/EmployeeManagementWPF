using EmployeeManagementWPF.Properties;
using MongoDB.Driver;
using System;

namespace EmployeeManagementWPF.Service.Database;

class MongoConnection
{
    protected MongoConnection() { }

    private static readonly Lazy<IMongoDatabase> _database = new(() =>
    {
        var connectionString = Settings.Default.mongo_connection_string;
        var client = new MongoClient(connectionString);
        return client.GetDatabase("employeemanagement");
    });

    public static IMongoDatabase Database => _database.Value;
}
