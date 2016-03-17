using System;
using System.Text;
using System.Xml;

namespace RH.HeadShop.Render.Helpers
{
    public class ColladaExporter
    {
        public static void AddMaterial(COLLADA collada, String textureFileName)
        {
            
        }

        public static void AddGeometry()
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

            collada.Items = new object[5];
            collada.Items[0] = new library_images();
            collada.Items[1] = new library_materials();
            collada.Items[2] = new library_effects();
            collada.Items[3] = new library_geometries();
            collada.Items[4] = new library_visual_scenes
            {
                visual_scene = new []
                {
                    new visual_scene
                    {
                        id = "DefaultScene",
                        node = new []
                        {
                            new node
                            {
                                id = "RootNode",
                                name = "RootNode",
                                //node1 = new []
                                //{
                                //    //node

                                //}
                            }
                        }
                    }
                }
            };
        }
    }
}
