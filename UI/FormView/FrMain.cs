using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionCtrl;
using System.IO;
using System.Threading;

namespace UI
{
    public partial class FrMain : Form
    {
        public static FrSys frsys = new FrSys();
        public static FrRun frrun = new FrRun();
        public static FrProduct frproduct = new FrProduct();
        public static FrUser frsuser = new FrUser();
        public static FrRst frrst = new FrRst();
        public static FrMain frmain = null;

        //KeyboardHook k_hook = new KeyboardHook();

        public FrMain()
        {
            InitializeComponent();
            frmain = this;
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //if (Control.IsKeyLocked(Keys.NumLock) == true) return;
            ////check mouse pos
            //if (!this.GetTopLevel()) return;
            //if (Control.MousePosition.X < 700 || Control.MousePosition.Y > 70) return;

            //if ((VAR.gsys_set.status != CONST.SYS_STATUS_STANDBY) && (VAR.gsys_set.status != CONST.SYS_STATUS_PAUSE)) return;

            //VAR.gsys_set.bquit = false;
            ////左右按键
            //if (e.KeyData == Keys.Left)
            //{
            //    MT.AXIS_X.JOG_VMove(ref VAR.gsys_set.bquit, AXIS.DIR_N);
            //    e.Handled = true;
            //}
            //else if (e.KeyData == Keys.Right)
            //{
            //    MT.AXIS_X.JOG_VMove(ref VAR.gsys_set.bquit, AXIS.DIR_P);
            //    e.Handled = true;
            //}

            ////上下按键
            //if (e.KeyData == Keys.Up)
            //{
            //    MT.AXIS_Y.JOG_VMove(ref VAR.gsys_set.bquit, AXIS.DIR_N);
            //    e.Handled = true;
            //}
            //else if (e.KeyData == Keys.Down)
            //{
            //    MT.AXIS_Y.JOG_VMove(ref VAR.gsys_set.bquit, AXIS.DIR_P);
            //    e.Handled = true;
            //}

            ////上页下页按键
            //if (e.KeyData == Keys.PageUp)
            //{
            //    MT.AXIS_R.JOG_VMove(ref VAR.gsys_set.bquit, AXIS.DIR_N);
            //    e.Handled = true;
            //}
            //else if (e.KeyData == Keys.PageDown)
            //{
            //    MT.AXIS_R.JOG_VMove(ref VAR.gsys_set.bquit, AXIS.DIR_P);
            //    e.Handled = true;
            //}
            ////手动速度切换
            //if (e.KeyData == Keys.End)
            //{
            //    if (MT.AXIS_X.spd_cur == MT.AXIS_X.spd_manual_high)
            //    {
            //        foreach (AXIS ax in MT.AxisList) ax.SetToManualLowSpd();
            //    }
            //    else
            //    {
            //        foreach (AXIS ax in MT.AxisList) ax.SetToManualHighSpd();
            //    }
            //    e.Handled = true;
            //}
        }

        private void hook_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Control.IsKeyLocked(Keys.NumLock) == true) return;
            if (VAR.gsys_set.status != EM_SYS_STA.STANDBY && VAR.gsys_set.status != EM_SYS_STA.PAUSE) return;

            if (e.KeyData == Keys.Add)
            {
                if (tsc_step_sel.SelectedIndex < (tsc_step_sel.Items.Count - 1)) tsc_step_sel.SelectedIndex++;
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Subtract)
            {
                if (tsc_step_sel.SelectedIndex > 0) tsc_step_sel.SelectedIndex--;
                e.Handled = true;
            }

