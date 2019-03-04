using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionCtrl;

namespace UI
{
    public partial class FrRun : Form
    {
       public  delegate bool CamControl();
      public  CamControl CamOpen;
      public CamControl CamClose;
    
        public bool bupdate
        {
            get { return timer_500ms.Enabled; }
            set { timer_500ms.Enabled = value; }
        }

        //#region
        //class ParameterHelper
        //{
        //    public int[] X_Cor
        //    {
        //        get;
        //        set;
        //    }
        //    public int[] Y_Cor
        //    {
        //        get;
        //        set;
        //    }
        //    public int[] U_Cor
        //    {
        //        get;
        //        set;
        //    }
        //    public int[] V_Cor
        //    {
        //        get;
        //        set;
        //    }
        //    //仿射变换模型六参数
        //    public double m1
        //    {
        //        get
        //        {
        //            return ((U_Cor[1] - U_Cor[0]) - m2 * (Y_Cor[1] - Y_Cor[0])) / (X_Cor[1] - X_Cor[0]);
        //        }
        //    }
        //    public double m2
        //    {
        //        get
        //        {
        //            return ((U_Cor[1] - U_Cor[0]) * (X_Cor[2] - X_Cor[0]) - (U_Cor[2] - U_Cor[0]) * (X_Cor[1] - X_Cor[0])) /
        //                ((Y_Cor[1] - Y_Cor[0]) * (X_Cor[2] - X_Cor[0]) - (Y_Cor[2] - Y_Cor[0]) * (X_Cor[1] - X_Cor[0]));
        //        }
        //    }
        //    public double m3
        //    {
        //        get
        //        {
        //            return ((V_Cor[1] - V_Cor[0]) - m4 * (Y_Cor[1] - Y_Cor[0])) / (X_Cor[1] - X_Cor[0]);
        //        }
        //    }
        //    public double m4
        //    {
        //        get
        //        {
        //            return ((V_Cor[1] - V_Cor[0]) * (X_Cor[2] - X_Cor[0]) - (V_Cor[2] - V_Cor[0]) * (X_Cor[1] - X_Cor[0])) /
        //                ((Y_Cor[1] - Y_Cor[0]) * (X_Cor[2] - X_Cor[0]) - (Y_Cor[2] - Y_Cor[0]) * (X_Cor[1] - X_Cor[0]));
        //        }
        //    }
        //    public double tx
        //    {
        //        get
        //        {
        //            return U_Cor[0] - m1 * X_Cor[0] - m2 * Y_Cor[0];
        //        }
        //    }
        //    public double ty
        //    {
        //        get
        //        {
        //            return V_Cor[0] - m3 * X_Cor[0] - m4 * Y_Cor[0];
        //        }
        //    }
        //}
        //#endregion

        public FrRun()
        {
            InitializeComponent();
           
        }

