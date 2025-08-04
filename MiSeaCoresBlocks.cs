using Engine;
using System;
//保持命名规范
//namespace必须是MiSeaCore
//镐类 [材质]PickaceBlock
//剑类 [材质]MacheteBlock
//斧类 [材质]AxeBlock
//同材质最好放一起并使用 //@ [材质] 标明
//便于搜索
namespace MiSeaCore {

//@ Steel/钢
    public class SteelMacheteBlock : MacheteBlock {
        public SteelPickaxeBlock()
            : base(240, 27)
            {
            }
        public static int Index = 114514;
    }

        public class SteelPickaceBlock : PickaxeBlock {
        public SteelPickaxeBlock()
            : base(27, 240)
        {
        }
        public static int Index = 114514;
    }

//@ Red/红水晶(不加Crystal是因为好拼)

    public class RedMacheteBlock : MacheteBlock {
        public RedMacheteBlock()
            : base(37, 252)
            {
            }
        public static int Index = 114514;
    }

    public class RedPickaxeBlock : PickaxeBlock {
       public RedPickaxeBlock()
            : base(37, 252)
        {
        }
        public static int Index = 114514;
    }

    public class RedAxeBlock : AxeBlock {
    public RedPickaxeBlock()
            : base(37, 252)
        {
        }
        public static int Index = 114514;
    }

    public class RedOreBlock : CubeBlock {
        public static int Index = 114514;
    }

    public class RedChunkBlock : ChunkBlock {
    public RedChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(255,0,0), smooth: false)
            {
            }
        public static int Index = 114514;
    }

//@ Purple/紫水晶 --啥子老狗Purple不是Purpie

    public class PurpleOreBlock : CubeBlock {
        public static int Index = 114514;
    }

    public class PurpleChunkBlock : ChunkBlock {
        public PurpleChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(128, 0, 128), smooth: false)
            {
            }
        public static int Index = 114514;
    }
//我留的源码为Beta1.5，这里紫水晶就俩，需要扩充

//@ Gold/金

    public class GoldBlock : CubeBlock {
        public static int Index = 114514;
    }

    public class GoldChunkBlock : ChunkBlock {
        public GoldChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(255,215,0), smooth: false)
            {
            }
        public static int Index = 114514;
    }

    public class GoldIngotBlock : MiSeaIngotBlock{
        public GoldIngotBlock()
            : base( IronIngot, 200, 200, 0, 255)
            {
            }
        public static int Index = 114514;
    }


//@ SeaStone/海蓝石

    public class SeaStoneChunkBlock : ChunkBlock {
        public SeaStoneChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(37,255,37), smooth: false)
            //这里颜色好像不对，不管了
     {
     }
        public static int Index = 114514;
    }
    
    public class SeaStoneBlock : CubeBlock {
        public static int Index = 114514;
    }

//@ Other/其他

    public class CoalPowderBlock : ChunkBlock {
        public CoalPowderBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(0,0,0), smooth: false)
            {
            }
        public static int Index = 114514;
    }

//@ Me/大肉区(后面迁移到MiSeaCoresItems或者MiSeaCoresFoods)
    public class ManBlock : FoodBlock {
        public static int Index = 114514;
    }

    public class CookMeBlock : FoodBlock {
        
        public static int Index = 114514;
    }

    public class RawMeBlock : FoodBlock {
        
        public static int Index = 114514;
    }

}

//@ MiSea/抽象类区
public abstract class MiSeaIngotBlock : Block
{
    public string m_meshName;
    public Color m_color;
    public BlockMesh m_standaloneBlockMesh = new();

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
            m_color); // 使用传入的颜色
        base.Initialize();
    }

    public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z)
    {
    }

    public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData)
    {
        BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh,m_color, 2f * size, ref matrix, environmentData);
    }
}
//该抽象类的使用方法
/*
public class TemplateBlock : MiSeaIngotBlock 
{
    public TemplateBlock()
        : base("IronIngot", 255, 0, 0, 255) // 传入RGBA值,IronIngot(以原版铁材质做稳纹理染色，如果效果不佳修改Draw的m_color为color)，这里是红色不透明
    {
    }
}
*/
//>>