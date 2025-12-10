using SkiaSharp.Extended.UI.Controls;
using System.Diagnostics;
using LottieAnimation.Utilities;
using LottieAnimation.Models;
using System.Reflection;

namespace LottieAnimation
{
    public partial class MainPage : ContentPage
    {
        private CancellationTokenSource? updateCancellationTokenSource;

        private bool isPlaying = false;
        private bool isLooping = true;
        private bool isDraggingSlider = false;
        private bool isSettingProgress = false;
        private bool isAnimationCompleting = false;
        private bool hasAnimationEnded = false;

        private double currentProgress = 0;
        private double totalDuration = 3.0;

        private int totalFrames = 90;
        private int frameRate = 30;

        public MainPage()
        {
            InitializeComponent();
            LoadIcons();
            LoadDefaultAnimation();

            lottieAnimation.AnimationCompleted += OnAnimationCompleted;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                StopUpdatingFrameInfo();

                lottieAnimation.IsAnimationEnabled = false;
                isPlaying = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnDisappearing: {ex.Message}");
            }
        }

        #region Load defaults

        private async void LoadDefaultAnimation()
        {
            try
            {
                StopUpdatingFrameInfo();
                lottieAnimation.IsAnimationEnabled = false;

                string jsonContent = null;

                using var stream = await FileSystem.OpenAppPackageFileAsync("animation.json");
                using var reader = new StreamReader(stream);

                jsonContent = await reader.ReadToEndAsync();

                if (!string.IsNullOrEmpty(jsonContent))
                {
                    ParseAnimationInfo(jsonContent);
                }
                else
                {
                    frameRate = 30;
                    totalFrames = 90;
                    totalDuration = 3.0;
                }

                lottieAnimation.Source = new SKFileLottieImageSource
                {
                    File = "animation.json"
                };
                lottieAnimation.RepeatCount = isLooping ? -1 : 0;

                // Reset to initial state
                currentProgress = 0;
                playbackSlider.Value = 0;
                hasAnimationEnded = false;
                UpdateFrameInfo();
            }
            catch (Exception ex)
            {
                frameRate = 30;
                totalFrames = 90;
                totalDuration = 3.0;
                UpdateFrameInfo();
            }
        }

        private void LoadIcons()
        {
            playPauseButton.ImageSource = ImageSource.FromFile("play.png");
        }

        #endregion

        #region Slider Events

        private void OnSliderDragStarted(object sender, EventArgs e)
        {
            try
            {
                isDraggingSlider = true;
                bool wasPlaying = isPlaying;

                StopUpdatingFrameInfo();

                if (isPlaying)
                {
                    lottieAnimation.IsAnimationEnabled = false;
                }

                Debug.WriteLine($"Slider drag started - wasPlaying: {wasPlaying}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in drag started: {ex.Message}");
            }
        }