        private void cb_product_list_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
         bool cam_open()
        {
            short ret=0;
            try
            {
               ret= axGeneralVisionControl1.OpenCamera();
               if (ret!=0)
                   return false;
               else

                return true;
            }

            catch (Exception e)
            {
                VAR.ErrMsg(e.ToString());
                return false;
            }
        
             ;
        }
         bool cam_close()
        {

            try
            {
                   axGeneralVisionControl1.CloseVision();
                    return true;
            }

            catch(Exception e)
            {
                VAR.ErrMsg(e.ToString());
                return false;
            }
        }
        public void product_list_update()
        {
            int itm_id = 0;
            int j=0;
            cb_product_list.Items.Clear();

            foreach (string str in UI.COM.product.product_list)
            {
                cb_product_list.Items.Add(str);

                if (!str.Equals(VAR.gsys_set.cur_product_name))
                    itm_id++;
                else
                    cb_product_list.Text = str;
            }
            
        }
        public EM_RES cam_get(out ST_XYZ res,int cam_id)
        {
            short cam = (short)(cam_id);
            short re= 0;
            double x = 0;
            double y = 0;
            double z = 0;
            res.x = 0;
            res.y = 0;
            res.z = 0;
            try
            {

                re = axGeneralVisionControl1.TriggerCam(cam, 0,ref x, ref y, ref z);
                    if (re != 0)
                    {
                        VAR.ErrMsg("拍照失败");
                        return EM_RES.ERR;
                    }
                    res.x = x;
                    res.y = y;
                    res.z = z;
                return EM_RES.OK;
            }
            catch(Exception e)
            {
                VAR.ErrMsg(e.ToString());
                return EM_RES.ERR;
            }
        }
        private void FrRun_Load(object sender, EventArgs e)
        {
            //COM.traybox_fd.SetSta(TrayBox.EM_STA.UNTEST);
            //COM.traybox_ok.SetSta(TrayBox.EM_STA.FULL);
            //COM.traybox_ng.SetSta(TrayBox.EM_STA.FULL);

            //COM.traybox_fd.NewBox(Product.EM_CM_RES.UNTEST);
            //COM.traybox_ok.NewBox(Product.EM_CM_RES.NONE);
            //COM.traybox_ng.NewBox(Product.EM_CM_RES.NONE);

            //traybox_fd.box = COM.traybox_fd;
            //traybox_ok.box = COM.traybox_ok;
            //traybox_ng.box = COM.traybox_ng;
            bool ret;
            UI.COM.MVS.CamGet = new CAMERA(cam_get);
            VAR.sys_inf.Init(lb_war_inf, MT.OUT_Bee,VAR.gsys_set.beep_tmr);//lb_war_inf
            VAR.msg.StartUpdate(dvg_msg);
            COM.vs_msg.StartUpdate(dgv_vs);
            Thread.Sleep(2000);
            Application.DoEvents();
            product_list_update();
            CamOpen = new CamControl(cam_open);
            CamClose = new CamControl(cam_close);
            if (CamOpen != null)
            {
              ret=  CamOpen();
              if (!ret)
              {
                  VAR.ErrMsg("打开相机失败");
                  VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, "打开相机失败", 0);
              }
            }
            switch (cTabControl1.SelectedIndex)
            {                    
                case 1:
                    ax_pable.axis_x = MT.AxList_WS_FEED[0];
                    ax_pable.axis_y = MT.AxList_WS_FEED[1];
                    ax_pable.axis_z = MT.AxList_WS_FEED[2];
                    ax_pable.axis_a = MT.AxList_WS_FEED[3];
                    ax_pable.update_show();
                    break;

                case 0:

                    ax_pable.axis_x = MT.AxList_WS_GET[0];
                    ax_pable.axis_y = MT.AxList_WS_GET[1];
                    ax_pable.axis_z = MT.AxList_WS_GET[2];
                    ax_pable.axis_a = MT.AxList_WS_GET[3];
                    ax_pable.update_show();
                    break;
                default: break;
            }

        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            //int r = ((int)(COM.product.TrayBeforTestA.list_cam.ElementAt(0).list_res.ElementAt(0).res) + 1);
            //if (r > 5) r = 0;
            //COM.product.TrayBeforTestA.list_cam.ElementAt(0).list_res.ElementAt(0).res = (Product.EM_CM_RES)r;
            //tray1.tray_dat = COM.product.TrayBeforTestA;
            //tray1.Refresh();
            Action.th_run();

        }


        private void btn_stop_Click(object sender, EventArgs e)
        {
            Action.stop();
          
        }

        delegate void UpdateCallback();
        public void UpdateDisplay()
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            int n = 0;
            while (!IsHandleCreated)
            {
                //解决窗体关闭时出现“访问已释放句柄“的异常
                if (Disposing || IsDisposed)
                    return;
                Application.DoEvents();
                Thread.Sleep(1);
                if (n++ > 100) return;
            }
            if (InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {

                UpdateCallback d = new UpdateCallback(UpdateDisplay);
                BeginInvoke(d, new object[] {});
            }
            else
            {

                product_list_update();

                //traybox_fd.UpdateShow();
                //traybox_ok.UpdateShow();
                //traybox_ng.UpdateShow();

                //tray_fd.tray_dat = traybox_fd.box.tray_cur;
                //tray_ok.tray_dat = traybox_ok.box.tray_cur;
                //tray_ng.tray_dat = traybox_ng.box.tray_cur;

                //tray_fd.UpdateShow();                
                //tray_ok.UpdateShow();
                //tray_ng.UpdateShow();
               // ws1.UpdateShow();
              //  ws2.UpdateShow();

              //ws5.UpdateShow();

               // Turnplate.UpdateShow();
                if (UI.Action.bNullRun)
                    checkBox_null_run.Checked = true;
            }
        }
        private void timer_500ms_Tick(object sender, EventArgs e)
        {
            Task show = new Task(() =>
                {
                    UpdateDisplay();
                }
            );
            show.Start();
        }

