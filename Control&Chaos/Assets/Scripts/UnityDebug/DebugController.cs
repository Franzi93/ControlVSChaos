using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using Conditional = System.Diagnostics.ConditionalAttribute;
using UnityEngine;


namespace Dmdrn.UnityDebug
{
    public class DebugController : MonoBehaviour
    {
        public class CameraRenderEvents : MonoBehaviour
        {
            private Camera renderCamera;
            private Queue<System.Action> onPostRenderActions = new Queue<System.Action>();

            public Matrix4x4 projectionMatrix => renderCamera.projectionMatrix;

            private void Awake()
            {
                renderCamera = GetComponent<Camera>();

                Debug.Assert(renderCamera != null);
            }

            public void ExecuteOnPostRender(System.Action action)
            {
                onPostRenderActions.Enqueue(action);
            }

            private void OnPostRender()
            {
                while (onPostRenderActions.Count > 0)
                {
                    System.Action action = onPostRenderActions.Dequeue();
                    action();
                }
            }
        }


        public class LogMessage
        {
            public LogType type;

            public string condition;
            public string stackTrace;
            public bool isExpanded;
        }


        public class Action
        {
            public string name;
            public KeyCode key;
            public System.Action callback;

            public void Execute()
            {
                if (callback == null)
                {
                    Debug.LogWarning("Action '" + key + "' (" + name + ") has no callback!");
                }
                else
                {
                    callback();
                }
            }
        }


        public class UiPanel
        {
            public string name;
            public System.Action onGui;
        }


        public class TweakValue
        {
            public delegate float Getter();
            public delegate void Setter(float value);

            public string name;

            public Getter getter;
            public Setter setter;

            public float minimum;
            public float maximum;

            public float defaultValue;

            public bool isExpanded;

            public float value
            {
                get { return getter(); }
                set { setter(value); }
            }

            public void Reset()
            {
                value = defaultValue;
            }
        }


        public class WatchedValue
        {
            public delegate object Getter();

            public string name;
            public Getter getter;

            public object value
            {
                get { return getter(); }
            }
        }


        public class DebugLine
        {
            public delegate Vector3 Getter();

            public string name;
            public bool isVisible;
            public Color color;
            public Getter[] points;
            public Action visibilityToggleAction;
            public CameraRenderEvents camera;
        }


        private class FpsCounter
        {
            public float samplesPerSecond = 4.0f;
            public float framesPerSecond { get; private set; }
            public double deltaTime { get; private set; }

            private int frameCount = 0;
            private double totalDeltaTime = 0;

            private Stopwatch stopwatch = new Stopwatch();

            public void Update()
            {
                if (!stopwatch.IsRunning)
                {
                    stopwatch.Start();
                }

                frameCount++;

                deltaTime = stopwatch.ElapsedMilliseconds / 1000.0;
                totalDeltaTime += deltaTime;

                if (totalDeltaTime > 1.0f / samplesPerSecond)
                {
                    framesPerSecond = (float)(frameCount / totalDeltaTime);
                    frameCount = 0;

                    totalDeltaTime -= 1.0 / samplesPerSecond;
                }

                stopwatch.Restart();
            }
        }


        private struct FrameInfo
        {
            public double deltaTime;
        }


        public static DebugController instance { get; private set; }

        public bool debugActionsAllowed = true;
        public bool showUi = true;

        [Range(0.1f, 5f)]
        public float uiScale = 1f;

        public int logBufferSize = 1024;

        private bool isUiOpen;
        private bool showFpsCounter;

        private Dictionary<string, Action> actions = new Dictionary<string, Action>();
        private Dictionary<string, UiPanel> uiPanels = new Dictionary<string, UiPanel>();
        private Dictionary<string, WatchedValue> watchedValues = new Dictionary<string, WatchedValue>();
        private Dictionary<string, TweakValue> tweakValues = new Dictionary<string, TweakValue>();
        private Dictionary<string, DebugLine> debugLines = new Dictionary<string, DebugLine>();
        private LinkedList<LogMessage> logs = new LinkedList<LogMessage>();

        private UiPanel currentUiPanel;

