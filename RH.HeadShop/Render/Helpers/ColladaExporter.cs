using System;
using System.Text;
using System.Xml;

namespace RH.HeadShop.Render.Helpers
{
    public class ColladaExporter
    {
        public static void AddMaterial(String textureFileName)
        {

        }

        public static void Initialize(COLLADA collada)
        {
            collada.asset = new asset
            {
                modified = DateTime.Now,
                created = DateTime.Now,
                unit = new assetUnit
                {
                    meter = 0.0099999997,
                    name = "cm"
                },
                up_axis = UpAxisType.Y_UP,
                contributor = new[]
                {
                    new assetContributor
                    {
                       authoring_tool = "DAZ Studio 4.6"
                    }
                }
            };

            collada.scene = new COLLADAScene
            {
                instance_visual_scene = new InstanceWithExtra
                {
                    url = "#DefaultScene"
                }
            };

            collada.version = VersionType.Item141;
        }
    }
}
