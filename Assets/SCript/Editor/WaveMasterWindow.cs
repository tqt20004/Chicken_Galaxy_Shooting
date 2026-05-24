using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

public class WaveMasterWindow : EditorWindow
{
    private WaveManager _manager;
    private Vector2 _scrollPos;
    private Vector2 _leftScrollPos;
    private WaveData _editingWaveData; // Lưu file đang mở để sửa bên cột phải

    [MenuItem("Tools/Wave Master Editor")]
    public static void ShowWindow()
    {
        WaveMasterWindow window = GetWindow<WaveMasterWindow>("Wave Master");
        window.minSize = new Vector2(900, 550);
    }

    private void OnGUI()
    {
        // Phủ nền xám đậm nguyên bảng
        Rect windowRect = new Rect(0, 0, position.width, position.height);
        EditorGUI.DrawRect(windowRect, new Color(0.12f, 0.13f, 0.15f, 1f));

        GUIStyle windowTitleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 18,
            alignment = TextAnchor.MiddleCenter,
            margin = new RectOffset(0, 0, 10, 10)
        };
        windowTitleStyle.normal.textColor = new Color(0.2f, 0.85f, 1f);

        GUIStyle sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            margin = new RectOffset(0, 0, 5, 5)
        };
        sectionHeaderStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);

        GUIStyle entityBlockStyle = new GUIStyle(EditorStyles.helpBox)
        {
            padding = new RectOffset(12, 12, 10, 10),
            margin = new RectOffset(0, 0, 0, 8)
        };

        GUILayout.Label("🎛️ STAGE & WAVE MASTER PRO", windowTitleStyle);

        if (_manager == null) _manager = GameObject.FindAnyObjectByType<WaveManager>();

        EditorGUILayout.BeginHorizontal();

        // ==========================================
        // CỘT TRÁI: ĐIỀU KHIỂN & SCENE MANAGEMENT (Rộng 40%)
        // ==========================================
        EditorGUILayout.BeginVertical(GUILayout.Width(350), GUILayout.MinWidth(300));
        _leftScrollPos = EditorGUILayout.BeginScrollView(_leftScrollPos, "box");

        GUILayout.Label("🔗 CORE SYSTEM", sectionHeaderStyle);
        _manager = (WaveManager)EditorGUILayout.ObjectField("Manager Target", _manager, typeof(WaveManager), true);

        if (_manager == null)
        {
            EditorGUILayout.HelpBox("Kéo WaveManager vào đây để kích hoạt!", MessageType.Error);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            return;
        }

        GUILayout.Space(10);

        // 1. QUẢN LÝ MẢNG WAVE (STAGE SEQUENCE)
        GUILayout.Label("📚 STAGE SEQUENCE (MẢNG WAVE)", sectionHeaderStyle);
        SerializedObject serializedManager = new SerializedObject(_manager);
        SerializedProperty waveArrayProp = serializedManager.FindProperty("waveDataArray");
        EditorGUILayout.PropertyField(waveArrayProp, new GUIContent("Danh sách Wave"), true);

        GUILayout.Space(10);

        // 2. QUẢN LÝ MỐC SPAWN
        GUILayout.Label("📍 SPATIAL DATA (8 POINTS)", sectionHeaderStyle);
        SerializedProperty spawnPointsProp = serializedManager.FindProperty("spawnPointArray");
        EditorGUILayout.PropertyField(spawnPointsProp, new GUIContent("Mốc Spawn"), true);
        serializedManager.ApplyModifiedProperties();

        GUILayout.Space(15);

        // 3. RUNTIME CONTROL PANEL (LIVE CHEAT)
        GUILayout.Label("⚡ RUNTIME CONTROL PANEL", sectionHeaderStyle);
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("OFFLINE: Enter PLAY Mode để mở bộ debug.", MessageType.None);
        }
        else
        {
            EditorGUILayout.BeginVertical("helpBox");
            EditorGUILayout.LabelField($"Tiến độ Stage:", $"Wave {_manager.curIndexWave + 1} / {_manager.waveDataArray.Length}", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Trạng thái hiện tại:", _manager.curWaveState.ToString(), EditorStyles.boldLabel);

            // Theo dõi đúng timer dựa vào State
            if (_manager.curWaveState == WaveStates.WaveIntro)
            {
                FieldInfo introTimerField = typeof(WaveManager).GetField("introTimer", BindingFlags.NonPublic | BindingFlags.Instance);
                float iTimer = introTimerField != null ? (float)introTimerField.GetValue(_manager) : 0f;
                EditorGUILayout.LabelField("Chờ Intro:", $"{3f - iTimer:F1}s");
            }
            else
            {
                EditorGUILayout.LabelField("Thời gian Combat:", $"{_manager.waveTimer:F2}s");
            }

            GUILayout.Space(8);

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
            if (GUILayout.Button("🔄 Force Start Stage", GUILayout.Height(28))) _manager.StartStageWaves();

            GUI.backgroundColor = new Color(0.85f, 0.65f, 0.2f);
            if (GUILayout.Button("⏩ Fast-Forward (Xả quái)", GUILayout.Height(28))) ForceEndWaveSpawning();
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        GUILayout.Box("", GUILayout.Width(2), GUILayout.ExpandHeight(true));

        // ==========================================
        // CỘT PHẢI: THIẾT KẾ DATA DRIVEN ENTITY (Rộng 60%)
        // ==========================================
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

        GUILayout.Label("📦 WAVE ELEMENT DESIGNER", sectionHeaderStyle);

        // Cho phép ông kéo trực tiếp file WaveData vào đây để edit, hoặc lấy wave đang chạy
        _editingWaveData = (WaveData)EditorGUILayout.ObjectField("Wave Đang Thiết Kế", _editingWaveData, typeof(WaveData), false);

        if (_editingWaveData != null)
        {
            GUILayout.Space(5);
            _editingWaveData.waveName = EditorGUILayout.TextField("Tên Wave", _editingWaveData.waveName);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(0.15f, 0.6f, 0.3f);
            if (GUILayout.Button("➕ Add Spawn Event", GUILayout.Height(30)))
            {
                if (_editingWaveData.waveList == null) _editingWaveData.waveList = new System.Collections.Generic.List<WaveElement>();
                _editingWaveData.waveList.Add(new WaveElement());
            }

            GUI.backgroundColor = new Color(0.2f, 0.5f, 0.7f);
            if (GUILayout.Button("⚡ Auto-Sort Timeline", GUILayout.Height(30)))
            {
                if (_editingWaveData.waveList != null)
                    _editingWaveData.waveList = _editingWaveData.waveList.OrderBy(w => w.spawnTime).ToList();
            }
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;

            GUILayout.Space(10);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (_editingWaveData.waveList == null || _editingWaveData.waveList.Count == 0)
            {
                EditorGUILayout.HelpBox("Trống! Bấm Add Spawn Event để thêm quái.", MessageType.Info);
            }
            else
            {
                for (int i = 0; i < _editingWaveData.waveList.Count; i++)
                {
                    var element = _editingWaveData.waveList[i];

                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical(entityBlockStyle);
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.BeginHorizontal();
                    GUIStyle indexStyle = new GUIStyle(EditorStyles.boldLabel);
                    indexStyle.normal.textColor = new Color(1f, 0.65f, 0f);
                    GUILayout.Label($"🔹 EVENT #{i + 1:00}", indexStyle);
                    GUILayout.FlexibleSpace();

                    GUI.backgroundColor = new Color(0.8f, 0.2f, 0.2f);
                    if (GUILayout.Button("✕", GUILayout.Width(22), GUILayout.Height(18)))
                    {
                        _editingWaveData.waveList.RemoveAt(i);
                        EditorUtility.SetDirty(_editingWaveData);
                        break;
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(4);

                    element.entity = (EntityData)EditorGUILayout.ObjectField("Prefab Config", element.entity, typeof(EntityData), false);
                    element.spawnTime = EditorGUILayout.Slider("Thời Điểm (Giây)", element.spawnTime, 0f, 300f);
                    element.spawnPointIndex = EditorGUILayout.IntSlider("Mốc Spawn (0-7)", element.spawnPointIndex, 0, 7);

                    _editingWaveData.waveList[i] = element;
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndScrollView();

            if (GUI.changed && !Application.isPlaying) EditorUtility.SetDirty(_editingWaveData);
        }
        else
        {
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("Chưa chọn WaveData. Kéo thả 1 cục Data từ Project hoặc từ mảng bên trái sang đây để đục code.", MessageType.Info);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (GUI.changed && !Application.isPlaying) EditorUtility.SetDirty(_manager);
    }

    // Cheat hack đẩy Max index để xả hết timeline, ép Manager nhảy sang WaveOutro
    private void ForceEndWaveSpawning()
    {
        if (_manager.curWaveData == null || _manager.curWaveState != WaveStates.WaveCombat)
        {
            Debug.LogWarning("Chỉ bấm Skip được khi đang ở State Combat!");
            return;
        }

        FieldInfo indexField = typeof(WaveManager).GetField("currentEventIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        if (indexField != null)
        {
            indexField.SetValue(_manager, _manager.curWaveData.waveList.Count);
        }
    }
}