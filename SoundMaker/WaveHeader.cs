namespace SoundMaker
{

    //https://www.youfit.co.jp/archives/1418

    /// <summary>Waveヘッダ情報</summary>
    class WaveHeader
    {
        /// <summary>RIFF識別子</summary>
        public const string IDENTIFIER = "RIFF";

        /// <summary>チャンクサイズ</summary>
        public int ChankSize { get; set; }  = 0;

        /// <summary>フォーマット</summary>
        public const string FORMAT = "WAVE";

        /// <summary>fmt識別子</summary>
        public const string FMT_IDENTIFER = "fmt ";

        /// <summary>fmtチャンクのバイト数</summary>
        public int FmtChank = 16;

        /// <summary>音声フォーマット 1:非圧縮のリニアPCM</summary>
        public int SoundFormat { get; set; } = 1;

        /// <summary>チャンネル数 1:モノラル</summary>
        public int ChannelCount { get; set; } = 1;

        /// <summary>サンプリング周波数(Hz) 44.1kHz=44100Hz</summary>
        public int SamplingRate { get; set; } = 44100;

        /// <summary>1秒あたりバイト数平均 44.1kHz、16bit、モノラル＝44100x2x1=88,200</summary>
        public int BytesPerSecond { get; set; } = 88200;

        /// <summary>ブロックサイズ</summary>
        public int BlockSize { get; set; } = 2;

        /// <summary>サンプルごとのビット数</summary>
        public int BitPerSample { get; set; } = 16;

        /// <summary>拡張パラメータのサイズ</summary>
        public int ExtensionParameterSize { get; set; } = 0;

        /// <summary>拡張パラメータ</summary>
        public byte[] ExtensionParameter { get; set; } = null;

        /// <summary>サブチャンク識別子</summary>
        public const string SUBCHANK = "data";

        /// <summary>サブチャンクサイズ</summary>
        public int SubchankSize { get; set; } = 0;
    }
}
