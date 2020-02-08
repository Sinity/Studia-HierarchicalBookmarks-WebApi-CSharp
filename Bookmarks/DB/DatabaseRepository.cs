using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookmarksApp.Models;
using BookmarksApp.Utils.DatabaseSettings;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace BookmarksApp.Infrastructure {
    public class DatabaseRepository {
        private readonly IMongoDatabase DB;

        public DatabaseRepository(IOptions<Settings> settingsOptions) {
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;

            var client = new MongoClient(settingsOptions.Value.MongoDbConnection);
            DB = client.GetDatabase(settingsOptions.Value.MongoDatabaseName);

            BsonClassMap.RegisterClassMap<Bookmark>(x => {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.MapIdMember(y => y.Id);
            });

            BsonClassMap.RegisterClassMap<Models.Tag>(x => {
                x.AutoMap();
                x.SetIgnoreExtraElements(true);
                x.MapIdMember(y => y.Id);
            });
        }

        private IMongoCollection<T> GetCollection<T>() {
            return DB.GetCollection<T>(typeof(T).Name);
        }

        public IQueryable<BookmarksApp.Models.Bookmark> GetBookmarksData() {
            return GetCollection<BookmarksApp.Models.Bookmark>().AsQueryable();
        }

        public IQueryable<BookmarksApp.Models.Tag> GetTagsData() {
            return GetCollection<BookmarksApp.Models.Tag>().AsQueryable();
        }

        public async Task Insert<T>(T value) {
            await GetCollection<T>().InsertOneAsync(value);
        }

        public async Task Update<T>(Expression<Func<T, bool>> predicate, T value) {
            await GetCollection<T>().ReplaceOneAsync(predicate, value);
        }

        public async Task Delete<T>(Expression<Func<T, bool>> predicate) {
            await GetCollection<T>().DeleteOneAsync(predicate);
        }
    }
}