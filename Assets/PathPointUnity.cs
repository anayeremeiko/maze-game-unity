using MazeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class PathPointUnity : MonoBehaviour
    {
        public int Value { get; set; }

        public PathPointUnity(PathPoint point)
        {
            Value = point.Value;
        }
    }
}
