using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basler.Pylon;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.Timers;
using HalconDotNet;

namespace Detecting_System
{
    public class MyBasler
    {
        #region Basler相機參數
        public Camera camera;
        public HObject image = new HObject();
        private PixelDataConverter converter = new PixelDataConverter();
        private Stopwatch stopWatch = new Stopwatch();
        /// <summary>
        /// 相機列表
        /// </summary>
        private ListView AllCameras = new ListView();
        /// <summary>
        /// 相機IP
        /// </summary>
        public IPAddress ip = IPAddress.Parse("192.0.0.0");
        public bool IsConnected = false;
        //相機增益,曝光參數
        public double GainMaximum = new double();
        public double GainMinimum = new double();
        public double Gain = new double();
        public double ExposureTimeMaximum = new double();
        public double ExposureTimeMinimum = new double();
        public double ExposureTime = new double();
        /// <summary>
        /// 設置相機打開時初始拍攝模式 0=軟體觸發,1=硬體觸發
        /// </summary>
        public int TriggerMode = 0;

        /// <summary>
        /// 图像处理自定义委托
        /// </summary>
        /// <param name="hImage">halcon图像变量</param>
        public delegate void delegateProcessHImage(HObject hImage);
        /// <summary>
        /// 图像处理委托事件
        /// </summary>
        public event delegateProcessHImage eventProcessImage;

        private static object LockObject = new Object();

         // 定义数据检查Timer
        private System.Timers.Timer updateDeviceListTimer = new System.Timers.Timer();
 
         // 检查更新锁
         private static int CheckUpDateLock = 0;
 
         ///
         /// 设定数据检查Timer参数
         ///
         internal void GetTimerStart()
         {
             // 循环间隔时间(3秒鐘)
             updateDeviceListTimer.Interval =3000;
             // 允许Timer执行
             updateDeviceListTimer.Enabled = true;
             // 定义回调
             updateDeviceListTimer.Elapsed += new ElapsedEventHandler(CheckUpdatetimer_Elapsed);
             // 定义多次循环
             updateDeviceListTimer.AutoReset = true;
         }
 
         /// <summary>
         /// timer事件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void CheckUpdatetimer_Elapsed(object sender, ElapsedEventArgs e)
         {
            // 加锁检查更新锁
             lock (LockObject)
             {
                 if (CheckUpDateLock == 0) CheckUpDateLock = 1;
                 else return;
             }         
           
            //More code goes here.
           //具体实现功能的方法
             UpdateDeviceList();
              // 解锁更新检查锁
             lock (LockObject)
             {
                 CheckUpDateLock = 0;
             }            
         }

