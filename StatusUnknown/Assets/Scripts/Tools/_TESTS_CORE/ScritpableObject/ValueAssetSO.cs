using UnityEngine;

// https://www.youtube.com/watch?v=RZJWwn40T8E Value/Reference pattern <3 (also cf Ryan Hipple 2017 Unite talk)
// this basically does what I do whith my class templates, but way better and with no need to create a new class for each new content

/// <summary>
///  Turns any value as a project wide data to automatically update all assets with that value
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ValueAssetSO<T> : ScriptableObject
{
    public T value;
}
