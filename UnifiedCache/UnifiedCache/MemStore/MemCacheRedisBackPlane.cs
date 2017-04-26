using UnifiedCache.UnifiedCache;
using UnifiedCache.Utility.Redis;
using StackExchange.Redis;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace UnifiedCache.MemStore
{
    /// <summary>
    /// PubSub Implementation Using Redis For MemStorage <see cref="MemStorage"/>
    /// </summary>
    /// <seealso cref="UnifiedCache.UnifiedCache.IPubSubBackPlane" />
    internal class MemCacheRedisBackPlane : IPubSubBackPlane
    {
        /// <summary>
        /// The subscriber
        /// </summary>
        private readonly ISubscriber Subscriber;
        /// <summary>
        /// Gets or sets the cache handler.
        /// </summary>
        /// <value>
        /// The cache handler.
        /// </value>
        public static Action<RedisChannel, RedisValue> CacheHandler { get; set; }

        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        /// <value>
        /// The name of the channel.
        /// </value>
        public string ChannelName
        {
            get
            {
                return nameof(MemCacheRedisBackPlane);
            }
        }

        /// <summary>
        /// Determines whether this instance is connected.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return this.Subscriber != null && RedisFactory.Connection.IsConnected;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemCacheRedisBackPlane"/> class.
        /// </summary>
        public MemCacheRedisBackPlane()
        {
            this.Subscriber = RedisFactory.Connection?.GetSubscriber();
        }

        /// <summary>
        /// Subscribes the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="flags">The flags.</param>
        public void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
        {
            if (this.IsConnected())
            {
                Subscriber.Subscribe(channel, handler, flags);
            }
        }

        /// <summary>
        /// Subscribes the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="flag">The flag.</param>
        public void Subscribe(string channel, Action<RedisChannel, RedisValue> handler, CommandFlags flag = CommandFlags.None)
        {
            Subscriber.Subscribe(new RedisChannel(channel, RedisChannel.PatternMode.Auto), handler, flag);
        }

        /// <summary>
        /// Unsubscribes all.
        /// </summary>
        public void UnsubscribeAll()
        {
            if (this.IsConnected())
                this.Subscriber.UnsubscribeAll();
        }

        /// <summary>
        /// Unsubscribes the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="flag">The flag.</param>
        public void Unsubscribe(string channel, Action<RedisChannel, RedisValue> handler, CommandFlags flag = CommandFlags.None)
        {
            if (this.IsConnected())
                this.Subscriber.Unsubscribe(new RedisChannel(channel, RedisChannel.PatternMode.Auto), handler, flag);
        }

        /// <summary>
        /// Unsubscribes the asynchronous.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="flag">The flag.</param>
        /// <returns></returns>
        public Task UnsubscribeAsync(string channel, Action<RedisChannel, RedisValue> handler, CommandFlags flag = CommandFlags.None)
        {
            return (this.IsConnected()) ? this.Subscriber.UnsubscribeAsync(new RedisChannel(channel, RedisChannel.PatternMode.Auto), handler, flag) : null;
        }

        /// <summary>
        /// Publishes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Publish(string value) => Publish(ChannelName, value);

        /// <summary>
        /// Publishes the asynchronous.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Task<long> PublishAsync(string value) => PublishAsync(ChannelName, value);

        /// <summary>
        /// Publishes the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.Exception">Redis Publish Activity Failed </exception>
        public void Publish(string channel, string value)
        {
            if (this.IsConnected())
            {
                this.Subscriber.Publish(channel, value);
            }
            else
            {
                throw new Exception("Redis Publish Activity Failed ");
            }
        }

        /// <summary>
        /// Publishes the asynchronous.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Task<long> PublishAsync(string channel, string value)
        {
            return (this.IsConnected()) ? this.Subscriber.PublishAsync(channel, value) : null;
        }

        /// <summary>
        /// Starts the subscribe.
        /// </summary>
        public void StartSubscribe()
        {
            CacheHandler = (x, y) =>
            {
                RunAction<RedisValue>(y);
            };
            Subscriber.Subscribe(ChannelName, CacheHandler);
        }

        /// <summary>
        /// Runs the action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The value.</param>
        public void RunAction<T>(T val)
        {
            var key = val.ToString();
            MemoryCache.Default.Remove(key);
        }
    }
}