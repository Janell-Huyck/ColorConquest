namespace ColorConquest.Services;

/// <summary>
/// Last known move count from the Game screen so Settings can warn before changing difficulty.
/// Updated from the game page when the view model's move count changes.
/// </summary>
public static class GameSessionSnapshot
{
    public static int LastReportedMoveCount { get; private set; }

    public static void ReportMoveCount(int moves) =>
        LastReportedMoveCount = Math.Max(0, moves);

    /// <summary>Call when difficulty change is confirmed or progress is abandoned.</summary>
    public static void ClearProgress() => LastReportedMoveCount = 0;
}
