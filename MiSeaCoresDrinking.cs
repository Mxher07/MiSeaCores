//using Engine;
//using Game;
//using GameEntitySystem;
//using System;
//using System.Text;
//using System.Xml.Linq;
//using TemplatesDatabase;
//using XmlUtilities;

//namespace MiSeaCore
//{
//    /// <summary>
//    /// 存储水相关数据的结构体，包括方块值、水量和生病概率。
//    /// </summary>
//    public class WaterData
//    {
//        public int BlockValue;
//        public float WaterValue;
//        public float SicknessProbability;
//    }

//    /// <summary>
//    /// 玩家饮水系统组件，实现饮水机制、UI显示和脱水效果。
//    /// </summary>
//    public class ComponentMiSeaCoresDrinking : Component, IUpdateable
//    {
//        private float m_water;
//        private float m_lastWater;
//        private Game.Random m_random = new Game.Random();

//        private SubsystemGameInfo m_subsystemGameInfo;
//        private SubsystemTime m_subsystemTime;
//        private ComponentPlayer m_componentPlayer;
//        private SubsystemAudio m_subsystemAudio;
//        private ComponentCreature m_componentCreature;

//        /// <summary>
//        /// 获取或设置当前玩家的水量，值被限制在 [0, 1] 范围内。
//        /// </summary>
//        public float Water
//        {
//            get => m_water;
//            set => m_water = MathUtils.Saturate(value);
//        }

//        /// <summary>
//        /// 获取更新顺序，默认值。
//        /// </summary>
//        public UpdateOrder UpdateOrder => UpdateOrder.Default;

//        /// <summary>
//        /// 每帧更新方法，处理饮水逻辑、UI更新和脱水效果。
//        /// </summary>
//        /// <param name="dt">时间增量（秒）</param>
//        public void Update(float dt)
//        {
//            if (m_componentPlayer.ComponentHealth.Health <= 0f)
//                return;

//            UpdateWater();

//            Widget guiModalPanel = m_componentPlayer.ComponentGui.ModalPanelWidget;
//            if (guiModalPanel is ClothingWidget clothingWidget)
//            {
//                BevelledButtonWidget waterButton = clothingWidget.Children.Find<BevelledButtonWidget>("WaterBotton", true);
//                Widget iconWidget = waterButton.Children.Find<RectangleWidget>("WaterIcon", true);

//                Vector3 playerPosition = m_componentPlayer.ComponentBody.Position;
//                Vector3 cellPosition = new Vector3(
//                    Terrain.ToCell(playerPosition.X),
//                    Terrain.ToCell(playerPosition.Y) - 1,
//                    Terrain.ToCell(playerPosition.Z)
//                );

//                int cellX = Terrain.ToCell(cellPosition.X);
//                int cellY = Terrain.ToCell(cellPosition.Y);
//                int cellZ = Terrain.ToCell(cellPosition.Z);

//                bool isNearWater = CheckWaterInSurroundingCells(cellX, cellY, cellZ);

//                float immersion = m_componentPlayer.ComponentBody.ImmersionFactor;
//                bool isSneaking = m_componentPlayer.ComponentBody.IsSneaking;

//                // 根据玩家是否在水中或靠近水源设置图标颜色
//                if (immersion > 0.25f)
//                    iconWidget.ColorTransform = Color.InkBlue * immersion;
//                else if (isSneaking && isNearWater)
//                    iconWidget.ColorTransform = Color.InkBlue;
//                else
//                    iconWidget.ColorTransform = Color.Gray;

//                if (waterButton.IsClicked)
//                {
//                    ProcessWaterConsumption(playerPosition, cellX, cellY, cellZ);
//                }

//                waterButton.IsEnabled = (isSneaking && isNearWater) || immersion > 0f;
//            }
//        }

//        /// <summary>
//        /// 检查玩家周围是否有水源方块（ID为18）。
//        /// </summary>
//        /// <param name="x">中心格子的X坐标</param>
//        /// <param name="y">中心格子的Y坐标</param>
//        /// <param name="z">中心格子的Z坐标</param>
//        /// <returns>如果周围有水则返回true，否则返回false</returns>
//        private bool CheckWaterInSurroundingCells(int x, int y, int z)
//        {
//            SubsystemTerrain terrain = m_componentPlayer.m_subsystemTerrain;

