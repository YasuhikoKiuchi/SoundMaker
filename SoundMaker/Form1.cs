using System.ComponentModel;

namespace SoundMaker
{
    /// <summary>フォームクラス</summary>
    public partial class Form1 : Form
    {
        /// <summary>WAVEデータ</summary>
        private int[] waveData;

        /// <summary>ピクチャボックスの横幅</summary>
        private readonly int width = 0;

        /// <summary>ピクチャボックスの高さの半分</summary>
        private readonly float height = 0;

        /// <summary>コンストラクタ</summary>
        public Form1()
        {
            InitializeComponent();

            width = pictureBox1.Width;
            height = pictureBox1.Height / 2;

            //waveData = MakeWave(128000, 2000, 0.1);
            waveData = MakeWave(44100 * 3, 2000, 0.5);

            WaveWriter w = new WaveWriter();
            w.Execute("test.wav", waveData);
        }

        /// <summary>
        /// 波データを作る
        /// </summary>
        /// <param name="dataSize">サイズ</param>
        /// <param name="r">波の半径</param>
        /// <param name="dt">角度増分</param>
        /// <returns></returns>
        private int[] MakeWave(int dataSize, int r, double dt)
        {
            var list = new List<int>();

            double theta = 0;
            for (int i = 0; i < dataSize; i++)
            {
                int a = (int)(Math.Sin(theta) * r);
                list.Add(a);
                theta += dt;
            }

            return list.ToArray();
        }

        /// <summary>
        /// 矩形波データを作る
        /// </summary>
        /// <param name="dataSize">サイズ</param>
        /// <param name="r">波の半径</param>
        /// <param name="dt">角度増分</param>
        /// <returns></returns>
        private int[] MakeWave2(int dataSize, int r, double dt)
        {
            var list = new List<int>();

            double theta = 0;
            for (int i = 0; i < dataSize; i++)
            {
                int a = (int)(Math.Sin(theta) * r);
                list.Add(a > 0 ? 1 : -r);
                theta += dt;
            }

            return list.ToArray();
        }

        /// <summary>
        /// ランダムノイズデータを作る
        /// </summary>
        /// <param name="dataSize">サイズ</param>
        /// <param name="r">波の半径</param>
        /// <param name="dt">角度増分</param>
        /// <returns></returns>
        private int[] MakeWave3(int dataSize, int r, double dt)
        {
            var list = new List<int>();
            var rand = new Random();

            for (int i = 0; i < dataSize; i++)
            {
                int a = rand.Next(0, r);
                list.Add(a);
            }

            return list.ToArray();
        }


        /// <summary>
        /// ピーン・・・という感じの波データを作る
        /// </summary>
        /// <param name="dataSize">サイズ</param>
        /// <param name="r">波の半径</param>
        /// <param name="dt">角度増分</param>
        /// <returns></returns>
        private int[] MakeWave4(int dataSize, int r, double dt)
        {
            var list = new List<int>();
            double r2 = r;
            double dr = r2 / dataSize * 2;

            double theta = 0;
            for (int i = 0; i < dataSize; i++)
            {
                int a = (int)(Math.Sin(theta) * r2);
                list.Add(a);
                theta += dt;
                r2 -= dr;
                if (r2 < 0) r2 = 0;
            }

            return list.ToArray();
        }

        /// <summary>
        /// 音階が変わる波データ(ピコピコした感じの)を作る
        /// </summary>
        /// <param name="dataSize">サイズ</param>
        /// <param name="r">波の半径</param>
        /// <param name="dt">角度増分</param>
        /// <returns></returns>
        private int[] MakeWave5(int dataSize, int r, double dt)
        {
            var list = new List<int>();

            double theta = 0;
            for (int i = 0; i < dataSize; i++)
            {
                int a = (int)(Math.Sin(theta) * r);
                list.Add(a);
                theta += dt;
                if (i > 0 && i % 10000 == 0) dt /= 2.0;
            }

            return list.ToArray();
        }

        /// <summary>
        /// 音階が変わる波データ(ドレミファソラシド)を作る
        /// </summary>
        /// <param name="dataSize">サイズ</param>
        /// <param name="r">波の半径</param>
        /// <param name="dt">角度増分</param>
        /// <returns></returns>
        private int[] MakeWave6(int dataSize, int r, double dt)
        {
            var list = new List<int>();
            double[] hz = { 261.626, 293.665, 329.628, 349.228, 391.995, 440.000, 493.883, 523.251 }; // 音声周波数
            var omega = new double[hz.Length];
            for(int i = 0; i < hz.Length; i++)
            {
                omega[i] = 2 * Math.PI * hz[i] / 44100; // 角周波数を1秒あたりのサンプル数で割る
            }
            dt = omega[0];
            int j = 1;

            double theta = 0;
            for (int i = 0; i < dataSize; i++)
            {
                int a = (int)(Math.Sin(theta) * r);
                list.Add(a);
                theta += dt;
                if (i > 0 && i % 7800 == 0) // 7800サンプルごとに音の高さを変える
                {
                    dt = omega[j++];
                    if (j >= omega.Length) j = omega.Length - 1;
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Y座標の計算
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private float CalcY(int v)
        {
            return v * (height - 20) / 12000 + height;
        }

        /// <summary>
        /// pictureBox1 描画イベント
        /// </summary>
        /// <param name="sender">イベント発生オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (waveData != null)
            {
                float x = 0;
                float y = CalcY(waveData[0]);
                int max = waveData.Length;
                //if (max > width) max = width; // 描画モードA
                float dx = (float)width / max; // 描画モードB
                for (int i = 1; i < max; i++)
                {
                    //float x2 = x + 1;  // 描画モードA
                    float x2 = i * dx + dx; // 描画モードB
                    float y2 = CalcY(waveData[i]);
                    e.Graphics.DrawLine(Pens.Yellow, x, y, x2, y2);
                    x = x2;
                    y = y2;
                }
            }
        }
    }
}