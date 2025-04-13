using System.Text;

namespace SoundMaker
{
    /// <summary>
    /// WAVEファイルの出力
    /// </summary>
    class WaveWriter
    {
        /// <summary>
        /// WAVEファイルの出力
        /// </summary>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="arr">データ配列</param>
        /// <returns>処理成否</returns>
        public bool Execute(string filepath, int[] arr)
        {
            try
            {
                using (FileStream s = new FileStream(filepath, FileMode.OpenOrCreate))
                {
                    WaveHeader h = new WaveHeader();
                    int headerSize = 44;
                    int totalFileSize = headerSize + arr.Length;
                    h.ChankSize = totalFileSize - 8;
                    h.SubchankSize = arr.Length;
                    int size = WriteHeader(s, h);

                    for (int i = 0; i < arr.Length; i++)
                    {
                        WriteInt(s, arr[i], 2);
                        size += 2;
                    }
                    size += arr.Length;

                }
                return true;
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message.IndexOf("test.wav' にアクセスできません。") > -1)
                {
                    MessageBox.Show("プレーヤーを閉じてから実行してください。");
                }
                else
                {
                    MessageBox.Show(ex.ToString());
                }
                return false;
            }
        }

        /// <summary>
        /// ヘッダ書き込み
        /// </summary>
        /// <param name="s">ファイルストリームオブジェクト</param>
        /// <param name="h">ヘッダ情報</param>
        /// <returns>書き込んだサイズ</returns>
        private int WriteHeader(FileStream s, WaveHeader h)
        {
            int size = 0;
            size += WriteString(s, WaveHeader.IDENTIFIER);
            size += WriteInt(s, h.ChankSize, 4);

            size += WriteString(s, WaveHeader.FORMAT);
            size += WriteString(s, WaveHeader.FMT_IDENTIFER);

            size += WriteInt(s, h.FmtChank, 4);
            size += WriteInt(s, h.SoundFormat, 2);
            size += WriteInt(s, h.ChannelCount, 2);
            size += WriteInt(s, h.SamplingRate, 4);
            size += WriteInt(s, h.BytesPerSecond, 4);
            size += WriteInt(s, h.BlockSize, 2);
            size += WriteInt(s, h.BitPerSample, 2);
            size += WriteString(s, WaveHeader.SUBCHANK);
            size += WriteInt(s, h.SubchankSize, 4);

            return size;
        }

        /// <summary>
        /// 文字列の書き込み
        /// </summary>
        /// <param name="s">ファイルストリームオブジェクト</param>
        /// <param name="str">書き込む文字列</param>
        /// <returns>書き込んだサイズ</returns>
        private int WriteString(FileStream s, string str)
        {
            byte[] b = Encoding.ASCII.GetBytes(str);
            s.Write(b, 0, b.Length);
            return b.Length;
        }

        /// <summary>
        /// 数値の書き込み
        /// </summary>
        /// <param name="s">ファイルストリームオブジェクト</param>
        /// <param name="num">書き込む数値</param>
        /// <param name="size">書き込みサイズ</param>
        /// <returns>書き込んだサイズ</returns>
        private int WriteInt(FileStream s, int num, int size)
        {
            byte[] b = new byte[size];
            Array.Copy(BitConverter.GetBytes(num), b, size);
            s.Write(b, 0, b.Length);
            return b.Length;
        }
    }
}
