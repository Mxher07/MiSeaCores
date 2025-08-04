using Engine;
using Engine.Graphics;
using Game;
using Silk.NET.OpenGLES;

namespace MiSeaCores
{
    /// <summary>
    /// 定义弥撒核心模组中的方块类型集合。
    /// </summary>
    public class MiSeaCoreBlocks
    {
        /// <summary>
        /// 钢锭方块类，继承自游戏中的IngotBlock基类。
        /// </summary>
        public class SteelIngotBlock : Game.IngotBlock
        {
            /// <summary>
            /// 初始化钢锭方块实例，并设置默认显示名称和描述。
            /// </summary>
            public SteelIngotBlock() : base("IronIngot")
            {
                DefaultDisplayName = "Steel Ingot";
                DefaultDescription = "Steel Ingot";
            }

            /// <summary>
            /// 表示该方块在游戏中的索引编号。
            /// </summary>
            public static int Index = 407;
        }

        /// <summary>
        /// 弥撒平面方块类，继承自MiSeaCrossBlocks抽象类。
        /// </summary>
        public class MiSeaFlatBlock : MiSeaCrossBlocks
        {
            /// <summary>
            /// 表示该方块在游戏中的索引编号。
            /// </summary>
            public static int Index = 408;

            /// <summary>
            /// 初始化弥撒平面方块实例，并设置默认属性。
            /// </summary>
            public MiSeaFlatBlock()
               : base("Textures/FlatBlocks/MiSeaFlatBlock")
            {
                DefaultCategory = "MiSeaFlatBlock";
                InHandScale = 0.2f;
                FirstPersonScale = 0.4f;
                IsPlaceable = false;
                MaxStacking = 40;
            }

            /// <summary>
            /// 获取指定值对应的纹理槽位数量。
            /// </summary>
            /// <param name="value">方块的值。</param>
            /// <returns>返回固定值16。</returns>
            public override int GetTextureSlotCount(int value)
            {
                return 16; 
            }

            /// <summary>
            /// 创建方块被破坏时的粒子系统。
            /// </summary>
            /// <param name="subsystemTerrain">地形子系统。</param>
            /// <param name="position">粒子生成的位置。</param>
            /// <param name="value">方块的值。</param>
            /// <param name="strength">破坏强度。</param>
            /// <returns>返回创建的粒子系统。</returns>
            public override BlockDebrisParticleSystem CreateDebrisParticleSystem(SubsystemTerrain subsystemTerrain, Vector3 position, int value, float strength)
            {
                return new BlockDebrisParticleSystem(subsystemTerrain, position, strength, DestructionDebrisScale, new Color(144, 238, 144), GetFaceTextureSlot(0, value));
            }

            /// <summary>
            /// 绘制方块的3D模型。
            /// </summary>
            /// <param name="primitivesRenderer">图元渲染器。</param>
            /// <param name="value">方块的值。</param>
            /// <param name="color">绘制颜色。</param>
            /// <param name="size">绘制大小。</param>
            /// <param name="matrix">变换矩阵。</param>
            /// <param name="environmentData">环境数据。</param>
            public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
            {
                BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
            }

            /// <summary>
            /// 生成地形顶点数据（此方法为空实现）。
            /// </summary>
            /// <param name="generator">方块几何生成器。</param>
            /// <param name="geometry">地形几何对象。</param>
            /// <param name="value">方块的值。</param>
            /// <param name="x">X坐标。</param>
            /// <param name="y">Y坐标。</param>
            /// <param name="z">Z坐标。</param>
            public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
            {
            }

            /// <summary>
            /// 根据方块值获取其显示名称。
            /// </summary>
            /// <param name="subsystemTerrain">地形子系统。</param>
            /// <param name="value">方块的值。</param>
            /// <returns>返回根据数据提取的中文名称。</returns>
            public override string GetDisplayName(SubsystemTerrain subsystemTerrain, int value)
            {
                switch (Terrain.ExtractData(value))
                {
                    case 0:
                        return "纤维绳";
                    case 1:
                        return "皮革丝";
                    case 2:
                        return "碎石锤";
                    case 3:
                        return "草之精华";
                    case 4:
                        return "石之精华";
                    case 5:
                        return "土之精华";
                    case 6:
                        return "木之精华";
                    case 7:
                        return "火之精华";
                    case 8:
                        return "金飞盘";
                    case 9:
                        return "铜飞盘";
                    case 10:
                        return "铁飞盘";
                    case 11:
                        return "草榻";
                    default:
                        return "MiSeaFlatBlock";
                }
            }

