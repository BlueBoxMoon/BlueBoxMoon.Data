namespace BlueBoxMoon.Data.EntityFramework
{
    /// <summary>
    /// Provides a dependency service that plugins can use to get to the system
    /// JSON serializer. Must be implemented by application code.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Deserializes the given JSON string into the type specified by
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to decode the JSON into.</typeparam>
        /// <param name="json">The JSON string to be decoded.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        T Deserialize<T>( string json );

        /// <summary>
        /// Serializes the given object into a JSON string.
        /// </summary>
        /// <param name="value">The object to be serialized.</param>
        /// <returns>A string that contains the JSON representation of the <paramref name="value"/>.</returns>
        string Serialize( object value );
    }
}
