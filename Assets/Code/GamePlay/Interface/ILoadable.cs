using UnityEngine;


public interface ILoadable
{
    public void RequiredResources(string path /* = from Resource directory */);
    public void PostLoad();
}