        // Updates the list of available camera devices.
        private void UpdateDeviceList()
        {
            try
            {
                // Ask the camera finder for a list of camera devices.
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();
                allCameras = CameraFinder.Enumerate();
                ListView.ListViewItemCollection items = AllCameras.Items;

                
                // Loop over all cameras found.
                foreach (ICameraInfo cameraInfo in allCameras)
                {
                    // Loop over all cameras in the list of cameras.
                    bool newitem = true;
                    foreach (ListViewItem item in items)
                    {
                        ICameraInfo tag = item.Tag as ICameraInfo;

                        // Is the camera found already in the list of cameras?
                        if (tag[CameraInfoKey.FullName] == cameraInfo[CameraInfoKey.FullName])
                        {
                            tag = cameraInfo;
                            newitem = false;
                            break;
                        }
                    }

                    // If the camera is not in the list, add it to the list.
                    if (newitem)
                    {
                        // Create the item to display.
                        ListViewItem item = new ListViewItem(cameraInfo[CameraInfoKey.FriendlyName]);

                        // Create the tool tip text.
                        string toolTipText = "";
                        foreach (KeyValuePair<string, string> kvp in cameraInfo)
                        {
                            toolTipText += kvp.Key + ": " + kvp.Value + "\n";
                        }
                        item.ToolTipText = toolTipText;
                        // Store the camera info in the displayed item.
                        item.Tag = cameraInfo;

                        // Attach the device data.
                        AllCameras.Items.Add(item);
                        if (toolTipText.Contains(ip.ToString()))
                        {    
                            // Create a new camera object.
                            if (camera != null)
                            {
                                DestroyCamera();
                            }
                            ICameraInfo selectedCamera = item.Tag as ICameraInfo;
                            camera = new Camera(selectedCamera);
                            // Destroy the old camera object.
                            OpenCameraAndSet();
                            IsConnected = true;
                        }
                           
                        
                    }
                }
                // Remove old camera devices that have been disconnected.
                foreach (ListViewItem item in items)
                {
                    bool exists = false;

                    // For each camera in the list, check whether it can be found by enumeration.
                    foreach (ICameraInfo cameraInfo in allCameras)
                    {
                        if (((ICameraInfo)item.Tag)[CameraInfoKey.FullName] == cameraInfo[CameraInfoKey.FullName])
                        {
                            exists = true;
                            break;
                        }
                    }
                    // If the camera has not been found, remove it from the list view.
                    if (!exists)
                    {
                        AllCameras.Items.Remove(item);
                    }
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
        //打開相機並獲得相機參數
        private void OpenCameraAndSet()
        {
            try
            {
                camera.CameraOpened += Configuration.AcquireContinuous;

                // Register for the events of the image provider needed for proper operation.
                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;

                // Open the connection to the c amera device.
                camera.Open();
                if (TriggerMode == 0)
                {
                    SetTriggerMode_Software();
                }
                else if (TriggerMode == 1)
                {
                    SetTriggerMode_Line1();
                }
                // Set the parameter for the controls.

                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    GainMaximum = camera.Parameters[PLCamera.GainAbs].GetMaximum();
                    GainMinimum = camera.Parameters[PLCamera.GainAbs].GetMinimum();
                    if (Gain == 0 || Gain < GainMinimum || Gain > GainMaximum)
                        Gain = camera.Parameters[PLCamera.GainAbs].GetValue();
                    else
                        SetGain(Gain);
                }
                else if (camera.Parameters.Contains(PLCamera.GainRaw))
                {
                    GainMaximum = camera.Parameters[PLCamera.GainRaw].GetMaximum();
                    GainMinimum = camera.Parameters[PLCamera.GainRaw].GetMinimum();
                    if (Gain == 0 || Gain < GainMinimum || Gain > GainMaximum)
                        Gain = camera.Parameters[PLCamera.GainRaw].GetValue();
                    else
                        SetGain(Gain);
                }
                else
                {
                    GainMaximum = camera.Parameters[PLCamera.Gain].GetMaximum();
                    GainMinimum = camera.Parameters[PLCamera.Gain].GetMinimum();
                    Gain = camera.Parameters[PLCamera.Gain].GetValue();
                }

                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    ExposureTimeMaximum = camera.Parameters[PLCamera.ExposureTimeAbs].GetMaximum();
                    ExposureTimeMinimum = camera.Parameters[PLCamera.ExposureTimeAbs].GetMinimum();
                    if (ExposureTime == 0 || ExposureTime < ExposureTimeMinimum || ExposureTime > ExposureTimeMaximum)
                        ExposureTime = camera.Parameters[PLCamera.ExposureTimeAbs].GetValue();
                    else
                        SetExposureTime(ExposureTime);
                }
                else
                {
                    ExposureTimeMaximum = camera.Parameters[PLCamera.ExposureTime].GetMaximum();
                    ExposureTimeMinimum = camera.Parameters[PLCamera.ExposureTime].GetMinimum();
                    if (ExposureTime == 0 || ExposureTime < ExposureTimeMinimum || ExposureTime > ExposureTimeMaximum)
                        ExposureTime = camera.Parameters[PLCamera.ExposureTime].GetValue();
                    else
                        SetExposureTime(ExposureTime);
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        // Shows exceptions in a message box.
        private void ShowException(Exception exception)
        {
            MessageBox.Show("Exception caught:\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Closes the camera object and handles exceptions.
        public void DestroyCamera()
        {
            // Destroy the camera object.
            try
            {
                //如果相機還連著,自動切換為軟體觸發
                if(IsConnected)
                    SetTriggerMode_Software();
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                    IsConnected = false;
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }

            // Destroy the converter object.
            if (converter != null)
            {
                converter.Dispose();
                converter = null;
            }
        }
        // Occurs when a device with an opened connection is removed.
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
            //    BeginInvoke(new EventHandler<EventArgs>(OnConnectionLost), sender, e);
            //    return;
            //}

            // Close the camera object.
            DestroyCamera();
            // Because one device is gone, the list needs to be updated.
            UpdateDeviceList();
        }

        // Occurs when the connection to a camera device is opened.
        private void OnCameraOpened(Object sender, EventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
            //    BeginInvoke(new EventHandler<EventArgs>(OnCameraOpened), sender, e);
            //    return;
            //}
        }

        // Occurs when the connection to a camera device is closed.
        private void OnCameraClosed(Object sender, EventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
            //    BeginInvoke(new EventHandler<EventArgs>(OnCameraClosed), sender, e);
            //    return;
            //}
        }

        // Occurs when a camera starts grabbing.
        private void OnGrabStarted(Object sender, EventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
            //    BeginInvoke(new EventHandler<EventArgs>(OnGrabStarted), sender, e);
            //    return;
            //}

            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed.

            stopWatch.Reset();

            // Do not update the device list while grabbing to reduce jitter. Jitter may occur because the GUI thread is blocked for a short time when enumerating.
            updateDeviceListTimer.Stop();
        }

        // Occurs when an image has been acquired and is ready to be processed.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
            //    // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
            //    BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
            //    return;
            //}

            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();

                        image = new HObject();
                        image = null;
                        if (IsMonoData(grabResult))
                        {
                            //如果是黑白图像，则利用GenImage1算子生成黑白图像
                            byte[] buffer = grabResult.PixelData as byte[];
                            IntPtr p = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                            //HOperatorSet
                            //HOperatorSet.GenImageConst(out image, "byte", grabResult.Width, grabResult.Height);
                            HOperatorSet.GenImage1(out image, "byte", grabResult.Width, grabResult.Height, p);
                         
                            //image_Basler[My.NowCamera].GenImage1("byte", grabResult.Width, grabResult.Height, p);
                            //HObject ExpTmpOutVar_0;
                            //HObject ExpTmpOutVar_0; 
                            //HOperatorSet.Rgb1ToGray(image, out ExpTmpOutVar_0);
                            //image.Dispose();
                            //image = ExpTmpOutVar_0;
                        }
                        else
                        {
                            if (grabResult.PixelTypeValue != PixelType.RGB8packed)
                            {
                                //如果图像不是RGB8格式，则将图像转换为RGB8，然后生成彩色图像
                                //因为GenImageInterleaved算子能够生成的图像的数据，常见的格式只有RGB8
                                //如果采集的图像是RGB8则不需转换，具体生成图像方法请自行测试编写。
                                byte[] buffer_rgb = new byte[grabResult.Width * grabResult.Height * 3];
                                Basler.Pylon.PixelDataConverter convert = new PixelDataConverter();
                                convert.OutputPixelFormat = PixelType.RGB8packed;
                                convert.Convert(buffer_rgb, grabResult);
                                IntPtr p = Marshal.UnsafeAddrOfPinnedArrayElement(buffer_rgb, 0);
                                HOperatorSet.GenImageInterleaved(out image, p, "rgb", grabResult.Width, grabResult.Height, 0, "byte", grabResult.Width, grabResult.Height, 0, 0, -1, 0);
                              
                            }
                        }
                        eventProcessImage(image);
                        image.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }

        //判断是否为黑白图像
        private Boolean IsMonoData(IGrabResult iGrabResult)//判断图像是否为黑白格式
        {
            switch (iGrabResult.PixelTypeValue)
            {
                case PixelType.Mono1packed:
                case PixelType.Mono2packed:
                case PixelType.Mono4packed:
                case PixelType.Mono8:
                case PixelType.Mono8signed:
                case PixelType.Mono10:
                case PixelType.Mono10p:
                case PixelType.Mono10packed:
                case PixelType.Mono12:
                case PixelType.Mono12p:
                case PixelType.Mono12packed:
                case PixelType.Mono16:
                    return true;
                default:
                    return false;
            }
        }

        // Occurs when a camera has stopped grabbing.
        private void OnGrabStopped(Object sender, GrabStopEventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
            //    BeginInvoke(new EventHandler<GrabStopEventArgs>(OnGrabStopped), sender, e);
            //    return;
            //}

            // Reset the stopwatch.
            stopWatch.Reset();

            // Re-enable the updating of the device list.
            updateDeviceListTimer.Start();

            // If the grabbed stop due to an error, display the error message.
            if (e.Reason != GrabStopReason.UserRequest)
            {
                MessageBox.Show("A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateDeviceListTimer_Tick(object sender, EventArgs e)
        {
            UpdateDeviceList();
        }

        //相机初始化
        public void CameraInit(int n)
        {
            //自由运行模式
            camera.CameraOpened += Configuration.AcquireContinuous;

            // 注册回调事件。
            camera.ConnectionLost += OnConnectionLost;//断开连接事件
            camera.CameraOpened += OnCameraOpened;
            camera.CameraClosed += OnCameraClosed;
            camera.StreamGrabber.GrabStarted += OnGrabStarted;//抓取开始事件  
            camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;//抓取图片事件
            camera.StreamGrabber.GrabStopped += OnGrabStopped;//结束抓取事件
            //打开相机
        }

        // Starts the grabbing of a single image and handles exceptions.
        public void OneShot()
        {
            try
            {
                // Starts the grabbing of one image.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }


        // Starts the continuous grabbing of images and handles exceptions.
        public void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        // Stops the grabbing of images and handles exceptions.
        public void Stop()
        {
            // Stop the grabbing.
            try
            {
                camera.StreamGrabber.Stop();
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
        public bool SetGain(double gain)
        {
            bool result = false;
            try
            {
                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    camera.Parameters[PLCamera.GainAbs].SetValue((long)gain);
                }
                else if (camera.Parameters.Contains(PLCamera.GainRaw))
                {
                    camera.Parameters[PLCamera.GainRaw].SetValue((long)gain);
                }
                else
                {
                    camera.Parameters[PLCamera.Gain].SetValue((long)gain);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool SetExposureTime(double exposureTime)
        {
            bool result = false;
            try
            {
                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(exposureTime);
                }
                else
                {
                    camera.Parameters[PLCamera.ExposureTime].SetValue(exposureTime);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 設置為IO觸發
        /// </summary>
        public void SetTriggerMode_Line1()
        {
            try
            {
                // Select the Frame Start trigger
                camera.Parameters[PLCamera.TriggerSelector].SetValue(PLCamera.TriggerSelector.FrameStart);
                // Enable triggered image acquisition for the Frame Start trigger
                camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.On);

                camera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Line1);

                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 設置為軟體觸發
        /// </summary>
        public void SetTriggerMode_Software()
        {
            // Select the Frame Start trigger
            camera.Parameters[PLCamera.TriggerSelector].SetValue(PLCamera.TriggerSelector.FrameStart);
            // Enable triggered image acquisition for the Frame Start trigger
            camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.Off);

        }

        #endregion
    }
}
