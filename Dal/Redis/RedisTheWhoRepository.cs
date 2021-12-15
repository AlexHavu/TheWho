using System;
using StackExchange.Redis;
using Tipalti.Redis;

namespace Tipalti.TheWho.Dal.Redis
{
    public interface IRedisTheWhoRepository
    {

    }
    public class RedisTheWhoRepository : BaseStringRepository<TheWhoRedisEntity>, IRedisTheWhoRepository
    {


        public RedisTheWhoRepository(IRedisConnectionProvider redisConnectionProvider) : this(redisConnectionProvider?.RedisDatabase)
        {
        }
        public RedisTheWhoRepository(IDatabase redisDatabase) : base (redisDatabase)
        {
        }

        protected override string GetKey(TheWhoRedisEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override string GetKey(params object[] keyParams)
        {
            throw new NotImplementedException();
        }
    }
}
