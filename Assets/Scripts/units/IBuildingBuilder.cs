using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Xianxiao
{
    public interface IBuildingBuilder
    {
        public GameObject Build(BuildingSO building, Vector3 targetLocation);
        public Owner Owner { get; }
        public void CancelBuilding();
    }
}
