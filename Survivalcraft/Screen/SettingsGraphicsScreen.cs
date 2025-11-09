using Engine;
using System.Xml.Linq;

namespace Game
{
    public class SettingsGraphicsScreen : Screen
    {
        public BevelledButtonWidget m_virtualRealityButton;
        public TextBoxWidget m_gammaInput;
        public ContainerWidget m_vrPanel;
        
        public SettingsGraphicsScreen()
        {
            XElement node = ContentManager.Get<XElement>("Screens/SettingsGraphicsScreen");
            LoadContents(this, node);
            m_virtualRealityButton = Children.Find<BevelledButtonWidget>("VirtualRealityButton");
            m_gammaInput = Children.Find<TextBoxWidget>("GammaInput");
            m_vrPanel = Children.Find<ContainerWidget>("VrPanel");
            m_vrPanel.IsVisible = false;
            
            // 强制设置亮度值为2f
            SettingsManager.Brightness = 2f;
        }

        public override void Update()
        {
            // Update VR button state
            m_virtualRealityButton.IsEnabled = false;
            m_virtualRealityButton.Text = (SettingsManager.UseVr ? "Enabled" : "Disabled");
            
            // 亮度值锁定为2f
            m_gammaInput.IsEnabled = false;
            m_gammaInput.Text = "2.00";
            SettingsManager.Brightness = 2f;

            if (Input.Back || Input.Cancel || Children.Find<ButtonWidget>("TopBar.Back").IsClicked)
            {
                ScreensManager.SwitchScreen(ScreensManager.PreviousScreen);
            }
        }
    }
}
