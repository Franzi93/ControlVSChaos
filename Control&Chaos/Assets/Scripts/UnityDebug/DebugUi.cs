using UnityEngine;
using System.Collections.Generic;

namespace Dmdrn.UnityDebug
{
    public static class DebugUi
    {
        public static class Styles
        {
            public static GUIStyle button;
            public static GUIStyle buttonAlignedLeft;
            public static GUIStyle textButton;
            public static GUIStyle label;
            public static GUIStyle smallLabel;
            public static GUIStyle headline;
            public static GUIStyle panel;
            public static GUIStyle paddedArea;
            public static GUIStyle toggleButton;

            public static GUIStyle horizontalSlider;
            public static GUIStyle horizontalSliderThumb;

            public static GUIStyle horizontalScrollbar;
            public static GUIStyle horizontalScrollbarThumb;

            public static GUIStyle verticalScrollbar;
            public static GUIStyle verticalScrollbarThumb;

            public static GUIStyle solidRectangle;

            static Styles()
            {
                button = new GUIStyle(GUI.skin.button);
                buttonAlignedLeft = new GUIStyle(GUI.skin.button);
                textButton = new GUIStyle(GUI.skin.button);
                label = new GUIStyle(GUI.skin.label);
                smallLabel = new GUIStyle(GUI.skin.label);
                headline = new GUIStyle(GUI.skin.label);
                panel = new GUIStyle(GUI.skin.window);
                paddedArea = new GUIStyle(GUI.skin.window);
                toggleButton = new GUIStyle(GUI.skin.button);

                horizontalSlider = new GUIStyle(GUI.skin.horizontalSlider);
                horizontalSliderThumb = new GUIStyle(GUI.skin.horizontalSliderThumb);

                horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
                horizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);

                verticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
                verticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb);