        private FpsCounter fpsCounter = new FpsCounter();

        private FrameInfo[] profilingFrames = new FrameInfo[60];
        private int profilingFrameIndex;
        private bool profilerEnabled;

        private Material debugLineMaterial;

        private void Awake()
        {
            if (instance != null)
            {
                throw new System.Exception("Multiple instances of DebugController!");
            }

            instance = this;

            DontDestroyOnLoad(gameObject);

#if DMDRN_DEBUG
            Application.logMessageReceived += LogMessageReceived;

            Shader shader = Shader.Find("Hidden/Internal-Colored");

            if (shader == null)
            {
                throw new System.Exception("Debug Line Shader not found!");
            }

            if (debugLineMaterial == null)
            {
                debugLineMaterial = new Material(shader);
                debugLineMaterial.hideFlags = HideFlags.HideAndDontSave;

                debugLineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                debugLineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                debugLineMaterial.SetInt("_ZWrite", 0);
            }


            //
            // Default Panels & Values
            //

            AddPanel("System", OnGuiSystem);
            AddPanel("Actions", OnGuiActions);
            AddPanel("Watched Values", OnGuiWatchedValues);
            AddPanel("Tweak Values", OnGuiTweakValues);
            AddPanel("Performance", OnGuiPerformance);
            AddPanel("Quality", OnGuiQuality);
            AddPanel("Debug UI", OnGuiDebugUi);
            AddPanel("Log", OnGuiLog);

            AddAction("Show/Hide Debug UI", KeyCode.Alpha0, () => showUi = !showUi);

            WatchValue("Time.realtimeSinceStartup", () => Time.realtimeSinceStartup);

            WatchValue(
                "Total memory",
                () => string.Format("{0:F2} MB", (System.GC.GetTotalMemory(false) / 1024f) / 1024f)
            );

            AddTweakValue("Time.timeScale", 0, 8, () => Time.timeScale, (v) => Time.timeScale = v);
#endif
        }

        [Conditional("DMDRN_DEBUG")]
        private void Update()
        {
            //
            // Handle Debug Actions
            //

            if (debugActionsAllowed && Input.anyKeyDown)
            {
                foreach (Action action in actions.Values)
                {
                    if (action.key != KeyCode.None && Input.GetKeyDown(action.key))
                    {
                        action.Execute();
                    }
                }
            }


            //
            // Update FPS Counter
            //

            if (showFpsCounter || profilerEnabled)
            {
                fpsCounter.Update();
            }


            //
            // Collect Profiler Data
            //

            if (profilerEnabled)
            {
                profilingFrames[profilingFrameIndex] = new FrameInfo
                {
                    deltaTime = fpsCounter.deltaTime
                };

                profilingFrameIndex = (profilingFrameIndex + 1) % profilingFrames.Length;
            }


            //
            // Draw Debug Lines
            //

            if (debugLines.Count > 0)
            {
                foreach (DebugLine line in debugLines.Values)
                {
                    if (!line.isVisible)
                    {
                        continue;
                    }

                    line.camera.ExecuteOnPostRender(() => RenderDebugLine(line));
                }
            }
        }


        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            LogMessage message = new LogMessage
            {
                type = type,
                condition = condition,
                stackTrace = stackTrace
            };

            logs.AddFirst(message);

            while (logs.Count > logBufferSize)
            {
                logs.RemoveLast();
            }
        }


        public TweakValue AddTweakValue(
            string name,
            float minimum,
            float maximum,
            TweakValue.Getter getter,
            TweakValue.Setter setter)
        {
            return AddTweakValue(name, minimum, maximum, getter(), getter, setter);
        }


        public TweakValue AddTweakValue(
            string name,
            float minimum,
            float maximum,
            float defaultValue,
            TweakValue.Getter getter,
            TweakValue.Setter setter)
        {
            if (tweakValues.ContainsKey(name))
            {
                Log.Warning("Overriding tweak value '" + name + "'! Are you sure this is intended?");
            }

            TweakValue value = new TweakValue
            {
                name = name,
                getter = getter,
                setter = setter,
                defaultValue = defaultValue,
                minimum = minimum,
                maximum = maximum
            };

            tweakValues[name] = value;

            return value;
        }


