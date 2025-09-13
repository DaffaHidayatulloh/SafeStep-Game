using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzlePiece[] puzzlePieces;   // isi semua piece di inspector
    public GameObject completeUI;        // UI yang muncul ketika puzzle selesai
    public GameObject FullPuzzle;

    [Header("Setup")]
    public int levelIndex = 1;  // Level ke berapa
    public int miniGameIndex = 1; // Mini game ke berapa (1-3)
    public int rewardScore = 50; // Score reward jika berhasil

    private bool puzzleCompleted = false;

    void Update()
    {
        if (!puzzleCompleted && CheckAllPiecesPlaced())
        {
            puzzleCompleted = true;
            OnPuzzleComplete();
        }
    }

    private bool CheckAllPiecesPlaced()
    {
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (!piece.IsPlaced()) // cek status piece
                return false;
        }
        return true;
    }

    private void OnPuzzleComplete()
    {
        Debug.Log("Puzzle Complete!");

        // Buat key unik berdasarkan level & mini game
        string completionKey = $"Puzzle_Level{levelIndex}_Mini{miniGameIndex}_Completed";
        string scoreKey = $"Score_Level{levelIndex}";

        // --- Cek apakah mini game ini sudah pernah diselesaikan ---
        if (PlayerPrefs.GetInt(completionKey, 0) == 0)
        {
            // Tambah score ke level terkait
            int currentScore = PlayerPrefs.GetInt(scoreKey, 0);
            currentScore += rewardScore;
            PlayerPrefs.SetInt(scoreKey, currentScore);

            // Tandai mini game sudah selesai
            PlayerPrefs.SetInt(completionKey, 1);
            PlayerPrefs.Save();

            Debug.Log($"Score {scoreKey} Sekarang: " + currentScore);
        }
        else
        {
            Debug.Log("Mini game ini sudah pernah diselesaikan, tidak menambah score lagi.");
        }

        // UI handling
        if (completeUI != null)
            completeUI.SetActive(true);

        if (FullPuzzle != null)
            FullPuzzle.SetActive(false);
    }
}

