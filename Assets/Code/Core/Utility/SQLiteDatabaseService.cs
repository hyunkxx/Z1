using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows.Input;
using Mono.Data.Sqlite;
using UnityEngine;


public sealed class SQLiteDatabaseService : IDisposable
{
    public IDbConnection Connection => connection;
    private IDbConnection connection;

    private string DatabaseName = "GameDatabase";

    public SQLiteDatabaseService()
    {
        /* temp code
         * Application.persistentDataPath
        */
        string path = $"URI=file:{Application.streamingAssetsPath}/Database/GameDatabase.db";
        Debug.Log(path);

        connection = new SqliteConnection(path);
        connection.Open();
    }

    public int ExecuteNonQuery(IDbCommand InCommand)
    {
        return InCommand.ExecuteNonQuery();
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

    public IDataReader ExcuteReader(IDbCommand InCommand)
    {
        return InCommand.ExecuteReader();
    }

    public void ExcuteReader(string query, Dictionary<string, object> parameters = null)
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

            using (IDataReader reader = ExcuteReader(command))
            {
                while (reader.Read())
                {

                }
            }
        }
    }

    public T GetDataClass<T>(int id, params object[] args) where T : IModel, new()
    {
        T obj = new T();
        string query = $"SELECT * FROM {typeof(T).Name} WHERE ID = @id";

        using(IDbCommand command = connection.CreateCommand())
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
                    obj.Initialize(reader, args);
                }
            }
        }

        return obj;
    }

    public List<T> GetDataClassList<T>(params object[] args) where T : IModel, new()
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
                    obj.Initialize(reader, args);
                    objList.Add(obj);
                }
            }
        }

        return objList;
    }

    public void Dispose()
    {
        connection?.Dispose();
    }
}