            /// <summary>
            /// 获取方块的详细描述信息。
            /// </summary>
            /// <param name="value">方块的值。</param>
            /// <returns>返回根据数据提取的中文描述。</returns>
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
                            return "由植物纤维编织而成的绳索，可以用于合成";
                        case 1:
                            return "使用剪刀裁剪的皮革丝线，可以用于合成";
                        case 2:
                            return "用于挖掘和合成的工具";
                        case 3:
                            return "蕴含草元素力量的精华";
                        case 4:
                            return "蕴含石元素力量的精华";
                        case 5:
                            return "蕴含土元素力量的精华";
                        case 6:
                            return "蕴含木元素力量的精华";
                        case 7:
                            return "蕴含火元素力量的精华";
                        case 8:
                            return "通过精炼矿物锭制作的飞盘，投掷伤害较高";
                        case 9:
                            return "通过精炼矿物锭制作的飞盘，投掷伤害较高";
                        case 10:
                            return "通过精炼矿物锭制作的飞盘，投掷伤害较高";
                        case 11:
                            return "由草编织而成的简易床铺，也可以用来防塌陷";
                        default:
                            return "弥撒核心模组中的平面方块";
                    }
                }
            }
        }
    }

    /// <summary>
    /// 弥撒抽象类，继承自FlatBlock，用于定义具有特定纹理和行为的平面方块。
    /// </summary>
    public abstract class MiSeaCrossBlocks : FlatBlock
    {
        /// <summary>
        /// 方块使用的纹理资源。
        /// </summary>
        public Texture2D m_texture;

        /// <summary>
        /// 纹理资源的路径。
        /// </summary>
        public string m_textureRoute;

        /// <summary>
        /// 初始化弥撒交叉方块实例并设置纹理路径。
        /// </summary>
        /// <param name="textureRoute">纹理资源的路径。</param>
        public MiSeaCrossBlocks(string textureRoute)
        {
            m_textureRoute = textureRoute;
        }

        /// <summary>
        /// 获取指定值对应的纹理槽位数量，默认返回1。
        /// </summary>
        /// <param name="value">方块的值。</param>
        /// <returns>返回固定值1。</returns>
        public override int GetTextureSlotCount(int value)
        {
            return 1;
        }

        /// <summary>
        /// 获取指定面和值对应的纹理槽位，默认返回0。
        /// </summary>
        /// <param name="face">面索引。</param>
        /// <param name="value">方块的值。</param>
        /// <returns>返回固定值0。</returns>
        public override int GetFaceTextureSlot(int face, int value)
        {
            return 0;
        }

        /// <summary>
        /// 初始化方块资源，加载纹理。
        /// </summary>
        public override void Initialize()
        {
            m_texture = ContentManager.Get<Texture2D>(m_textureRoute);
        }

        /// <summary>
        /// 生成地形顶点数据，调用生成器生成交叉面顶点。
        /// </summary>
        /// <param name="generator">方块几何生成器。</param>
        /// <param name="geometry">地形几何对象。</param>
        /// <param name="value">方块的值。</param>
        /// <param name="x">X坐标。</param>
        /// <param name="y">Y坐标。</param>
        /// <param name="z">Z坐标。</param>
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
        {
            generator.GenerateCrossfaceVertices(this, value, x, y, z, Color.White, GetFaceTextureSlot(0, value), geometry.GetGeometry(m_texture).SubsetAlphaTest);
        }

        /// <summary>
        /// 绘制方块的3D模型，并根据环境数据调整颜色。
        /// </summary>
        /// <param name="primitivesRenderer">图元渲染器。</param>
        /// <param name="value">方块的值。</param>
        /// <param name="color">绘制颜色。</param>
        /// <param name="size">绘制大小。</param>
        /// <param name="matrix">变换矩阵。</param>
        /// <param name="environmentData">环境数据。</param>
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            color *= BlockColorsMap.Grass.Lookup(environmentData.Temperature, environmentData.Humidity);

            BlocksManager.DrawFlatOrImageExtrusionBlock(primitivesRenderer, value, size, ref matrix, m_texture, color, isEmissive: false, environmentData);
        }
    }
}