using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth _auth;
    private DatabaseReference _dbReference;
    private string _playerId;
    public int currentHighestScore;
    public BaseStat curBaseStat;
    public List<int> baseStatID;

    public static FirebaseManager Instance { get; private set; }

    private void OnEnable()
    {
        GameEvents.OnShipChanged += ReceiveData;
    }
    private void OnDisable()
    {
        GameEvents.OnShipChanged -= ReceiveData;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi qua scene mới
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private async void Start()
    {
        GameEvents.RequestChangeShip(curBaseStat);
        // Khởi tạo Firebase Core
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            _auth = FirebaseAuth.DefaultInstance;
            _dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Chạy login ẩn danh
            await SignInAnonymouslyAsync();
            await LoadGameProgressAsync();
        }
        else
        {
            Debug.LogError($"[Firebase] Khởi tạo thất bại: {dependencyStatus}");
        }
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            var authResult = await _auth.SignInAnonymouslyAsync();
            _playerId = authResult.User.UserId;
            Debug.Log($"<color=green>[Firebase] Đăng nhập OK! PlayerID: {_playerId}</color>");


        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase] Lỗi Đăng nhập: {e.Message}");
        }
    }
    public async Task<PlayerData> LoadGameProgressAsync()
    {
        // Chặn nếu chưa có PlayerId (chưa login)
        if (string.IsNullOrEmpty(_playerId))
        {
            Debug.LogWarning("[Firebase] Chưa có PlayerID, không thể load data.");
            return null;
        }

        try
        {
            // Bước 1: Kéo ảnh chụp dữ liệu (Snapshot) từ đúng đường dẫn của user
            DataSnapshot snapshot = await _dbReference.Child("users").Child(_playerId).GetValueAsync();

            // Bước 2: Kiểm tra xem user này đã từng có data trên server chưa
            if (snapshot.Exists)
            {
                // Bước 3: Nếu ĐÃ CÓ data -> Lấy chuỗi JSON thô ra
                string jsonTho = snapshot.GetRawJsonValue();

                // Ép chuỗi JSON thô ngược thành Object PlayerData
                PlayerData dataLoaded = JsonUtility.FromJson<PlayerData>(jsonTho);

                // QUAN TRỌNG: Cập nhật kỷ lục cũ vào biến chạy nền để hàm Save hoạt động đúng
                currentHighestScore = dataLoaded.highestScore;

                Debug.Log($"<color=yellow>[Firebase] Load data thành công! Kỷ lục cũ: {currentHighestScore} | Gold: {dataLoaded.gold}</color>");
                return dataLoaded;
            }
            else
            {
                // Nếu CHƯA CÓ data (Tài khoản mới tinh) -> Khởi tạo cục data mặc định bằng 0
                Debug.Log("[Firebase] User mới chưa có data trên mây. Khởi tạo progress mặc định (0).");
                currentHighestScore = 0;

                PlayerData newData = new PlayerData { highestScore = 0, gold = 0 };
                return newData;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase] Lỗi khi kéo data về: {e.Message}");
            return null;
        }
    }
    //public async Task SaveHighestScoreAsync(int score)
    //{
    //    if (string.IsNullOrEmpty(_playerId)) return;

    //    PlayerData data = new PlayerData { highestScore = score };
    //    string json = JsonUtility.ToJson(data);

    //    try
    //    {
    //        // Đẩy data lên kho theo đường dẫn: users -> [playerId] -> dữ liệu score
    //        await _dbReference.Child("users").Child(_playerId).SetRawJsonValueAsync(json);
    //        Debug.Log($"<color=cyan>[Firebase] Đã đồng bộ highestScore ({score}) lên mây Singapore!</color>");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"[Firebase] Lỗi lưu data: {e.Message}");
    //    }
    //}
    public async Task SaveGameProgressAsync(int newScore, int currentGold)
    {
        if (string.IsNullOrEmpty(_playerId)) return;

        // Chỉ cập nhật nếu trận này phá kỷ lục cũ
        if (newScore > currentHighestScore)
        {
            currentHighestScore = newScore;
        }

        PlayerData data = new PlayerData 
        { 
            highestScore = currentHighestScore,
            gold = currentGold 
        };

        string json = JsonUtility.ToJson(data);

        try
        {
            await _dbReference.Child("users").Child(_playerId).SetRawJsonValueAsync(json);
            Debug.Log($"<color=cyan>[Firebase] Đã đồng bộ progress (Score: {currentHighestScore} | Gold: {currentGold}) lên mây!</color>");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Firebase] Lỗi lưu data: {e.Message}");
        }
    }
    public void ReceiveData(BaseStat baseStat) { this.curBaseStat = baseStat; }
}