
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Color_Editer_Convert
{


    public partial class Form1 : Form
    {
        double M_PI = 3.14159265358979323846;
        Image image;
        Bitmap bmp;
        Bitmap rgbbmp, Hbmp, Sbmp, Vbmp, edgebmp;
        double vV, vH, vS;
        double[,] wH, wS, wV;
        double angle;

        public Form1()
        {
            InitializeComponent();
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.Title = "d";
            openFileDialog1.Filter = " All Files(*.*) |*.*| Tiff Files(*.tiff) |*.tiff| Jpg Files(jpg.*) |jpg.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strFilename = openFileDialog1.FileName;
                image = Image.FromFile(strFilename);
                bmp = new Bitmap(image, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bmp;
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = ((trackBar1.Value - 10) * 10).ToString() + "%";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = ((trackBar2.Value - 10) * 10).ToString() + "%";
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = ((trackBar3.Value - 10) * 10).ToString() + "%";
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private double MAX(double a, double b)
        {
            if (a > b) return a;
            else return b;
            
        }
        private double MIN(double a, double b)
        {
            if (a < b) return a;
            
            else return b;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nWidth = image.Width;
            int nHeight = image.Height;
            wH = new double[nWidth, nHeight];
            wS = new double[nWidth, nHeight];
            wV = new double[nWidth, nHeight];
            Bitmap bmp2 = new Bitmap(image, nWidth, nHeight);
            Bitmap dst = new Bitmap(image, nWidth, nHeight);
            Hbmp = new Bitmap(image, nWidth, nHeight);
            Sbmp = new Bitmap(image, nWidth, nHeight);
            Vbmp = new Bitmap(image, nWidth, nHeight);
            Color Hcolor, Scolor, Vcolor;
            double vR, vG, vB, H, S, V;
            double vMin, vMax, delta;
            for (int y = 0; y < nHeight; y++)
            {
                for (int x = 0; x + 2 < nWidth; x++)
                {
                    vB = bmp2.GetPixel(x, y).B / 255.0;
                    vG = bmp2.GetPixel(x, y).G / 255.0;
                    vR = bmp2.GetPixel(x, y).R / 255.0;

                    vMax = MAX(MAX(vB, vG), vR);
                    vMin = MIN(MIN(vB, vG), vR);

                    delta = vMax - vMin;

                    V = vMax;

                    if (delta == 0)
                    {
                        S = 0;
                        H = 0;
                        Hcolor = Color.FromArgb((int)(H * 255 / 360.0), (int)(H * 255 / 360.0), (int)(H * 255 / 360.0));
                        Scolor = Color.FromArgb((int)(S * 255.0), (int)(S * 255.0), (int)(S * 255.0));
                        Vcolor = Color.FromArgb((int)(V * 255.0), (int)(V * 255.0), (int)(V * 255.0));
                        Hbmp.SetPixel(x, y, Hcolor);
                        Sbmp.SetPixel(x, y, Scolor);
                        Vbmp.SetPixel(x, y, Vcolor);
                        wH[x, y] = H;
                        wS[x, y] = S;
                        wV[x, y] = V;
                    }
                    else
                    {
                        S = delta / vMax;

                        if (vR == vMax)
                            H = (vG - vB) / delta;
                        else if (vG == vMax)
                            H = 2 + (vB - vR) / delta;
                        else
                            H = 4 + (vR - vG) / delta;
                        H *= 60.0;
                        if (H < 0)
                            H += 360.0;
                        wH[x, y] = H;
                        wS[x, y] = S;
                        wV[x, y] = V;
                        Hcolor = Color.FromArgb((int)(H * 255 / 360.0), (int)(H * 255 / 360.0), (int)(H * 255 / 360.0));
                        Scolor = Color.FromArgb((int)(S * 255.0), (int)(S * 255.0), (int)(S * 255.0));
                        Vcolor = Color.FromArgb((int)(V * 255.0), (int)(V * 255.0), (int)(V * 255.0));
                        Hbmp.SetPixel(x, y, Hcolor);
                        Sbmp.SetPixel(x, y, Scolor);
                        Vbmp.SetPixel(x, y, Vcolor);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(Hbmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(Sbmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(Vbmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int nWidth = image.Width;
            int nHeight = image.Height;
            Bitmap bmp2 = new Bitmap(image, nWidth, nHeight);
            Bitmap dst = new Bitmap(image, nWidth, nHeight);
            rgbbmp = new Bitmap(image, nWidth, nHeight);
            Color rgbcolor;

            double f, t, n, p;
            int af, at, an, ap;
            for (int y = 0; y < nHeight; y++)
            {

                for (int x = 0; x + 2 < nWidth; x++)
                {
                    vS = wS[x, y];
                    vH = wH[x, y];
                    vV = wV[x, y];
                    vV = vV * (trackBar1.Value) / 10;
                    vH = vH * (trackBar2.Value) / 10;
                    vS = vS * (trackBar3.Value) / 10;

                    if (vS == 0)
                    {

                        int a = (int)(vV * 255);
                        if (a > 255) a = 255;

                        rgbcolor = Color.FromArgb(a, a, a);

                        rgbbmp.SetPixel(x, y, rgbcolor);
                    }
                    else
                    {
                        while (vH >= 360)
                            vH -= 360;
                        while (vH < 0)
                            vH += 360;
                        vH /= 60.0;
                        int k = (int)vH;
                        f = vH - k;
                        t = vV * (1 - vS);
                        n = vV * (1 - vS * f);
                        p = vV * (1 - vS * (1 - f));
                        at = (int)(t * 255);
                        if (at >= 255) at = 255;
                        else if (at < 0) at = 0;
                        an = (int)(n * 255);
                        if (an >= 255) an = 255;
                        else if (an < 0) an = 0;
                        ap = (int)(p * 255);
                        if (ap >= 255) ap = 255;
                        else if (ap < 0) ap = 0;
                        af = (int)(vV * 255);
                        if (af >= 255) af = 255;
                        else if (af < 0) af = 0;

                        switch (k)
                        {
                            case 1:
                                rgbcolor = Color.FromArgb(an, af, at);
                                rgbbmp.SetPixel(x, y, rgbcolor);
                                break;
                            case 2:
                                rgbcolor = Color.FromArgb(at, af, ap);
                                rgbbmp.SetPixel(x, y, rgbcolor);
                                break;
                            case 3:
                                rgbcolor = Color.FromArgb(at, an, af);
                                rgbbmp.SetPixel(x, y, rgbcolor);
                                break;
                            case 4:
                                rgbcolor = Color.FromArgb(ap, at, af);
                                rgbbmp.SetPixel(x, y, rgbcolor);
                                break;
                            case 5:
                                rgbcolor = Color.FromArgb(af, at, an);
                                rgbbmp.SetPixel(x, y, rgbcolor);
                                break;
                            default:
                                rgbcolor = Color.FromArgb(af, ap, at);
                                rgbbmp.SetPixel(x, y, rgbcolor);
                                break;
                        }
                    }

                }

            }

            bmp = new Bitmap(rgbbmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
        }
        //소벨 엣지 변환
        private void button8_Click(object sender, EventArgs e) 
        {
            bmp = new Bitmap(image);
            edgebmp = new Bitmap(SobelEdgeDetect(bmp));
            bmp = new Bitmap(edgebmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
        }
        private Bitmap SobelEdgeDetect(Bitmap original)
        {
            Bitmap b = original;
            Bitmap bb = original;
            int width = b.Width;
            int height = b.Height;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            int[,] allPixR = new int[width, height];
            int[,] allPixG = new int[width, height];
            int[,] allPixB = new int[width, height];

            int limit = 128 * 128;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixR[i, j] = b.GetPixel(i, j).R;
                    allPixG[i, j] = b.GetPixel(i, j).G;
                    allPixB[i, j] = b.GetPixel(i, j).B;
                }
            }

            int new_rx = 0, new_ry = 0;
            int new_gx = 0, new_gy = 0;
            int new_bx = 0, new_by = 0;
            int rc, gc, bc;
            for (int i = 1; i < b.Width - 1; i++)
            {
                for (int j = 1; j < b.Height - 1; j++)
                {

                    new_rx = 0;
                    new_ry = 0;
                    new_gx = 0;
                    new_gy = 0;
                    new_bx = 0;
                    new_by = 0;
                    rc = 0;
                    gc = 0;
                    bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rc = allPixR[i + hw, j + wi];
                            new_rx += gx[wi + 1, hw + 1] * rc;
                            new_ry += gy[wi + 1, hw + 1] * rc;

                            gc = allPixG[i + hw, j + wi];
                            new_gx += gx[wi + 1, hw + 1] * gc;
                            new_gy += gy[wi + 1, hw + 1] * gc;

                            bc = allPixB[i + hw, j + wi];
                            new_bx += gx[wi + 1, hw + 1] * bc;
                            new_by += gy[wi + 1, hw + 1] * bc;
                        }
                    }
                    if (new_rx * new_rx + new_ry * new_ry > limit || new_gx * new_gx + new_gy * new_gy > limit || new_bx * new_bx + new_by * new_by > limit)
                        bb.SetPixel(i, j, Color.White);
                    else
                        bb.SetPixel(i, j, Color.Black);
                }
            }
            return bb;

        }
        
        
        //직선 그리기
        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap linebmp;
            linebmp = edgebmp;
            double[] arrRho = new double[100];
            double[] arrTheta = new double[100];
            string nTNum = "";
            string nTVal = "";
            if (InputBox("문턱값 입력", "입력 : ", ref nTNum) == DialogResult.OK)
            {

            }
            if (InputBox("경계선 픽셀", "입력 (0~255) : ", ref nTVal) == DialogResult.OK)
            {

            }
            int nLine = HoughLines(linebmp, int.Parse(nTNum), int.Parse(nTVal), 1.0, 100, arrRho, arrTheta);
          
            //for (int i = 0; i < 91; i++ )
                //MessageBox.Show(arrTheta[i].ToString());
            for (int i = 0; i < nLine; i++)
            {
                if(arrTheta[i] == 90) //수직선
                {
                    DrawLine(linebmp, (int)arrRho[i], 0, (int)arrRho[i], linebmp.Height-1, 255,0,0);
                }
                else
                {
                    int x1 = 0;
                    int y1 = (int)(arrRho[i]/Math.Cos(arrTheta[i]*M_PI/180) + 0.5);
                    int x2 = linebmp.Width - 1;
                    int y2 = (int)(((arrRho[i]-x2*Math.Sin(arrTheta[i]*M_PI/180))/Math.Cos(arrTheta[i]*M_PI/180) + 0.5));
                   
                    DrawLine(linebmp, x1, y1, x2, y2, 255, 0, 0);

                }
            }
            bmp = new Bitmap(linebmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;

        }

        int HoughLines(Bitmap imageIn, int nTNum, int nTVal, double resTheta, int numLine, double[] pRho, double[] pTheta)
        {
            int nWidth = imageIn.Width;
            int nHeight = imageIn.Height;

            int numRhoH = (int)(Math.Sqrt((double)(nWidth * nWidth + nHeight * nHeight))); // 영상 대각선의 길이
            int numRho = numRhoH * 2; // rho의 음수 영역을 위해 2를 곱함
            int numThe = (int)(180 / resTheta);
            int numTrans = numRho * numThe; // rho와 theta 조합의 출현 횟수를 저장하는 공간

            double[] sinLUT = new double[numThe]; // sin 함수 룩업 테이블
            double[] cosLUT = new double[numThe]; // cos 함수 룩업 테이블
            double toRad = M_PI / numThe;

            for (int theta = 0; theta < numThe; theta++)
            {
                sinLUT[theta] = (double)Math.Sin(theta * toRad);
                cosLUT[theta] = (double)Math.Cos(theta * toRad);
            }

            int[] pCntTrans = new int[numTrans];
            Array.Clear(pCntTrans, 0, numTrans);


            for (int r = 0; r < nHeight; r++)
            {
                for (int c = 0; c < nWidth; c++)
                {
                    if ((int)(imageIn.GetPixel(c, r).G) > nTVal) // 경계선 픽셀
                    {
                        for (int theta = 0; theta < numThe; theta++)
                        {
                            int rho = (int)(c * sinLUT[theta] + r * cosLUT[theta] + numRhoH + 0.5);
                            pCntTrans[rho * numThe + theta]++;
                        }
                    }
                }
            }

            // nThreshold을넘는 결과 저장
            int nLine = 0;
            for (int i = 0; i < numTrans && nLine < numLine; i++)
            {
                if (pCntTrans[i] > nTNum)
                {
                    pRho[nLine] = (int)(i / numThe); // rho의 인덱스
                    pTheta[nLine] = (i - pRho[nLine] * numThe) * resTheta; //theta 의 인덱스
                    pRho[nLine] = pRho[nLine] - numRhoH; // 음수 값이 차지하는 위치만큼 뺄셈
                    nLine++;
                }
            }
            return nLine;
        }
        
        void DrawLine(Bitmap canvas, int x1, int y1, int x2, int y2, int R, int G, int B)
        {
            // MessageBox.Show("좌표 : " + x1 + "  " + x2 + " " + y1 + "  " + y2);
            int xs, ys, xe, ye;
            int dx, dy;
            if (x1 == x2) // 수직선
            {
                if (y1 < y2) { ys = y1; ye = y2; }
                else { ys = y2; ye = y1; }
                for (int r = ys; r <= ye; r++)
                {
                    canvas.SetPixel(x1, r, Color.Red);
                }
                return;
            }

            double a = (double)(y2 - y1) / (double)(x2 - x1); // 기울기

            dx = x2 - x1;
            dy = y2 - y1;

            double rad = Math.Atan2(dy, dx);
            double degree = (rad * 180) / Math.PI;

            if (a != 0)
            {
                angle = degree;
            }

            int nHeight = canvas.Height;

            if ((a > -1) && (a < 1)) // 가로축에 가까움
            {
                if (x1 < x2)
                {
                    xs = x1; xe = x2; ys = y1; ye = y2;
                }
                else
                {
                    xs = x2; xe = x1; ys = y2; ye = y1;
                }
                for (int c = xs; c <= xe; c++)
                {
                    int r = (int)(a * (c - xs) + ys + 0.5);
                    if (r < 0 || r >= nHeight)
                        continue;

                    canvas.SetPixel(c, r, Color.Red);
                }
            }
            else // 세로축에 가까움
            {
                double invA = 1.0 / a;
                if (y1 < y2) { ys = y1; ye = y2; xs = x1; xe = x2; }
                else { ys = y2; ye = y1; xs = x2; xe = x1; }
                for (int r = ys; r <= ye; r++)
                {
                    int c = (int)(invA * (r - ys) + xs + 0.5);
                    if (r < 0 || r >= nHeight)
                        continue;
                    canvas.SetPixel(c, r, Color.Red);
                }

            }
            bmp = new Bitmap(canvas, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
        } 


        



        private void button9_Click(object sender, EventArgs e)
        {
            Bitmap linebmp;
            linebmp = new Bitmap(edgebmp);
            double[] arrRho = new double[100];
            double[] arrTheta = new double[100];
            string nTNum = "";
            string nTVal = "";
            if (InputBox("문턱값 입력", "입력 : ", ref nTNum) == DialogResult.OK)
            {
             
            }
            if (InputBox("경계선 픽셀", "입력 (0~255) : ", ref nTVal) == DialogResult.OK)
            {
                
            }
            int nLine = HoughLines(linebmp, int.Parse(nTNum), int.Parse(nTVal), 1.0, 100, arrRho, arrTheta);
                        
            for (int i = 0; i < nLine; i++)
            {
                if (arrTheta[i] == 90) //수직선
                {
                    DrawLine(linebmp, (int)arrRho[i], 0, (int)arrRho[i], linebmp.Height - 1, 255, 0, 0);
                }
                else
                {
                    int x1 = 0;
                    int y1 = (int)(arrRho[i] / Math.Cos(arrTheta[i] * M_PI / 180) + 0.5);
                    int x2 = linebmp.Width - 1;
                    int y2 = (int)(((arrRho[i] - x2 * Math.Sin(arrTheta[i] * M_PI / 180)) / Math.Cos(arrTheta[i] * M_PI / 180) + 0.5));

                    DrawLine(linebmp, x1, y1, x2, y2, 0, 255, 0);

                    if (y1 > y2)
                        DrawLine(linebmp, x1, y1, x2, y1, 255, 0, 0);
                    else
                        DrawLine(linebmp, x1, y2, x2, y2, 255, 0, 0);

                }
            }
            bmp = new Bitmap(linebmp, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
            if (angle > 0)
                MessageBox.Show("사진이 오른쪽으로" + Math.Abs(angle).ToString("F1") + "도 기울어져 있습니다");
            else
                MessageBox.Show("사진이 왼쪽으로" + Math.Abs(angle).ToString("F1") + "도 기울어져 있습니다");
        }
        private int IN_IMG(int x, int lo, int hi)
        {
            if (x < lo) return lo;
            else if (x > hi) return hi;
            else return x;
        }
        
        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Bitmap bmp5 = new Bitmap(image);
            bmp = new Bitmap(RotateImage(bmp5, new PointF(image.Width / 2, image.Height / 2), -(float)angle), (int)(image.Width), (int)(image.Height));
            pictureBox2.Image = bmp;


            /*gBuf = new int[image.Height, image.Width];
            rBuf = new int[image.Height, image.Width];
            bBuf = new int[image.Height, image.Width];
            double cx = bmp4.Width / 2.0;
            double cy = bmp4.Height / 2.0;
            
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                {
                    gBuf[y, x] = (int)bmp4.GetPixel(x, y).G;
                    rBuf[y, x] = (int)bmp4.GetPixel(x, y).R;
                    bBuf[y, x] = (int)bmp4.GetPixel(x, y).B;
                }
            double zoomX = 1.1;
            double zoomY = 1.1;
            Color c;
            //gray();

            int[,] grBuf = biLinearInterpolate(gBuf, zoomX, zoomY);
            int[,] rdBuf = biLinearInterpolate(rBuf, zoomX, zoomY);
            int[,] blBuf = biLinearInterpolate(bBuf, zoomX, zoomY);
            Bitmap bit = new Bitmap((int)(bmp4.Width * zoomX), (int)(bmp4.Height * zoomY));
            for (int i =0; i < (int)(bmp4.Height * zoomY); i++)
            {
                for (int j = 0; j < (int)(bmp4.Width * zoomX); j++)
                {
                    c = Color.FromArgb(rdBuf[i, j], grBuf[i, j], blBuf[i, j]);
                    bit.SetPixel(j, i, c);
                }
            }
            bmp = new Bitmap(bit, pictureBox2.Width, pictureBox2.Height);*/
            

            
            /*
            int x1=0, x2=0, y1=0, y2=0;
            int a, b,c;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    a = bmp4.GetPixel(x, y).R;
                    c = image.Width;
                    if (c >= image.Width) c = image.Width-1;
                    b = bmp4.GetPixel(c, y).R;
                  if (b >= image.Width) b = image.Width-1;
                    if (0 == a && 0 < b)
                    {
                        x1 = x;
                    }
                    a = bmp4.GetPixel(x, y).R;
                    c = image.Height;
                    if (c >= image.Height) c = image.Height - 1;
                    b = bmp4.GetPixel(x , c).R;
                    //if (b >= image.Height) b = image.Height- 1;
                    if (0 ==a && 0 < b)
                    {
                        y1 = y;
                    }
                    
                    /*{
                        if (x1 > x) x1 = x;
                        if (x2 < x) x2 = x;
                        if (y1 > y) y1 = y;
                        if (y2 < y) y2 = y;
                    }
                }
            }
            bmp4.SetPixel(x1, y1, Color.Red);
            pictureBox2.Image = bmp5;
            MessageBox.Show("x1 : " + x1.ToString() + "y1 : " + y1.ToString() + "x2 : " + x2.ToString() + "y2 : " + y2.ToString());




               Bitmap bmp1 = new Bitmap(image);
               Bitmap bmp2 = new Bitmap(image);
               int m_nWidth = image.Width;
               int m_nHeight = image.Height;
               double cR = Math.Cos(m_dRotation * M_PI / 180.0);
               double sR = Math.Sin(m_dRotation * M_PI / 180.0);
               double z = 1.0 / m_dScale;
               double cx = m_nWidth / 2.0;  // 영상 가로 중심
               double cy = m_nHeight / 2.0; // 영상 세로 중심

               m_dH1 = z * cR;
               m_dH2 = z * sR;
               m_dH3 = z * (cR * (-cx) + sR * (-cy)) + cx - m_dTransX;
               m_dH4 = -z * sR;
               m_dH5 = z * cR;
               m_dH6 = z * (-sR * (-cx) + cR * (-cy)) + cy - m_dTransY;
               m_dH7 = 0;
               m_dH8 = 0;
               m_dH9 = 1;
            

               int r, c;
               for (r = 0; r < m_nHeight; r++)
               {
                   for (c = 0; c < m_nWidth; c++)
                   {
                       double srcX = z * (cR * (c - cx) + sR * (r - cy)) + cx - m_dTransX;
                       double srcY = z * (-sR * (c - cx) + cR * (r - cy)) + cy - m_dTransY;

                       if (srcX >= 0 && srcX < m_nWidth && srcY >= 0 && srcY < m_nHeight)
                       {
                           switch (m_nSelIntp)
                           {
                               case 0:
                                
                                   Color color = bmp2.GetPixel(c,r).G;
                                   int px = IN_IMG((int)(srcX + 0.5), 0, m_nWidth);
                                   int py = IN_IMG((int)(srcY + 0.5), 0, m_nHeight);
                                   bmp1.SetPixel(px,py,);
                                   break;
                               case 1:
                                   m_pOut[r * m_nWStep + c] = m_imageIn.BiLinearIntp(srcX, srcY);
                                   break;
                               case 2:
                                   m_pOut[r * m_nWStep + c] = m_imageIn.CubicConvIntp(srcX, srcY);
                                   break;
                               case 3:
                                   m_pOut[r * m_nWStep + c] = m_imageIn.BiCubicIntp(srcX, srcY);
                                   break;
                               default:
                                   break;
                           }
                       }
                   }
               }*/
        }
        /*public int[,] biLinearInterpolate(int[,] gBuf, double zoomX, double zoomY)
        {
            Bitmap gBitmap = new Bitmap(image);
            int newHeight = (int)(gBitmap.Height * zoomY);
            int newWidth = (int)(gBitmap.Width * zoomX);

            int[,] R = new int[newHeight, newWidth];
            int x, y;
            double sourceX, sourceY;
            int isourceX, isourceY;
            double nw, ne, sw, se, alpha, beta;
            double cx = newWidth / 2.0;
            double cy = newHeight / 2.0;
            double cR = Math.Cos(m_dRotation * M_PI / 180.0);
            double sR = Math.Sin(m_dRotation * M_PI / 180.0);

            for (y = 0; y < newHeight; y++)
                for (x = 0; x < newWidth; x++)
                {
                    sourceX = x / zoomX;
                    sourceY = y / zoomY;
                    //isourceX = (int)(zoomX * (cR * (-cx) + sR * (-cy)) + cx - m_dTransX);
                    isourceX = (int)sourceX;
                    //isourceY = (int)(zoomY * (-sR * (-cx) + cR * (-cy)) + cy - m_dTransY);
                    isourceY = (int)sourceY;
                    if (isourceX < 0 || isourceX >= gBitmap.Width - 1 || isourceY < 0 || isourceY >= gBitmap.Height - 1)
                        R[y, x] = 0;
                    else
                    {
                        nw = (double)gBuf[isourceY, isourceX];
                        ne = (double)gBuf[isourceY, isourceX + 1];
                        sw = (double)gBuf[isourceY + 1, isourceX];
                        se = (double)gBuf[isourceY + 1, isourceX + 1];
                        beta = Math.Abs(sourceX - isourceX);
                        alpha = Math.Abs(sourceY - isourceY);
                        R[y, x] = biLinearInterpolate(nw, ne, se, se, beta, alpha);
                    }
                }
            return R;
        }

        private int biLinearInterpolate(double nw, double ne, double sw, double se, double p, double q)
        {
            double t, b;
            t = nw + p * (ne - nw);
            b = sw + p * (se - sw);
            return (int)(t + q * (b - t));
        }*/

        private void button11_Click(object sender, EventArgs e)
        {
            double zoom;
            if (angle > 0.6 && angle < 1.5) zoom = 1.05;
            else if (angle > 1.6 && angle < 2.5) zoom = 1.07;
            else if (angle > 2.6 && angle < 3.5) zoom = 1.08;
            else if (angle > 3.6 && angle < 4.5) zoom = 1.11;
            else if (angle > 4.6 && angle < 5.5) zoom = 1.13;
            else if (angle > 5.6 && angle < 6.5) zoom = 1.155;
            else if (angle > 6.6 && angle < 7.5) zoom = 1.177;
            else if (angle > 7.6 && angle < 8.5) zoom = 1.2;
            else if (angle > 8.6 && angle < 9.5) zoom = 1.23;
            else if (angle > 9.6 && angle < 10.5) zoom = 1.247;
            else if (angle > 10.6 && angle < 11.5) zoom = 1.27;
            else if (angle > 11.6 && angle < 12.5) zoom = 1.293;
            else if (angle > 12.6 && angle < 13.5) zoom = 1.315;
            else if (angle > 13.6 && angle < 14.5) zoom = 1.335;
            else if (angle > 14.6 && angle < 15.5) zoom = 1.355;
            else if (angle > 15.6 && angle < 16.5) zoom = 1.375;
            else if (angle > 16.6 && angle < 17.5) zoom = 1.395;
            else if (angle > 17.6 && angle < 18.5) zoom = 1.415;
            else if (angle > 18.6 && angle < 19.5) zoom = 1.435;
            else if (angle > 19.6 && angle < 20.5) zoom = 1.455;
            else zoom = 1;

            ImageConversion(0, zoom, zoom, 0, 0);
            pictureBox2.Image = bmp;
        }
        private void ImageConversion(double dDegree, double dExpX, double dExpY, int iMoveValX, int iMoveValY)
        {
            Bitmap bBitmap = new Bitmap(bmp);
            int i, j;
            int iTempX, iTempY;
            Color cSetColor;
            int iMoveX, iMoveY;
            double dX, dY, dValX, dValY;
            double dAngle;
            double dCos, dSin;
            int iResultR, iResultG, iResultB;
            int iHalfX, iHalfY;
            int[,] iBitDatasR = new int[image.Width, image.Height];
            int[,] iBitDatasG = new int[image.Width, image.Height];
            int[,] iBitDatasB = new int[image.Width, image.Height];

            iHalfX = image.Width / 2;
            iHalfY = image.Height / 2;
            dAngle = dDegree * Math.PI / 180;					// 라디안의 산출
            dCos = Math.Cos(dAngle);							// 코사인의 산출
            dSin = Math.Sin(dAngle);							// 사인의 산출

            // 화상의 기하 변환
            for (i = -iHalfY; i < iHalfY; i++)
                for (j = -iHalfX; j < iHalfX; j++)
                {
                    iMoveY = i;	// 세로 방향에의 이동량의 반영
                    iMoveX = j;	// 횡방향에의 이동량의 반영
                    dY = (iMoveX * dSin + iMoveY * dCos) / dExpY;	// Y 좌표의 회전 변환과 세로 방향에의 확대율의 반영
                    dX = (iMoveX * dCos - iMoveY * dSin) / dExpX;	// X좌표의 회전 변환과 횡방향에의 확대율의 반영
                    // Y 좌표가 0 보다 큰 경우
                    if (dY > 0)
                        iTempY = (int)dY;	// 수치의 설정
                    // Y 좌표가 0 이하의 경우
                    else
                        iTempY = (int)(dY - 1);	// 수치의 설정
                    // X 좌표가 0 보다 큰 경우
                    if (dX > 0)
                        iTempX = (int)dX;	// 수치의 설정
                    // X 좌표가 0 이하의 경우
                    else
                        iTempX = (int)dX - 1;	// 수치의 설정

                    dValY = dY - iTempY;	// 변환 전후의 Y 좌표의 차분 산출
                    dValX = dX - iTempX;	// 변환 전후의 X 좌표의 차분 산출

                    // 설정 색정보를 취득하는 경우
                    if ((iTempY >= -iHalfY) && (iTempY < iHalfY - 1) && (iTempX >= -iHalfX) && (iTempX < iHalfX - 1))
                    {
                        iResultB = (int)((1.0 - dValY) * ((1.0 - dValX) * bBitmap.GetPixel(iTempX + iHalfX, iTempY + iHalfY).B
                            + dValX * bBitmap.GetPixel(iTempX + iHalfX, iTempY + 1 + iHalfY).B)
                            + dValY * ((1.0 - dValX) * bBitmap.GetPixel(iTempX + 1 + iHalfX, iTempY + iHalfY).B
                            + dValX * bBitmap.GetPixel(iTempX + 1 + iHalfX, iTempY + 1 + iHalfY).B));
                        iResultR = (int)((1.0 - dValY) * ((1.0 - dValX) * bBitmap.GetPixel(iTempX + iHalfX, iTempY + iHalfY).R
                            + dValX * bBitmap.GetPixel(iTempX + iHalfX, iTempY + 1 + iHalfY).R)
                            + dValY * ((1.0 - dValX) * bBitmap.GetPixel(iTempX + 1 + iHalfX, iTempY + iHalfY).R
                            + dValX * bBitmap.GetPixel(iTempX + 1 + iHalfX, iTempY + 1 + iHalfY).R));
                        iResultG = (int)((1.0 - dValY) * ((1.0 - dValX) * bBitmap.GetPixel(iTempX + iHalfX, iTempY + iHalfY).G
                            + dValX * bBitmap.GetPixel(iTempX + iHalfX, iTempY + 1 + iHalfY).G)
                            + dValY * ((1.0 - dValX) * bBitmap.GetPixel(iTempX + 1 + iHalfX, iTempY + iHalfY).G
                            + dValX * bBitmap.GetPixel(iTempX + 1 + iHalfX, iTempY + 1 + iHalfY).G));
                    }// 색정보의 취득

                    // 설정 색정보를 취득하지 않는 경우
                    else
                    {
                        iResultR = 0;	//0 으로 설정
                        iResultG = 0;
                        iResultB = 0;
                    }
                    // iResult가 0보다 작은 경우
                    if (iResultR < 0)
                        iResultR = 0;	//0 으로 설정
                    if (iResultG < 0)
                        iResultG = 0;	//0 으로 설정
                    if (iResultB < 0)
                        iResultB = 0;	//0 으로 설정
                    // iResult가 255 큰 경우
                    if (iResultR > 255)
                        iResultR = 255;	// 255으로 설정
                    if (iResultG > 255)
                        iResultG = 255;	// 255으로 설정
                    if (iResultB > 255)
                        iResultB = 255;	// 255으로 설정
                    iBitDatasR[j + iHalfX, i + iHalfY] = iResultR;	// 계산된 결과 보관
                    iBitDatasG[j + iHalfX, i + iHalfY] = iResultG;
                    iBitDatasB[j + iHalfX, i + iHalfY] = iResultB;
                }

            // 변환 결과 표시
            bmp = new Bitmap(image.Width, image.Height);
            for (i = 0; i < image.Width; i++)
                for (j = 0; j < image.Height; j++)
                {
                    cSetColor = Color.FromArgb(iBitDatasR[i, j], iBitDatasG[i, j], iBitDatasB[i, j]);	// iArrayBitData값에 의한 색 설정
                    bBitmap.SetPixel(i, j, cSetColor);						// 픽셀 색 설정
                }
            bmp = new Bitmap(bBitmap, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;	// 변환결과 출력
        }

        
    }
}