        public void Remove(TweakValue value)
        {
            if (value == null) return;

            RemoveTweakValue(value.name);
        }


        public void RemoveTweakValue(string name)
        {
            if (!tweakValues.ContainsKey(name))
            {
                Log.Warning("Trying to remove tweak value '" + name + "' but it doesn't exist!");
                return;
            }

            tweakValues.Remove(name);
        }


        public WatchedValue WatchValue(string name, WatchedValue.Getter getter)
        {
            if (watchedValues.ContainsKey(name))
            {
                Log.Warning("Overriding watched value '" + name + "'! Are you sure this is intended?");
            }

            WatchedValue value = new WatchedValue
            {
                name = name,
                getter = getter
            };

            watchedValues[name] = value;

            return value;
        }


        public void Unwatch(WatchedValue value)
        {
            if (value == null) return;

            UnwatchValue(value.name);
        }


        public void UnwatchValue(string name)
        {
            if (!watchedValues.ContainsKey(name))
            {
                Log.Warning("Trying to remove watched value '" + name + "' but it doesn't exist!");
                return;
            }

            watchedValues.Remove(name);
        }


        public UiPanel AddPanel(string name, System.Action onGui)
        {
            if (uiPanels.ContainsKey(name))
            {
                Log.Warning("Overriding debug UI panel '" + name + "'! Are you sure this is intended?");
            }

            UiPanel panel = new UiPanel
            {
                name = name,
                onGui = onGui
            };

            uiPanels[name] = panel;

            return panel;
        }


        public void Remove(UiPanel panel)
        {
            if (panel == null) return;

            RemovePanel(panel.name);
        }


        public void RemovePanel(string name)
        {
            UiPanel panel;

            if (!uiPanels.TryGetValue(name, out panel))
            {
                Log.Warning("Trying to remove UI panel '" + name + "' but it doesn't exist!");
                return;
            }

            if (currentUiPanel == panel)
            {
                currentUiPanel = null;
            }

            uiPanels.Remove(name);
        }


        public Action AddAction(string name, System.Action callback)
        {
            return AddAction(name, KeyCode.None, callback);
        }


        public Action AddAction(string name, KeyCode keyCode, System.Action callback)
        {
            if (actions.ContainsKey(name))
            {
                Log.Warning("Overriding action '" + name + "'! Are you sure this is intended?");
            }

            Action action = new Action
            {
                name = name,
                key = keyCode,
                callback = callback
            };

            actions[name] = action;

            return action;
        }

        public void Remove(Action action)
        {
            if (action == null) return;

            RemoveAction(action.name);
        }


        public void RemoveAction(string name)
        {
            if (!actions.ContainsKey(name))
            {
                Log.Warning("Trying to remove action '" + name + "' but it doesn't exist!");
                return;
            }

            actions.Remove(name);
        }


        public DebugLine AddDebugLine(string name, Camera camera, Color color, params DebugLine.Getter[] points)
        {
            Debug.Assert(camera != null);

            if (points == null || points.Length < 2)
            {
                throw new System.Exception("A debug line must have at least two points!");
            }

            if (debugLines.ContainsKey(name))
            {
                Log.Warning("Overriding debug line '" + name + "'! Are you sure this is intended?");
            }

            DebugLine line = new DebugLine
            {
                name = name,
                color = color,
                points = points
            };

            line.visibilityToggleAction = AddAction(
                string.Format("Show/Hide Debug Line \"{0}\"", name),
                () => line.isVisible = !line.isVisible
            );

            CameraRenderEvents renderEvents = camera.GetComponent<CameraRenderEvents>();

            if (renderEvents == null)
            {
                renderEvents = camera.gameObject.AddComponent<CameraRenderEvents>();
            }

            line.camera = renderEvents;

            debugLines[name] = line;

            return line;
        }


