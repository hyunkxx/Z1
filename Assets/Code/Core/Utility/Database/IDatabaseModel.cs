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
    public void Serialize(IDbCommand command);
    public void Deserialize(IDataRecord record, params object[] args);
}