//            int[,] offsets = {
//            {-1, 0, 1}, {1, 0, -1}, {-1, 0, -1}, {1, 0, 1},
//            {-1, 0, 0}, {0, 0, -1}, {1, 0, 0}, {0, 0, 1},
//            {0, 0, 0}
//            };

//            for (int i = 0; i < offsets.GetLength(0); i++)
//            {
//                if (terrain.Terrain.GetCellContents(x + offsets[i, 0], y + offsets[i, 1], z + offsets[i, 2]) == 18)
//                    return true;
//            }

//            return false;
//        }

//        /// <summary>
//        /// 处理玩家饮水逻辑，包括播放音效、增加水量、处理过量饮水和生病概率。
//        /// </summary>
//        /// <param name="position">玩家位置</param>
//        /// <param name="x">玩家所在格子的X坐标</param>
//        /// <param name="y">玩家所在格子的Y坐标</param>
//        /// <param name="z">玩家所在格子的Z坐标</param>
//        private void ProcessWaterConsumption(Vector3 position, int x, int y, int z)
//        {
//            float immersionFactor = m_componentPlayer.ComponentBody.ImmersionFactor;
//            float waterGain = m_random.Float(0.05f, 0.15f) * MathUtils.Max(immersionFactor, 0.25f);

//            m_componentPlayer.m_subsystemAudio.PlaySound("Audio/Sinking", 0.3f, 0f, position, 0.5f, true);

//            float overConsumption = MathUtils.Clamp((Water + waterGain - 1f) / 2f, 0f, 0.25f);
//            Water += waterGain;

//            if (m_random.Bool(0.00083116884f))
//            {
//                HandleSickness();
//            }

//            if (overConsumption > 0f)
//            {
//                HandleOverConsumption(overConsumption);
//            }
//        }

//        /// <summary>
//        /// 处理玩家生病逻辑，如果当前未生病则开始生病，否则延长生病时间。
//        /// </summary>
//        private void HandleSickness()
//        {
//            ComponentSickness sicknessComponent = m_componentPlayer.ComponentSickness;

//            if (!sicknessComponent.IsSick && sicknessComponent.m_sicknessDuration == 0f)
//            {
//                sicknessComponent.StartSickness();
//                sicknessComponent.m_sicknessDuration = 5f;
//            }
//            else
//            {
//                sicknessComponent.m_sicknessDuration += 5f;
//            }
//        }

//        /// <summary>
//        /// 处理过量饮水的后果，包括伤害玩家、减少食物值和可能引发生病。
//        /// </summary>
//        /// <param name="overConsumptionAmount">过量饮水的量</param>
//        private void HandleOverConsumption(float overConsumptionAmount)
//        {
//            m_componentPlayer.ComponentHealth.Injure(overConsumptionAmount / 2f, null, false, "\n撑死了?\nSupport to death");

//            if (m_componentPlayer.ComponentSickness.m_sicknessDuration == 0f)
//            {
//                m_componentPlayer.ComponentSickness.StartSickness();
//                m_componentPlayer.ComponentSickness.m_sicknessDuration = 0.5f;
//            }

//            m_componentPlayer.ComponentVitalStats.Food -= overConsumptionAmount;
//        }

//        /// <summary>
//        /// 从存档中加载组件数据。
//        /// </summary>
//        /// <param name="valuesDictionary">包含组件数据的字典</param>
//        /// <param name="idToEntityMap">ID到实体的映射表</param>
//        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
//        {
//            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
//            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
//            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(true);
//            m_subsystemAudio = Project.FindSubsystem<SubsystemAudio>(true);
//            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);

//            Water = valuesDictionary.GetValue<float>("Water");
//            m_lastWater = Water;
//        }

//        /// <summary>
//        /// 将组件数据保存到存档中。
//        /// </summary>
//        /// <param name="valuesDictionary">用于保存数据的字典</param>
//        /// <param name="entityToIdMap">实体到ID的映射表</param>
//        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
//        {
//            valuesDictionary.SetValue("Water", Water);
//        }

