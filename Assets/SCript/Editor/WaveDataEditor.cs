//using UnityEngine;
//using UnityEditor;
//using System.Linq;

//[CustomEditor(typeof(WaveData))]
//public class WaveDataEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        WaveData waveData = (WaveData)target;

//        // Đổi màu chữ tiêu đề cho nó rực rỡ tí
//        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
//        titleStyle.normal.textColor = new Color(0.2f, 0.8f, 0.9f);
//        titleStyle.fontSize = 14;

//        GUILayout.Space(10);
//        GUILayout.Label("⚙️ BẢNG THIẾT KẾ NHỊP ĐỘ WAVE", titleStyle);
//        waveData.waveName = EditorGUILayout.TextField("Tên Wave:", waveData.waveName);

//        GUILayout.Space(10);

//        if (waveData.waveList == null)
//            waveData.waveList = new System.Collections.Generic.List<WaveElement>();

//        // Thiết kế Layout 2 nút bấm ngang hàng nhìn cho gọn
//        EditorGUILayout.BeginHorizontal();

//        GUI.backgroundColor = Color.green;
//        if (GUILayout.Button("➕ Thêm Quái Mới", GUILayout.Height(30)))
//        {
//            waveData.waveList.Add(new WaveElement());
//        }

//        GUI.backgroundColor = Color.yellow;
//        if (GUILayout.Button("⚡ Sắp Xếp Theo Giây", GUILayout.Height(30)))
//        {
//            // Dùng Linq để sắp xếp lại danh sách theo thời gian tăng dần
//            waveData.waveList = waveData.waveList.OrderBy(w => w.spawnTime).ToList();
//        }

//        EditorGUILayout.EndHorizontal();
//        GUI.backgroundColor = Color.white; // Reset lại màu mặc định

//        EditorGUILayout.Space(10);

//        // Vòng lặp hiển thị danh sách quái
//        for (int i = 0; i < waveData.waveList.Count; i++)
//        {
//            var element = waveData.waveList[i];

//            EditorGUILayout.BeginVertical("box");

//            EditorGUILayout.BeginHorizontal();
//            GUILayout.Label($"Thực thể #{i + 1}", EditorStyles.boldLabel);
//            GUILayout.FlexibleSpace();

//            // Nút xóa màu đỏ nhỏ gọn góc bên phải
//            GUI.backgroundColor = Color.red;
//            if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(18)))
//            {
//                waveData.waveList.RemoveAt(i);
//                break; // Dừng loop khi xóa để tránh lỗi index
//            }
//            GUI.backgroundColor = Color.white;
//            EditorGUILayout.EndHorizontal();

//            // 1. Ô chọn ScriptableObject Data của Quái
//            element.entity = (EntityData)EditorGUILayout.ObjectField("Data Cấu Hình:", element.entity, typeof(EntityData), false);

//            // 2. Thanh Slider kéo từ 0 đến 300 giây (5 phút)
//            element.spawnTime = EditorGUILayout.Slider("Giây xuất hiện:", element.spawnTime, 0f, 300f);

//            // 3. CHỐT HẠ: Thanh chọn vị trí từ mốc 0 đến 7 (8 cái spawnPoint ngoài Scene)
//            element.spawnPointIndex = EditorGUILayout.IntSlider("Mốc Vị Trí (0-7):", element.spawnPointIndex, 0, 7);

//            // Gán ngược dữ liệu vừa chỉnh sửa vào struct trong list
//            waveData.waveList[i] = element;

//            EditorGUILayout.EndVertical();
//            EditorGUILayout.Space(5);
//        }

//        // Nếu có thay đổi gì thì Unity tự động lưu (để không bị mất data khi tắt Engine)
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(waveData);
//        }
//    }
//}