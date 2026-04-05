using Farola.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Testcontainers.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Farola.Infrastructure.Tests.Services
{
    public class RedisCacheServiceTests : IAsyncLifetime
    {
        private readonly RedisContainer _redisContainer = new RedisBuilder().Build();
        private IDistributedCache _distributedCache = null!;
        private IConnectionMultiplexer _redisMultiplexer = null!;
        private RedisCacheService _service = null!;

        public async Task InitializeAsync()
        {
            await _redisContainer.StartAsync();
            var connectionString = _redisContainer.GetConnectionString();
            var options = new RedisCacheOptions
            {
                Configuration = connectionString
            };
            _distributedCache = new RedisCache(Options.Create(options));
            _redisMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);
            _service = new RedisCacheService(_distributedCache, _redisMultiplexer);
        }

        public async Task DisposeAsync() => await _redisContainer.DisposeAsync();

        [Fact]
        public async Task SetAndGet_ShouldStoreAndRetrieve()
        {
            var key = "test_key";
            var value = new TestData(1, "Test");
            await _service.SetAsync(key, value, TimeSpan.FromMinutes(1));
            var retrieved = await _service.GetAsync<TestData>(key);
            Assert.NotNull(retrieved);
            Assert.Equal(1, retrieved.Id);
            Assert.Equal("Test", retrieved.Name);
        }

        [Fact]
        public async Task Remove_ShouldDeleteKey()
        {
            var key = "remove_me";
            await _service.SetAsync(key, "value");
            await _service.RemoveAsync(key);
            var retrieved = await _service.GetAsync<string>(key);
            Assert.Null(retrieved);
        }

        [Fact]
        public async Task RemoveByPrefix_ShouldDeleteMatchingKeys()
        {
            await _service.SetAsync("prefix:1", "a");
            await _service.SetAsync("prefix:2", "b");
            await _service.SetAsync("other:3", "c");
            await _service.RemoveByPrefixAsync("prefix:");
            Assert.Null(await _service.GetAsync<string>("prefix:1"));
            Assert.Null(await _service.GetAsync<string>("prefix:2"));
            Assert.NotNull(await _service.GetAsync<string>("other:3"));
        }

        private record TestData(int Id, string Name);
    }
}
