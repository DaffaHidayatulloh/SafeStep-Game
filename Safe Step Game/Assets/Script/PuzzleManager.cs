using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzlePiece[] puzzlePieces;   // isi semua piece di inspector
    public GameObject completeUI;        // UI yang muncul ketika puzzle selesai
    public GameObject FullPuzzle;

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
        if (completeUI != null)
            completeUI.SetActive(true);
        FullPuzzle.SetActive(false);
    }
}
