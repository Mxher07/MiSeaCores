using Engine;
using Engine.Graphics;
using Game;
using Silk.NET.OpenGLES;

namespace MiSeaCore
{
    public class MiSeaCoreBlocks
    {
        public class SteelIngotBlock : Game.IngotBlock
        {
            public SteelIngotBlock() : base("IronIngot")
            {
                DefaultDisplayName = "Steel Ingot";
                DefaultDescription = "Steel Ingot";
            }
            public static int Index = 407;
        }
        public class MiSeaFlatBlock : MiSeaCrossBlocks
        {
            public static int Index = 408;
            public MiSeaFlatBlock()
               : base("Textures/FlatBlocks/MiSeaFlatBlock")  //所有flat方块的贴图整一张256x256的图片上,和原版一样，贴图是16x16
            {
                DefaultCategory = "MiSeaFlatBlock";
                InHandScale = 0.2f;
                FirstPersonScale = 0.4f;
                IsPlaceable = false;
                MaxStacking = 40;
            }
            public override int GetTextureSlotCount(int value)
            {
                return 16;
            }
            public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
            {
                return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, new Color(144, 238, 144), GetFaceTextureSlot(0, value));
            }
            public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
            {
                BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
            }
            public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
            {
            }
            public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
            {
                switch (Terrain.ExtractData(value))
                {
                    case 0:
                        return "硬皮革";
                    case 1:
                        return "纤维绳";
                    case 2:
                        return "皮革丝";
                    case 3:
                        return "碎石锤";
                    case 4:
                        return "大铜块";
                    case 5:
                        return "草之精华";
                    case 6:
                        return "石之精华";
                    case 7:
                        return "土之精华";
                    case 8:
                        return "木之精华";
                    case 9:
                        return "火之精华";
                    case 10:
                        return "金币";
                    case 11:
                        return "铜币";
                    case 12:
                        return "铁币";
                    case 13:
                        return "草榻";
                    default:
                        return "MiSeaFlatBlock";
                }
            }
            public override string GetDescription(int value)
            {
                int data = Terrain.ExtractData(value);
                string result;
                if (LanguageControl.TryGetBlock(string.Format("{0}:{1}", (object)this.GetType().Name, (object)data), "Description", out result))
                {
                    return result;
                }
                else
                {
                    switch (data)
                    {
                        case 0:
                            return "经过特殊处理的皮革，更加坚韧耐用";
                        case 1:
                            return "由植物纤维编织而成的绳索";
                        case 2:
                            return "精细加工的皮革丝线";
                        case 3:
                            return "用于研磨和粉碎的工具";
                        case 4:
                            return "纯度较高的铜块";
                        case 5:
                            return "蕴含草元素力量的精华";
                        case 6:
                            return "蕴含石元素力量的精华";
                        case 7:
                            return "蕴含土元素力量的精华";
                        case 8:
                            return "蕴含木元素力量的精华";
                        case 9:
                            return "蕴含火元素力量的精华";
                        case 10:
                            return "弥撒世界通用的金币";
                        case 11:
                            return "弥撒世界通用的铜币";
                        case 12:
                            return "弥撒世界通用的铁币";
                        case 13:
                            return "由草编织而成的简易床铺";
                        default:
                            return "弥撒核心模组中的平面方块";
                    }
                }
            }
        }
    }
    /// <summary>
    /// 弥撒抽象类
    /// </summary>
    public abstract class MiSeaCrossBlocks : FlatBlock
    {
        public Texture2D m_texture;
        public string m_textureRoute;

        public MiSeaCrossBlocks(string textureRoute)
        {
            m_textureRoute = textureRoute;
        }
        public override int GetTextureSlotCount(int value)
        {
            return 1;
        }
        public override int GetFaceTextureSlot(int face, int value)
        {
            return 0;
        }
        public override void Initialize()
        {
            m_texture = ContentManager.Get<Texture2D>(m_textureRoute);
        }
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
            generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(m_texture).SubsetAlphaTest);
        }

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            color *= BlockColorsMap.Grass.Lookup(environmentData.Temperature, environmentData.Humidity);

            BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
        }

    }
}
