//Modified at 3: 11 on August 4rd, 2025. <<
using Engine;
using System;
//我不知道Initialize是否必须实现，反正全写了
//保持命名规范
//namespace必须是MiSeaCore
//镐类 [材质]PickaceBlock
//剑类 [材质]MacheteBlock
//斧类 [材质]AxeBlock
//同材质最好放一起并使用 //@ [材质] 标明
//便于搜索
namespace MiSeaCore {

//@ Steel/钢
//锭在MiSeaCoresFlatBlocks里被实现，记得移过来
    public class SteelMacheteBlock : MacheteBlock {
        public SteelPickaxeBlock()
            : base(240, 27)
            {
            }
        public override void Initialize() {
            base.Initialize();
        }
    }

        public class SteelPickaceBlock : PickaxeBlock {
        public SteelPickaxeBlock()
            : base(27, 240)
        {
        }
        public override void Initialize() {
            base.Initialize();
        }
    }

//@ Red/红水晶(不加Crystal是因为好拼)

    public class RedMacheteBlock : MacheteBlock {
        public RedMacheteBlock()
            : base(37, 252)
            {
            }
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class RedPickaxeBlock : PickaxeBlock {
       public RedPickaxeBlock()
            : base(37, 252)
        {
        }
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class RedAxeBlock : AxeBlock {
    public RedPickaxeBlock()
            : base(37, 252)
        {
        }
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class RedOreBlock : CubeBlock {
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class RedChunkBlock : ChunkBlock {
    public RedChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(255,0,0), smooth: false)
            {
            }
        public override void Initialize() {
            base.Initialize();
        }
    }

//@ Purple/紫水晶 --啥子老狗Purple不是Purpie

    public class PurpleOreBlock : CubeBlock {
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class PurpleChunkBlock : ChunkBlock {
        public PurpleChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(128, 0, 128), smooth: false)
            {
            }
        public override void Initialize() {
            base.Initialize();
        }
    }
//我留的源码为Beta1.5，这里紫水晶就俩，需要扩充

//@ Gold/金

    public class GoldBlock : CubeBlock {
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class GoldenChunkBlock : ChunkBlock {
        public GoldenChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(255,215,0), smooth: false)
            {
            }
        public override void Initialize() {
            base.Initialize();
        }
    }

/*
//============这个需要重写，但是我不会，记得改成用原版锭===========
    public class GoldenIngotBlock : IronIngotBlock {
        public override void Initialize() {
            base.Initialize();
        }
        
        public override void GenerateTerrainVertices(BlockGeometryGenerator generator, TerrainGeometry geometry, int value, int x, int y, int z) {
            // 保留原有方法实现
        }
        
        public override void DrawBlock(PrimitivesRenderer3D primitivesRenderer, int value, Color color, float size, ref Matrix matrix, DrawBlockEnvironmentData environmentData) {
            // 保留原有方法实现
            BlocksManager.DrawMeshBlock(primitivesRenderer, m_standaloneBlockMesh, Color.Yellow, 2f * size, ref matrix, environmentData);
        }
    }
    */

//@ SeaStone/海蓝石

    public class SeaStoneChunkBlock : ChunkBlock {
        public SeaStoneChunkBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(37,255,37), smooth: false)
            //这里颜色好像不对，不管了
     {
     }
        public override void Initialize() {
            base.Initialize();
        }
    }
    
    public class SeaStoneBlock : CubeBlock {
        public override void Initialize() {
            base.Initialize();
        }
    }

//@ Other/其他

    public class CoalPowderBlock : ChunkBlock {
        public CoalPowderBlock()
            : base(Matrix.CreateRotationX(1f) * Matrix.CreateRotationZ(0f), Matrix.CreateTranslation(0.0625f, 0.4375f, 0f), new Color(0,0,0), smooth: false)
            {
            }
        public override void Initialize() {
            base.Initialize();
        }
    }

//@ Me/大肉区(后面迁移到MiSeaCoresItems或者MiSeaCoresFoods)
    public class ManBlock : FoodBlock {
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class CookMeBlock : FoodBlock {
        
        public override void Initialize() {
            base.Initialize();
        }
    }

    public class RawMeBlock : FoodBlock {
        
        public override void Initialize() {
            base.Initialize();
        }
    }

}
//>>