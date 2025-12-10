using LottieAnimation.Models;
using SkiaSharp.Extended.UI.Controls;
using System.Text;
using System.Text.Json;

namespace LottieAnimation.Utilities
{
    public static class LottieLoader
    {
        public static SKFileLottieImageSource FromFile(string fileName)
        {
            return new SKFileLottieImageSource
            {
                File = fileName
            };
        }

        public static LottieInfo GetAnimationInfo(string jsonContent)
        {
            try
            {
                var info = new LottieInfo();

                // Try to parse using System.Text.Json for more reliable parsing
                try
                {
                    using var document = JsonDocument.Parse(jsonContent);
                    var root = document.RootElement;

                    // Get version
                    if (root.TryGetProperty("v", out var version))
                        info.Version = version.GetString() ?? "";

                    // Get frame rate
                    if (root.TryGetProperty("fr", out var frameRate))
                    {
                        if (frameRate.ValueKind == JsonValueKind.Number)
                            info.FrameRate = frameRate.GetInt32();
                    }

                    // Get width
                    if (root.TryGetProperty("w", out var width))
                    {
                        if (width.ValueKind == JsonValueKind.Number)
                            info.Width = width.GetInt32();
                    }

                    // Get height
                    if (root.TryGetProperty("h", out var height))
                    {
                        if (height.ValueKind == JsonValueKind.Number)
                            info.Height = height.GetInt32();
                    }

                    // Get in-point (start frame)
                    if (root.TryGetProperty("ip", out var inPoint))
                    {
                        if (inPoint.ValueKind == JsonValueKind.Number)
                            info.InPoint = inPoint.GetDouble();
                    }

                    // Get out-point (end frame)
                    if (root.TryGetProperty("op", out var outPoint))
                    {
                        if (outPoint.ValueKind == JsonValueKind.Number)
                            info.OutPoint = outPoint.GetDouble();
                    }

                    // Calculate duration
                    if (info.FrameRate > 0 && info.OutPoint > info.InPoint)
                    {
                        info.TotalFrames = (int)(info.OutPoint - info.InPoint);
                        info.Duration = info.TotalFrames / (double)info.FrameRate;
                    }
                }
                catch
                {
                    // Fallback to regex parsing if JSON parsing fails
                    ParseWithRegex(jsonContent, info);
                }

                // Get file size
                info.FileSizeKB = Encoding.UTF8.GetByteCount(jsonContent) / 1024.0;

                return info;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing animation info: {ex.Message}");
                return new LottieInfo
                {
                    FrameRate = 30,
                    TotalFrames = 90,
                    Duration = 3.0
                };
            }
        }

        private static void ParseWithRegex(string jsonContent, LottieInfo info)
        {
            try
            {
                // Get version
                var versionMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""v""\s*:\s*""([^""]+)""");
                if (versionMatch.Success)
                    info.Version = versionMatch.Groups[1].Value;

                // Get fps
                var fpsMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""fr""\s*:\s*([\d.]+)");
                if (fpsMatch.Success)
                    info.FrameRate = (int)double.Parse(fpsMatch.Groups[1].Value);

                // Get width & height
                var widthMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""w""\s*:\s*(\d+)");
                if (widthMatch.Success)
                    info.Width = int.Parse(widthMatch.Groups[1].Value);

                var heightMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""h""\s*:\s*(\d+)");
                if (heightMatch.Success)
                    info.Height = int.Parse(heightMatch.Groups[1].Value);

                // Get in-point and out-point
                var ipMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""ip""\s*:\s*([\d.]+)");
                if (ipMatch.Success)
                    info.InPoint = double.Parse(ipMatch.Groups[1].Value);

                var opMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""op""\s*:\s*([\d.]+)");
                if (opMatch.Success)
                    info.OutPoint = double.Parse(opMatch.Groups[1].Value);

                // Calculate duration
                if (info.FrameRate > 0 && info.OutPoint > info.InPoint)
                {
                    info.TotalFrames = (int)(info.OutPoint - info.InPoint);
                    info.Duration = info.TotalFrames / (double)info.FrameRate;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Regex parsing error: {ex.Message}");
            }
        }

        public static string FormatDuration(double seconds)
        {
            if (seconds < 60)
            {
                return $"{seconds:F2}s";
            }

            int minutes = (int)(seconds / 60);
            double remainingSeconds = seconds % 60;

            return $"{minutes}m {remainingSeconds:F2}s";
        }

        public static string GetAnimationSummary(LottieInfo info)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Version: {info.Version}");
            sb.AppendLine($"FPS: {info.FrameRate}");
            sb.AppendLine($"Dimensions: {info.Width}×{info.Height}");
            sb.AppendLine($"Frames: {info.TotalFrames}");
            sb.AppendLine($"Duration: {FormatDuration(info.Duration)}");
            sb.AppendLine($"Size: {info.FileSizeKB:F2} KB");

            return sb.ToString();
        }
    }
}