namespace LottieAnimation.Models
{
    public class LottieInfo
    {
        public string Version { get; set; } = "";
        public int FrameRate { get; set; } = 30;
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 512;
        public double FileSizeKB { get; set; } = 0;

        // Frame information
        public double InPoint { get; set; } = 0;
        public double OutPoint { get; set; } = 90;
        public int TotalFrames { get; set; } = 90;

        public double Duration { get; set; } = 3.0;

        public override string ToString()
        {
            return $"Lottie Animation - Version: {Version}, FPS: {FrameRate}, " +
                   $"Size: {Width}×{Height}, Frames: {TotalFrames}, Duration: {Duration:F2}s";
        }
    }
}