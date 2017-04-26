namespace UnifiedCache.UnifiedCache
{
    /// <summary>
    /// Defines Interface for BackPlane Communication Using PubSub Mechanism
    /// </summary>
    public interface IPubSubBackPlane:IBackPlane
    {
        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        /// <value>
        /// The name of the channel.
        /// </value>
        string ChannelName { get; }

        /// <summary>
        /// Publishes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Publish(string key);

        /// <summary>
        /// Starts the subscribe.
        /// </summary>
        void StartSubscribe();

        /// <summary>
        /// Runs the action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">The value.</param>
        void RunAction<T>(T val);
    }
}