using Engine;
using Engine.Media;
using System.Xml.Linq;

namespace Game
{
    public abstract class ButtonWidget : CanvasWidget
    {
        protected static readonly string DefaultStylePath = "Styles/ButtonStyle_Universal";

        public abstract bool IsClicked { get; }

        public abstract bool IsChecked { get; set; }

        public abstract bool IsAutoCheckingEnabled { get; set; }

        public abstract string Text { get; set; }

        public abstract BitmapFont Font { get; set; }

        public abstract Color Color { get; set; }

        protected virtual XElement LoadButtonStyle(string customStylePath = null)
        {
            // 尝试加载自定义样式
            if (!string.IsNullOrEmpty(customStylePath))
            {
                var customStyle = ContentManager.Get<XElement>(customStylePath);
                if (customStyle != null)
                    return customStyle;
            }

            // 尝试加载默认样式
            var defaultStyle = ContentManager.Get<XElement>(DefaultStylePath);
            if (defaultStyle != null)
                return defaultStyle;

            // 如果都失败了,返回null
            return null;
        }
    }
}
