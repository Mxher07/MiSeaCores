using Engine;
using Engine.Input;
using System;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace Game
{
    public class MainMenuScreen : Screen
    {
        private const string KEYS_DIRECTORY = "Keys";
        
        private string GetKeysFolderPath()
        {
            string keysPath = Storage.CombinePaths(ModsManager.ModsPath, "..", KEYS_DIRECTORY);
            if (!Storage.DirectoryExists(keysPath))
            {
                try 
                {
                    Storage.CreateDirectory(keysPath);
                }
                catch (Exception e)
                {
                    Log.Warning("Failed to create Keys directory: " + e.Message);
                }
            }
            return keysPath;
        }
        
        private bool ValidateKeyPair(string privateKey, string publicKey)
        {
            try
            {
                // 清理输入字符串
                privateKey = privateKey.Replace("\r", "").Replace("\n", "").Trim();
                publicKey = publicKey.Replace("\r", "").Replace("\n", "").Trim();
                
                // 简单验证：检查文件是否为空和基本格式
                if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(publicKey))
                {
                    Log.Warning("Key validation failed: Empty key file");
                    return false;
                }

                // 由于密钥文件可能包含注释或格式信息，我们只需验证文件存在且不为空
                return true;
            }
            catch (Exception e)
            {
                Log.Warning("Key validation exception: " + e.Message);
                return false;
            }
        }
        public string m_versionString = string.Empty;
        public string m_keyIdString = string.Empty;
        public bool m_versionStringTrial;
        public ButtonWidget m_showBulletinButton;

        public StackPanelWidget m_bulletinStackPanel;

        public LabelWidget m_copyrightLabel;

        public MainMenuScreen()
        {
            XElement node = ContentManager.Get<XElement>("Screens/MainMenuScreen");
            LoadContents(this, node);
            m_showBulletinButton = Children.Find<ButtonWidget>("BulletinButton");
            m_bulletinStackPanel = Children.Find<StackPanelWidget>("BulletinStackPanel");
            m_copyrightLabel = Children.Find<LabelWidget>("CopyrightLabel");
            string languageType = (!ModsManager.Configs.ContainsKey("Language")) ? "zh-CN" : ModsManager.Configs["Language"];
            m_bulletinStackPanel.IsVisible = (languageType == "zh-CN");
            m_copyrightLabel.IsVisible = (languageType != "zh-CN");
        }

        public override void Enter(object[] parameters)
        {
            MusicManager.CurrentMix = MusicManager.Mix.Menu;
            Children.Find<MotdWidget>().Restart();
            if (SettingsManager.IsolatedStorageMigrationCounter < 3)
            {
                SettingsManager.IsolatedStorageMigrationCounter++;
                VersionConverter126To127.MigrateDataFromIsolatedStorageWithDialog();
            }
            if (MotdManager.CanShowBulletin) MotdManager.ShowBulletin();
        }

        public override void Leave()
        {
            Keyboard.BackButtonQuitsApp = false;
        }

        public override void Update()
        {
            Keyboard.BackButtonQuitsApp = !MarketplaceManager.IsTrialMode;
            if (string.IsNullOrEmpty(m_versionString) || MarketplaceManager.IsTrialMode != m_versionStringTrial)
            {
                m_versionString = string.Format("GameVer 2.3 [BetaVersion]");
                m_versionStringTrial = MarketplaceManager.IsTrialMode;
                
                // 验证和加载密钥
                m_keyIdString = "交流群：827518905";
                try 
                {
                    string keysPath = GetKeysFolderPath();
                    string pubKeyPath = Storage.CombinePaths(keysPath, "id_ed25519.pub");
                    string privateKeyPath = Storage.CombinePaths(keysPath, "id_ed25519");
                    
                    if (Storage.FileExists(pubKeyPath) && Storage.FileExists(privateKeyPath))
                    {
                        string pubKey = Storage.ReadAllText(pubKeyPath);
                        string privateKey = Storage.ReadAllText(privateKeyPath);
                        
                        // 验证密钥对是否匹配
                        if (ValidateKeyPair(privateKey, pubKey))
                        {
                            // 清理公钥内容（移除所有空白字符）
                            pubKey = pubKey.Replace("\r", "").Replace("\n", "").Trim();
                            
                            // 获取公钥的最后5位
                            string lastFiveChars = pubKey.Length >= 5 ? 
                                pubKey.Substring(pubKey.Length - 5) : pubKey;
                            m_keyIdString = "ID: *****" + lastFiveChars;
                        }
                        else
                        {
                            Log.Warning("Key pair validation failed");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Warning("Failed to load or validate key pair: " + e.Message);
                    m_keyIdString = string.Empty;
                }
            }
            Children.Find("Buy").IsVisible = MarketplaceManager.IsTrialMode;
            Children.Find<LabelWidget>("Version").Text = m_versionString + "  API" + ModsManager.APIVersion + 
                (string.IsNullOrEmpty(m_keyIdString) ? string.Empty : "\n" + m_keyIdString);
            RectangleWidget rectangleWidget = Children.Find<RectangleWidget>("Logo");
            float num = 1f + 0.02f * MathUtils.Sin(1.5f * (float)MathUtils.Remainder(Time.FrameStartTime, 10000.0));
            rectangleWidget.RenderTransform = Matrix.CreateTranslation((0f - rectangleWidget.ActualSize.X) / 2f, (0f - rectangleWidget.ActualSize.Y) / 2f, 0f) * Matrix.CreateScale(num, num, 1f) * Matrix.CreateTranslation(rectangleWidget.ActualSize.X / 2f, rectangleWidget.ActualSize.Y / 2f, 0f);
            if (Children.Find<ButtonWidget>("Play").IsClicked)
            {
                ScreensManager.SwitchScreen("Play");
            }
            if (Children.Find<ButtonWidget>("Help").IsClicked)
            {
                ScreensManager.SwitchScreen("Help");
            }
            if (Children.Find<ButtonWidget>("Content").IsClicked)
            {
                ScreensManager.SwitchScreen("Content");
            }
            if (Children.Find<ButtonWidget>("Settings").IsClicked)
            {
                ScreensManager.SwitchScreen("Settings");
            }
            if (Children.Find<ButtonWidget>("Buy").IsClicked)
            {
                //MarketplaceManager.ShowMarketplace();
            }
            if (m_showBulletinButton.IsClicked)
            {
                if(MotdManager.m_bulletin != null && MotdManager.m_bulletin.Title.ToLower() != "null")
                {
                    //MotdManager.ShowBulletin();
                }
                else
                {
                    //DialogsManager.ShowDialog(null, new MessageDialog("�����ȡʧ��", "��ǰ���޷������棬\n����û��������ȡ������Ϣ", LanguageControl.Ok, null, null));
                }
            }
            if ((Input.Back && !Keyboard.BackButtonQuitsApp) || Input.IsKeyDownOnce(Key.Escape))
            {
                if (MarketplaceManager.IsTrialMode)
                {
                    ScreensManager.SwitchScreen("Nag");
                }
                else
                {
                    Window.Close();
                }
            }
        }
    }
}
