using System.ComponentModel;

namespace SoundMaker
{
    /// <summary>�t�H�[���N���X</summary>
    public partial class Form1 : Form
    {
        /// <summary>WAVE�f�[�^</summary>
        private int[] waveData;

        /// <summary>�s�N�`���{�b�N�X�̉���</summary>
        private readonly int width = 0;

        /// <summary>�s�N�`���{�b�N�X�̍����̔���</summary>
        private readonly float height = 0;

        /// <summary>�R���X�g���N�^</summary>
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
        /// �g�f�[�^�����
        /// </summary>
        /// <param name="dataSize">�T�C�Y</param>
        /// <param name="r">�g�̔��a</param>
        /// <param name="dt">�p�x����</param>
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
        /// ��`�g�f�[�^�����
        /// </summary>
        /// <param name="dataSize">�T�C�Y</param>
        /// <param name="r">�g�̔��a</param>
        /// <param name="dt">�p�x����</param>
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
        /// �����_���m�C�Y�f�[�^�����
        /// </summary>
        /// <param name="dataSize">�T�C�Y</param>
        /// <param name="r">�g�̔��a</param>
        /// <param name="dt">�p�x����</param>
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
        /// �s�[���E�E�E�Ƃ��������̔g�f�[�^�����
        /// </summary>
        /// <param name="dataSize">�T�C�Y</param>
        /// <param name="r">�g�̔��a</param>
        /// <param name="dt">�p�x����</param>
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
        /// ���K���ς��g�f�[�^(�s�R�s�R����������)�����
        /// </summary>
        /// <param name="dataSize">�T�C�Y</param>
        /// <param name="r">�g�̔��a</param>
        /// <param name="dt">�p�x����</param>
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
        /// ���K���ς��g�f�[�^(�h���~�t�@�\���V�h)�����
        /// </summary>
        /// <param name="dataSize">�T�C�Y</param>
        /// <param name="r">�g�̔��a</param>
        /// <param name="dt">�p�x����</param>
        /// <returns></returns>
        private int[] MakeWave6(int dataSize, int r, double dt)
        {
            var list = new List<int>();
            double[] hz = { 261.626, 293.665, 329.628, 349.228, 391.995, 440.000, 493.883, 523.251 }; // �������g��
            var omega = new double[hz.Length];
            for(int i = 0; i < hz.Length; i++)
            {
                omega[i] = 2 * Math.PI * hz[i] / 44100; // �p���g����1�b������̃T���v�����Ŋ���
            }
            dt = omega[0];
            int j = 1;

            double theta = 0;
            for (int i = 0; i < dataSize; i++)
            {
                int a = (int)(Math.Sin(theta) * r);
                list.Add(a);
                theta += dt;
                if (i > 0 && i % 7800 == 0) // 7800�T���v�����Ƃɉ��̍�����ς���
                {
                    dt = omega[j++];
                    if (j >= omega.Length) j = omega.Length - 1;
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Y���W�̌v�Z
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private float CalcY(int v)
        {
            return v * (height - 20) / 12000 + height;
        }

        /// <summary>
        /// pictureBox1 �`��C�x���g
        /// </summary>
        /// <param name="sender">�C�x���g�����I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g����</param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (waveData != null)
            {
                float x = 0;
                float y = CalcY(waveData[0]);
                int max = waveData.Length;
                //if (max > width) max = width; // �`�惂�[�hA
                float dx = (float)width / max; // �`�惂�[�hB
                for (int i = 1; i < max; i++)
                {
                    //float x2 = x + 1;  // �`�惂�[�hA
                    float x2 = i * dx + dx; // �`�惂�[�hB
                    float y2 = CalcY(waveData[i]);
                    e.Graphics.DrawLine(Pens.Yellow, x, y, x2, y2);
                    x = x2;
                    y = y2;
                }
            }
        }
    }
}