        private void RenderDebugLine(DebugLine line)
        {
            GL.PushMatrix();
            GL.LoadProjectionMatrix(line.camera.projectionMatrix);

            debugLineMaterial.SetPass(0);

            GL.Begin(GL.LINES);
            {
                GL.Color(line.color);

                for (int i = 1; i < line.points.Length; ++i)
                {
                    GL.Vertex(line.points[i - 1]());
                    GL.Vertex(line.points[i]());
                }
            }
            GL.End();

            GL.PopMatrix();
        }


        public void RemoveDebugLine(string name)
        {
            if (!debugLines.ContainsKey(name))
            {
                Log.Warning("Trying to remove debug line '" + name + "' but it doesn't exist!");
                return;
            }

            DebugLine line = debugLines[name];

            Remove(line.visibilityToggleAction);
            debugLines.Remove(name);
        }


        public void Remove(DebugLine line)
        {
            if (line == null) return;

            RemoveDebugLine(line.name);
        }


#if DMDRN_DEBUG
        private void OnGUI()
        {
            if (!showUi)
            {
                return;
            }

            DebugUi.overallScale = uiScale;

            DebugUi.PrepareGuiUpdate();

            DebugUi.BeginMainArea();

            GUILayout.BeginHorizontal();

            string toggleLabel = isUiOpen ? "-" : "+";

            if (DebugUi.Button(toggleLabel, DebugUi.Width(100), DebugUi.Height(100)))
            {
                isUiOpen = !isUiOpen;
            }

            GUILayout.FlexibleSpace();

            if (showFpsCounter)
            {
                DebugUi.Label(string.Format("{0:F1} fps", fpsCounter.framesPerSecond));
            }

            GUILayout.EndHorizontal();

            if (!isUiOpen)
            {
                return;
            }


            GUILayout.BeginHorizontal();


            //
            // Draw Tabs
            //

            GUILayout.BeginVertical();

            foreach (UiPanel tab in uiPanels.Values)
            {
                if (DebugUi.Button(tab.name))
                {
                    currentUiPanel = tab;
                }
            }

            GUILayout.EndVertical();


            //
            // Draw Panel
            //

            if (currentUiPanel != null)
            {
                DebugUi.BeginPanel();

                DebugUi.Headline(currentUiPanel.name);

                currentUiPanel.onGui();

                DebugUi.EndPanel();
            }


            //
            // --
            //

            GUILayout.EndHorizontal();
            DebugUi.EndMainArea();
        }
#endif

        private void OnGuiSystem()
        {
            DebugUi.SmallLabel("App Version: " + Application.version);
            DebugUi.Space(20);

            DebugUi.SmallLabel("Device Type: " + SystemInfo.deviceType);
            DebugUi.SmallLabel("Device Model: " + SystemInfo.deviceModel);
            DebugUi.SmallLabel("Device Name: " + SystemInfo.deviceName);
            DebugUi.SmallLabel("Device UID:\n" + SystemInfo.deviceUniqueIdentifier);

            DebugUi.Headline("Operating System");
            DebugUi.SmallLabel("System Name: " + SystemInfo.operatingSystem);
            DebugUi.SmallLabel("System Family: " + SystemInfo.operatingSystemFamily);

            DebugUi.Headline("CPU & Memory");
            DebugUi.SmallLabel("CPUs: " + SystemInfo.processorCount);
            DebugUi.SmallLabel("CPU Type: " + SystemInfo.processorType);
            DebugUi.SmallLabel(string.Format("CPU Frequency: {0:F2} GHz", SystemInfo.processorFrequency / 1000f));
            DebugUi.SmallLabel(string.Format("Memory: {0:F2} GB", SystemInfo.systemMemorySize / 1024f));

            DebugUi.Headline("Graphics");
            DebugUi.SmallLabel("Device Type: " + SystemInfo.graphicsDeviceType);
            DebugUi.SmallLabel("Device Name: " + SystemInfo.graphicsDeviceName);
            DebugUi.SmallLabel("Device Version: " + SystemInfo.graphicsDeviceVersion);
            DebugUi.SmallLabel(string.Format("Vendor: {0} (ID: {1})", SystemInfo.graphicsDeviceVendor, SystemInfo.graphicsDeviceVendorID));
            DebugUi.SmallLabel(string.Format("Memory: {0:F2} GB", SystemInfo.graphicsMemorySize / 1024f));
            DebugUi.SmallLabel("Shader Level: " + SystemInfo.graphicsShaderLevel);

            DebugUi.Headline("Battery");
            DebugUi.SmallLabel(string.Format("Battery Level: {0}%", Mathf.RoundToInt(SystemInfo.batteryLevel * 100f)));
            DebugUi.SmallLabel("Battery Status: " + SystemInfo.batteryStatus);

#if UNITY_IOS && !UNITY_EDITOR
        DebugUi.Headline("iOS Specific");
        DebugUi.SmallLabel("Device Generation: " + UnityEngine.iOS.Device.generation);
        DebugUi.SmallLabel("System Version: " + UnityEngine.iOS.Device.systemVersion);
        DebugUi.SmallLabel("Vendor: " + UnityEngine.iOS.Device.vendorIdentifier);
        DebugUi.SmallLabel("Low Power Mode: " + UnityEngine.iOS.Device.lowPowerModeEnabled);
#endif
        }


