using RH.HeadEditor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RH.HeadEditor.Helpers
{
    class ProfileSmoothing
    {
        private RenderMesh renderMesh;
        private Dictionary<Guid, KeyValuePair<MeshUndoInfo, MeshUndoInfo>> smoothingInfo = new Dictionary<Guid, KeyValuePair<MeshUndoInfo, MeshUndoInfo>>();
        public ProfileSmoothing(RenderMesh mesh, Dictionary<Guid, MeshUndoInfo> undoInfo)
        {
            renderMesh = mesh;
            var newMesh = renderMesh.Parts.ToDictionary(part => part.Guid, part => part.GetUndoInfo());
            foreach(var oldMeshInfo in undoInfo)
            {
                MeshUndoInfo meshInfo;
                if(newMesh.TryGetValue(oldMeshInfo.Key, out meshInfo))
                {
                    smoothingInfo.Add(oldMeshInfo.Key, new KeyValuePair<MeshUndoInfo, MeshUndoInfo>(oldMeshInfo.Value, meshInfo));
                }
            }
        }

        public void Smooth(float percent)
        {
            KeyValuePair<MeshUndoInfo, MeshUndoInfo> info;
            foreach (var part in renderMesh.Parts)
            {
                if(smoothingInfo.TryGetValue(part.Guid, out info))
                {
                    MeshUndoInfo smoothingData = new MeshUndoInfo();
                    foreach(var p in info.Key.Points)
                    {
                        var p2 = info.Value.Points[p.Key];
                        smoothingData.Points.Add(p.Key, p2 + ((p.Value - p2) * percent));
                    }
                    part.Undo(smoothingData);
                }
            }
        }
    }
}
