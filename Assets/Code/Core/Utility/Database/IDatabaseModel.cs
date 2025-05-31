using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Windows.Input;
using Mono.Data.Sqlite;

using UnityEngine;


public interface IDatabaseModel<T>
{
    /*
     * Map object data to SQLite command parameters
     */
    public void Serialize(IDbCommand command);

    /* 
     * Reads values from the database record and maps them to the object's fields.
     * Returns the ID, which serves as the dictionary key for this object.
     */
    public int Deserialize(IDataRecord record, params object[] args);
}