                solidRectangle = new GUIStyle(GUI.skin.box);
                solidRectangle.normal.background = Texture2D.whiteTexture;
            }
        }

        public static Vector2 baseScreenSize = new Vector2(1440, 2960);
        private static Stack<Color> guiColors = new Stack<Color>();

        public static Color guiColor
        {
            get { return GUI.color; }
            set { GUI.color = value; }
        }

        static DebugUi()
        {
            InitStyles();
        }


        public static float overallScale = 1f;

        public static float screenScaleFactor
        {
            get { return Screen.width / baseScreenSize.x; }
        }


        public static float scaleFactor
        {
            get { return screenScaleFactor * overallScale; }
        }


        public static void PrepareGuiUpdate()
        {
            guiColors.Clear();

            InitStyles();
        }


        private static void InitStyles()
        {
            int fontSize = ScaleInt(40);

            Styles.button.fontSize = ScaleInt(fontSize * 1.1f);
            Styles.button.padding = UniformRectOffset(20, 30);

            Styles.buttonAlignedLeft.fontSize = Styles.button.fontSize;
            Styles.buttonAlignedLeft.padding = Styles.button.padding;
            Styles.buttonAlignedLeft.alignment = TextAnchor.MiddleLeft;

            Styles.textButton = new GUIStyle(Styles.buttonAlignedLeft);
            Styles.textButton.wordWrap = true;

            Styles.label.fontSize = fontSize;

            Styles.smallLabel.fontSize = ScaleInt(fontSize * .9f);

            Styles.headline.fontSize = ScaleInt(fontSize * 1.4f);
            Styles.headline.fontStyle = FontStyle.Bold;

            Styles.panel.padding = UniformRectOffset(50);

            Styles.paddedArea.padding = UniformRectOffset(20);

            Styles.horizontalSlider.fixedHeight = Scale(60);

            Styles.horizontalSliderThumb.fixedHeight = Scale(60);
            Styles.horizontalSliderThumb.fixedWidth = Scale(60);

            Styles.horizontalScrollbar.fixedHeight = Scale(60);
            Styles.horizontalScrollbarThumb.fixedHeight = Scale(60);

            Styles.verticalScrollbar.fixedWidth = Scale(60);
            Styles.verticalScrollbarThumb.fixedWidth = Scale(60);

            Styles.toggleButton.fontSize = fontSize;
            Styles.toggleButton.padding = UniformRectOffset(20, 30);

            Styles.solidRectangle.padding = UniformRectOffset(0);
            Styles.solidRectangle.margin = UniformRectOffset(0);
        }


        public static void PushGuiColor(Color color)
        {
            guiColors.Push(guiColor);
            guiColor = color;
        }


        public static Color PopGuiColor()
        {
            Color color = guiColors.Pop();
            guiColor = color;

            return color;
        }


        public static RectOffset UniformRectOffset(int offset)
        {
            int scaledOffset = ScaleInt(offset);

            return new RectOffset(scaledOffset, scaledOffset, scaledOffset, scaledOffset);
        }


        public static RectOffset UniformRectOffset(int horizontalOffset, int verticalOffset)
        {
            int hOffset = ScaleInt(horizontalOffset);
            int vOffset = ScaleInt(verticalOffset);

            return new RectOffset(hOffset, hOffset, vOffset, vOffset);
        }


        public static bool Toggle(string label, bool value)
        {
            //TODO(cba): Figure out a way to nicely scale toggle buttons

            //        GUILayout.BeginHorizontal();

            bool result = GUILayout.Toggle(value, label, Styles.toggleButton);
            //        Label(label);

            //        GUILayout.EndHorizontal();

            return result;
        }


        public static float HorizontalSlider(
            float value,
            float leftValue,
            float rightValue,
            params GUILayoutOption[] options)
        {
            return GUILayout.HorizontalSlider(
                value,
                leftValue,
                rightValue,
                Styles.horizontalSlider,
                Styles.horizontalSliderThumb,
                options
            );
        }

        public static void BeginMainArea()
        {
            GUILayout.BeginVertical(Styles.paddedArea);
        }


        public static void EndMainArea()
        {
            GUILayout.EndVertical();
        }


        public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
        {
            return GUILayout.BeginScrollView(
                scrollPosition,
                Styles.horizontalScrollbar,
                Styles.verticalScrollbar,
                options
            );
        }


        public static void EndScrollView()
        {
            GUILayout.EndScrollView();
        }


        public static void BeginPanel()
        {
            GUILayout.BeginVertical(Styles.panel, GUILayout.MinWidth(Scale(1000)));
        }


        public static void EndPanel()
        {
            GUILayout.EndVertical();
        }


        public static GUILayoutOption Width(float value)
        {
            return GUILayout.Width(Scale(value));
        }


        public static GUILayoutOption Height(float value)
        {
            return GUILayout.Height(Scale(value));
        }


        public static void Space(float pixels)
        {
            GUILayout.Space(Scale(pixels));
        }


        public static void Headline(string text, params GUILayoutOption[] options)
        {
            Space(30);
            Label(text, Styles.headline, options);
            Space(10);
        }


        public static void Headline(string text, Color color, params GUILayoutOption[] options)
        {
            Space(30);
            Label(text, color, Styles.headline, options);
            Space(10);
        }


        public static void SmallLabel(string text, params GUILayoutOption[] options)
        {
            Label(text, Styles.smallLabel, options);
        }


        public static void SmallLabel(string text, Color color, params GUILayoutOption[] options)
        {
            PushGuiColor(color);
            SmallLabel(text, options);
            PopGuiColor();
        }


        public static void Label(string text, params GUILayoutOption[] options)
        {
            Label(text, Styles.label, options);
        }


        public static void Label(string text, Color color, params GUILayoutOption[] options)
        {
            Label(text, color, Styles.label, options);
        }


        public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.Label(text, style, options);
        }


        public static void Label(string text, Color color, GUIStyle style, params GUILayoutOption[] options)
        {
            PushGuiColor(color);
            Label(text, style, options);
            PopGuiColor();
        }


        public static bool Button(string label, params GUILayoutOption[] options)
        {
            return Button(label, Styles.button, options);
        }


        public static bool Button(string label, Color color, GUIStyle style, params GUILayoutOption[] options)
        {
            PushGuiColor(color);
            bool pressed = Button(label, style, options);
            PopGuiColor();

            return pressed;
        }


        public static bool Button(string label, Color color, params GUILayoutOption[] options)
        {
            return Button(label, color, Styles.button, options);
        }

        public static bool Button(string label, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUILayout.Button(label, style, options);
        }


        public static float Scale(float value)
        {
            return value * scaleFactor;
        }


        public static int ScaleInt(float value)
        {
            return (int)Scale(value);
        }
    }
}