        private void tray_ok_Load(object sender, EventArgs e)
        {

        }

        private void dvg_msg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
          
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
        
        }



        private void button1_Click_2(object sender, EventArgs e)
        {
            CamOpen();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EM_RES ret = EM_RES.OK;
            ST_XYZ res=new ST_XYZ();
            int cam_id = 2;
            ret = UI.COM.MVS.get_vs(out res, cam_id);
            if (ret != EM_RES.OK)
            {
                MessageBox.Show("拍照失败");
                return;
            }
            MessageBox.Show(res.x.ToString() + "\n" + res.y.ToString() + "\n" + res.z.ToString());
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            EM_RES ret = EM_RES.OK;
            ST_XYZ res=new ST_XYZ();
            int cam_id=1;
            ret = UI.COM.MVS.get_vs(out res, cam_id);
            if(ret!=EM_RES.OK)
            {
                MessageBox.Show("拍照失败");
                return;
            }
            MessageBox.Show(res.x.ToString() + "\n" + res.y.ToString() + "\n" + res.z.ToString());
        }

        private void bt_mark_cam1_Click(object sender, EventArgs e)
        {
          int cam_id = 1;
          EM_RES ret=  UI.COM.MVS.cam_get_scle(ref VAR.gsys_set.bquit, cam_id);
          if (ret == EM_RES.OK)
              MessageBox.Show("获取比例成功");
          else
              MessageBox.Show("获取比例失败");
        }

        private void tableLayoutPanel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void bt_mark_cam2_Click(object sender, EventArgs e)
        {
            int cam_id =2;
            EM_RES ret = UI.COM.MVS.cam_get_scle(ref VAR.gsys_set.bquit, cam_id);
            if (ret == EM_RES.OK)
                MessageBox.Show("获取比例成功");
            else
                MessageBox.Show("获取比例失败");
        }

        private void bt_close_cam_Click(object sender, EventArgs e)
        {
            
            CamClose();
        }

        private void cTabControl1_TabIndexChanged(object sender, EventArgs e)
        {
                   ax_pable.clear();

                   switch (((CTabControl)sender).SelectedIndex)
                   {
                       case 1:
                       
                           ax_pable.axis_x = MT.AxList_WS_FEED[0];
                           ax_pable.axis_y = MT.AxList_WS_FEED[1];
                           ax_pable.axis_z = MT.AxList_WS_FEED[2];
                           ax_pable.axis_a = MT.AxList_WS_FEED[3];
                           ax_pable.update_show();
                           break;

                       case 0:
                       
                           ax_pable.axis_x = MT.AxList_WS_GET[0];
                           ax_pable.axis_y = MT.AxList_WS_GET[1];
                           ax_pable.axis_z = MT.AxList_WS_GET[2];
                           ax_pable.axis_a = MT.AxList_WS_GET[3];
                           ax_pable.update_show();
                           break;
                       default: break;
                   }
        }

        private void cTabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ax_pable.clear();

            switch (((CTabControl)sender).SelectedIndex)
            {
                case 1:

                    ax_pable.axis_x = MT.AxList_WS_FEED[0];
                    ax_pable.axis_y = MT.AxList_WS_FEED[1];
                    ax_pable.axis_z = MT.AxList_WS_FEED[2];
                    ax_pable.axis_a = MT.AxList_WS_FEED[3];
                    ax_pable.update_show();
                    break;

                case 0:

                    ax_pable.axis_x = MT.AxList_WS_GET[0];
                    ax_pable.axis_y = MT.AxList_WS_GET[1];
                    ax_pable.axis_z = MT.AxList_WS_GET[2];
                    ax_pable.axis_a = MT.AxList_WS_GET[3];
                    ax_pable.update_show();
                    break;
                default: break;
            }
        }

        private void axGeneralVisionControl1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox_null_run_CheckedChanged(object sender, EventArgs e)
        {
            UI.Action.bNullRun = !UI.Action.bNullRun;
        }



     

 
    }
}