            if ((e.KeyData == Keys.Left) || (e.KeyData == Keys.Right) || (e.KeyData == Keys.Up) || (e.KeyData == Keys.Down) || (e.KeyData == Keys.PageUp) || (e.KeyData == Keys.PageDown))
            {
                //MT.AXIS_X.JOG_Stop();
                //MT.AXIS_Y.JOG_Stop();
                //MT.AXIS_R.JOG_Stop();
                e.Handled = true;
            }
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private Form FindForm(string formName)
        {
            foreach (Form form in Application.OpenForms)//获得所有打开的窗体
            {
                if (form.Name == formName)
                {
                    return form;
                }
            }
            return null;
        }
        private void rbtn_sys_CheckedChanged(object sender, EventArgs e)
        {
            //Form form = new FrSys();
            //pnl_sub.Controls.Clear();
            //form.TopLevel = false;
            //form.FormBorderStyle = FormBorderStyle.None;
            //pnl_sub.Controls.Add(form);
            //form.Left = 0;
            //form.Top = 0;
            //form.Show();
        }
        public void form_sel(string btn_name, string page_name="", string page_name2 = "")
        {
            //    if (frrun != null) frrun.timer_update.Enabled = false;
            //    if (frsys != null) frsys.timer_update.Enabled = false;

            Font ft = new Font("Microsoft Sans Serif", 18, FontStyle.Bold);
            rbtn_run.Font = ft;
            rbtn_product.Font = ft;
            rbtn_sys.Font = ft;

            rbtn_run.ForeColor = Color.DarkGray;
            rbtn_product.ForeColor = Color.DarkGray;
            rbtn_sys.ForeColor = Color.DarkGray;

            rbtn_run.BackColor = Color.Transparent;
            rbtn_product.BackColor = Color.Transparent;
            rbtn_sys.BackColor = Color.Transparent;

            Form form = null;
            ft = new Font("Microsoft Sans Serif", 22, FontStyle.Bold);
            frsys.bupdate = false;
            frrst.bupdate = false;
            frrun.bupdate = false;
            switch (btn_name)
            {
                default:
                case "rbtn_run":
                    rbtn_run.Checked = true;
                    rbtn_run.ForeColor = Color.WhiteSmoke;
                    rbtn_run.Font = ft;
                    if (frrun == null) frrun = new FrRun();
                    form = frrun;
                    frrun.bupdate = true;
                    break;
                case "rbtn_product":
                    rbtn_product.Checked = true;
                    rbtn_product.ForeColor = Color.WhiteSmoke;
                    rbtn_product.Font = ft;
                    if (frproduct == null) frproduct = new FrProduct();
                    form = frproduct;

                    ////page select
                    //if (frproduct.ctb_prodcut.TabPages[page_name] != null) frproduct.ctb_prodcut.TabPages[page_name].Select();
                    //if (page_name == "tb_tg_cfg")
                    //{
                    //    //VisionRun.Display = new VisionDisplay(frproduct.cogRecordDisplay_live, "");
                    //    if (frproduct.ctb_tg_view.TabPages[page_name2] != null) frproduct.ctb_tg_view.TabPages[page_name2].Select();
                    //}
                    //else if (page_name == "tb_tg_vs")
                    //{
                    //    //VisionRun.Display = new VisionDisplay(frproduct.DisPlayAndImageMask1.CogRecordDisplay, "");
                    //    if (frproduct.ctb_vs_cfg.TabPages[page_name2] != null) frproduct.ctb_vs_cfg.TabPages[page_name2].Select();
                    //}
                    //else if (page_name == "tb_ofs")
                    //{
                    //    //VisionRun.Display = new VisionDisplay(frproduct.cogRecordDisplay_ofs, "");
                    //    if (frproduct.ctb_ofs.TabPages[page_name2] != null) frproduct.ctb_ofs.TabPages[page_name2].Select();
                    //}
                    break;
                case "rbtn_sys":
                    rbtn_sys.Checked = true;
                    rbtn_sys.ForeColor = Color.WhiteSmoke;
                    rbtn_sys.Font = ft;
                    if (frsys == null) frsys = new FrSys();
                    form = frsys;
                    frsys.bupdate = true;
                    ////page select
                    //if (frsys.ctb_sys.TabPages[page_name] != null) frsys.ctb_sys.TabPages[page_name].Select();
                    //if (page_name == "tb_cali")
                    //{
                    //    if (frsys.ctb_cali.TabPages[page_name2] != null) frsys.ctb_cali.TabPages[page_name2].Select();
                    //}
                    //frsys.timer_update.Enabled = true;
                    //if (VisionRun.Display.m_strName != "frsysCogRecordDisplay")
                    //    VisionRun.Display = new VisionDisplay(frsys.CogRecordDisplay, "frsysCogRecordDisplay");
                    break;
                case "rbtn_user":
                    rbtn_user.Checked = true;
                    rbtn_user.ForeColor = Color.WhiteSmoke;
                    rbtn_user.Font = ft;
                    if (frsuser == null) frsuser = new FrUser();
                    form = frsuser;
                    break;

                case "rbtn_rst":
                    rbtn_rst.Checked = true;
                    rbtn_rst.ForeColor = Color.WhiteSmoke;
                    rbtn_rst.Font = ft;
                    if (frrst == null) frrst = new FrRst();
                    form = frrst;
                    frrst.bupdate = true;
                    break;
            }

            if (form == null) return;
            pnl_sub.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            pnl_sub.Controls.Add(form);
            form.Left = 0;
            form.Top = 0;
            form.Width = pnl_sub.Width;
            form.Height = pnl_sub.Height - 8;
            form.Show();
        }

