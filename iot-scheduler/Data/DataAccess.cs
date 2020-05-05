using System;
using System.Collections.Generic;
using iot_scheduler.Entities;
using MongoDB.Driver;

namespace iot_scheduler
{
    public static class DataAccess<T> where T : IEntity
    {
        private static IMongoDatabase MongoDatabase => MongoClient.GetDatabase(AppConfiguration.MongoDatabase);

        private static string CollectionName
        {
            get
            {
                if (typeof(T) == typeof(Schedule))
                    return "schedules";

                throw new NotImplementedException();
            }
        }

        private static MongoClient MongoClient => new MongoClient(AppConfiguration.MongoDbUrl);

        public static void Insert(T recordObj)
        {
            MongoDatabase.GetCollection<T>(CollectionName).InsertOne(recordObj);
        }

        public static void Replace(T recordObj)
        {
            var collection = MongoDatabase.GetCollection<T>(CollectionName);
            var filterDefinition = Builders<T>.Filter.Eq("_id", recordObj.Id);
            collection.ReplaceOne(filterDefinition, recordObj);
        }

        public static T GetRecord(string id)
        {
            var collection = MongoDatabase.GetCollection<T>(CollectionName);
            var result = collection.Find(x => x.Id == id).FirstOrDefault();
            return result;
        }

        public static List<T> GetRecords()
        {
            var collection = MongoDatabase.GetCollection<T>(CollectionName);
            var result = collection.Find(_ => true).ToList();
            return result;
        }

        public static void DeleteRecord(string id)
        {
            var collection = MongoDatabase.GetCollection<T>(CollectionName);
            collection.DeleteMany(x => x.Id == id);
        }
    }
}