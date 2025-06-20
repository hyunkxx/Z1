using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Windows.Input;
using Mono.Data.Sqlite;

using UnityEngine;


public sealed class DatabaseService : IDisposable
{
    public IDbConnection Connection => connection;
    private IDbConnection connection;

    public DatabaseService(string path)
    {
        connection = new SqliteConnection(path);
        connection.Open();
    }

    public int ExecuteNonQuery(IDbCommand InCommand)
    {
        return InCommand.ExecuteNonQuery();
    }

    public IDataReader ExcuteReader(IDbCommand InCommand)
    {
        return InCommand.ExecuteReader();
    }

    public void AddParameter(IDbCommand command, string name, object value)
    {
        var param = command.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        command.Parameters.Add(param);
    }

    public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        using (IDbCommand command = Connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            
            foreach (var param in parameters)
            {
                IDbDataParameter dbParam = command.CreateParameter();
                dbParam.ParameterName = param.Key;
                dbParam.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(dbParam);
            }

            return command.ExecuteNonQuery();
        }
    }

    public IDataReader ExcuteReader(string query, Dictionary<string, object> parameters = null)
    {
        IDbCommand command = Connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = query;

        foreach (var param in parameters)
        {
            IDbDataParameter dbParam = command.CreateParameter();
            dbParam.ParameterName = param.Key;
            dbParam.Value = param.Value ?? DBNull.Value;
            command.Parameters.Add(dbParam);
        }

        return ExcuteReader(command);
    }

    public bool TableExists(string tableName)
    {
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;";
            var param = cmd.CreateParameter();
            param.ParameterName = "@tableName";
            param.Value = tableName;
            cmd.Parameters.Add(param);

            var result = cmd.ExecuteScalar();
            return result != null && result != DBNull.Value;
        }
    }

    public T MakeClassByID<T>(int id, params object[] args) where T : IDatabaseModel<T>, new()
    {
        T obj = new T();
        string query = $"SELECT * FROM {typeof(T).Name} WHERE ID = @id";

        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            IDbDataParameter param = command.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            command.Parameters.Add(param);

            using (IDataReader reader = ExcuteReader(command))
            {
                if (reader.Read())
                {
                    obj.Deserialize(reader, args);
                }
            }
        }

        return obj;
    }

    public List<T> MakeListFromTable<T>(params object[] args) where T : IDatabaseModel<T>, new()
    {
        List<T> objList = new List<T>();
        string query = $"SELECT * FROM {typeof(T).Name}";
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            using (IDataReader reader = ExcuteReader(command))
            {
                while (reader.Read())
                {
                    T obj = new T();
                    obj.Deserialize(reader, args);
                    objList.Add(obj);
                }
            }
        }

        return objList;
    }

    public Dictionary<int, T> MakeDictionaryFromTable<T>(params object[] args) where T : IDatabaseModel<T>, new()
    {
        Dictionary<int, T> table = new Dictionary<int, T>();
        string query = $"SELECT * FROM {typeof(T).Name}";
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            using (IDataReader reader = ExcuteReader(command))
            {
                while (reader.Read())
                {
                    T obj = new T();
                    int ID = obj.Deserialize(reader, args);

                    if(!table.ContainsKey(ID))
                    {
                        table.Add(ID, obj);
                    }
                    else
                    {
                        Debug.LogError($"Duplicate key detected: {typeof(T).Name} {ID}");
                    }
                }
            }
        }

        return table;
    }

    public void Dispose()
    {
        connection?.Dispose();
    }
}
