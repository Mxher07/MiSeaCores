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
            
            // Set default gamma value if not initialized
            if (SettingsManager.Brightness == 0f)
            {
                SettingsManager.Brightness = 2f;
            }
        }

        public override void Update()
        {
            // Update VR button state
            m_virtualRealityButton.IsEnabled = false;
            m_virtualRealityButton.Text = (SettingsManager.UseVr ? "Enabled" : "Disabled");
            
            // Handle gamma input
            if (m_gammaInput.Text != SettingsManager.Brightness.ToString("0.##"))
            {
                if (float.TryParse(m_gammaInput.Text, out float gamma))
                {
                    // Clamp gamma value between 0.1 and 100
                    gamma = MathUtils.Clamp(gamma, 0.1f, 100f);
                    SettingsManager.Brightness = gamma;
                    m_gammaInput.Text = gamma.ToString("0.##");
                }
            }
            
            // If textbox is empty, show current value
            if (string.IsNullOrEmpty(m_gammaInput.Text))
            {
                m_gammaInput.Text = SettingsManager.Brightness.ToString("0.##");
            }

            if (Input.Back || Input.Cancel || Children.Find<ButtonWidget>("TopBar.Back").IsClicked)
            {
                ScreensManager.SwitchScreen(ScreensManager.PreviousScreen);
            }
        }
    }
}
