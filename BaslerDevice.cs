using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Basler.Pylon;

namespace BaslerCameraViewer
{
    #region 函數回傳值
    public enum ImageCaptureDeviceRet : int
    {
        ERROR = -1,
        ERROR_TIMEOUT = -994,
        ERROR_INVALID_RESOLUTION = -995,
        ERROR_RESOLUTION_NOT_SET = -996,
        ERROR_CAMERA_ALREADY_INITIALIZED = -997,
        ERROR_CAMERA_NOT_INITIALIZED = -998,
        ERROR_UNKNOWN = -999,
        SUCCESS = 0
    }
    #endregion

    public class BaslerDevice
    {
        #region 變數宣告
        private Camera mCamera;
        private bool mExecuting;
        public bool mIsInitializeComplete;
        private Semaphore mSemaphore;
        private Bitmap mBitmap;
        private PixelDataConverter mConverter;
        public int mTimeout;
        //private int mCount;
        #endregion

        public BaslerDevice()
        {
            mExecuting = false;
            mIsInitializeComplete = false;
            mConverter = new PixelDataConverter();
            mSemaphore = new Semaphore(0, 1);
            mTimeout = 1000;
            //mCount = 0;
        }

        ~BaslerDevice()
        {
            if (mIsInitializeComplete)
                mCamera.Close();
        }

        public ImageCaptureDeviceRet initialCamera(string cameraSerialNumber)
        {
            if (mIsInitializeComplete)
            {
                throw new InvalidOperationException("Camera " + cameraSerialNumber + " is already initialized.");
            }
            
            try
            {
                mCamera = new Camera(cameraSerialNumber);
                mCamera.Open();
                mCamera.Parameters[PLCamera.TriggerSelector].SetValue(PLCamera.TriggerSelector.FrameStart);
                mCamera.Parameters[PLCamera.TriggerActivation].SetValue(PLCamera.TriggerActivation.FallingEdge);
                mCamera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.On);
                mCamera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Line1);
                //mCamera.Parameters[PLCamera.LineSelector].SetValue(PLCamera.LineSelector.Line1);
                //mCamera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(2.0);
                mCamera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);

                //mCamera.Parameters[PLCamera.ExposureAuto].SetValue(PLCamera.ExposureAuto.Continuous);
                mCamera.Parameters[PLCamera.ExposureTime].SetValue(100000);
                mCamera.Parameters[PLCamera.GainAuto].SetValue(PLCamera.GainAuto.Off);
                //mCamera.Parameters[PLCamera.GainSelector].SetValue(PLCamera.GainSelector.DigitalAll);
                mCamera.Parameters[PLCamera.Gain].SetValue(5);
                mCamera.Parameters[PLCamera.Width].SetValue(1280);
                mCamera.Parameters[PLCamera.Height].SetValue(1024);
                //mCamera.Parameters[PLCamera.OffsetX].SetValue(80);
                //mCamera.Parameters[PLCamera.OffsetY].SetValue(0);
                mCamera.Parameters[PLCamera.CenterX].SetValue(true);
                mCamera.Parameters[PLCamera.CenterY].SetValue(true);

                mCamera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                mIsInitializeComplete = true;
                return ImageCaptureDeviceRet.SUCCESS;
            }
            catch
            {
                mIsInitializeComplete = false;
                return ImageCaptureDeviceRet.ERROR_INVALID_RESOLUTION;
            }
        }

        public ImageCaptureDeviceRet startCapture()
        {
            if (!mIsInitializeComplete)
            {
                throw new InvalidOperationException("Camera is not initialized.");
            }

            if (!mCamera.StreamGrabber.IsGrabbing)
            {
                mCamera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }

            return ImageCaptureDeviceRet.SUCCESS;
        }

        public ImageCaptureDeviceRet captureImage(out Bitmap image)
        {
            if (!mIsInitializeComplete)
            {
                throw new InvalidOperationException("Camera is not initialized.");
            }

            if (mCamera.Parameters[PLCamera.TriggerSource].GetValue() != PLCamera.TriggerSource.Line1)
            {
                mCamera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Line1);
            }

            mExecuting = true;

            if (!mSemaphore.WaitOne(mTimeout))
            {
                //throw new TimeoutException("Capture timeout.");
                mExecuting = false;
                image = null;
                return ImageCaptureDeviceRet.ERROR_TIMEOUT;
            }

            while (mExecuting)
            {
                System.Threading.Thread.Sleep(1);
            }

            image = (Bitmap)mBitmap.Clone();
            mBitmap.Dispose();

            return ImageCaptureDeviceRet.SUCCESS;
        }

        public ImageCaptureDeviceRet captureSingleImage(out Bitmap image)
        {
            if (!mIsInitializeComplete)
            {
                throw new InvalidOperationException("Camera is not initialized.");
            }

            if (mCamera.Parameters[PLCamera.TriggerSource].GetValue() != PLCamera.TriggerSource.Software)
            {
                mCamera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Software);
            }

            mExecuting = true;
            mCamera.ExecuteSoftwareTrigger();

            if (!mSemaphore.WaitOne(mTimeout))
            {
                //throw new TimeoutException("Capture timeout.");
                mExecuting = false;
                image = null;
                return ImageCaptureDeviceRet.ERROR_TIMEOUT;
            }

            while (mExecuting)
            {
                System.Threading.Thread.Sleep(1);
            }

            image = (Bitmap)mBitmap.Clone();
            mBitmap.Dispose();

            return ImageCaptureDeviceRet.SUCCESS;
        }

        public ImageCaptureDeviceRet stopCapture()
        {
            if (!mIsInitializeComplete)
            {
                throw new InvalidOperationException("Camera is not initialized.");
            }

            mCamera.StreamGrabber.Stop();
            return ImageCaptureDeviceRet.SUCCESS;
        }

        public ImageCaptureDeviceRet closeCamera()
        {
            if (!mIsInitializeComplete)
            {
                throw new InvalidOperationException("Camera is not initialized.");
            }

            mCamera.Close();
            mIsInitializeComplete = false;
            return ImageCaptureDeviceRet.SUCCESS;
        }

        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            IGrabResult grabResult = e.GrabResult;

            mBitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
            BitmapData bmpData = mBitmap.LockBits(new Rectangle(0, 0, mBitmap.Width, mBitmap.Height), ImageLockMode.ReadWrite, mBitmap.PixelFormat);
            mConverter.OutputPixelFormat = PixelType.BGRA8packed;
            IntPtr ptrBmp = bmpData.Scan0;
            mConverter.Convert(ptrBmp, bmpData.Stride * mBitmap.Height, grabResult);
            mBitmap.UnlockBits(bmpData);

            mSemaphore.Release();
            mSemaphore = new Semaphore(0, 1);
            mExecuting = false;
        }
    }
}