        private void OnGuiActions()
        {
            Action actionToExecute = null;

            foreach (Action action in actions.Values)
            {
                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                string label = (action.key == KeyCode.None)
                             ? action.name
                             : string.Format("[{0}] {1}", action.key, action.name);

                if (DebugUi.Button(label, DebugUi.Styles.buttonAlignedLeft, GUILayout.ExpandWidth(true)))
                {
                    actionToExecute = action;
                }

                GUILayout.EndHorizontal();
            }

            if (actionToExecute != null)
            {
                actionToExecute.Execute();
            }
        }


        private void OnGuiWatchedValues()
        {
            foreach (KeyValuePair<string, WatchedValue> pair in watchedValues)
            {
                DebugUi.Label(string.Format("{0}: {1}", pair.Key, pair.Value.value));
            }
        }


        private void OnGuiTweakValues()
        {
            foreach (TweakValue tweakValue in tweakValues.Values)
            {
                string buttonLabel = string.Format("{0}: {1}", tweakValue.name, tweakValue.value);

                GUILayout.BeginVertical(GUI.skin.box);

                if (DebugUi.Button(buttonLabel, DebugUi.Styles.buttonAlignedLeft))
                {
                    tweakValue.isExpanded = !tweakValue.isExpanded;
                }

                if(tweakValue.isExpanded)
                {
                    tweakValue.value = DebugUi.HorizontalSlider(
                        tweakValue.value,
                        tweakValue.minimum,
                        tweakValue.maximum,
                        GUILayout.ExpandWidth(true)
                    );

                    GUILayout.BeginHorizontal();

                    DebugUi.Space(10);
                    DebugUi.SmallLabel(tweakValue.value.ToString());

                    GUILayout.FlexibleSpace();

                    if (DebugUi.Button("Reset", GUILayout.ExpandWidth(false)))
                    {
                        tweakValue.Reset();
                    }

                    GUILayout.EndHorizontal();

                    DebugUi.Space(20);

                }

                GUILayout.EndVertical();
            }
        }


        private void OnGuiDebugUi()
        {
            DebugUi.Label("Scale: ");

            GUILayout.BeginHorizontal();

            float scaleStep = 0.05f;

            if (DebugUi.Button(" - ", GUILayout.ExpandWidth(false)))
            {
                uiScale = Mathf.Max(0.01f, uiScale - (uiScale * scaleStep));
            }

            if (DebugUi.Button(" + ", GUILayout.ExpandWidth(false)))
            {
                uiScale += uiScale * scaleStep;
            }

            GUILayout.EndHorizontal();
        }


        private void OnGuiQuality()
        {
            string[] names = QualitySettings.names;

            DebugUi.Label("Set Quality Level");

            int index = -1;
            int currentLevel = QualitySettings.GetQualityLevel();

            for (int i=0; i < names.Length; ++i)
            {
                bool clicked = false;

                if(i == currentLevel)
                {
                    if (DebugUi.Button(names[i], Color.green))
                    {
                        clicked = true;
                    }
                }
                else
                {
                    if (DebugUi.Button(names[i]))
                    {
                        clicked = true;
                    }
                }


                if (clicked)
                {
                    index = i;
                }
            }

            if(index >= 0)
            {
                Log.Message("Setting quality to: " + names[index]);
                QualitySettings.SetQualityLevel(index);
            }
        }


