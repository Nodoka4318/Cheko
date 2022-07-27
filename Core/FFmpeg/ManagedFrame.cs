using FFmpeg.AutoGen;
using System;

namespace mp4Utl.Core.FFmpeg
{
    /// <summary>
    /// ラップされた <see cref="AVFrame"/> を表現します。解放漏れを防止します。
    /// </summary>
    public unsafe class ManagedFrame : IDisposable
    {
        public ManagedFrame(AVFrame* frame)
        {
            this.frame = frame;
        }

        private readonly AVFrame* frame;

        /// <summary>
        /// ラップされたフレームを取得します。このフレームを独自に解放しないでください。
        /// </summary>
        public AVFrame* Frame { get => frame; }

        ~ManagedFrame()
        {
            DisposeUnManaged();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DisposeUnManaged();
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;

        private void DisposeUnManaged()
        {
            if (isDisposed) { return; }

            AVFrame* aVFrame = frame;
            ffmpeg.av_frame_free(&aVFrame);

            isDisposed = true;
        }
    }
}
