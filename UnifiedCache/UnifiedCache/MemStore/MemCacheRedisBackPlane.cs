using StackExchange.Redis;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using UnifiedCache.Lib.RedisStore;
using UnifiedCache.Lib.UnifiedCache;

namespace UnifiedCache.Lib.MemStore
{
    internal class MemCacheRedisBackPlane : IPubSubBackPlane
    {
        private ISubscriber Subscriber = null;
        public static Action<RedisChannel, RedisValue> CacheHandler { get; set; }

        public string ChannelName
        {
            get
            {
                return "MemCacheRedisBackPlane";
            }
        }

        public bool IsConnected()
        {
            return this.Subscriber != null && RedisFactory.Connection.IsConnected;
        }

        public MemCacheRedisBackPlane()
        {
            this.Subscriber = RedisFactory.Connection?.GetSubscriber();
        }

        public void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
        {
            if (IsConnected())
            {
                Subscriber.Subscribe(channel, handler, flags);
            }
        }

        public void Subscribe(string channel, Action<RedisChannel, RedisValue> handler, CommandFlags flag = CommandFlags.None)
        {
            Subscriber.Subscribe(new RedisChannel(channel, RedisChannel.PatternMode.Auto), handler, flag);
        }

        public void UnsubscribeAll()
        {
            if (IsConnected())
                this.Subscriber.UnsubscribeAll();
        }

        public void Unsubscribe(string channel, Action<RedisChannel, RedisValue> handler, CommandFlags flag = CommandFlags.None)
        {
            if (IsConnected())
                this.Subscriber.Unsubscribe(new RedisChannel(channel, RedisChannel.PatternMode.Auto), handler, flag);
        }

        public Task UnsubscribeAsync(string channel, Action<RedisChannel, RedisValue> handler, CommandFlags flag = CommandFlags.None)
        {
            return (IsConnected()) ? this.Subscriber.UnsubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Auto), handler, flag) : null;
        }

        public void Publish(string value) => Publish(ChannelName, value);

        public Task<long> PublishAsync(string value) => PublishAsync(ChannelName, value);

        public void Publish(string channel, string value)
        {
            if (IsConnected())
            {
                this.Subscriber.Publish(channel, value);
            }
            else
            {
                throw new Exception("Redis Publish Activity Failed ");
            }
        }

        public Task<long> PublishAsync(string channel, string value)
        {
            return (IsConnected()) ? this.Subscriber.PublishAsync(channel, value) : null;
        }

        public void StartSubscribe()
        {
            CacheHandler = (x, y) =>
            {
                RunAction<RedisValue>(y);
            };
            Subscriber.Subscribe(ChannelName, CacheHandler);
        }

        public void RunAction<T>(T val)
        {
            var key = val.ToString();
            MemoryCache.Default.Remove(key);
        }
    }
}