//        /// <summary>
//        /// 当实体被添加到世界时调用，创建饮水UI。
//        /// </summary>
//        public override void OnEntityAdded()
//        {
//            if (m_componentPlayer.ComponentGui == null)
//                return;

//            CreateWaterBarUI();
//        }

//        /// <summary>
//        /// 创建饮水UI，包括上下两个水条和容器。
//        /// </summary>
//        private void CreateWaterBarUI()
//        {
//            ValueBarWidget lowerBar = new ValueBarWidget
//            {
//                Name = "WaterL",
//                LayoutDirection = LayoutDirection.Horizontal,
//                VerticalAlignment = WidgetAlignment.Center,
//                BarsCount = 10,
//                BarBlending = false,
//                HalfBars = true,
//                LitBarColor = Color.SkyBlue,
//                UnlitBarColor = Color.Transparent,
//                BarSize = new Vector2(9f, 15f),
//                Spacing = 1.25f,
//                BarSubtexture = ContentManager.Get<Subtexture>("MiSeaCoresDrinking/WaterL"),
//                TextureLinearFilter = false,
//                Value = 0.56f
//            };

//            ValueBarWidget upperBar = new ValueBarWidget
//            {
//                Name = "WaterU",
//                LayoutDirection = LayoutDirection.Horizontal,
//                VerticalAlignment = WidgetAlignment.Center,
//                BarsCount = 10,
//                BarBlending = false,
//                HalfBars = true,
//                LitBarColor = Color.White,
//                UnlitBarColor = Color.White,
//                BarSize = new Vector2(9f, 15f),
//                Spacing = 1.25f,
//                BarSubtexture = ContentManager.Get<Subtexture>("MiSeaCoresDrinking/WaterU"),
//                TextureLinearFilter = true
//            };

//            CanvasWidget container = new CanvasWidget
//            {
//                Name = "MiSeaCoresWaterBarList",
//                VerticalAlignment = WidgetAlignment.Far,
//                HorizontalAlignment = WidgetAlignment.Center,
//                Margin = new Vector2(0f, 90f)
//            };

//            CanvasWidget barContainer = new CanvasWidget();
//            StackPanelWidget stackPanel = new StackPanelWidget
//            {
//                Direction = LayoutDirection.Horizontal
//            };

//            barContainer.Children.Add(lowerBar);
//            barContainer.Children.Add(upperBar);

//            stackPanel.Children.Add(new CanvasWidget { Size = new Vector2(300f, 0f) });
//            stackPanel.Children.Add(new CanvasWidget { Size = new Vector2(0f, 0f) });
//            stackPanel.Children.Add(barContainer);

//            container.Children.Add(stackPanel);
//            m_componentPlayer.ComponentGui.ControlsContainerWidget.Children.Add(container);
//        }

//        /// <summary>
//        /// 当实体从世界中移除时调用，移除饮水UI。
//        /// </summary>
//        public override void OnEntityRemoved()
//        {
//            if (m_componentPlayer.ComponentGui == null)
//                return;

//            RemoveWaterBarUI();
//        }

//        /// <summary>
//        /// 移除饮水UI组件。
//        /// </summary>
//        private void RemoveWaterBarUI()
//        {
//            CanvasWidget waterBarList = m_componentPlayer.ComponentGui.ControlsContainerWidget
//                .Children.Find<CanvasWidget>("MiSeaCoresWaterBarList", true);

//            if (waterBarList != null)
//            {
//                m_componentPlayer.ComponentGui.ControlsContainerWidget.Children.Remove(waterBarList);
//            }
//        }

//        /// <summary>
//        /// 更新玩家的水量，根据游戏模式和玩家行为消耗水量。
//        /// </summary>
//        public void UpdateWater()
//        {
//            float timeDelta = m_subsystemTime.GameTimeDelta;
//            WorldSettings settings = m_subsystemGameInfo.WorldSettings;

//            if (settings.GameMode != null && settings.AreAdventureSurvivalMechanicsEnabled)
//            {
//                float hunger = m_componentPlayer.ComponentLevel.HungerFactor;

