using System.Text.Json;
using NewVariant.Exceptions;
using NewVariant.Interfaces;


namespace Max.N 
{
    /// <summary>
    /// Represents a simple in-memory database that can store and retrieve entities of various types.
    /// </summary>
    public class DataBase : IDataBase {
        private readonly Dictionary<string, object> _tables = new();
        /// <summary>
        /// Creates a new table in the database for entities of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of entity to create a table for.</typeparam>
        public void CreateTable<T>() where T : IEntity {
            string tableName = typeof(T).Name;
            if (!_tables.ContainsKey(tableName)) {
                _tables[tableName] = new List<T>();
            }
        }
        /// <summary>
        /// Inserts a new entity of type <typeparamref name="T"/> into the corresponding table in the database.
        /// </summary>
        /// <typeparam name="T">The type of entity to insert.</typeparam>
        /// <param name="getEntity">A function that returns the entity to insert.</param>
        /// <exception cref="DataBaseException">Thrown when the table for <typeparamref name="T"/> doesn't exist.</exception>
        public void InsertInto<T>(Func<T> getEntity) where T : IEntity {
            string tableName = typeof(T).Name;
            if (!_tables.ContainsKey(tableName)) {
                throw new DataBaseException($"Table '{tableName}' doesn't exist.");
            }

            T entity = getEntity();
            ((List<T>)_tables[tableName]).Add(entity);
        }
        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> from the corresponding table in the database.
        /// </summary>
        /// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /// <returns>An enumerable sequence of entities of type <typeparamref name="T"/>.</returns>
        /// <exception cref="DataBaseException">Thrown when the table for <typeparamref name="T"/> doesn't exist.</exception>
        public IEnumerable<T> GetTable<T>() where T : IEntity {
            string tableName = typeof(T).Name;
            if (!_tables.ContainsKey(tableName)) {
                throw new DataBaseException($"Table '{tableName}' doesn't exist.");
            }

            return ((List<T>)_tables[tableName]);
        }
        /// <summary>
        /// Serializes all entities of type <typeparamref name="T"/> in the corresponding table to a JSON file at the specified path.
        /// </summary>
        /// <typeparam name="T">The type of entity to serialize.</typeparam>
        /// <param name="path">The path to save the JSON file to.</param>
        /// <exception cref="DataBaseException">Thrown when the table for <typeparamref name="T"/> doesn't exist.</exception>
        public void Serialize<T>(string path) where T : IEntity {
            string tableName = typeof(T).Name;
            if (!_tables.ContainsKey(tableName)) {
                throw new DataBaseException($"Table '{tableName}' doesn't exist.");
            }

            using var fileStream = new FileStream(path, FileMode.Create);
            var options = new JsonSerializerOptions { WriteIndented = true };
            JsonSerializer.Serialize(fileStream, _tables[tableName], typeof(List<T>), options);
        }
        /// <summary>
        /// Deserializes the contents of a JSON file into the table for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity to deserialize.</typeparam>
        /// <param name="path">The path to the JSON file.</param>
        public void Deserialize<T>(string path) where T : IEntity {
            string tableName = typeof(T).Name;
            if (!_tables.ContainsKey(tableName)) {
                throw new DataBaseException($"Table '{tableName}' doesn't exist.");
            }

            using var streamReader = new StreamReader(path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            _tables[tableName] = JsonSerializer.Deserialize<List<T>>(streamReader.ReadToEnd(), options) ?? new List<T>();
        }
    }
}
