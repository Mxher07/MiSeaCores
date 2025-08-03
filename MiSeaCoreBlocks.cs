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
            //Modified at 8: 37 on August 3rd, 2025. <<
               : base("Textures/FlatBlocks/MiSeaFlatBlock")  //所有flat方块的贴图整一张512x512的图片上,和原版一样，贴图是32x32
               // >>
            {
                DefaultCategory = "MiSeaFlatBlock";
                InHandScale = 0.2f;
                FirstPersonScale = 0.4f;
                IsPlaceable = false;
                MaxStacking = 40;
            }
            public override int GetTextureSlotCount(int value)
            {
            //Modified at 10: 02 on August 3rd, 2025. <<
                return 16; //666填32的这辈子有了😭😭
            //>>
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
                    //Modified at 8: 34 on August 3rd, 2025. <<
                    case 10:
                        return "金飞盘";
                    case 11:
                        return "铜飞盘";
                    case 12:
                        return "铁飞盘";
                    //>>
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
                        //Modified at 8: 36 on August 3rd, 2025. <<
                        case 0:
                            return "使用木棍支撑加固处理的皮革，更加坚韧耐用，可以用于制作衣服";
                        case 1:
                            return "由植物纤维编织而成的绳索，可以用于合成";
                        case 2:
                            return "使用剪刀裁剪的皮革丝线，可以用于合成";
                        case 3:
                            return "用于挖掘和合成的工具";
                        case 4:
                            return "防爆用方块";
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
                            return "通过精炼矿物锭制作的飞盘，投掷伤害较高";
                        case 11:
                            return "通过精炼矿物锭制作的飞盘，投掷伤害较高";
                        case 12:
                            return "通过精炼矿物锭制作的飞盘，投掷伤害较高";
                        case 13:
                            return "由草编织而成的简易床铺，也可以用来防塌陷";
                        //>>
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