        private void OnGuiPerformance()
        {
            showFpsCounter = DebugUi.Toggle("Show FPS Counter", showFpsCounter);
            profilerEnabled = DebugUi.Toggle("Mini Profiler", profilerEnabled);


            //
            // Determine Minimum & Maximum Frame Time
            //

            if (profilerEnabled)
            {
                int targetFrameRate = Application.targetFrameRate;

                float targetTime = 1f / targetFrameRate;

                float maxFrameTime = targetTime;
                float minFrameTime = float.MaxValue;

                for (int i = 0; i < profilingFrames.Length; ++i)
                {
                    FrameInfo frame = profilingFrames[i];

                    if (frame.deltaTime > maxFrameTime)
                    {
                        maxFrameTime = (float)frame.deltaTime;
                    }

                    if (frame.deltaTime < minFrameTime)
                    {
                        minFrameTime = (float)frame.deltaTime;
                    }
                }


                //
                // Draw Mini Profiler
                //

                DebugUi.Label(string.Format("Target Frame Rate: {0}", targetFrameRate));
                DebugUi.Label(string.Format("Target Frame Time: {0:F2} ms", targetTime * 1000f));

                float width = 900f;
                float height = 100f;

                GUILayout.BeginHorizontal(DebugUi.Width(width), DebugUi.Height(height));

                float boxWidth = width / profilingFrames.Length;

                for (int i = 0; i < profilingFrames.Length; ++i)
                {
                    FrameInfo frame = profilingFrames[i];

                    float nValue = (float)(frame.deltaTime / maxFrameTime);

                    GUILayout.BeginVertical(DebugUi.Width(boxWidth), DebugUi.Height(height));
                    GUILayout.FlexibleSpace();

                    Color color;

                    if (i == profilingFrameIndex)
                    {
                        color = Color.white;
                    }
                    else
                    {
                        color = (frame.deltaTime > targetTime)
                              ? Color.red
                              : Color.green;
                    }

                    DebugUi.PushGuiColor(color);

                    GUILayout.Box(
                        DebugUi.Styles.solidRectangle.normal.background,
                        DebugUi.Styles.solidRectangle,
                        DebugUi.Height(nValue * height),
                        DebugUi.Width(boxWidth)
                    );

                    DebugUi.PopGuiColor();

                    GUILayout.EndVertical();
                }

                GUILayout.EndHorizontal();

                DebugUi.Label(string.Format("Min Frame Time: {0:F2} ms", minFrameTime * 1000f));
                DebugUi.Label(string.Format("Max Frame Time: {0:F2} ms", maxFrameTime * 1000f));
            }
        }


        private Vector2 logScrollPosition;

        private void OnGuiLog()
        {
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (DebugUi.Button("Clear", GUILayout.ExpandWidth(false)))
            {
                logs.Clear();
            }

            GUILayout.EndHorizontal();


            logScrollPosition = DebugUi.BeginScrollView(logScrollPosition);

            LinkedListNode<LogMessage> node = logs.First;

            while (node != null)
            {
                LogMessage message = node.Value;

                Color color;

                if (message.type == LogType.Error || message.type == LogType.Exception)
                {
                    color = Color.red;
                }
                else if (message.type == LogType.Warning)
                {
                    color = Color.yellow;
                }
                else
                {
                    color = Color.white;
                }


                if (DebugUi.Button(message.condition, color, DebugUi.Styles.textButton))
                {
                    message.isExpanded = !message.isExpanded;
                }

                if (message.isExpanded)
                {
                    DebugUi.BeginPanel();
                    DebugUi.SmallLabel(message.stackTrace);
                    DebugUi.EndPanel();
                }

                node = node.Next;
            }

            DebugUi.EndScrollView();
        }
    }
}