//                Water -= hunger * timeDelta / 2180f;

//                float walkDistance = m_componentPlayer.ComponentLocomotion.LastWalkOrder?.Length() ?? 0f;
//                Water -= hunger * timeDelta * walkDistance / 2880f;

//                Water -= hunger * m_componentPlayer.ComponentLocomotion.LastJumpOrder / 1280f;

//                if (m_componentPlayer.ComponentMiner.DigCellFace != null)
//                {
//                    Water -= hunger * timeDelta / 2880f;
//                }

//                ComponentSleep sleepComponent = m_componentPlayer.ComponentSleep;
//                if (!sleepComponent.IsSleeping)
//                {
//                    HandleDehydrationEffects();
//                }
//            }
//            else
//            {
//                Water = 0.95f;
//            }
//            m_lastWater = Water;
//        }

//        /// <summary>
//        /// 处理脱水效果，如果玩家水量为0则定期造成伤害。
//        /// </summary>
//        private void HandleDehydrationEffects()
//        {
//            if (Water <= 0f)
//            {
//                if (m_subsystemTime.PeriodicGameTimeEvent(2.5, 0.0))
//                {
//                    m_componentPlayer.ComponentHealth.Injure(0.08f, null, false, "你渴死了.");
//                    m_componentPlayer.ComponentVitalStats.Stamina -= 0.1f;

//                    if (m_subsystemGameInfo.WorldSettings.GameMode != GameMode.Creative)
//                    {
//                        m_componentPlayer.ComponentVitalStats.Sleep -= 0.005f;
//                    }

//                    m_componentPlayer.ComponentVitalStats.Wetness -= 0.2f;
//                }
//            }
//            else
//            {
//                ShowDehydrationWarnings();
//            }
//        }

//        /// <summary>
//        /// 显示脱水警告信息，根据水量变化显示不同级别的警告。
//        /// </summary>
//        private void ShowDehydrationWarnings()
//        {
//            if (Water < 0.25f && m_lastWater >= 0.25f)
//            {
//                m_componentPlayer.ComponentGui.DisplaySmallMessage("要脱水了！", Color.Red, true, true);
//            }
//            else if (Water < 0.5f && m_lastWater >= 0.5f)
//            {
//                m_componentPlayer.ComponentGui.DisplaySmallMessage("好渴！", Color.White, true, true);
//            }
//            else if (Water < 0.85f && m_lastWater >= 0.85f)
//            {
//                m_componentPlayer.ComponentGui.DisplaySmallMessage("有点渴!", Color.White, true, true);
//            }
//        }
//    }

//    /// <summary>
//    /// 饮水模组加载器类，用于处理与饮水相关的逻辑，包括UI显示、饮水值更新、物品消耗等。
//    /// </summary>
//    public class MiSeaCoresDrinkingModLoader : ModLoader
//    {
//        /// <summary>
//        /// 存储所有饮水数据的静态列表。
//        /// </summary>
//        private static List<WaterData> m_waterDatas = new List<WaterData>();

//        /// <summary>
//        /// 存储从XML格式的文件加载的饮水物品配置数据。
//        /// </summary>
//        private XElement items;

//        /// <summary>
//        /// 获取只读的饮水数据列表。
//        /// </summary>
//        public static ReadOnlyList<WaterData> WaterDatas
//        {
//            get
//            {
//                return new ReadOnlyList<WaterData>(m_waterDatas);
//            }
//        }

//        /// <summary>
//        /// 模组初始化方法，注册各种钩子并加载饮水数据。
//        /// </summary>
//        public override void __ModInitialize()
//        {
//            ModsManager.RegisterHook("GuiUpdate", this);
//            ModsManager.RegisterHook("ClothingProcessSlotItems", this);
//            ModsManager.RegisterHook("BlocksInitalized", this);
//            ModsManager.RegisterHook("ClothingWidgetOpen", this);
//            items = LoadWaterData();
//        }