        private void OnSliderDragCompleted(object sender, EventArgs e)
        {
            try
            {
                isDraggingSlider = false;
                hasAnimationEnded = false; // Reset end flag when manually seeking

                double finalProgress = Math.Max(0, Math.Min(0.99, playbackSlider.Value));
                currentProgress = finalProgress;

                SetAnimationProgress(currentProgress);

                Task.Delay(100).ContinueWith(_ =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (isPlaying)
                        {
                            lottieAnimation.IsAnimationEnabled = true;
                            StartUpdatingFrameInfo();
                        }
                        else
                        {
                            UpdateFrameInfo();
                        }
                    });
                });

                Debug.WriteLine($"Slider drag completed - progress: {currentProgress:F2}, isPlaying: {isPlaying}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in drag completed: {ex.Message}");
            }
        }

        private void OnPlaybackSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (isDraggingSlider && !isSettingProgress)
            {
                try
                {
                    double safeProgress = Math.Max(0, Math.Min(0.99, e.NewValue));

                    currentProgress = safeProgress;
                    SetAnimationProgress(currentProgress);
                    UpdateFrameInfo();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in slider value changed: {ex.Message}");
                }
            }
        }

        #endregion

        #region Buttons Event

        private void OnPlayPauseClicked(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                PauseAnimation();
            }
            else
            {
                // If animation has ended, restart from beginning
                if (hasAnimationEnded)
                {
                    Debug.WriteLine("Restarting animation from beginning");
                    currentProgress = 0;
                    playbackSlider.Value = 0;
                    SetAnimationProgress(0);
                    hasAnimationEnded = false;
                    UpdateFrameInfo();
                }

                PlayAnimation();
            }
        }

        private void OnToggleLoopClicked(object sender, EventArgs e)
        {
            try
            {
                isLooping = !isLooping;

                bool wasPlaying = isPlaying;
                if (wasPlaying)
                {
                    lottieAnimation.IsAnimationEnabled = false;
                    StopUpdatingFrameInfo();
                }

                lottieAnimation.RepeatCount = isLooping ? -1 : 0;
                loopButtonBorder.Opacity = isLooping ? 1.0 : 0.5;

                if (wasPlaying)
                {
                    Task.Delay(50).ContinueWith(_ =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            lottieAnimation.IsAnimationEnabled = true;
                            StartUpdatingFrameInfo();
                        });
                    });
                }

                Debug.WriteLine($"Loop toggled: {isLooping}, wasPlaying: {wasPlaying}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error toggling loop: {ex.Message}");
            }
        }

        private async void OnSelectAnimationClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Animation JSON File",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.json" } },
                        { DevicePlatform.Android, new[] { "application/json" } },
                        { DevicePlatform.WinUI, new[] { ".json" } },
                        { DevicePlatform.MacCatalyst, new[] { "json" } }
                    })
                });

                if (result != null)
                {
                    PauseAnimation();
                    await Task.Delay(100);

                    var jsonContent = await File.ReadAllTextAsync(result.FullPath);
                    ParseAnimationInfo(jsonContent);

                    lottieAnimation.Source = new SKFileLottieImageSource
                    {
                        File = result.FullPath
                    };

                    lottieAnimation.RepeatCount = isLooping ? -1 : 0;

                    currentProgress = 0;
                    playbackSlider.Value = 0;
                    hasAnimationEnded = false;
                    UpdateFrameInfo();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading file: {ex.Message}");
            }
        }

        #endregion

        #region Animation Events

        private void OnAnimationCompleted(object sender, EventArgs e)
        {
            try
            {
                if (isAnimationCompleting) return;
                isAnimationCompleting = true;

                Debug.WriteLine($"Animation completed - isLooping: {isLooping}, isPlaying: {isPlaying}");

                if (!isLooping)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            // Stop everything first
                            StopUpdatingFrameInfo();
                            lottieAnimation.IsAnimationEnabled = false;
                            isPlaying = false;
                            hasAnimationEnded = true;

                            playPauseButton.ImageSource = ImageSource.FromFile("play.png");

                            // Set to end frame
                            currentProgress = 0.99;
                            playbackSlider.Value = currentProgress;
                            SetAnimationProgress(currentProgress);
                            UpdateFrameInfo();

                            Debug.WriteLine("Animation completed (non-looping) - stopped at end");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error in animation completed handler: {ex.Message}");
                        }
                        finally
                        {
                            isAnimationCompleting = false;
                        }
                    });
                }
                else
                {
                    // For looping animations, just reset the flag
                    isAnimationCompleting = false;
                    hasAnimationEnded = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in animation completed: {ex.Message}");
                isAnimationCompleting = false;
            }
        }

        #endregion

        #region Animation Handlers

        private void PlayAnimation()
        {
            try
            {
                isPlaying = true;
                playPauseButton.ImageSource = ImageSource.FromFile("pause.png");

                lottieAnimation.IsAnimationEnabled = true;
                StartUpdatingFrameInfo();

                Debug.WriteLine($"Animation started - Progress: {currentProgress:F2}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error playing animation: {ex.Message}");
            }
        }

        private void PauseAnimation()
        {
            try
            {
                isPlaying = false;
                playPauseButton.ImageSource = ImageSource.FromFile("play.png");

                lottieAnimation.IsAnimationEnabled = false;
                StopUpdatingFrameInfo();

                currentProgress = GetAnimationProgress();
                UpdateFrameInfo();

                Debug.WriteLine($"Animation paused at progress: {currentProgress:F2}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error pausing animation: {ex.Message}");
            }
        }

        private void SetAnimationProgress(double progress)
        {
            if (isSettingProgress)
            {
                return;
            }

            try
            {
                isSettingProgress = true;

                progress = Math.Max(0, Math.Min(0.99, progress));

                PropertyInfo? progressProperty = lottieAnimation.GetType().GetProperty("Progress");
                if (progressProperty != null)
                {
                    if (progressProperty.PropertyType == typeof(TimeSpan))
                    {
                        var timeSpan = TimeSpan.FromSeconds(totalDuration * progress);
                        var maxTime = TimeSpan.FromSeconds(totalDuration * 0.99);

                        if (timeSpan > maxTime)
                        {
                            timeSpan = maxTime;
                        }

                        progressProperty.SetValue(lottieAnimation, timeSpan);
                    }
                    else if (progressProperty.PropertyType == typeof(float))
                    {
                        progressProperty.SetValue(lottieAnimation, (float)progress);
                    }
                    else if (progressProperty.PropertyType == typeof(double))
                    {
                        progressProperty.SetValue(lottieAnimation, progress);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting progress: {ex.Message}");
            }
            finally
            {
                isSettingProgress = false;
            }
        }

        private double GetAnimationProgress()
        {
            try
            {
                PropertyInfo? progressProperty = lottieAnimation.GetType().GetProperty("Progress");

                if (progressProperty != null)
                {
                    object? value = progressProperty.GetValue(lottieAnimation);

                    if (value is TimeSpan timeSpan)
                    {
                        if (totalDuration > 0)
                        {
                            double progress = timeSpan.TotalSeconds / totalDuration;
                            return Math.Max(0, Math.Min(0.99, progress));
                        }
                    }
                    else if (value is float floatValue)
                    {
                        return Math.Max(0, Math.Min(0.99, floatValue));
                    }
                    else if (value is double doubleValue)
                    {
                        return Math.Max(0, Math.Min(0.99, doubleValue));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting progress: {ex.Message}");
            }

            return currentProgress;
        }

        #endregion

        #region Frame Info

        private void UpdateFrameInfo()
        {
            try
            {
                double normalizedProgress = Math.Max(0, Math.Min(1, currentProgress));

                int currentFrame = (int)Math.Round(normalizedProgress * totalFrames);
                currentFrame = Math.Min(currentFrame, totalFrames);

                double currentTime = normalizedProgress * totalDuration;

                currentFrameLabel.Text = $"Frame:\n({currentFrame}/{totalFrames})";
                totalFrameLabel.Text = $"FPS:\n({frameRate})";
                durationLabel.Text = $"Time:\n({currentTime:F2}s/{totalDuration:F2}s)";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating frame info: {ex.Message}");
            }
        }

        private void StartUpdatingFrameInfo()
        {
            try
            {
                StopUpdatingFrameInfo();

                updateCancellationTokenSource = new CancellationTokenSource();
                var token = updateCancellationTokenSource.Token;

                Task.Run(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            await MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                if (!isDraggingSlider && !isSettingProgress && isPlaying && !isAnimationCompleting)
                                {
                                    double newProgress = GetAnimationProgress();

                                    // Safety check: if near end and not looping, let OnAnimationCompleted handle it
                                    if (!isLooping && newProgress >= 0.97)
                                    {
                                        Debug.WriteLine($"Near end: {newProgress:F2}, waiting for completion event");
                                        return;
                                    }

                                    if (Math.Abs(currentProgress - newProgress) > 0.005)
                                    {
                                        currentProgress = newProgress;

                                        if (Math.Abs(playbackSlider.Value - currentProgress) > 0.01)
                                        {
                                            playbackSlider.Value = currentProgress;
                                        }

                                        UpdateFrameInfo();
                                    }
                                }
                            });

                            await Task.Delay(50, token);
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error in update loop: {ex.Message}");
                        }
                    }
                }, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting updates: {ex.Message}");
            }
        }

        private void StopUpdatingFrameInfo()
        {
            try
            {
                updateCancellationTokenSource?.Cancel();
                updateCancellationTokenSource?.Dispose();
                updateCancellationTokenSource = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error stopping updates: {ex.Message}");
            }
        }

        #endregion

        private void ParseAnimationInfo(string jsonContent)
        {
            try
            {
                var info = LottieLoader.GetAnimationInfo(jsonContent);

                frameRate = info.FrameRate > 0 ? info.FrameRate : 30;

                var ipMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""ip""\s*:\s*([\d.]+)");
                var opMatch = System.Text.RegularExpressions.Regex.Match(jsonContent, @"""op""\s*:\s*([\d.]+)");

                if (ipMatch.Success && opMatch.Success && frameRate > 0)
                {
                    double inPoint = double.Parse(ipMatch.Groups[1].Value);
                    double outPoint = double.Parse(opMatch.Groups[1].Value);

                    totalFrames = (int)Math.Round(outPoint - inPoint);
                    totalDuration = totalFrames / (double)frameRate;
                }
                else
                {
                    totalFrames = frameRate * 3;
                    totalDuration = 3.0;
                }

                Debug.WriteLine($"Animation Info - FPS: {frameRate}, Duration: {totalDuration:F2}s, Frames: {totalFrames}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing animation info: {ex.Message}");

                frameRate = 30;
                totalFrames = 90;
                totalDuration = 3.0;
            }
        }
    }
}