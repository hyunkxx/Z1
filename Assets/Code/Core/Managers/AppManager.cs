using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;


public sealed class AppManager : Singleton<AppManager>
{
    protected override void Awake()
    {
        base.Awake();

        //Database.Instance.Initialize();
        //AssetLoader.Initialize();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //AssetLoader.UnInitialize();
    }
}

public sealed class BuildVersion : IDatabaseModel<BuildVersion>
{
    public BuildVersion() {}
    public BuildVersion(int Dirty) 
    {
        AppVersion = Application.version;
        this.Dirty = Dirty; 
    }

    private int ID;
    private int Dirty;
    private string AppVersion;

    public int Major { get; private set; }
    public int Minor { get; private set; }

    public static bool operator > (BuildVersion lhs, BuildVersion rhs)
    {
        if (lhs.Major != rhs.Major)
            return lhs.Major > rhs.Major;
        else if (lhs.Minor != rhs.Minor)
            return lhs.Minor > rhs.Minor;
        else
            return lhs.Dirty > rhs.Dirty;
    }

    public static bool operator < (BuildVersion lhs, BuildVersion rhs)
    {
        if (lhs.Major != rhs.Major)
            return lhs.Major < rhs.Major;
        else if (lhs.Minor != rhs.Minor)
            return lhs.Minor < rhs.Minor;
        else
            return lhs.Dirty < rhs.Dirty;
    }

    //public static bool operator == (BuildVersion lhs, BuildVersion rhs)
    //{
    //    return lhs.Major == rhs.Major
    //        && lhs.Minor == rhs.Minor
    //        && lhs.Dirty == rhs.Dirty;
    //}

    //public static bool operator !=(BuildVersion lhs, BuildVersion rhs)
    //{
    //    return lhs.Major != rhs.Major
    //        && lhs.Minor != rhs.Minor
    //        && lhs.Dirty != rhs.Dirty;
    //}

    public static BuildVersion GetLatestVersion(BuildVersion lhs, BuildVersion rhs)
    {
        if (lhs.Major != rhs.Major)
        {
            return lhs.Major > rhs.Major ? lhs : rhs;
        }
        else if (lhs.Minor != rhs.Minor)
        {
            return lhs.Minor > rhs.Minor ? lhs : rhs;
        }
        else
        {
            return lhs.Dirty > rhs.Dirty ? lhs : rhs;
        }
    }

    public void Deserialize(IDataRecord record, params object[] args)
    {
        ID = record.GetInt32(0);
        Dirty = record.GetInt32(1);
        AppVersion = record.GetString(2);

        string[] parts = AppVersion.Split('.');
        Major = int.Parse(parts[0]);
        Minor = int.Parse(parts[1]);
    }

    public void Serialize(IDbCommand command)
    {
        var paramId = command.CreateParameter();
        paramId.ParameterName = "@ID";
        paramId.Value = ID;
        command.Parameters.Add(paramId);

        var paramName = command.CreateParameter();
        paramName.ParameterName = "@Dirty";
        paramName.Value = Dirty;
        command.Parameters.Add(paramName);

        var paramQuantity = command.CreateParameter();
        paramQuantity.ParameterName = "@AppVersion";
        paramQuantity.Value = AppVersion;
        command.Parameters.Add(paramQuantity);
    }
}