//        /// <summary>
//        /// 当服装界面打开时，添加饮水按钮到界面中。
//        /// </summary>
//        /// <param name="componentGui">GUI组件。</param>
//        /// <param name="clothingWidget">服装界面控件。</param>
//        public override void ClothingWidgetOpen(ComponentGui componentGui, ClothingWidget clothingWidget)
//        {
//            // 创建按钮容器
//            CanvasWidget buttonContainer = new CanvasWidget
//            {
//                Name = "WaterBottonC",
//                Size = new Vector2(80f, 80f)
//            };

//            // 设置按钮容器位置
//            clothingWidget.SetWidgetPosition(buttonContainer, new Vector2?(new Vector2(24f, 64f)));

//            // 创建垂直堆叠面板
//            StackPanelWidget stackPanel = new StackPanelWidget
//            {
//                Direction = LayoutDirection.Vertical,
//                VerticalAlignment = WidgetAlignment.Center
//            };

//            // 创建饮水按钮
//            BevelledButtonWidget drinkButton = new BevelledButtonWidget
//            {
//                Name = "WaterBotton",
//                Text = string.Empty,
//                Style = ContentManager.Get<XElement>("Styles/ButtonStyle_70x60")
//            };

//            // 创建高亮图标
//            RectangleWidget iconHighlighted = new RectangleWidget
//            {
//                Name = "WaterIcon",
//                Size = new Vector2(40.01f),
//                OutlineColor = Color.Transparent,
//                FillColor = Color.White,
//                Subtexture = ContentManager.Get<Subtexture>("MiSeaCoresDrinking/WaterL"),
//                ColorTransform = Color.InkBlue,
//                HorizontalAlignment = WidgetAlignment.Center,
//                VerticalAlignment = WidgetAlignment.Center
//            };

//            // 创建普通图标
//            RectangleWidget iconNormal = new RectangleWidget
//            {
//                Size = new Vector2(40f),
//                OutlineColor = Color.Transparent,
//                FillColor = Color.White,
//                Subtexture = ContentManager.Get<Subtexture>("MiSeaCoresDrinking/WaterL"),
//                ColorTransform = Color.White,
//                HorizontalAlignment = WidgetAlignment.Center,
//                VerticalAlignment = WidgetAlignment.Center
//            };

//            // 添加图标到按钮
//            drinkButton.Children.Add(iconNormal);
//            drinkButton.Children.Add(iconHighlighted);

//            // 添加按钮到面板和容器
//            stackPanel.Children.Add(drinkButton);
//            buttonContainer.Children.Add(stackPanel);
//            clothingWidget.Children.Add(buttonContainer);
//        }

//        /// <summary>
//        /// GUI更新方法，用于更新饮水条的显示。
//        /// </summary>
//        /// <param name="componentGui">GUI组件。</param>
//        public override void GuiUpdate(ComponentGui componentGui)
//        {
//            // 如果玩家死亡，则不更新饮水条
//            if (componentGui.m_componentPlayer.ComponentHealth.Health <= 0f)
//                return;

//            // 查找饮水条控件并更新其值
//            ValueBarWidget waterBar = componentGui.ControlsContainerWidget.Children.Find<ValueBarWidget>("WaterL", true);
//            if (waterBar != null)
//            {
//                ComponentMiSeaCoresDrinking drinkingComponent = componentGui.Entity.FindComponent<ComponentMiSeaCoresDrinking>();
//                if (drinkingComponent != null)
//                {
//                    waterBar.Value = drinkingComponent.Water;
//                }
//            }

//            // 控制饮水条容器的可见性
//            CanvasWidget waterBarContainer = componentGui.ControlsContainerWidget.Children.Find<CanvasWidget>("MiSeaCoresWaterBarList", true);
//            if (waterBarContainer != null)
//            {
//                waterBarContainer.IsVisible = componentGui.m_subsystemGameInfo.WorldSettings.GameMode != null;
//            }
//        }

//        /// <summary>
//        /// 处理玩家使用饮水物品的逻辑。
//        /// </summary>
//        /// <param name="componentPlayer">玩家组件。</param>
//        /// <param name="block">方块对象。</param>
//        /// <param name="slotIndex">物品槽索引。</param>
//        /// <param name="value">物品值。</param>
//        /// <param name="count">物品数量。</param>
//        /// <returns>是否处理成功。</returns>
//        public override bool ClothingProcessSlotItems(ComponentPlayer componentPlayer, Block block, int slotIndex, int value, int count)
//        {
//            Game.Random randomGenerator = new Game.Random();
//            WaterData waterData = m_waterDatas.Find((WaterData data) => data.BlockValue == value);

