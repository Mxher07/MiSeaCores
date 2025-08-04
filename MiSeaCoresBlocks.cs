using Engine;
using Engine.Graphics;
using Game;
using System;

// 保持命名规范
// namespace必须是MiSeaCore
// 镐类 [材质]PickaceBlock
// 剑类 [材质]MacheteBlock
// 斧类 [材质]AxeBlock
// 同材质最好放一起并使用 //@ [材质] 标明
// 便于搜索
namespace MiSeaCore 
{
    #region Steel/钢材质系列方块
    
    public class SteelMacheteBlock : MacheteBlock 
    {
        public SteelMacheteBlock() : base(240, 27) { }
        public static int Index = 114514;
    }

    public class SteelPickaceBlock : PickaxeBlock 
    {
        public SteelPickaceBlock() : base(27, 240) { }
        public static int Index = 114514;
    }
    
    #endregion

    #region Red/红水晶系列方块
    
    public class RedMacheteBlock : MacheteBlock 
    {
        public RedMacheteBlock() : base(37, 252) { }
        public static int Index = 114514;
    }

    public class RedPickaxeBlock : PickaxeBlock 
    {
        public RedPickaxeBlock() : base(37, 252) { }
        public static int Index = 114514;
    }

    public class RedAxeBlock : AxeBlock 
    {
        public RedAxeBlock() : base(37, 252) { }
        public static int Index = 114514;
    }

    public class RedOreBlock : CubeBlock 
    {
        public static int Index = 114514;
    }

    public class RedChunkBlock : ChunkBlock 
    {
        public RedChunkBlock() : base(
            Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), 
            Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), 
            new Color(255, 0, 0), 
            smooth: false) { }
        
        public static int Index = 114514;
    }
    
    #endregion

    #region Purple/紫水晶系列方块
    
    public class PurpleOreBlock : CubeBlock 
    {
        public static int Index = 114514;
    }

    public class PurpleChunkBlock : ChunkBlock 
    {
        public PurpleChunkBlock() : base(
            Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), 
            Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), 
            new Color(128, 0, 128), 
            smooth: false) { }
        
        public static int Index = 114514;
    }
    
    #endregion

    #region Gold/金系列方块
    
    public class GoldBlock : CubeBlock 
    {
        public static int Index = 114514;
    }

    public class GoldChunkBlock : ChunkBlock 
    {
        public GoldChunkBlock() : base(
            Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), 
            Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), 
            new Color(255, 215, 0), 
            smooth: false) { }
        
        public static int Index = 114514;
    }

    public class GoldIngotBlock : MiSeaIngotBlock
    {
        public GoldIngotBlock() : base("IronIngot", 200, 200, 0, 255) { }
        public static int Index = 114514;

        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
        {
            // 调用基类的绘制方法
            base.DrawBlock(primitivesRenderer, value, color, size, ref matrix, environmentData);
        }
    }
    
    #endregion

    #region SeaStone/海蓝石系列方块
    
    public class SeaStoneChunkBlock : ChunkBlock 
    {
        public SeaStoneChunkBlock() : base(
            Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), 
            Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), 
            new Color(37, 255, 37), 
            smooth: false) { }
        
        public static int Index = 114514;
    }
    
    public class SeaStoneBlock : CubeBlock 
    {
        public static int Index = 114514;
    }
    
    #endregion

    #region Other/其他系列方块
    
    public class CoalPowderBlock : ChunkBlock 
    {
        public CoalPowderBlock() : base(
            Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), 
            Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), 
            new Color(0, 0, 0), 
            smooth: false) { }
        
        public static int Index = 114514;
    }
    
    #endregion
}

#region MiSea/抽象类区

/// <summary>
/// 自定义金属锭方块抽象类，支持自定义颜色和模型
/// </summary>
public abstract class MiSeaIngotBlock : Block
{
    protected string m_meshName;
    protected Color m_color;
    protected BlockMesh m_standaloneBlockMesh = new BlockMesh();

    /// <summary>
    /// 初始化金属锭方块
    /// </summary>
    /// <param name="meshName">模型网格名称</param>
    /// <param name="r">红色分量</param>
    /// <param name="g">绿色分量</param>
    /// <param name="b">蓝色分量</param>
    /// <param name="a">透明度分量</param>
    public MiSeaIngotBlock(string meshName, byte r, byte g, byte b, byte a)
    {
        m_meshName = meshName;
        m_color = new Color(r, g, b, a);
    }

    public override void Initialize()
    {
        Model model = ContentManager.Get<Model>("Models/Ingots");
        Matrix boneAbsoluteTransform = BlockMesh.GetBoneAbsoluteTransform(model.FindMesh(m_meshName).ParentBone);
        
        m_standaloneBlockMesh.AppendModelMeshPart(
            model.FindMesh(m_meshName).MeshParts[0],
            boneAbsoluteTransform * Matrix.CreateTranslation(0f, -0.1f, 0f),
            makeEmissive: false,
            flipWindingOrder: false,
            doubleSided: false,
            flipNormals: false,
            m_color);
            
        base.Initialize();
    }

    public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
    {
        // 空实现，使用默认网格渲染
    }

    public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
    {
        BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, m_color, 2f * size, ref matrix, environmentData);
    }
}

#endregion