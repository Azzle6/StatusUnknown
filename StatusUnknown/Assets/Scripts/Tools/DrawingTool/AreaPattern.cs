namespace Tools.DrawingTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    
    public class AreaPattern
    {
        public bool IsEmpty => this.PointsData.Count == 0;
        public string PatternName { get; private set; }
    
        public readonly Dictionary<Vector2Int, E_PointState> PointsData;
        public Vector2Int Size { get; private set; }
        public Vector2Int Anchor { get; private set; }

        public AreaPattern(string path)
        {
            this.PointsData = new Dictionary<Vector2Int, E_PointState>();
            this.Deserialize(path);
        }

        private void Deserialize(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".ptrn")
            {
                this.PatternName = Path.GetFileNameWithoutExtension(path);
                string[] lines = File.ReadAllLines(path);
                string[] groupSeparators = new[] { "[", "]" };
                string[] contentSeparators = new [] { ";", ":" };
                string[] splitGroups = lines[0].Split(groupSeparators, StringSplitOptions.RemoveEmptyEntries);

                string[] splitContent = splitGroups[0].Split(contentSeparators, StringSplitOptions.RemoveEmptyEntries);
                this.Size = new Vector2Int(int.Parse(splitContent[0]), int.Parse(splitContent[1]));
            
                splitContent = splitGroups[1].Split(contentSeparators, StringSplitOptions.RemoveEmptyEntries);
                this.Anchor = new Vector2Int(int.Parse(splitContent[0]), int.Parse(splitContent[1]));

                this.PointsData.Clear();
                for (int i = 1; i < lines.Length; i++)
                {
                    splitGroups = lines[i].Split(groupSeparators, StringSplitOptions.RemoveEmptyEntries);

                    if (splitGroups[0].Length > 0)
                    {
                        foreach (string info in splitGroups)
                        {
                            string[] values = info.Split(contentSeparators, StringSplitOptions.RemoveEmptyEntries);
                            Vector2Int pointCoordinates = new Vector2Int(int.Parse(values[0]), int.Parse(values[1]));
                            this.PointsData.Add(pointCoordinates, (E_PointState)int.Parse(values[2]));
                        }
                    }
                }
            }
        }

        public Dictionary<Vector2Int, E_PointState> GetActivePointsRelativeToAnchor()
        {
            Dictionary<Vector2Int, E_PointState> result = new ();
            foreach (KeyValuePair<Vector2Int, E_PointState> point in this.PointsData)
            {
                if(point.Value != E_PointState.EMPTY)
                    result.Add(point.Key - this.Anchor, point.Value);
            }
            return result;
        }
    }

    public enum E_PointState
    {
        EMPTY,
        FILL,
    }
}