//            ComponentMiSeaCoresDrinking drinkingComponent = componentPlayer.Entity.FindComponent<ComponentMiSeaCoresDrinking>();

//            bool hasNutritionalValue = block.GetNutritionalValue(value) > 0f;
//            bool isHungerLow = componentPlayer.ComponentVitalStats.Food < 0.98f;
//            bool isValidWaterItem = waterData != null && drinkingComponent != null;
//            int emptybucketid = BlocksManager.GetBlockIndex<EmptyBucketBlock>();

//            // 判断是否可以饮水
//            if (((hasNutritionalValue && isHungerLow) || !hasNutritionalValue) && isValidWaterItem)
//            {
//                // 如果不是食物，则移除物品并可能添加桶
//                if (!hasNutritionalValue)
//                {
//                    componentPlayer.ComponentMiner.Inventory.RemoveSlotItems(slotIndex, 1);
//                    if (block is BucketBlock)
//                    {
//                        componentPlayer.ComponentMiner.Inventory.AddSlotItems(slotIndex, emptybucketid, 1);
//                    }
//                }

//                // 计算过量饮水值并更新饮水值
//                float overConsumption = MathUtils.Clamp((drinkingComponent.Water + waterData.WaterValue - 1f) / 2f, 0f, 0.25f);
//                drinkingComponent.Water += waterData.WaterValue;

//                // 播放饮水音效
//                componentPlayer.m_subsystemAudio.PlaySound("Audio/Sinking", 1f, 0f, componentPlayer.ComponentBody.Position, 1f, true);

//                // 如果过量饮水，则造成伤害并可能引发疾病
//                if (overConsumption > 0f)
//                {
//                    componentPlayer.ComponentHealth.Injure(overConsumption, null, false, "\n撑死了?\nSupport to death");

//                    if (componentPlayer.ComponentSickness.m_sicknessDuration == 0f && block.GetSicknessProbability(value) <= 0f)
//                    {
//                        componentPlayer.ComponentSickness.StartSickness();
//                        componentPlayer.ComponentSickness.m_sicknessDuration = 0.5f;
//                    }

//                    float nutritionValue = block.GetNutritionalValue(value);
//                    componentPlayer.ComponentVitalStats.Food -= overConsumption / 2f + nutritionValue * 0.05f;
//                }
//            }

//            return false;
//        }

//        /// <summary>
//        /// 方块初始化完成后，加载并处理饮水物品数据。
//        /// </summary>
//        public override void BlocksInitalized()
//        {
//            if (items == null)
//                return;

//            // 遍历所有饮水物品配置并添加到列表中
//            foreach (XElement element in items.Elements())
//            {
//                int resultBlockValue = CraftingRecipesManager.DecodeResult(XmlUtils.GetAttributeValue<string>(element, "Result"));
//                float waterValue = XmlUtils.GetAttributeValue<float>(element, "WaterValue");
//                float sicknessProbability = XmlUtils.GetAttributeValue<float>(element, "SicknessProbability");

//                WaterData waterDataEntry = new WaterData
//                {
//                    BlockValue = resultBlockValue,
//                    WaterValue = waterValue,
//                    SicknessProbability = sicknessProbability
//                };

//                if (waterValue != 0f)
//                {
//                    m_waterDatas.Add(waterDataEntry);
//                }
//            }
//        }

//        /// <summary>
//        /// 从文件中加载饮水数据。
//        /// </summary>
//        /// <returns>包含饮水数据的XElement对象。</returns>
//        private XElement LoadWaterData()
//        {
//            XElement waterData = null;
//            Entity.GetFiles(".MiSeaWater", delegate (string filename, Stream stream)
//            {
//                waterData = XmlUtils.LoadXmlFromStream(stream, Encoding.UTF8, true);
//                stream.Close();
//            });
//            return waterData;
//        }
//    }
//}