        private void FrMain_Load(object sender, EventArgs e)
        {
            EM_RES ret;

            VAR.msg.ShowMsgCfg(1000, (Msg.EM_MSGTYPE)0xffff);
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "系统启动...");
            //load sys config

            VAR.gsys_set.LoadSysCfg();
          
            VAR.gsys_set.UpdatePro += new Update_product(UI.Action.Update_pro);
            VAR.gsys_set.UpdatePro += new Update_product(UI.COM.MVS.LoadInf);

            VAR.gsys_set.status = EM_SYS_STA.UNKOWN;
            VAR.gsys_set.bclose = false;

            VAR.sys_inf.Set(EM_ALM_STA.WAR_YELLOW_FLASH, "正在加载", 100, true);

            Task GetData = new Task(() =>
            {
                ret = MT.PosInit();
                if (ret != EM_RES.OK)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "位置数据加载失败!");

                }
                else VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "位置数据加载成功!");

                COM.product.TrayGet.PosList = MT.pos_tray_get;
                COM.product.TrayBackOK.PosList = MT.pos_tray_bk_ok;
                COM.product.TrayBackAANG.PosList = MT.pos_tray_bk_AANG;
                COM.product.TrayBackPPNG.PosList = MT.pos_tray_bk_ng;
                //加载产品
                COM.product.LoadProductList();

                ret = COM.product.LoadDat(VAR.gsys_set.cur_product_name);
                if (ret != EM_RES.OK) VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "产品数据加载失败!");
                else VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "产品数据加载成功!");
            }
            );
            GetData.Start();
           

            //硬件初始化
            Task connect = new Task(() =>
            {
                ret = MT.card_Init(Path.GetFullPath("..") + "\\syscfg\\");
                if (ret != EM_RES.OK) VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "板卡始化失败!");
                else VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "板卡始化成功!");               
            }
            );
            connect.Start();

            //create form运行窗口设计
            if (frrun == null) frrun = new FrRun();
            pnl_sub.Controls.Clear();
            frrun.TopLevel = false;
            frrun.FormBorderStyle = FormBorderStyle.None;
            frrun.Show();
            pnl_sub.Controls.Add(frrun);
        
            Application.DoEvents();
            Thread.Sleep(10);

            for (int n = 0; n < 3000; n++)
            {
                if (connect.IsCompleted) break;
                Thread.Sleep(10);
                Application.DoEvents();
            }
            timer_reconnect.Enabled = true;
            if (frrun != null) frrun.bupdate = true;
           
            MT.AXIS_bullet_move.ChkSafeSen = new CHK_AXIS_SAFE(WsTrayMove.ck_ax_safe);

            MT.GPIO_OUT_roll_plate.ChkSafe = new CHK_GPIO_SAFE(WSROLL.ck_roll_safe); 
            MT.AXIS_FEED_X.ChkSafePos = new CHK_AXIS_SAFE(WSFeed.ck_ax_safe);
            MT.AXIS_FEED_X.ChkSafeSen = new CHK_AXIS_SAFE(WSFeed.ck_ax_safe);
            MT.AXIS_FEED_Y.ChkSafePos = new CHK_AXIS_SAFE(WSFeed.ck_ax_safe);
            MT.AXIS_FEED_Y.ChkSafeSen = new CHK_AXIS_SAFE(WSFeed.ck_ax_safe);

            MT.AXIS_GET_X.ChkSafePos = new CHK_AXIS_SAFE(WSGet.ck_ax_safe);
            MT.AXIS_GET_Y.ChkSafePos = new CHK_AXIS_SAFE(WSGet.ck_ax_safe);

            MT.AXIS_BACK_X.ChkSafePos = new CHK_AXIS_SAFE(WSBack.ck_ax_safe);
            MT.AXIS_BACK_Y.ChkSafePos = new CHK_AXIS_SAFE(WSBack.ck_ax_safe);

            MT.AXIS_bullet_feed.ChkSafePos = new CHK_AXIS_SAFE(WsBuFD.axZ_Safe);
            MT.AXIS_bullet_feed.ChkSafeSen = new CHK_AXIS_SAFE(WsBuFD.axZ_Safe);
            MT.AXIS_bullet_back.ChkSafePos = new CHK_AXIS_SAFE(WsBuBK.axZ_Safe);
            MT.AXIS_bullet_back.ChkSafeSen = new CHK_AXIS_SAFE(WsBuBK.axZ_Safe);

            foreach (POS mpos in MT.pos_list_get)
            {
                mpos.UpdatePos();
            }
            foreach (POS mpos in MT.pos_list_bull_move)
            {
                mpos.UpdatePos();
            }
            foreach (POS mpos in MT.pos_list_feed)
            {
                mpos.UpdatePos();
            }
            foreach (POS mpos in MT.pos_list_back)
            {
                mpos.UpdatePos();
            }

         //   VAR.sys_inf.Set(EM_ALM_STA.WAR_YELLOW_FLASH, "待回零", 100, true);

            if (MT.bCardInit)
            {
                ////打开刹车
                MT.GPIO_OUT_brake_back_z.SetOn();
                MT.GPIO_OUT_brake_feed_z.SetOn();
                MT.GPIO_OUT_light.SetOn();               
            }
       

            
        }
        private void rbtn_run_Click(object sender, EventArgs e)
        {
            
            switch (((RadioButton)sender).Name)
            {
                case "rbtn_run":
                    form_sel(((RadioButton)sender).Name);
                    break;
                case "rbtn_product":
                    switch (frproduct.ctb_product.SelectedTab.Name)
                    {
                        case "tb_tg_cfg":                            
                            form_sel(((RadioButton)sender).Name, frproduct.ctb_product.SelectedTab.Name);
                            break;
                        case "tb_tg_vs":
                            form_sel(((RadioButton)sender).Name, frproduct.ctb_product.SelectedTab.Name);
                            break;
                        case "tb_ofs":
                            form_sel(((RadioButton)sender).Name, frproduct.ctb_product.SelectedTab.Name);
                            break;
                        default:
                            form_sel(((RadioButton)sender).Name, frproduct.ctb_product.SelectedTab.Name);
                            break;
                    }
                    break;
                case "rbtn_sys":
                    form_sel(((RadioButton)sender).Name, frsys.ctb_sys.SelectedTab.Name);
                    break;
                default:
                    form_sel(((RadioButton)sender).Name);
                    break;
            }

        }


        private void timer_update_Tick(object sender, EventArgs e)
        {
            lb_Date.Text = System.DateTime.Today.ToString("yyyy-MM-dd");
            lb_Time.Text = System.DateTime.Now.ToString("HH:mm:ss");
            //轴卡异常
            //if (!MT.bCardInit )
            //{
            //    VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, "板卡初始化失败");
            //    VAR.gsys_set.bquit = true;
            //    timer_key.Enabled = false;
            //    return;
            //}
            //timer_key.Enabled = true;
           

            //if (MT.GPIO_IN_EMG.isOFF)
            //{
            //    if (VAR.gsys_set.status != EM_SYS_STA.EMG)
            //    {
            //        VAR.gsys_set.status = EM_SYS_STA.EMG;
            //        VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, "EMG");
            //    }
            //}
           // else
            {
                foreach (AXIS ax in MT.AxList_ALL)
                {
                    if (!ax.isSVRON)
                    {
                        if (VAR.gsys_set.status != EM_SYS_STA.ERR)
                        {
                            VAR.gsys_set.status = EM_SYS_STA.ERR;
                            VAR.gsys_set.bquit = true;
                            VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, ax.disc + "未使能");

                        }
                        
                    }
                    else if (ax.isALM)
                    {
                        if (VAR.gsys_set.status != EM_SYS_STA.ERR)
                        {
                            VAR.gsys_set.status = EM_SYS_STA.ERR;
                            VAR.gsys_set.bquit = true;
                            VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, ax.disc + "报警", 0);
                        }
                    }
                    else if(ax.isEMG)
                    {
                        if (VAR.gsys_set.status != EM_SYS_STA.EMG)
                        {
                            VAR.gsys_set.status = EM_SYS_STA.EMG;
                            VAR.gsys_set.bquit = true;
                            VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, ax.disc + "急停,请复位", 0);
                        }
                    }
                    else if(ax.home_status!=AXIS.HOME_STA.OK)
                    {
                        if (VAR.gsys_set.status != EM_SYS_STA.ERR)
                        {
                            VAR.gsys_set.status = EM_SYS_STA.ERR;
                            VAR.gsys_set.bquit = true;
                            VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, ax.disc + "未复位！", 0);
                        }
                    }
                }
            }
        }

        private void timer_key_Tick(object sender, EventArgs e)
        {
              timer_key.Interval = 100;
             if (VAR.gsys_set.status == EM_SYS_STA.PAUSE)
                    COUNT_DATA.ct_pause += timer_key.Interval;
               //    //在待机或暂停状态，安全光栅触发时，停止移动。
               // if ((VAR.gsys_set.status == EM_SYS_STA.STANDBY || VAR.gsys_set.status == EM_SYS_STA.PAUSE) && MT.ChkSafeSen() != EM_RES.OK)
               //{
               //     MT.AllAxStop();       
               //     VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH,  "不安全", 0);
               //     return;
               // }
                 //if (MT.GPIO_IN_emg_key1.isOFF)
                 //   {
                 //       VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, "急停按下", -1);
                 //       VAR.gsys_set.status = EM_SYS_STA.EMG;
                 //       VAR.gsys_set.bquit = true;
                 //       return;
                 //   }              
                //else             
                // if (MT.GPIO_IN_emg_key1.isON )
                //  {
                        
                //            VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, "急停");
                //              if(!VAR.gsys_set.bquit)
                //            VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "急停按键按下");
                //            VAR.gsys_set.status = EM_SYS_STA.EMG;
                //            VAR.gsys_set.bquit = true;
                //          //  MT.GPIO_OUT_WL.SetOff();
                //            return;                     
                //   }
                   
           
            //    //按键处理
            //    if (MT.isReady == false) return;
            //    //任意消掉报警声
            //    if (VAR.gsys_set.status == CONST.SYS_STATUS_UNKOWN || VAR.gsys_set.status == CONST.SYS_STATUS_CALI)
            //    {
            //        if (MT.GPIO_IN_key_start.isON || MT.GPIO_IN_KEY_STOP.isON)
            //        {
            //            MT.GPIO_OUT_ALM_BEEP.SetOff();
            //            return;
            //        }
            //    }
            //系统异常返回
                if (VAR.gsys_set.status == EM_SYS_STA.ERR || VAR.gsys_set.status == EM_SYS_STA.WARNING || VAR.gsys_set.status == EM_SYS_STA.EMG)
                {
                    VAR.gsys_set.bquit = true;
                }

                //按键处理
                //if (VAR.gsys_set.status != EM_SYS_STA.UNKOWN && VAR.gsys_set.status != EM_SYS_STA.ERR && VAR.gsys_set.status != EM_SYS_STA.EMG)
                //{
                //    if (MT.GPIO_IN_key_start.isON)
                //    {
                //        Thread.Sleep(10);
                //        if (MT.GPIO_IN_key_start.isON)
                //        {
                //          VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "开始按键按下");                                                                   
                //            foreach (AXIS ax in MT.AxList_ALL)
                //            {
                //                if (ax.isEMG || ax.isALM) return;
                //            }

                //          //  chk safe
                //            if (MT.ChkSafeSen() != EM_RES.OK) return;

                //         //   run
                //          //  VAR.gsys_set.user_idx = 0;//访问权限
                //            VAR.gsys_set.bquit = false;

                //            Action.th_run();
                //        }
                //    }
                //}
                //else
                //{
                //   //// 任意消掉报警声
                //   // if (MT.GPIO_IN_key_start.isON || MT.GPIO_IN_KEY_STOP.isON)
                //   // {
                //   //    // MT.GPIO_OUT_ALM_BEEP.SetOff();
                //   //     return;
                //   // }
                //}


              //  任意消掉报警声
                //if (MT.GPIO_IN_KEY_STOP.isON || MT.GPIO_IN_key_start.isON)
                //{
                  //  MT.GPIO_OUT_ALM_BEEP.SetOff();

                    //if (VAR.gsys_set.status != CONST.SYS_STATUS_UNKOWN && VAR.gsys_set.status != CONST.SYS_STATUS_EMG && VAR.gsys_set.status != CONST.SYS_STATUS_ERR)
                    //{
                    //    if (COM.MounterGetRunStatus() == CONST.SYS_STATUS_RUN)
                    //    {
                    //        VAR.gsys_set.status = CONST.SYS_STATUS_RUN;
                    //        Action.VAR.sys_inf.Set(CONST.EM_ALM_STA.NOR_BLUE, "运行", -1, true);
                    //    }
                    //    else if (COM.MounterGetRunStatus() == CONST.SYS_STATUS_PAUSE)
                    //    {
                    //        VAR.gsys_set.status = CONST.SYS_STATUS_PAUSE;
                    //        Action.VAR.sys_inf.Set(CONST.EM_ALM_STA.NOR_GREEN, "暂停", -1, true);
                    //    }
                    //    else if (COM.MounterGetRunStatus() == CONST.SYS_STATUS_STANDBY)
                    //    {
                    //        VAR.gsys_set.status = CONST.SYS_STATUS_STANDBY;
                    //        Action.VAR.sys_inf.Set(CONST.EM_ALM_STA.NOR_GREEN, "就绪", -1, true);
                    //    }
                    //}
                //}
        }

        private void lb_Date_Click(object sender, EventArgs e)
        {
            //if (VAR.gsys_set.status == CONST.SYS_STATUS_STANDBY)
            {
                ts_manual.Top = pnl_sub.Top;
                ts_manual.Left = tbl_main.Width - ts_manual.Width;
                ts_manual.Visible = !ts_manual.Visible;
                if (tsc_step_sel.SelectedIndex < 0 || tsc_step_sel.SelectedIndex >= tsc_step_sel.Items.Count) tsc_step_sel.SelectedIndex = 1;
            }
        }
        private void StrToAxis(String pch, ref AXIS axis, ref AXIS.AX_DIR dir)
        {
            //if (true == pch.Equals("X向左"))
            //{
            //    axis = MT.AXIS_X;
            //    dir = AXIS.DIR_N;
            //}
            //else if (true == pch.Equals("X向右"))
            //{
            //    axis = MT.AXIS_X;
            //    dir = AXIS.DIR_P;
            //}
            //else if (true == pch.Equals("Y向前"))
            //{
            //    axis = MT.AXIS_Y;
            //    dir = AXIS.DIR_P;
            //}
            //else if (true == pch.Equals("Y向后"))
            //{
            //    axis = MT.AXIS_Y;
            //    dir = AXIS.DIR_N;
            //}
            //else if (true == pch.Equals("Z向上"))
            //{
            //    axis = MT.AXIS_Z;
            //    dir = AXIS.DIR_N;
            //}
            //else if (true == pch.Equals("Z向下"))
            //{
            //    axis = MT.AXIS_Z;
            //    dir = AXIS.DIR_P;
            //}
            //else if (true == pch.Equals("R顺时针"))
            //{
            //    axis = MT.AXIS_R;
            //    dir = AXIS.DIR_P;
            //}
            //else if (true == pch.Equals("R逆时针"))
            //{
            //    axis = MT.AXIS_R;
            //    dir = AXIS.DIR_N;
            //}            
            //else
            //{
            //    axis = null;
            //    dir = AXIS.DIR_P;
            //}           

        }
        private void tsb_xn_MouseDown(object sender, MouseEventArgs e)
        {
            //ToolStripButton btn = (ToolStripButton)sender;
            //AXIS ax = null;
            //int dir = AXIS.DIR_P;
            //StrToAxis(btn.Text, ref ax, ref dir);
            //if (ax == null) return;
            ////z限制1mm步进
            //if ((ax.id == MT.AXIS_Z.id ) && ax.manual_step > 1) ax.manual_step = 1;
            //ax.JOG_Step(ref VAR.gsys_set.bquit, dir, ax.manual_step);
            //VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, btn.Text + " down," + ax.disc);
            //Thread.Sleep(500);
        }

        private void tsc_step_sel_Click(object sender, EventArgs e)
        {

        }

        private void tsb_xn_MouseUp(object sender, MouseEventArgs e)
        {
            ToolStripButton btn = (ToolStripButton)sender;
            AXIS ax = null;
            AXIS.AX_DIR dir = AXIS.AX_DIR.P;
            StrToAxis(btn.Text, ref ax, ref dir);
            if (ax == null) return;
            ax.JOG_Stop();
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, btn.Text + " up," + ax.disc);
        }

        private void tsb_stop_Click(object sender, EventArgs e)
        {
            VAR.gsys_set.bquit = true;
            MT.AllAxStop();
        }

        private void FrMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VAR.gsys_set.status == EM_SYS_STA.RUN)
            {
                if (MessageBox.Show("运行中，是否要停止?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    for (int n = 0; n < 100; n++)
                    {
                        VAR.gsys_set.bquit = true;
                        VAR.gsys_set.bpause = false;
                        //VAR.gsys_set.bclose = true;
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }
                    VAR.gsys_set.status = EM_SYS_STA.STANDBY;
                    return;
                }
            }
            if (MessageBox.Show("是确定要关闭软件?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                //k_hook.KeyDownEvent -= hook_KeyDown;//钩住键按下
                //k_hook.KeyUpEvent -= hook_KeyUp;//钩住键按下
                //k_hook.Stop();//安装键盘钩子
                //Acquistion.AllCameraDisconnect();
            }
            else e.Cancel = true;
        }

        private void tsc_step_sel_SelectedIndexChanged(object sender, EventArgs e)
        {
            double[] step_array = { 0.01, 0.1, 1, 10 };
            double step = 0.1;
            try
            {
                step = step_array[tsc_step_sel.SelectedIndex];
            }
            catch
            {
                MessageBox.Show("步距数据出错!");
                step = 0.1;
            }
            //foreach (AXIS ax in MT.AxisListExceptFd) ax.manual_step = step;
        }

        private void timer_reconnect_Tick(object sender, EventArgs e)
        {
            try
            {
                ((System.Windows.Forms.Timer)sender).Enabled = false;
                Task Reconnect = new Task(() =>
                {
                    MT.ChkAndReConnect();
                }
                );
                Reconnect.Start();
                for (int n = 0; n < 3000; n++)
                {
                    if (VAR.gsys_set.bclose) break;
                    if (Reconnect.IsCompleted) break;
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
            }
            finally
            {
                ((System.Windows.Forms.Timer)sender).Enabled = true;
            }  
        }

        private void rbtn_sys_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void pnl_sub_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axTcpClient_cmd_OnReceviceByte(byte[] date)
        {
            string str = Encoding.Default.GetString(date);
              // if (str.Equals(COMM.CMD_STA.AANG))
              //  COMM.Client_Sta = COMM.CMD_STA.AANG;
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.DBG, "客户端收到信息:" + str);

          
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
   
        }

        private void axTcpServer1_OnStateInfo(string msg, SocketHelper.SocketState state)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MT.OUT_Bee.SetOn();
            MT.OUT_RED_light.SetOn();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MT.OUT_Bee.SetOff();
            MT.OUT_RED_light.SetOff();
        }
    }
}
