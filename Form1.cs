using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basler.Pylon;
using System.Drawing.Imaging;
using System.Threading;
using BaslerCameraViewer;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;

namespace AOI_Baser_Camera
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        private bool mExecuting;
        public bool mIsInitializeComplete;
        private Semaphore mSemaphore;
        private PixelDataConverter mConverter;
        public int mTimeout;
        //string cameraSerialNumber = "21677823";
        string cameraSerialNumber = "22223587";
        //private int mCount;
        BaslerDevice baslerCapt;
        Image<Gray, Byte> grayImage;
        Image<Bgr, Byte> cvImg;

        public Form1()
        {
            InitializeComponent();
            try
            {
              baslerCapt = new BaslerDevice();
              //Camera  mCamera = new Camera(cameraSerialNumber);
              //   mCamera.Open();
            }
            catch (Exception e) {
                MessageBox.Show( e.ToString());
                this.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                ImageCaptureDeviceRet ret = baslerCapt.initialCamera(cameraSerialNumber); //connet to camera
                if (ret == ImageCaptureDeviceRet.SUCCESS)
                {
                    baslerCapt.startCapture();

                    //MessageBox.Show("The camera is connected");
                }
                else
                {
                    MessageBox.Show("The camera is not connected", ret.ToString());

                }
            }
            catch { }


        }

         

        private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            IGrabResult grabResult = e.GrabResult;

            this.bmp = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
            BitmapData bmpData = this.bmp.LockBits(new Rectangle(0, 0, this.bmp.Width, this.bmp.Height), ImageLockMode.ReadWrite, this.bmp.PixelFormat);
            mConverter.OutputPixelFormat = PixelType.BGRA8packed;
            IntPtr ptrBmp = bmpData.Scan0;
            mConverter.Convert(ptrBmp, bmpData.Stride * this.bmp.Height, grabResult);
            this.bmp.UnlockBits(bmpData);

            mSemaphore.Release();
            mSemaphore = new Semaphore(0, 1);
            mExecuting = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            
            Bitmap image;
            baslerCapt.captureSingleImage(out image);
            cvImg = new Image<Emgu.CV.Structure.Bgr, byte>(image);
            //rgb 都用50 做切斷值，切完後再把三個色碼合併
            //cvImg.ThresholdBinary(new Bgr(50, 50, 50), new Bgr(255, 255, 255)) 需傳回值
            //cvImg = cvImg.ThresholdBinary(new Bgr(50, 50, 50), new Bgr(255, 255, 255));
            //this.orgiPictureBox.Image = cvImg.ToBitmap();
            this.orgiPictureBox.Image = image;
            grayImage = cvImg.Convert<Gray, Byte>();
            //如果影像           0-->全黑,255-->全白
            //影像>160，設全白，小於160 設全黑
            //
            grayImage = grayImage.ThresholdBinary(new Gray(160), new Gray(255));
            this.picBox.Image = grayImage.ToBitmap();

        }

        private void Form1_Load(object sender, System.EventArgs e) { }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            baslerCapt.stopCapture();
            baslerCapt.closeCamera();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint()) {
                CvInvoke.BitwiseNot(grayImage, grayImage);//將影像黑白對調
                CvInvoke.FindContours(grayImage, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                double area=0;
                for (int i = 0; i < count; i++)
                {
                    //using (VectorOfPoint contour = contours[i]) { 
                    //Rectangle boundingBox = CvInvoke.BoundingRectangle(contour);
                    //CvInvoke.Rectangle(cvImg, boundingBox, new MCvScalar(255, 0, 255, 255), 3);
                    CvInvoke.DrawContours(cvImg, contours, i, new MCvScalar(255, 0, 255, 255), 3);
                     area = CvInvoke.ContourArea(contours[i]);
                   
                }
                CvInvoke.PutText(cvImg,
                       area.ToString(),
                       new System.Drawing.Point(10, 80),
                       FontFace.HersheyComplex,
                       3.0,
                       new Bgr(0, 255, 0).MCvScalar);
                this.picBox.Image = cvImg.ToBitmap();
            }
        }
    }
}
