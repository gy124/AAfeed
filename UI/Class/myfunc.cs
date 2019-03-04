using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using MotionCtrl;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;


namespace UI
{
   
    //运动函数委托
    public delegate EM_RES Mact(ref bool bquit);
    public delegate void MsgShow(string msg = "");
    public delegate EM_RES CAMERA(out ST_XYZ res, int camera_id = 1);

    #region COM
    public static class COM
    {
        //工站
        public static WS ws_back = new WS(0, MT.AXIS_BACK_X, MT.AXIS_BACK_Y, MT.AXIS_BACK_Z, MT.AXIS_BACK_A, MT.List_vacu_back, MT.List_vacu_back);
        public static WS ws_feed = new WS(1, MT.AXIS_FEED_X, MT.AXIS_FEED_Y, MT.AXIS_FEED_Z, MT.AXIS_FEED_A, MT.List_cyl_feed, MT.List_vacu_feed);
        //public static WS ws3 = new WS(2, MT.AXIS_WS3_F, MT.AXIS_WS3_B, MT.AXIS_WS3_U, MT.List_CLD_WS3_FR, MT.List_CLD_WS3_BK);

        public static WS ws_get = new WS(4, MT.AXIS_GET_X, MT.AXIS_GET_Y, MT.AXIS_GET_Z, MT.AXIS_GET_A, MT.List_cyl_get, MT.List_vacu_get);
        //public static List<WS> list_ws = new List<WS>() { ws1, ws2 , ws3 , ws4,ws5 };
        public static List<WS> list_ws = new List<WS>() { ws_back, ws_feed, ws_get };

        //    public static TrayBox traybox_fd = new TrayBox("供料仓", TrayBox.EM_DIR.ONLY_IN, 10, MT.AXIS_UL_FD_X, MT.AXIS_UL_FD_Z, MT.GPIO_IN_UL_INP_FD_TRAYBOX, MT.GPIO_IN_UL_RDY_FD_TRAY, MT.GPIO_OUT_UL_ZK_FD_TRAY, MT.GPIO_IN_UL_ZK_FD_TRAY);
        //      public static TrayBox traybox_ok = new TrayBox("OK料仓", TrayBox.EM_DIR.IN_OUT, 10, MT.AXIS_DL_OK_X, MT.AXIS_DL_OK_Z, MT.GPIO_IN_DL_INP_OK_TRAYBOX, MT.GPIO_IN_DL_RDY_OK_TRAY, MT.GPIO_OUT_DL_ZK_OK_TRAY, MT.GPIO_IN_DL_ZK_OK_TRAY);
        //  public static TrayBox traybox_ng = new TrayBox("NG料仓", TrayBox.EM_DIR.IN_OUT, 10, MT.AXIS_DL_NG_X, MT.AXIS_DL_NG_Z, MT.GPIO_IN_DL_INP_NG_TRAYBOX, MT.GPIO_IN_DL_RDY_NG_TRAY, MT.GPIO_OUT_DL_ZK_NG_TRAY, MT.GPIO_IN_DL_ZK_NG_TRAY);

        public static TrayBox traybox_get = new TrayBox("弹夹供料仓", ref MT.pos_bullet_feed_check_low, ref MT.pos_bullet_feed_check_top, 1, TrayBox.EM_DIR.ONLY_OUT, MT.CYL_bullet_feed_plate, 10, MT.AXIS_bullet_move, MT.AXIS_bullet_feed,
            MT.pos_bullet_feed_boxOUT, MT.pos_bullet_feed_boxIN, MT.CKPOS_bull_feed_box, MT.CKPOS_bull_feed_plate, MT.VACUM_move_plate);
        public static TrayBox traybox_back = new TrayBox("弹夹收料OK仓", ref  MT.pos_bullet_back_check_low, ref MT.pos_bullet_back_check_top, 2, TrayBox.EM_DIR.ONLY_IN, MT.CYL_bullet_back_plate, 10, MT.AXIS_bullet_move, MT.AXIS_bullet_back,
           MT.pos_bullet_back_boxOUT, MT.pos_bullet_back_boxIN, MT.CKPOS_bull_back_box, MT.CKPOS_bull_back_plate);
        
        //目标
        public static Product product = new Product();
      
        //左光箱
        //   public static LightBox LeftLightBox = new LightBox("左光箱", MT.AXIS_BOX_L_Y_NAF, MT.AXIS_BOX_L_Y_FAF, MT.AXIS_BOX_L_Z_DUST, MT.AXIS_BOX_L_Z_NAF);
        //右光箱
        //    public static LightBox RightLightBox = new LightBox("右光箱", MT.AXIS_BOX_R_Y_NAF, MT.AXIS_BOX_R_Y_FAF, MT.AXIS_BOX_R_Z_DUST, MT.AXIS_BOX_R_Z_NAF);

        //视觉
        public static Msg vs_msg = new Msg(200, Msg.EM_MSGTYPE.NOR | Msg.EM_MSGTYPE.ERR);
        public static VS MVS = new VS();
        #region 设备复位
        public static bool bhomeing = false;

        public static EM_RES Home()
        {
            try
            {
                VAR.gsys_set.bquit = false;
                bhomeing = true;
                //光箱/上料
                EM_RES resLB = EM_RES.OK;
                EM_RES resRB = EM_RES.OK;
                EM_RES resUL = EM_RES.OK;
                EM_RES resTM = EM_RES.OK;
                EM_RES resBFD = EM_RES.OK;
                EM_RES resBBK = EM_RES.OK;
                EM_RES resFD = EM_RES.OK;
                EM_RES resBK = EM_RES.OK;
                EM_RES resGT = EM_RES.OK;
                EM_RES res = EM_RES.OK;


                Task taskBBK = new Task(() => { resBBK = WsBuBK.home(ref VAR.gsys_set.bquit); });
                Task taskBFD = new Task(() => { resBFD = WsBuFD.home(ref VAR.gsys_set.bquit); });
                Task taskBK = new Task(() => { resBK = WSBack.home(ref VAR.gsys_set.bquit); });
                Task taskFD = new Task(() => { resFD = WSFeed.home(ref VAR.gsys_set.bquit); });
                Task taskGT = new Task(() => { resGT = WSGet.home(ref VAR.gsys_set.bquit); });
                Task taskTM = new Task(() => { resTM = WsTrayMove.home(ref VAR.gsys_set.bquit); });
                //等待退出
                while (!(WsBuBK.TaskRun.IsCompleted && WsBuFD.TaskRun.IsCompleted && WsTrayMove.TaskRun.IsCompleted))
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                    if (VAR.gsys_set.bquit)
                        break;
                }

                taskTM.Start();
                taskFD.Start();
                taskBBK.Start();
                taskBFD.Start();
                while (!VAR.gsys_set.bquit)
                {

                    if (taskTM.IsCompleted) break;
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                if (resTM != EM_RES.OK)
                {
                    // 停止所有
                    return EM_RES.ERR;
                }

                taskGT.Start();
                taskBK.Start();
                while (!VAR.gsys_set.bquit)
                {

                    if (taskGT.IsCompleted && taskBK.IsCompleted && taskBFD.IsCompleted && taskBBK.IsCompleted
                        && taskFD.IsCompleted) break;
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                if (resBFD == EM_RES.OK && resBBK == EM_RES.OK && resFD == EM_RES.OK && resBK == EM_RES.OK
                    && resGT == EM_RES.OK && resTM == EM_RES.OK) 
                    return EM_RES.OK;
                else

                    // 停止所有

                    if (VAR.gsys_set.bquit)
                        return EM_RES.QUIT;
                return EM_RES.ERR;







                //Task taskLeftLbHome = new Task(() => { resLB = LeftLightBox.Home(ref VAR.gsys_set.bquit); });
                //    Task taskRightLbHome = new Task(() => { resRB = RightLightBox.Home(ref VAR.gsys_set.bquit); });
                //    Task taskUploadHome = new Task(() => { resUL = UploadModle.Home(ref VAR.gsys_set.bquit); });
                //  taskLeftLbHome.Start();
                //        taskRightLbHome.Start();
                //          taskUploadHome.Start();

                //wait
                //         while (!VAR.gsys_set.bquit)
                //         {
                //if (taskLeftLbHome.IsCompleted && taskRightLbHome.IsCompleted && taskUploadHome.IsCompleted) break;
                //              if (  taskUploadHome.IsCompleted) break;
                //              Application.DoEvents();
                //              Thread.Sleep(10);
                //            }
                //          if (resLB != EM_RES.OK || resRB != EM_RES.OK || resUL != EM_RES.OK) res = EM_RES.ERR;

                //          if (VAR.gsys_set.bquit || res != EM_RES.OK)
                //            {
                //LeftLightBox.Stop();
                //     RightLightBox.Stop();
                //                 UploadModle.Stop();
                //                  return VAR.gsys_set.bquit ? EM_RES.QUIT : res;
                //             }



                //转盘/下料
                //EM_RES resTP = EM_RES.OK;
                //EM_RES resDL = EM_RES.OK;
                //Task taskTurnplateHome = new Task(() => { resTP = EM_RES.OK; });
                //Task taskDownloadHome = new Task(() => { resDL = DownloadModle.Home(ref VAR.gsys_set.bquit); });
                //taskTurnplateHome.Start();
                //taskDownloadHome.Start();

                //wait
                //while (!VAR.gsys_set.bquit)
                //{
                //    if (taskTurnplateHome.IsCompleted && taskDownloadHome.IsCompleted) break;
                //    Application.DoEvents();
                //    Thread.Sleep(10);
                //}

                //if (resTP != EM_RES.OK || resDL != EM_RES.OK) res = EM_RES.ERR;

                //if (VAR.gsys_set.bquit || res != EM_RES.OK)
                //{
                //    //taskTurnplateHome.Stop();
                //    DownloadModle.Stop();
                //    return VAR.gsys_set.bquit ? EM_RES.QUIT : res;
                //}

                //return EM_RES.OK;
            }
            catch
            {
                VAR.ErrMsg("全部回原中发生未知错误！");
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }
            finally
            {
                bhomeing = false;
            }
        }
        /// <summary>
        /// 停止轴运动，停止Home动作
        /// </summary>
        public static void Stop()
        {
            //LeftLightBox.Stop();
            //  RightLightBox.Stop();
            //  UploadModle.Stop();
            //taskTurnplateHome.Stop();
            //  DownloadModle.Stop();
            WsBuBK.Stop();
            WsBuFD.Stop();
            WSBack.Stop();
            WSFeed.Stop();
            WSGet.Stop();
            WsTrayMove.Stop();
        }
        #endregion
    }
    #endregion
    #region 运动控制
    public static class MT
    {
        public const int MAXNUM = 999;
        public enum WS_ID { get = 5, feed = 4, back = 3, bull_back = 2, bull_feed = 1, bull_mov = 6, tray = 7 }
        public enum WS_XNUM { four = 4, three = 3, two = 2, one = 1 }
        #region 板卡定义
        //public static CARD CARD_ECI2400_1 = new CARD(1, "192.168.0.101", 4, 24, 8, CARD.BRAND.ZMOTION, CARD.TYPE.MOTION, "ECI2400", "左光箱");
        //public static CARD CARD_ECI2400_2 = new CARD(2, "192.168.0.102", 4, 24, 8, CARD.BRAND.ZMOTION, CARD.TYPE.MOTION, "ECI2400", "右光箱");
        //public static CARD CARD_ECI2600_3 = new CARD(3, "192.168.0.103", 6, 24, 8, CARD.BRAND.ZMOTION, CARD.TYPE.MOTION, "ECI2600", "下料");
        //public static CARD CARD_ECI0064_4 = new CARD(4, "192.168.0.104", 0, 32, 32, CARD.BRAND.ZMOTION, CARD.TYPE.IO, "ECI0064", "下料");
        //public static CARD CARD_DMC3800_5 = new CARD(5, 0, 8, 16, 16, CARD.BRAND.LEADSHINE, CARD.TYPE.MOTION, "DMC3800", "上料");
        public static CARD CARD_GD_6 = new CARD(6, 0, 8, 16, 16, CARD.BRAND.GOOGOLTECH, CARD.TYPE.MOTION, "固高卡1", "固高测试");
        public static CARD CARD_GD_7 = new CARD(7, 1, 8, 16, 16, CARD.BRAND.GOOGOLTECH, CARD.TYPE.MOTION, "固高卡0", "固高测试");
        public static CARD CARD_GD_EX_0 = new CARD(8, 0, 8, 16, 16, CARD.BRAND.GOOGOLTECH, CARD.TYPE.CAN_IO, "固高卡扩展0", "固高测试", 0);
        public static CARD CARD_GD_EX_1 = new CARD(9, 1, 8, 16, 16, CARD.BRAND.GOOGOLTECH, CARD.TYPE.CAN_IO, "固高卡扩展1", "固高测试", 0);

        public static List<CARD> CardList = new List<CARD> { CARD_GD_6, CARD_GD_7, CARD_GD_EX_0, CARD_GD_EX_1 };
        #endregion
        #region IO 定义
        public static GPIO GPIO_OUT_ON_NULL = new GPIO(0, null, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.NULL, "OUT_ON_NULL", GPIO.IO_STA.OUT_ON);
        public static GPIO GPIO_IN_ON_NULL = new GPIO(0, null, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.NULL, "IN_ON_NULL", GPIO.IO_STA.IN_ON);
        public static GPIO GPIO_OUT_OFF_NULL = new GPIO(0, null, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.NULL, "OUT_OFF_NULL", GPIO.IO_STA.OUT_OFF);
        public static GPIO GPIO_IN_OFF_NULL = new GPIO(0, null, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.NULL, "IN_OFF_NULL", GPIO.IO_STA.IN_OFF);
        #region OUT
        //固高测试
        //弹夹气缸
        public static GPIO GO_CYL_bullet_up = new GPIO(0, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹顶升气缸");
        public static GPIO CKB_CYL_bullet_up = new GPIO(2, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "弹夹顶升气缸原位");
        public static GPIO OUT_CYL_bullet_up = new GPIO(3, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "弹夹顶升气缸动位");
        public static Cylinder CYL_bullet_up = new Cylinder(GO_CYL_bullet_up, OUT_CYL_bullet_up, CKB_CYL_bullet_up);

        public static GPIO GO_bullet_back_plate_in = new GPIO(3, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹收料载盘气缸进弹夹");
        public static GPIO OUT_CYL_bullet_back_plate = new GPIO(9, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "弹夹收料载盘气缸动位");
        public static GPIO CKB_CYL_bullet_back_plate = new GPIO(8, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "弹夹收料载盘气缸原位");
        public static GPIO GO_bullet_back_plate_out = new GPIO(13, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹收料载盘气缸（出弹夹");
        public static Cylinder CYL_bullet_back_plate = new Cylinder(2, GO_bullet_back_plate_out, OUT_CYL_bullet_back_plate, CKB_CYL_bullet_back_plate, GO_bullet_back_plate_in);

        public static GPIO GO_bullet_feed_plate_in = new GPIO(12, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹上料载盘拉出气缸进弹");
        public static GPIO OUT_CYL_bullet_feed_plate = new GPIO(6, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "弹夹上料载盘拉出气缸原位");
        public static GPIO CKB_CYL_bullet_feed_plate = new GPIO(7, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "弹夹上料载盘拉出气缸动位");
        public static GPIO GO_bullet_feed_plate_out = new GPIO(2, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹上料载盘拉出气缸（出弹夹）");
        public static Cylinder CYL_bullet_feed_plate = new Cylinder(2, GO_bullet_feed_plate_out, OUT_CYL_bullet_feed_plate, CKB_CYL_bullet_feed_plate, GO_bullet_feed_plate_in);

        public static GPIO GO_CYL_bullet_move_plate_up = new GPIO(4, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "载盘移位升降气缸");
        public static GPIO CKB_CYL_bullet_move_plate_up = new GPIO(10, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "载盘移位升降气缸原位");
        public static GPIO OUT_CYL_bullet_move_plate_up = new GPIO(11, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "载盘移位升降气缸动位");
        public static Cylinder CYL_bullet_move_plate_up = new Cylinder(GO_CYL_bullet_move_plate_up, OUT_CYL_bullet_move_plate_up, CKB_CYL_bullet_move_plate_up);
        //上料气缸
        public static GPIO GO_CYL_feed_left_up = new GPIO(3, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "上料左升降气缸");
        public static GPIO CKB_CYL_feed_left_up = new GPIO(5, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料左升降气缸原位");
        public static GPIO OUT_CYL_feed_left_up = new GPIO(6, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料左升降气缸动位");
        public static Cylinder CYL_feed_left_up = new Cylinder(GO_CYL_feed_left_up, OUT_CYL_feed_left_up, CKB_CYL_feed_left_up);

        public static GPIO GO_CYL_feed_right_up = new GPIO(5, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "上料右升降气缸");
        public static GPIO CKB_CYL_feed_right_up = new GPIO(9, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料右升降气缸原位");
        public static GPIO OUT_CYL_feed_right_up = new GPIO(10, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料右升降气缸动位");
        public static Cylinder CYL_feed_right_up = new Cylinder(GO_CYL_feed_right_up, OUT_CYL_feed_right_up, CKB_CYL_feed_right_up);

        public static GPIO GO_feed_left_clip_tight = new GPIO(12, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "上料左夹紧气缸（夹紧）");
        public static GPIO GO_feed_left_clip_loosen = new GPIO(4, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "上料左夹紧气缸（松开）");
        public static GPIO CKB_CYL_feed_left_clip = new GPIO(7, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料左夹紧气缸原位");
        public static GPIO OUT_CYL_feed_left_clip = new GPIO(8, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料左夹紧气缸动位");
        public static Cylinder CYL_feed_left_clip = new Cylinder(2, GO_feed_left_clip_tight, OUT_CYL_feed_left_clip, CKB_CYL_feed_left_clip, GO_feed_left_clip_loosen);

        public static GPIO GO_feed_right_clip_tight = new GPIO(13, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "上料右夹紧气缸（夹紧）");
        public static GPIO GO_feed_right_clip_loosen = new GPIO(6, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "上料右夹紧气缸（松开）");
        public static GPIO CKB_CYL_feed_right_clip = new GPIO(11, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料右夹紧气缸原位");
        public static GPIO OUT_CYL_feed_right_clip = new GPIO(12, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料右夹紧气缸动位");
        public static Cylinder CYL_feed_right_clip = new Cylinder(2, GO_feed_right_clip_tight, OUT_CYL_feed_right_clip, CKB_CYL_feed_right_clip, GO_feed_right_clip_loosen);

        public static GPIO OUT_VACUM_feed_hand_L = new GPIO(0, CARD_GD_EX_0, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "左夹爪真空0");
        public static GPIO OUT_VACUM_feed_hand_R = new GPIO(1, CARD_GD_EX_0, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "右夹爪真空1");
        public static GPIO OUT_RED_light = new GPIO(2, CARD_GD_EX_0, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "报警灯鸣1");
        public static GPIO OUT_Bee = new GPIO(3, CARD_GD_EX_0, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "报警灯鸣2");
        public static GPIO VACUM_feed_hand_L_check = new GPIO(6, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "左夹爪真空检测");
        public static GPIO VACUM_feed_hand_R_check = new GPIO(7, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "右夹爪真空检测");
        public static Cylinder VACUM_feed_hand_L = new Cylinder(OUT_VACUM_feed_hand_L, VACUM_feed_hand_L_check);
        public static Cylinder VACUM_feed_hand_R = new Cylinder(OUT_VACUM_feed_hand_R, VACUM_feed_hand_R_check);

        public static List<Cylinder> List_cyl_feed = new List<Cylinder> { CYL_feed_left_up, CYL_feed_right_up, CYL_feed_left_clip, CYL_feed_right_clip };
        public static List<Cylinder> List_vacu_feed = new List<Cylinder> { VACUM_feed_hand_L, VACUM_feed_hand_R };
        //取料气缸真空

        public static GPIO GO_CYL_get_up = new GPIO(2, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "取料旋转升降气缸");
        public static GPIO CKB_CYL_get_up = new GPIO(2, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "取料升降气缸原位22");
        public static GPIO OUT_CYL_get_up = new GPIO(3, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "取料升降气缸动位23");
        public static Cylinder CYL_get_up = new Cylinder(GO_CYL_get_up, OUT_CYL_get_up, CKB_CYL_get_up);

        public static GPIO OUT_VACUM_get_mouth = new GPIO(1, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "取料旋转真空");
        public static GPIO VACUM_get_mouth_check = new GPIO(4, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "取料旋转真空检测");
        public static Cylinder VACUM_get_mouth = new Cylinder(OUT_VACUM_get_mouth, VACUM_get_mouth_check);

        public static List<Cylinder> List_cyl_get = new List<Cylinder> { CYL_get_up };
        public static List<Cylinder> List_vacu_get = new List<Cylinder> { VACUM_get_mouth };
        //收料气缸真空
        public static GPIO GO_CYL_back_up = new GPIO(8, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "收料旋转升降气缸");
        public static GPIO CKB_CYL_back_up = new GPIO(0, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "收料升降气缸原位");
        public static GPIO OUT_CYL_back_up = new GPIO(1, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "收料升降气缸动位");
        public static Cylinder CYL_back_up = new Cylinder(GO_CYL_back_up, OUT_CYL_back_up, CKB_CYL_back_up);

        public static GPIO OUT_VACUM_back_mouth = new GPIO(7, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "收料旋转真空");
        public static GPIO VACUM_back_mouth_check = new GPIO(4, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "收料旋转真空检测");
        public static Cylinder VACUM_back_mouth = new Cylinder(OUT_VACUM_back_mouth, VACUM_back_mouth_check);

        public static List<Cylinder> List_cyl_back = new List<Cylinder> { CYL_back_up };
        public static List<Cylinder> List_vacu_back = new List<Cylinder> { VACUM_back_mouth };
        //检测工站
        public static GPIO GO_CYL_check_up = new GPIO(9, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "检测升降气缸");
        public static GPIO CKB_CYL_check_up = new GPIO(0, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "检测升降气缸原位20");
        public static GPIO OUT_CYL_check_up = new GPIO(1, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "检测升降气缸动位21");
        public static Cylinder CYL_check_up = new Cylinder(GO_CYL_check_up, OUT_CYL_check_up, CKB_CYL_check_up);

        public static GPIO GO_CYL_cover_open = new GPIO(1, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "推出开盖气缸");
        public static GPIO CKB_CYL_cover_open = new GPIO(2, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "推出开盖气缸原位");
        public static GPIO OUT_CYL_cover_open = new GPIO(3, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "推出开盖气缸动位");
        public static Cylinder CYL_cover_open = new Cylinder(GO_CYL_cover_open, OUT_CYL_cover_open, CKB_CYL_cover_open);

        public static GPIO GO_CYL_cover_close = new GPIO(0, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "推出合盖气缸");
        public static GPIO CKB_CYL_cover_close = new GPIO(4, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "推出扣盖气缸原位24");
        public static GPIO OUT_CYL_cover_close = new GPIO(5, CARD_GD_EX_1, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "推出扣盖气缸动位25");
        public static Cylinder CYL_cover_close = new Cylinder(GO_CYL_cover_close, OUT_CYL_cover_close, CKB_CYL_cover_close);





        public static GPIO OUT_VACUM_move_plate = new GPIO(5, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "载盘真空");
        public static GPIO VACUM_move_plate_check = new GPIO(12, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "载盘真空检测");
        public static Cylinder VACUM_move_plate = new Cylinder(OUT_VACUM_move_plate, VACUM_move_plate_check);




        public static GPIO OUT_VACUM_roll_plate1 = new GPIO(6, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘工位1负压");
        public static GPIO OUT_VACUM_roll_plate2 = new GPIO(7, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘工位2负压");
        public static GPIO OUT_VACUM_roll_plate3 = new GPIO(8, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘工位3负压");
        public static GPIO OUT_VACUM_roll_plate4 = new GPIO(9, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘工位4负压");
        public static GPIO OUT_VACUM_roll_plate5 = new GPIO(10, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘工位5负压");
        public static GPIO OUT_VACUM_roll_plate6 = new GPIO(11, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘工位6负压");
        public static GPIO VACUM_roll_plate1_check = new GPIO(2, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘工位1负压检测");
        public static GPIO VACUM_roll_plate2_check = new GPIO(3, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘工位2负压检测");
        public static GPIO VACUM_roll_plate3_check = new GPIO(4, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘工位3负压检测");
        public static GPIO VACUM_roll_plate4_check = new GPIO(5, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘工位4负压检测");
        public static GPIO VACUM_roll_plate5_check = new GPIO(6, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘工位5负压检测");
        public static GPIO VACUM_roll_plate6_check = new GPIO(7, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘工位6负压检测");
        public static Cylinder VACUM_roll_plate1 = new Cylinder(OUT_VACUM_roll_plate1, VACUM_roll_plate1_check);
        public static Cylinder VACUM_roll_plate2 = new Cylinder(OUT_VACUM_roll_plate2, VACUM_roll_plate2_check);
        public static Cylinder VACUM_roll_plate3 = new Cylinder(OUT_VACUM_roll_plate3, VACUM_roll_plate3_check);
        public static Cylinder VACUM_roll_plate4 = new Cylinder(OUT_VACUM_roll_plate4, VACUM_roll_plate4_check);
        public static Cylinder VACUM_roll_plate5 = new Cylinder(OUT_VACUM_roll_plate5, VACUM_roll_plate5_check);
        public static Cylinder VACUM_roll_plate6 = new Cylinder(OUT_VACUM_roll_plate6, VACUM_roll_plate6_check);

        public static List<Cylinder> List_vacu_roll = new List<Cylinder> { VACUM_roll_plate1, VACUM_roll_plate2,
            VACUM_roll_plate3, VACUM_roll_plate4, VACUM_roll_plate5, VACUM_roll_plate6 };
        public static GPIO GPIO_OUT_belet = new GPIO(14, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹皮带");
        public static GPIO GPIO_OUT_brake_back_z = new GPIO(15, CARD_GD_7, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "收料弹夹z刹车");
        public static GPIO GPIO_OUT_gugao_test114 = new GPIO(14, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "固高出口1.14");


        public static GPIO CKPOS_bull_feed_box = new GPIO(0, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料弹夹感应");
        public static GPIO CKPOS_bull_feed_plate = new GPIO(1, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料弹夹料盘感应");
        public static GPIO GPIO_IN_code_closed = new GPIO(13, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "治具扣盖检测");
        public static GPIO CKPOS_MOVE_get_plate = new GPIO(14, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "移动上料位载盘感应");
        public static GPIO GPIO_IN_gugao_test115 = new GPIO(15, CARD_GD_6, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "固高入口1.15");
        public static GPIO GPIO_IN_code_open = new GPIO(5, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "治具开盖检测");
        public static GPIO 
            
            GPIO_IN_emg_key1 = new GPIO(13, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "急停1按钮");
        public static GPIO CKPOS_MOVE_middle_plate = new GPIO(14, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "移动中间位载盘感应");
        public static GPIO CKPOS_MOVE_back_plate = new GPIO(15, CARD_GD_7, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "移动收料位载盘感应");

        public static GPIO CKPOS_bull_back_plate = new GPIO(0, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "收料弹夹感应");
        public static GPIO CKPOS_bull_back_box = new GPIO(1, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "收料料盒感应");
        public static GPIO CKPOS_bull_belt_come = new GPIO(8, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "皮带弹盒进料感应");
        public static GPIO CKPOS_bull_belt_topos = new GPIO(9, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "皮带弹盒到位感应9");
        public static GPIO CKPOS_bull_back_up_out = new GPIO(10, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "收弹夹顶出感应10");
        public static GPIO CKPOS_roll_plate_topos = new GPIO(11, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘到位感应11");
        public static GPIO CKPOS_roll_plate_home_point = new GPIO(14, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "转盘原点位13");

        //按键
        public static GPIO GPIO_IN_key_start = new GPIO(12, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "启动按钮12");
        public static GPIO GPIO_IN_emg_key2 = new GPIO(13, CARD_GD_EX_0, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "急停2按钮13");

        public static GPIO GPIO_OUT_roll_plate = new GPIO(10, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "转盘启动");
        public static GPIO GPIO_OUT_light = new GPIO(11, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "照明灯");
        public static GPIO GPIO_OUT_brake_feed_z = new GPIO(15, CARD_GD_6, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "弹夹上料Z刹车");

        //上料
        // public static GPIO GPIO_OUT_UL_ZK_FD_TRAY = new GPIO(3, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "供料料盘真空");

        //按键
        // public static GPIO GPIO_OUT_KL_START = new GPIO(6, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "开始按键灯");
        // public static GPIO GPIO_OUT_KL_STOP = new GPIO(7, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "停止按键灯");
        // public static GPIO GPIO_OUT_KL_RESET = new GPIO(8, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "复位按键灯");
        //警报
        //     public static GPIO GPIO_OUT_ALM_RED = new GPIO(9, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "红色塔灯");
        //      public static GPIO GPIO_OUT_ALM_YELLOW = new GPIO(10, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "黄色塔灯");
        //     public static GPIO GPIO_OUT_ALM_GREEN = new GPIO(11, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "绿色塔灯");
        //      public static GPIO GPIO_OUT_ALM_BEEPER = new GPIO(12, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "蜂鸣器");
        //相机
        //       public static GPIO GPIO_OUT_UL_CAM_FR = new GPIO(13, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "外相机触发");
        //       public static GPIO GPIO_OUT_UL_CAM_DW = new GPIO(14, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "下相机触发");
        //      public static GPIO GPIO_OUT_UL_CAM_BK = new GPIO(15, CARD_DMC3800_5, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.MT_CARD, "内相机触发");
        //料盘真空
        //    public static GPIO GPIO_OUT_DL_ZK_NG_TRAY = new GPIO(0, CARD_ECI2600_3, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.IO_CARD, "NG料盘真空");
        //     public static GPIO GPIO_OUT_DL_ZK_OK_TRAY = new GPIO(1, CARD_ECI2600_3, GPIO.IO_DIR.OUT, GPIO.IO_TYPE.IO_CARD, "OK料盘真空");

        //工站压盖

        #endregion
        #region IN
        //急停
        //public static GPIO GPIO_IN_EMG = new GPIO(0, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "急停键");
        ////上料
        //public static GPIO GPIO_IN_UL_ZK_N1 = new GPIO(1, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料吸头真空感应1");
        //public static GPIO GPIO_IN_UL_ZK_N2 = new GPIO(2, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "上料吸头真空感应2");
        //public static GPIO GPIO_IN_UL_INP_FD_TRAYBOX = new GPIO(3, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "供料仓在位感应");
        //public static GPIO GPIO_IN_UL_RDY_FD_TRAY = new GPIO(4, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "供料仓有料感应");
        //public static GPIO GPIO_IN_UL_ZK_FD_TRAY = new GPIO(5, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "供料盘真空感应");
        ////按键
        //public static GPIO GPIO_IN_KEY_START = new GPIO(6, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "开始键");
        //public static GPIO GPIO_IN_KEY_STOP = new GPIO(7, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "停止键");
        //public static GPIO GPIO_IN_KEY_RESET = new GPIO(8, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "复位键");
        ////安全门
        //public static GPIO GPIO_IN_FR_DOOR = new GPIO(9, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "前门光栅");
        //public static GPIO GPIO_IN_BK_DOOR = new GPIO(10, CARD_DMC3800_5, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "后门感应");

        //下料
        //夹爪夹位感应
        //public static GPIO GPIO_IN_DL_HD_HD1 = new GPIO(0, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应1");
        //public static GPIO GPIO_IN_DL_HD_HD2 = new GPIO(1, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应2");
        //public static GPIO GPIO_IN_DL_HD_HD3 = new GPIO(2, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应3");
        //public static GPIO GPIO_IN_DL_HD_HD4 = new GPIO(3, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应4");
        //public static GPIO GPIO_IN_DL_HD_HD5 = new GPIO(4, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应5");
        //public static GPIO GPIO_IN_DL_HD_HD6 = new GPIO(5, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应6");
        //public static GPIO GPIO_IN_DL_HD_HD7 = new GPIO(6, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应7");
        //public static GPIO GPIO_IN_DL_HD_HD8 = new GPIO(7, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应8");
        //public static GPIO GPIO_IN_DL_HD_HD9 = new GPIO(8, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应9");
        //public static GPIO GPIO_IN_DL_HD_HD10 = new GPIO(9, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应10");
        //public static GPIO GPIO_IN_DL_HD_HD11 = new GPIO(10, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应11");
        //public static GPIO GPIO_IN_DL_HD_HD12 = new GPIO(11, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应12");
        //public static GPIO GPIO_IN_DL_HD_HD13 = new GPIO(12, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应13");
        //public static GPIO GPIO_IN_DL_HD_HD14 = new GPIO(13, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应14");
        //public static GPIO GPIO_IN_DL_HD_HD15 = new GPIO(14, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应15");
        //public static GPIO GPIO_IN_DL_HD_HD16 = new GPIO(15, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪夹位感应16");
        ////夹爪下位感应
        //public static GPIO GPIO_IN_DL_DW_HD1 = new GPIO(16, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应1");
        //public static GPIO GPIO_IN_DL_DW_HD2 = new GPIO(17, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应2");
        //public static GPIO GPIO_IN_DL_DW_HD3 = new GPIO(18, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应3");
        //public static GPIO GPIO_IN_DL_DW_HD4 = new GPIO(19, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应4");
        //public static GPIO GPIO_IN_DL_DW_HD5 = new GPIO(20, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应5");
        //public static GPIO GPIO_IN_DL_DW_HD6 = new GPIO(21, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应6");
        //public static GPIO GPIO_IN_DL_DW_HD7 = new GPIO(22, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应7");
        //public static GPIO GPIO_IN_DL_DW_HD8 = new GPIO(23, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应8");
        //public static GPIO GPIO_IN_DL_DW_HD9 = new GPIO(24, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应9");
        //public static GPIO GPIO_IN_DL_DW_HD10 = new GPIO(25, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应10");
        //public static GPIO GPIO_IN_DL_DW_HD11 = new GPIO(26, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应11");
        //public static GPIO GPIO_IN_DL_DW_HD12 = new GPIO(27, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应12");
        //public static GPIO GPIO_IN_DL_DW_HD13 = new GPIO(28, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应13");
        //public static GPIO GPIO_IN_DL_DW_HD14 = new GPIO(29, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应14");
        //public static GPIO GPIO_IN_DL_DW_HD15 = new GPIO(30, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应15");
        //public static GPIO GPIO_IN_DL_DW_HD16 = new GPIO(31, CARD_ECI0064_4, GPIO.IO_DIR.IN, GPIO.IO_TYPE.IO_CARD, "下料夹爪下位感应16");
        ////料夹在位
        //public static GPIO GPIO_IN_DL_INP_NG_TRAYBOX = new GPIO(18, CARD_ECI2600_3, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "NG料仓在位感应");
        //public static GPIO GPIO_IN_DL_INP_OK_TRAYBOX = new GPIO(19, CARD_ECI2600_3, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "OK料仓在位感应");
        ////料夹有料
        //public static GPIO GPIO_IN_DL_RDY_NG_TRAY = new GPIO(20, CARD_ECI2600_3, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "NG料仓有料感应");
        //public static GPIO GPIO_IN_DL_RDY_OK_TRAY = new GPIO(21, CARD_ECI2600_3, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "OK料仓有料感应");
        ////料盘真空
        //public static GPIO GPIO_IN_DL_ZK_NG_TRAY = new GPIO(22, CARD_ECI2600_3, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "NG料盘真空感应");
        //public static GPIO GPIO_IN_DL_ZK_OK_TRAY = new GPIO(23, CARD_ECI2600_3, GPIO.IO_DIR.IN, GPIO.IO_TYPE.MT_CARD, "OK料盘真空感应");

        //工站压盖
        #endregion
        #region 气缸定义
        //料盘
        //  public static Cylinder CLD_UL_ZK_TRAY_FD = new Cylinder(GPIO_OUT_UL_ZK_FD_TRAY, GPIO_IN_UL_ZK_FD_TRAY);
        //夹爪     
        //  public static Cylinder CLD_DL_ZK_TRAY_NG = new Cylinder(GPIO_OUT_DL_ZK_NG_TRAY, GPIO_IN_DL_ZK_NG_TRAY);
        //   public static Cylinder CLD_DL_ZK_TRAY_OK = new Cylinder(GPIO_OUT_DL_ZK_OK_TRAY, GPIO_IN_DL_ZK_OK_TRAY);

        #endregion

        #endregion
        #region 轴定义
        //固高工位1取料
        public static AXIS AXIS_GET_X = new AXIS(1, CARD_GD_6, "取料X", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_GET_Y = new AXIS(0, CARD_GD_6, "取料Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_GET_Z = new AXIS(2, CARD_GD_6, "取料Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_GET_A = new AXIS(3, CARD_GD_6, "取料转角A", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //固高工位1收料
        public static AXIS AXIS_BACK_X = new AXIS(1, CARD_GD_7, "收料X", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_BACK_Y = new AXIS(0, CARD_GD_7, "收料Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_BACK_Z = new AXIS(2, CARD_GD_7, "收料Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_BACK_A = new AXIS(3, CARD_GD_7, "收料转角A", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //固高工位1收料
        public static AXIS AXIS_FEED_X = new AXIS(5, CARD_GD_6, "上料X", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_FEED_Y = new AXIS(4, CARD_GD_6, "上料Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_FEED_Z = new AXIS(6, CARD_GD_6, "上料Z", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_FEED_A = new AXIS(7, CARD_GD_6, "上料转角A", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //固高工位1收料
        public static AXIS AXIS_bullet_back = new AXIS(4, CARD_GD_7, "弹夹收料", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_bullet_feed = new AXIS(6, CARD_GD_7, "弹夹上料", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        public static AXIS AXIS_bullet_move = new AXIS(5, CARD_GD_7, "载盘移动", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //工位1
        //OTP光箱

        ////上料
        //public static AXIS AXIS_UL_X = new AXIS(0, CARD_DMC3800_5, "上料X", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_UL_Y = new AXIS(1, CARD_DMC3800_5, "上料Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_UL_Z = new AXIS(2, CARD_DMC3800_5, "上料Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_UL_U1 = new AXIS(3, CARD_DMC3800_5, "上料U1", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_UL_U2 = new AXIS(4, CARD_DMC3800_5, "上料U2", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_UL_FD_X = new AXIS(5, CARD_DMC3800_5, "供料X", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_UL_FD_Z = new AXIS(6, CARD_DMC3800_5, "供料Z", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);

        ////下料
        //public static AXIS AXIS_DL_Y = new AXIS(0, CARD_ECI2600_3, "下料Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_DL_Z = new AXIS(1, CARD_ECI2600_3, "下料Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_DL_OK_X = new AXIS(2, CARD_ECI2600_3, "OK料X", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_DL_OK_Z = new AXIS(3, CARD_ECI2600_3, "OK料Z", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_DL_NG_X = new AXIS(4, CARD_ECI2600_3, "NG料X", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_DL_NG_Z = new AXIS(5, CARD_ECI2600_3, "NG料Z", AXIS.MT_TYPE.STEP, AXIS.ENC_TYPE.NO, GPIO.IO_STA.OUT_ON);

        //左光箱
        //public static AXIS AXIS_BOX_L_Y_FAF = new AXIS(0, CARD_ECI2400_1, "左光箱远焦Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_BOX_L_Y_NAF = new AXIS(1, CARD_ECI2400_1, "左光箱近焦Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_BOX_L_Z_NAF = new AXIS(2, CARD_ECI2400_1, "左光箱近焦Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_BOX_L_Z_DUST = new AXIS(3, CARD_ECI2400_1, "左光箱污坏点Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);

        //右光箱
        //public static AXIS AXIS_BOX_R_Y_FAF = new AXIS(0, CARD_ECI2400_2, "右光箱远焦Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_BOX_R_Y_NAF = new AXIS(1, CARD_ECI2400_2, "右光箱近焦Y", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_BOX_R_Z_NAF = new AXIS(2, CARD_ECI2400_2, "右光箱近焦Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);
        //public static AXIS AXIS_BOX_R_Z_DUST = new AXIS(3, CARD_ECI2400_2, "右光箱污坏点Z", AXIS.MT_TYPE.SEVER, AXIS.ENC_TYPE.YES, GPIO.IO_STA.OUT_ON);

        //工站
        public static List<AXIS> AxList_bullet = new List<AXIS> { AXIS_bullet_back };

        public static List<AXIS> AxList_ALL = new List<AXIS> { AXIS_BACK_X, AXIS_BACK_Y, AXIS_BACK_Z,AXIS_BACK_A, AXIS_FEED_X, AXIS_FEED_Y,AXIS_FEED_A, AXIS_FEED_Z,
            AXIS_GET_X , AXIS_GET_Y, AXIS_GET_Z,AXIS_GET_A ,AXIS_bullet_back,AXIS_bullet_feed,AXIS_bullet_move};
        public static List<AXIS> AxList_WS_BACK = new List<AXIS> { AXIS_BACK_X, AXIS_BACK_Y, AXIS_BACK_Z, AXIS_BACK_A };
        public static List<AXIS> AxList_WS_FEED = new List<AXIS> { AXIS_FEED_X, AXIS_FEED_Y, AXIS_FEED_Z, AXIS_FEED_A };
        public static List<AXIS> AxList_WS_GET = new List<AXIS> { AXIS_GET_X, AXIS_GET_Y, AXIS_GET_Z, AXIS_GET_A };
        public static List<AXIS> AxList_WS_GET_XY = new List<AXIS> { AXIS_GET_X, AXIS_GET_Y, null, null };
        //光箱

        // public static List<AXIS> AxList_BOX_LEFT = new List<AXIS> { AXIS_BOX_L_Y_FAF, AXIS_BOX_L_Y_NAF, AXIS_BOX_L_Z_NAF, AXIS_BOX_L_Z_DUST };
        // public static List<AXIS> AxList_BOX_RIGHT = new List<AXIS> { AXIS_BOX_R_Y_FAF, AXIS_BOX_R_Y_NAF, AXIS_BOX_R_Z_NAF, AXIS_BOX_R_Z_DUST };

        ////上料
        //public static List<AXIS> AxList_UL = new List<AXIS> { AXIS_UL_X, AXIS_UL_Y, AXIS_UL_Z, AXIS_UL_U1, AXIS_UL_U2, AXIS_UL_FD_X, AXIS_UL_FD_Z };
        ////下料
        //public static List<AXIS> AxList_DL = new List<AXIS> { AXIS_DL_Y, AXIS_DL_Z, AXIS_DL_OK_X, AXIS_DL_OK_Z, AXIS_DL_NG_X, AXIS_DL_NG_Z };

        #endregion
        #region 位置

        public static POS pos_get_plate_star = new POS(AXIS_GET_X, AXIS_GET_Y, null, null, "取料盘起点", 1, (ushort)WS_ID.get);
        public static POS pos_get_plate_row = new POS(AXIS_GET_X, AXIS_GET_Y, null, null, "取料盘行点", 2, (ushort)WS_ID.get);
        public static POS pos_get_plate_line = new POS(AXIS_GET_X, AXIS_GET_Y, null, null, "取料盘列点", 3, (ushort)WS_ID.get);
        
        public static List<POS> pos_tray_get = new List<POS> { pos_get_plate_star, pos_get_plate_row, pos_get_plate_line };
        public static POS pos_get_photo_L = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "取料左拍照位", 3, (ushort)WS_ID.get);
        public static POS pos_get_photo_R = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "取料右拍照位", 8, (ushort)WS_ID.get);
        public static POS pos_get_put_L = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "左放料位", 4, (ushort)WS_ID.get);
        public static POS pos_get_to_put_L = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "缓左放料位", 5, (ushort)WS_ID.get);
        public static POS pos_get_to_put_R = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "缓右放料位", 6, (ushort)WS_ID.get);
        public static POS pos_get_put_R = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "右放料位", 8, (ushort)WS_ID.get);
        public static POS pos_get_safe = new POS(AXIS_GET_X, AXIS_GET_Y, null, AXIS_GET_A, "取料安全位位", 7, (ushort)WS_ID.get);
        public static POS pos_get_z_dwn = new POS(null, null, AXIS_GET_Z, null, "取料z下降位", 8, (ushort)WS_ID.get);
        public static POS pos_get_z_up = new POS(null, null, AXIS_GET_Z, null, "取料z抬升位", 9, (ushort)WS_ID.get);

        public static List<POS> pos_list_get = new List<POS> {
                pos_get_photo_R, pos_get_photo_L, pos_get_put_L, pos_get_to_put_L, pos_get_to_put_R,pos_get_put_R,
                   pos_get_safe,pos_get_z_dwn, pos_get_z_up };
        public static POS pos_back_plate_star = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "收料ok料盘起点", 0, (ushort)WS_ID.back);
        public static POS pos_back_plate_row = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "收料ok料盘行终点", 1, (ushort)WS_ID.back);
        public static POS pos_back_plate_line = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "收料ok料盘列终点", 2, (ushort)WS_ID.back);
        public static POS pos_back_NG_star = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "开图NG料盘起点", 0, (ushort)WS_ID.back);
        public static POS pos_back_NG_row = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "开图NG料盘行终点", 1, (ushort)WS_ID.back);
        public static POS pos_back_NG_line = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "开图NG料盘列终点", 2, (ushort)WS_ID.back);
        public static List<POS> pos_tray_bk_ng = new List<POS> { pos_back_NG_star, pos_back_NG_row, pos_back_NG_line };
        public static List<POS> pos_tray_bk_ok = new List<POS> { pos_back_plate_star, pos_back_plate_row, pos_back_plate_line };

        public static POS pos_back_AANG_star = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "AANG料盘起点", 0, (ushort)WS_ID.back);
        public static POS pos_back_AANG_row = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "AANG料盘行终点", 1, (ushort)WS_ID.back);
        public static POS pos_back_AANG_line = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "AANG料盘列终点", 2, (ushort)WS_ID.back);
        public static List<POS> pos_tray_bk_AANG = new List<POS> { pos_back_AANG_star, pos_back_AANG_row, pos_back_AANG_line };

        public static POS pos_back_safe = new POS(AXIS_BACK_X, AXIS_BACK_Y, AXIS_BACK_Z, AXIS_BACK_A, "取料安全位位", 3, (ushort)WS_ID.back);
        public static POS pos_back_L = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, AXIS_BACK_A, "收料左位", 4, (ushort)WS_ID.back);
        public static POS pos_back_R = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, AXIS_BACK_A, "收料右位", 5, (ushort)WS_ID.back);
        public static POS pos_back_z_dwn = new POS(null, null, AXIS_BACK_Z, null, " 收料Z下降位", 6, (ushort)WS_ID.back);
        public static POS pos_back_z_up = new POS(null, null, AXIS_BACK_Z, null, "取料Z上升位", 7, (ushort)WS_ID.back);
        public static POS pos_back_to_L = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, AXIS_BACK_A, "收料缓冲左位", 8, (ushort)WS_ID.back);
        public static POS pos_back_to_R = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, AXIS_BACK_A, "收料缓冲右位", 9, (ushort)WS_ID.back);
        public static POS pos_back_ng_plate_star = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "收料NG料盘起点", 8, (ushort)WS_ID.back);
        public static POS pos_back_ng_plate_row = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "收料NG料盘行终点", 9, (ushort)WS_ID.back);
        public static POS pos_back_ng_plate_line = new POS(AXIS_BACK_X, AXIS_BACK_Y, null, null, "收料NG料盘列终点", 10, (ushort)WS_ID.back);
        public static List<POS> pos_list_back = new List<POS> {pos_back_safe,pos_back_L,pos_back_R,pos_back_z_dwn,pos_back_z_up,pos_back_to_L,pos_back_to_R,
         };

        public static POS pos_feed_left_get = new POS(AXIS_FEED_X, AXIS_FEED_Y, null, null, "上料左爪取料位", 0, (ushort)WS_ID.feed);
        public static POS pos_feed_left_feed = new POS(AXIS_FEED_X, AXIS_FEED_Y, null, null, "上料左爪上料位", 1, (ushort)WS_ID.feed);
        public static POS pos_feed_right_back = new POS(AXIS_FEED_X, AXIS_FEED_Y, null, null, "上料右爪放回位", 2, (ushort)WS_ID.feed);
        public static POS pos_feed_z_dwn = new POS(null, null, AXIS_FEED_Z, null, "上料升高位", 3, (ushort)WS_ID.feed);
        public static POS pos_feed_z_up = new POS(null, null, AXIS_FEED_Z, null, "上料下降位", 4, (ushort)WS_ID.feed);
        public static POS pos_feed_safe = new POS(AXIS_FEED_X, AXIS_FEED_Y, null, AXIS_FEED_A, "上料安全位", 5, (ushort)WS_ID.feed);
        public static POS pos_feed_right_get = new POS(AXIS_FEED_X, AXIS_FEED_Y, AXIS_FEED_Z, AXIS_FEED_A, "上料右爪取料位", 6, (ushort)WS_ID.feed);

        public static POS pos_feed_photo = new POS(AXIS_FEED_X, AXIS_FEED_Y, AXIS_FEED_Z, AXIS_FEED_A, "上料拍照位", 7, (ushort)WS_ID.feed);
        public static List<POS> pos_list_feed = new List<POS> { pos_feed_left_get, pos_feed_left_feed, pos_feed_right_back,
            pos_feed_z_dwn, pos_feed_z_up ,pos_feed_safe,pos_feed_right_get,pos_feed_photo};

        public static POS pos_bullet_back_boxIN = new POS(null, null, AXIS_bullet_back, null, "弹收向下料盒进料位", 0, (ushort)WS_ID.bull_back);
        public static POS pos_bullet_back_boxOUT = new POS(null, null, AXIS_bullet_back, null, "弹收向上顶升出盒位", 1, (ushort)WS_ID.bull_back);
        public static POS pos_bullet_back_check_low = new POS(null, null, AXIS_bullet_back, null, "弹收检测最低点", 2, (ushort)WS_ID.bull_back);
        public static POS pos_bullet_back_check_top = new POS(null, null, AXIS_bullet_back, null, "弹收检测最高点", 3, (ushort)WS_ID.bull_back);
        public static POS pos_bullet_back_safe = new POS(null, null, AXIS_bullet_back, null, "弹收安全位", 4, (ushort)WS_ID.bull_back);
        public static List<POS> pos_list_bull_back = new List<POS> { pos_bullet_back_boxIN, pos_bullet_back_boxOUT, pos_bullet_back_check_low, pos_bullet_back_check_top, pos_bullet_back_safe };

        public static POS pos_bullet_feed_boxOUT = new POS(null, null, AXIS_bullet_feed, null, "上弹出料盒位置", 0, (ushort)WS_ID.bull_feed);
        public static POS pos_bullet_feed_safe = new POS(null, null, AXIS_bullet_feed, null, "上弹安全位", 1, (ushort)WS_ID.bull_feed);
        public static POS pos_bullet_feed_get_box = new POS(null, null, AXIS_bullet_feed, null, "上弹接空料盒位", 2, (ushort)WS_ID.bull_feed);
        public static POS pos_bullet_feed_check_low = new POS(null, null, AXIS_bullet_feed, null, "上弹检测最低点", 3, (ushort)WS_ID.bull_feed);
        public static POS pos_bullet_feed_check_top = new POS(null, null, AXIS_bullet_feed, null, "上弹检测最高点", 4, (ushort)WS_ID.bull_feed);
        public static POS pos_bullet_feed_boxIN = new POS(null, null, AXIS_bullet_feed, null, "上弹检测备料盒位置", 5, (ushort)WS_ID.bull_feed);
        public static List<POS> pos_list_bull_feed = new List<POS> { pos_bullet_feed_boxOUT, pos_bullet_feed_safe, pos_bullet_feed_boxIN, pos_bullet_feed_get_box, pos_bullet_feed_check_low, pos_bullet_feed_check_top };

        public static POS pos_bullet_move_wait = new POS(AXIS_bullet_move, null, null, null, "移弹缓冲位", 0, (ushort)WS_ID.bull_mov);
        public static POS pos_bullet_move_safe = new POS(AXIS_bullet_move, null, null, null, "移弹安全位", 1, (ushort)WS_ID.bull_mov);
        public static POS pos_bullet_move_get_tray = new POS(AXIS_bullet_move, null, null, null, "移弹取盘位", 2, (ushort)WS_ID.bull_mov);
        public static POS pos_bullet_move_put = new POS(AXIS_bullet_move, null, null, null, "移弹放盘位", 3, (ushort)WS_ID.bull_mov);
        public static POS pos_bullet_move_center = new POS(AXIS_bullet_move, null, null, null, "移弹中间位", 4, (ushort)WS_ID.bull_mov);
        public static List<POS> pos_list_bull_move = new List<POS> { pos_bullet_move_wait, pos_bullet_move_safe, pos_bullet_move_get_tray, pos_bullet_move_put, pos_bullet_move_center };

        #endregion
        #region 安全监测
        public static void SetSafeFunc()
        {
            //foreach (AXIS ax in AxisListMain)
            //{
            //    ax.ChkSafeSen = ChkSafeSen;
            //    ax.ChkSafePos = ChkSafePos;
            //}
        }

        public static EM_RES ChkSafePos(int id, double targe_pos = double.MaxValue)
        {
            EM_RES ret;

            //安全保护
            ret = ChkSafeSen(id);

            //if (AXIS_Z.id == id) return EM_RES.OK;

            //Z在原点且坐标接近0
            //if ((!AXIS_Z.isORG || (AXIS_Z.home_status != AXIS.HOME_STA.OK && Math.Abs(AXIS_Z.fcmd_pos) > 1)))
            //{
            //    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "Z不在原点，禁止移动载台!");
            //    return CONST.RES_MOVE_PROTECT;
            //}
            //if (!AXIS_Z.isORG && (AXIS_Z.home_status != AXIS.HOME_STA.OK && Math.Abs(AXIS_Z.fcmd_pos) > (TD.dt_pos_up + 0.1)))
            //{
            //    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "Z不在原点，禁止移动载台!");
            //    return EM_RES.MOVE_PROTECT;
            //}

            ////FDH在上位
            //if ((GPIO_IN_FDH_L_U.isOFF && GPIO_IN_FDH_L_L.isOFF) || (GPIO_IN_FDH_R_U.isOFF && GPIO_IN_FDH_R_R.isOFF))
            //{
            //    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "搬料未抬起，禁止移动载台!");
            //    return EM_RES.MOVE_PROTECT;
            //}

            ////R限制
            //if (AXIS_R.id == id && Math.Abs(AXIS_R.fenc_pos - targe_pos) > 3)
            //{
            //    if (AXIS_Y.fenc_pos > (AXIS_Y.slp - 50))
            //    {
            //        double[] pos = { -80, -100, -260, -290 };
            //        for (int n = 0; n < pos.Length; n++)
            //        {
            //            if (Math.Min(AXIS_R.fenc_pos, targe_pos) < pos[n] && pos[n] < Math.Max(AXIS_R.fenc_pos, targe_pos))
            //                return EM_RES.MOVE_PROTECT;
            //        }
            //    }
            //}

            return EM_RES.OK;
        }

        public static EM_RES ChkSafeSen(int id = 0, double target_pos = double.MaxValue)
        {
            //if (GPIO_IN_FR_DOOR.isOFF) return EM_RES.SAFE_PROTECT;

            //if (GPIO_IN_BK_DOOR_L.isOFF)
            //{
            //    GPIO_IN_BK_DOOR_L.AssertOFF();
            //    return EM_RES.SAFE_PROTECT;
            //}
            //if (GPIO_IN_BK_DOOR_R.isOFF)
            //{
            //    GPIO_IN_BK_DOOR_R.AssertOFF();
            //    return EM_RES.SAFE_PROTECT;
            //}

            //if (GPIO_IN_EMG.isOFF) return EM_RES.EMG;
            return EM_RES.OK;
        }
        public static bool isSafeSen
        {
            get
            {
                if (ChkSafeSen() == EM_RES.OK) return true;
                return false;
            }
        }
        #endregion
        #region 板卡初始化
        /// <summary>
        /// 轴卡初始化
        /// </summary>
        public static bool bCardInit
        {
            get
            {
                foreach (CARD card in CardList)
                {
                    if (!card.isReady) return false;
                }
                return true;
            }
        }
        public static EM_RES card_Init(String filename)
        {
            if (bCardInit) return EM_RES.OK;
            EM_RES res = EM_RES.OK;
            bool bok = true;
            foreach (CARD card in CardList)
            {
                if (!card.isReady)
                {
                    res = card.Init();
                    if (res != EM_RES.OK)
                        bok = false;
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            if (bok)
                return EM_RES.OK;
            else
                return EM_RES.ERR;

        }
        #endregion
        #region 检查/重连
        /// <summary>
        /// 检查所有板卡是否在线，否在重连
        /// </summary>
        /// <returns></returns>
        public static EM_RES ChkAndReConnect(string filename = "")
        {
            EM_RES res = EM_RES.OK;
            bool bok = true;
            foreach (CARD card in CardList)
            {
                res = card.ChkOnline(filename);
                if (res != EM_RES.OK) bok = false;
            }
            if (bok) return EM_RES.OK;
            else return EM_RES.ERR;
        }
        #endregion
        #region 关闭控制卡
        public static EM_RES Close()
        {
            EM_RES res = EM_RES.OK;
            bool bok = true;
            foreach (CARD card in CardList)
            {
                res = card.Close();
                if (res != EM_RES.OK) bok = false;
            }
            if (bok) return EM_RES.OK;
            else return EM_RES.ERR;
        }
        #endregion
        #region 设置轴到工作速度
        public static EM_RES SetAllAxToWorkSpd(double persent = 1.0)
        {
            EM_RES ret = EM_RES.OK;
            foreach (CARD card in CardList)
            {
                if (card.AxList == null) continue;
                foreach (AXIS ax in card.AxList)
                {
                    ret = ax.SetToWorkSpd(persent);
                    if (ret != EM_RES.OK) return ret;
                }
            }
            return ret;
        }
        public static EM_RES SetAllAxToManualSpd()
        {
            EM_RES ret = EM_RES.OK;
            foreach (CARD card in CardList)
            {
                if (card.AxList == null) continue;
                foreach (AXIS ax in card.AxList)
                {
                    ret = ax.SetToManualHighSpd();
                    if (ret != EM_RES.OK) return ret;
                }
            }
            return ret;
        }
        //public static EM_RES SetMainAxToCaliSpd()
        //{
        //    EM_RES ret = EM_RES.OK;
        //    foreach (AXIS ax in AxisListMain)
        //    {
        //        ret = ax.SetSpeed(ax.spd_start, ax.spd_work / 3, ax.spd_stop, 1, 1);
        //        if (ret != EM_RES.OK) return ret;
        //    }
        //    return ret;
        //}
        #endregion
        #region 所有轴停止
        public static void AllAxStop(List<AXIS> list_ax = null)
        {
            if (list_ax == null)
            {
                foreach (CARD card in CardList)
                {
                    if (card.AxList == null) continue;
                    card.AllCardStop();
                }
            }
            else
            {
                foreach (AXIS ax in list_ax)
                {
                    ax.Stop();
                }
            }

        }
        public static bool AllAxSvrOn()
        {
            bool all_on = true;
            foreach (CARD card in CardList)
            {
                if (card.AxList == null) continue;
                foreach (AXIS ax in card.AxList)
                {
                    if (ax.mt_type != AXIS.MT_TYPE.VIRTUAL && !ax.isSVRON)
                    {
                        all_on = false;
                        ax.SVRON = true;
                    }
                }
            }
            return all_on;
        }
        #endregion
        #region 先升Z再定位
        public static EM_RES ZupMove(ref bool bquit, ref AXIS ax_x, double xpos, int time_out_ms = 10000, bool bdoevent = false)
        {
            AXIS ax_null = null;
            return ZupMove(ref bquit, ref ax_x, xpos, ref ax_null, 0, ref ax_null, 0, ref ax_null, 0, time_out_ms, bdoevent);
        }
        public static EM_RES ZupMove(ref bool bquit, ref AXIS ax_x, double xpos, ref AXIS ax_y, double ypos, int time_out_ms = 10000, bool bdoevent = false)
        {
            AXIS ax_null = null;
            return ZupMove(ref bquit, ref ax_x, xpos, ref ax_y, ypos, ref ax_null, 0, ref ax_null, 0, time_out_ms, bdoevent);
        }
        public static EM_RES ZupMove(ref bool bquit, ref AXIS ax_x, double xpos, ref AXIS ax_y, double ypos, ref AXIS ax_z, double zpos, int time_out_ms = 10000, bool bdoevent = false)
        {
            AXIS ax_null = null;
            return ZupMove(ref bquit, ref ax_x, xpos, ref ax_y, ypos, ref ax_z, zpos, ref ax_null, 0, time_out_ms, bdoevent);
        }
        public static EM_RES ZupMove(ref bool bquit, ref AXIS ax_x, double xpos, ref AXIS ax_y, double ypos, ref AXIS ax_z, double zpos, ref AXIS ax_r, double rpos, int time_out_ms = 10000, bool bdoevent = false)
        {
            //bool bz_up = false;

            //if (ax_x != null && ax_x.id != AXIS_Z.id)
            //{
            //    bz_up = true;
            //}

            //if (ax_y != null && ax_y.id != AXIS_Z.id)
            //{
            //    bz_up = true;
            //}

            //if (ax_z != null && ax_z.id != AXIS_Z.id)
            //{
            //    bz_up = true;
            //}

            //if (ax_r != null && ax_r.id != AXIS_Z.id)
            //{
            //    bz_up = true;
            //}

            EM_RES ret = EM_RES.OK;
            //if (bz_up)
            //{
            //    ret = Move(ref bquit, ref AXIS_Z, 0, time_out_ms, bdoevent);
            //    if (ret != EM_RES.OK) return ret;
            //}

            ret = Move(ref bquit, ref ax_x, xpos, ref ax_y, ypos, ref ax_z, zpos, ref ax_r, rpos, time_out_ms, bdoevent);
            return ret;
        }
        #endregion
        #region 定位
        public static EM_RES Move(ref bool bquit, ref AXIS ax_x, double xpos, int time_out_ms = 10000, bool bdoevent = false)
        {
            AXIS ax_null = null;
            return Move(ref bquit, ref ax_x, xpos, ref ax_null, 0, ref ax_null, 0, ref ax_null, 0, time_out_ms, bdoevent);
        }
        public static EM_RES Move(ref bool bquit, ref AXIS ax_x, double xpos, ref AXIS ax_y, double ypos, int time_out_ms = 10000, bool bdoevent = false)
        {
            AXIS ax_null = null;
            return Move(ref bquit, ref ax_x, xpos, ref ax_y, ypos, ref ax_null, 0, ref ax_null, 0, time_out_ms, bdoevent);
        }
        public static EM_RES Move(ref bool bquit, ref AXIS ax_x, double xpos, ref AXIS ax_y, double ypos, ref AXIS ax_z, double zpos, int time_out_ms = 10000, bool bdoevent = false)
        {
            AXIS ax_null = null;
            return Move(ref bquit, ref ax_x, xpos, ref ax_y, ypos, ref ax_z, zpos, ref ax_null, 0, time_out_ms, bdoevent);
        }
        public static EM_RES Move(ref bool bquit, ref AXIS ax_x, double xpos, ref AXIS ax_y, double ypos, ref AXIS ax_z, double zpos, ref AXIS ax_r, double rpos, int time_out_ms = 10000, bool bdoevent = false)
        {
            EM_RES ret = EM_RES.OK;
            //start move
            if (ax_x != null)
            {
                ret = ax_x.MoveTo(ref bquit, xpos);
                if (ret != EM_RES.OK)
                    goto MOVE_END;
            }
            if (ax_y != null)
            {
                ret = ax_y.MoveTo(ref bquit, ypos);
                if (ret != EM_RES.OK) goto MOVE_END;
            }
            if (ax_z != null)
            {
                ret = ax_z.MoveTo(ref bquit, zpos);
                if (ret != EM_RES.OK) goto MOVE_END;
            }
            if (ax_r != null)
            {
                ret = ax_r.MoveTo(ref bquit, rpos);
                if (ret != EM_RES.OK) goto MOVE_END;
            }

            //wait
            if (ax_x != null)
            {
                ret = ax_x.WaitForMoveDone(ref bquit, xpos, time_out_ms, bdoevent);
                if (ret != EM_RES.OK) goto MOVE_END;
            }
            if (ax_y != null)
            {
                ret = ax_y.WaitForMoveDone(ref bquit, ypos, time_out_ms, bdoevent);
                if (ret != EM_RES.OK) goto MOVE_END;
            }
            if (ax_z != null)
            {
                ret = ax_z.WaitForMoveDone(ref bquit, zpos, time_out_ms, bdoevent);
                if (ret != EM_RES.OK) goto MOVE_END;
            }
            if (ax_r != null)
            {
                ret = ax_r.WaitForMoveDone(ref bquit, rpos, time_out_ms, bdoevent);
                if (ret != EM_RES.OK) goto MOVE_END;
            }

            return EM_RES.OK;

        MOVE_END:
            if (ax_x != null) ax_x.Stop();
            if (ax_y != null) ax_y.Stop();
            if (ax_z != null) ax_z.Stop();
            if (ax_r != null) ax_r.Stop();
            return ret;
        }
        public static EM_RES PosToByStr(ref bool bquit, List<POS> pos_list, String pos_disc)
        {
            int i = 0;
            EM_RES ret = EM_RES.OK;
            foreach (POS m_pos in pos_list)
            {
                if (m_pos.disc == pos_disc)
                {
                    ret = m_pos.MoveTo(ref bquit);
                    i++;
                }
            }
            if (i > 1)
            {
                return EM_RES.ERR;
            }
            else
                return ret;

        }
        public static EM_RES PosTo(ref bool bquit, params POS[] PosList)
        {

            EM_RES ret = EM_RES.OK;
            DialogResult res = DialogResult.OK;
            if (bquit) return EM_RES.QUIT;
            if (VAR.gsys_set.status != EM_SYS_STA.RUN)
                return EM_RES.ERR;
            foreach (POS m_pos in PosList)
            {
                if (m_pos == null)
                    return EM_RES.ERR;
                if (bquit) return EM_RES.QUIT;
                if (VAR.gsys_set.status != EM_SYS_STA.RUN)
                    return EM_RES.ERR;
                ret = m_pos.MoveTo(ref bquit);
                if (ret != EM_RES.OK)
                {
                    res = Action.WarningShow(m_pos.disc + "到位异常,是否停止？");
                    if (res == DialogResult.Cancel)
                    {
                        //warn_msg = cyl_mov_up.io_out.disc + "";
                        return EM_RES.OK;
                    }
                }

            }
            return ret;
        }
        public static EM_RES PosInit()
        {
            try
            {
                bool ret = true;
                List<List<POS>> AllPos = new List<List<POS>> { pos_list_back, pos_list_bull_back, pos_list_bull_move, pos_list_feed, pos_list_get };
                foreach (List<POS> mlist in AllPos)
                {
                    foreach (POS e in mlist)
                    {
                        ret = e.ReadUpdatePos();
                        if (!ret)
                            return EM_RES.ERR;

                    }


                }
                return EM_RES.OK;

            }
            catch
            {
                VAR.ErrMsg("位置加载异常");
                return EM_RES.ERR;
            }

        }
        #endregion
        #region 蜂鸣器
        public static EM_RES Beeper(int tmr)
        {
            Task beep = new Task(() =>
            {
                if (tmr > 0)
                {
                    //EM_RES ret = GPIO_OUT_ALM_BEEPER.SetOn();
                    //if (ret == EM_RES.OK)
                    //{
                    //    Thread.Sleep(tmr);
                    //    ret = GPIO_OUT_ALM_BEEPER.SetOff();
                    //}
                }
            }
                );
            beep.Start();
            return EM_RES.OK;
        }
        #endregion
        #region 轴复位
        public static EM_RES AxisHome(ref bool bquit, params AXIS[] axs)
        {
            //home task start
            foreach (AXIS ax in axs)
            {
                if (ax != null) ax.HomeTask(10000);
            }

            //wait end
            bool bok = true;
            while (true)
            {
                bok = true;
                foreach (AXIS ax in axs)
                {
                    if (ax != null && !ax.HomeTaskisEnd)
                        bok = false;
                }
                if (bok)
                    break;
                Application.DoEvents();
                Thread.Sleep(10);
                //quit
                if (bquit)
                {
                    foreach (AXIS ax in axs)
                    {
                        if (ax != null && !ax.HomeTaskisEnd)
                            ax.Stop();
                    }
                    return EM_RES.ERR;
                }
            }

            //check result
            bok = true;
            foreach (AXIS ax in axs)
            {
                if (ax != null && ax.home_status != AXIS.HOME_STA.OK)
                    bok = false;
            }
            if (bok == false)
            {
                foreach (AXIS ax in axs)
                {
                    if (ax != null && !ax.HomeTaskisEnd)
                        ax.Stop();
                }
                return EM_RES.ERR;
            }

            return EM_RES.OK;
        }
        public static void AxisHomeQuit(params AXIS[] axs)
        {
            foreach (AXIS ax in axs)
            {
                if (ax != null)
                {
                    ax.bhomequit = true;
                    ax.Stop();
                }
            }
        }
        public static void AxisHomeQuit(List<AXIS> axs)
        {
            foreach (AXIS ax in axs)
            {
                if (ax != null)
                {
                    ax.bhomequit = true;
                    ax.Stop();
                }
            }
        }
        #endregion
        #region 归位或避让
        public static EM_RES gotopos(bool breset = true, bool brun = false)
        {
            EM_RES ret = EM_RES.OK;

            ////检查是否需要移动
            //bool bneedmove = false;
            //if (breset && ((Math.Abs(MT.AXIS_X.fenc_pos) > 0.05))) bneedmove = true;
            //if (breset && ((Math.Abs(MT.AXIS_Y.fenc_pos) > 0.05))) bneedmove = true;
            //if (breset && ((Math.Abs(MT.AXIS_R.fenc_pos) > 0.05))) bneedmove = true;
            //if (!bneedmove) return CONST.RES_OK;

            ////safe check
            //ret = ChkSafeSen();
            //if (ret != CONST.RES_OK) return ret;

            ////正在进出料，不能移动
            //if (!brun && FDH.binuse)
            //{
            //    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "出料中gotopos定位");
            //    Thread.Sleep(10);
            //    if (FDH.binuse) return CONST.RES_MOVE_PROTECT;
            //}

            ////运行中，不能移动
            //if (COM.MounterGetRunStatus() == CONST.SYS_STATUS_RUN)
            //{
            //    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "运行中gotopos定位");
            //    return CONST.RES_ERR;
            //}

            //if (brun || VAR.gsys_set.status == CONST.SYS_STATUS_STANDBY || VAR.gsys_set.status == CONST.SYS_STATUS_PAUSE)
            //{
            //    VAR.gsys_set.bquit = false;
            //    ret = MT.ChkSafeSen();
            //    if (ret != CONST.RES_OK) return ret;

            //    if (VAR.gsys_set.status == CONST.SYS_STATUS_STANDBY) VAR.sys_inf.Set(CONST.EM_ALM_STA.NOR_GREEN, "就绪", -1, true);
            //    else if (VAR.gsys_set.status == CONST.SYS_STATUS_PAUSE) VAR.sys_inf.Set(CONST.EM_ALM_STA.NOR_GREEN, "暂停", -1, true);

            //    //set to workspd
            //    foreach (AXIS ax in AxisListExceptFd) ax.SetToWorkSpd();

            //    try
            //    {
            //        bgotopos = true;
            //        if (breset)
            //        {
            //            ret = ZupMove(ref VAR.gsys_set.bquit, ref AXIS_X, 0, ref AXIS_Y, 0, ref AXIS_R, 0, 10000, true);
            //            if (ret != CONST.RES_OK) return CONST.RES_ERR;
            //        }
            //        else
            //        {
            //            ret = ZupMove(ref VAR.gsys_set.bquit, ref AXIS_X, FDH.st_pos_ready.x, ref AXIS_Y, COM.MTER1.pos_br_y, ref AXIS_R, 0, 10000, true);
            //            if (ret != CONST.RES_OK) return CONST.RES_ERR;
            //        }
            //    }
            //    finally
            //    {
            //        bgotopos = false;
            //    }
            //}
            //else
            //{
            //    if (breset) VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "非待机状态禁止复位！");
            //    else VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "非待机状态禁止定位避让位!");
            //    return CONST.RES_ERR;
            //}
            return ret;
        }
        #endregion
        #region 执行带警告
        public static EM_RES OutCYL(ref bool bquit, bool ison, params Cylinder[] cyls)
        {
            EM_RES ret = EM_RES.OK;
            DialogResult res = DialogResult.OK;
            if (bquit) return EM_RES.QUIT;
            if (VAR.gsys_set.status != EM_SYS_STA.RUN)
                return EM_RES.ERR;
            //home task start
            foreach (Cylinder cyl in cyls)
            {
                if (cyl == null)
                    return EM_RES.ERR;
                if (bquit) return EM_RES.QUIT;
                if (VAR.gsys_set.status != EM_SYS_STA.RUN)
                    return EM_RES.ERR;
                if (ison)
                {
                    if (cyl.io_sen_on != null)
                    {
                        if (cyl.io_sen_on.Status == GPIO.IO_STA.IN_ON)
                        {
                            ret = EM_RES.OK;
                        }
                        else
                            ret = cyl.SetOn(ref bquit, 1000);
                    }
                    else
                        ret = cyl.SetOn(ref bquit, 1000);
                }

                else
                {
                    if (cyl.io_sen_off != null)
                    {
                        if (cyl.io_sen_off.Status == GPIO.IO_STA.IN_OFF)
                        {
                            ret = EM_RES.OK;
                        }
                        else
                            ret = cyl.SetOff(ref bquit, 1000);
                    }
                    else
                        ret = cyl.SetOff(ref bquit, 1000);
                }
                if (ret != EM_RES.OK)
                {
                    res = Action.WarningShow(cyl.io_out.disc + "到位异常,是否停止？");
                    if (res == DialogResult.Cancel)
                    {
                        //warn_msg = cyl_mov_up.io_out.disc + "";
                        return EM_RES.OK;
                    }
                }

            }
            return ret;
        }

        public static EM_RES SetCYL(ref Cylinder cyl, bool isON = true, EM_ALM_STA warntype = EM_ALM_STA.WAR_YELLOW, bool bwait = true, string str = "")
        {

            EM_RES ret = EM_RES.OK;
            DialogResult res = DialogResult.OK;
            if (cyl == null) return EM_RES.ERR;
            if (isON)
                ret = cyl.SetOn(ref VAR.gsys_set.bquit, bwait ? 3000 : 0);
            else
                ret = cyl.SetOff(ref VAR.gsys_set.bquit, bwait ? 3000 : 0);
            if (!Action.isReady) return EM_RES.QUIT;
            if (ret == EM_RES.ERR)
            {
                ret = Action.WarnShow(warntype, str + cyl.io_out.disc);
                if (ret == EM_RES.ERR)
                    VAR.gsys_set.status = EM_SYS_STA.ERR;
            }
            return ret;

        }
        public static EM_RES SetCYL(ref Cylinder cyl, ref string msg, bool isON = true, bool bwait = true)
        {

            EM_RES ret = EM_RES.OK;
            if (cyl == null) return EM_RES.ERR;
            if (isON)
                ret = cyl.SetOn(ref VAR.gsys_set.bquit, bwait ? 3000 : 0);
            else
                ret = cyl.SetOff(ref VAR.gsys_set.bquit, bwait ? 3000 : 0);
            if (!Action.isReady) return EM_RES.QUIT;
            if (ret == EM_RES.ERR)
                msg = cyl.io_out.disc;
            return ret;

        }
        public static EM_RES SetVCM(ref Cylinder cyl, bool isON = true, EM_ALM_STA warntype = EM_ALM_STA.WAR_YELLOW, bool bwait = false, string str = "")
        {
            EM_RES ret = SetCYL(ref cyl, isON, warntype, bwait, str);
            return ret;
        }
        public static EM_RES PosTo(ref POS pos, EM_ALM_STA warntype = EM_ALM_STA.WAR_YELLOW, bool bwait = true, string str = "")
        {

            EM_RES ret = EM_RES.OK;
            DialogResult res = DialogResult.OK;
            if (pos == null) return EM_RES.ERR;
            if (!Action.isReady) return EM_RES.QUIT;
            ret = pos.MoveTo(ref VAR.gsys_set.bquit, bwait);
            if (ret == EM_RES.ERR)
            {
                ret = Action.WarnShow(warntype, str + pos.disc);
                if (ret == EM_RES.ERR)
                    VAR.gsys_set.status = EM_SYS_STA.ERR;
                return EM_RES.ERR;
            }
            return ret;
        }
        public static EM_RES CkSen(ref GPIO IO, bool isON = true, EM_ALM_STA warntype = EM_ALM_STA.WAR_YELLOW, bool bwait = false, string str = "")
        {

            EM_RES ret = EM_RES.OK;
            GPIO.IO_STA sta;
            DialogResult res = DialogResult.OK;
            if (!Action.isReady) return EM_RES.QUIT;
            if (IO == null) return EM_RES.ERR;
            if (IO.dir != GPIO.IO_DIR.IN) return EM_RES.ERR;
            if (bwait)
            {
                if (isON)
                    sta = GPIO.IO_STA.IN_ON;
                else
                    sta = GPIO.IO_STA.IN_OFF;
                ret = IO.WaitForSta(ref VAR.gsys_set.bquit, sta, 5000);
                if (!Action.isReady) return EM_RES.QUIT;
                if (ret == EM_RES.OK) return EM_RES.OK;
            }
            if (IO.isON != isON)
            {
                ret = Action.WarnShow(warntype, str + IO.disc);
                if (ret == EM_RES.ERR)
                    VAR.gsys_set.status = EM_SYS_STA.ERR;
                return EM_RES.ERR;

            }
            return EM_RES.OK;
        }
        #endregion
    }
    #endregion

    #region 工站

    public class WS
    {
        //测试相关
        public List<MCInf.TestResult> list_res = new List<MCInf.TestResult>();

        public List<List<MCInf.TestResult>> list_test = new List<List<MCInf.TestResult>>();
        //工站编号
        public int num;
        public string disc
        {
            get
            {
                return string.Format("工站{0}", num + 1);
            }
        }
        //位置编号
        public int pos_idx;
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOWN,
            [Description("就绪")]
            REDAY,
            [Description("上料中")]
            UPLOAD,
            [Description("下料中")]
            DOWNLOAD,
            [Description("测试中")]
            TEST,
            [Description("复位中")]
            HOME,
            [Description("联机中")]
            LINK,
            [Description("错误")]
            ERR,
        }
        public EM_STA status = EM_STA.UNKNOWN;

        //位置设

        //测试模组行列
        public int cm_row = 2;
        public int cm_col = 8;
        public int cm_per_box = 2;

        //硬件相关
        public AXIS axis_x = null;
        public AXIS axis_y = null;
        public AXIS axis_z = null;
        public AXIS axis_a = null;
        public List<Cylinder> list_CYL = new List<Cylinder>();
        public List<Cylinder> list_VACUM = null;
        public List<POS> list_pos = null;
        public void WS_init(int num, AXIS axis_x, AXIS axis_y, AXIS axis_z, AXIS axis_a, List<Cylinder> list_CYL, List<Cylinder> list_VACUM, List<POS> list_pos = null, int cm_row = 2, int cm_col = 8, int cm_per_box = 2)
        {
            this.num = num;
            this.pos_idx = num;
            this.cm_col = cm_col;
            this.cm_row = cm_row;
            this.cm_per_box = cm_per_box;
            this.axis_x = axis_x;
            this.axis_y = axis_y;
            this.axis_z = axis_z;
            this.axis_a = axis_a;
            this.list_CYL = list_CYL;
            this.list_VACUM = list_VACUM;
            this.list_pos = list_pos;
            //for debug, 测试信息
            Random rdm = new Random();
            list_test.Clear();
            for (int m = 0; m < 10; m++)
            {
                list_res = new List<MCInf.TestResult>();
                list_res.Clear();
                for (int n = 0; n < cm_col * cm_row; n++)
                {
                    MCInf.TestResult res = new MCInf.TestResult();
                    res.TestFunc = "TEST" + m;
                    res.StationNum = num;
                    res.TextBoxNum = num << 8 + n;
                    res.NGCode = rdm.Next(0, 5);
                    Thread.Sleep(1);
                    res.CT = rdm.Next(0, 10);
                    list_res.Add(res);
                }
                list_test.Add(list_res);
            }
        }
        public WS(int num, AXIS axis_x, AXIS axis_y, AXIS axis_z, List<Cylinder> list_CYL, List<Cylinder> list_VACUM, int cm_row = 2, int cm_col = 8, int cm_per_box = 2)
        {
            AXIS axis_a = null;
            WS_init(num, axis_x, axis_y, axis_z, axis_a, list_CYL, list_VACUM, list_pos, cm_row, cm_col, cm_per_box);
        }
        public WS(int num, AXIS axis_x, AXIS axis_y, AXIS axis_z, AXIS axis_a, List<Cylinder> list_CYL, List<Cylinder> list_VACUM, List<POS> list_pos = null, int cm_row = 2, int cm_col = 8, int cm_per_box = 2)
        {

            WS_init(num, axis_x, axis_y, axis_z, axis_a, list_CYL, list_VACUM, list_pos, cm_row, cm_col, cm_per_box);
        }




        /// <summary>
        /// 回零
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>

        /// <summary>
        /// 旋转到上料位置
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        public EM_RES TurnToFeed(ref bool bquit)
        {
            return EM_RES.OK;
        }
        /// <summary>
        /// 旋转到测试位置
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        public EM_RES TurnToTest(ref bool bquit)
        {
            return EM_RES.OK;
        }


        /// <summary>
        /// </summary>
        /// <param name="bquit"></param>
        /// <param name="dly">等待超时时间ms</param>
        /// <param name="bdoevent"></param>
        /// <param name="cld_fr_idx">指定前排位置，保留</param>
        /// <param name="cld_bk_idx">指定后排位置，保留<param>
        #region 测试通信
        #endregion
    }
    #endregion
    #region 弹夹上下料
    public static class WsBuFD
    {
        public static TrayBox tbox_get = COM.traybox_get;
        static AXIS ax_Z = COM.traybox_get.ax_z;
        static Cylinder cyl_lock_up = MT.CYL_bullet_up;//上料盒二挂起气缸
        static Cylinder cyl_tray_out = MT.CYL_bullet_feed_plate;//上料拉盘气缸
        static GPIO sen_box_in = MT.CKPOS_bull_feed_box;
        static GPIO sen_tray_in = MT.CKPOS_bull_feed_plate;
        static GPIO sen_box_out = MT.CKPOS_bull_belt_come;
        static GPIO belet_move = MT.GPIO_OUT_belet;
        static GPIO sen_plate_at_get = MT.CKPOS_MOVE_get_plate;
        static POS PosBoxOUT = MT.pos_bullet_feed_boxOUT;
        static POS PosSafe = MT.pos_bullet_feed_safe;
        static POS PosGetBox = MT.pos_bullet_feed_get_box;
        static POS PosTrayLow = MT.pos_bullet_feed_check_low;
        static POS PosTrayTop = MT.pos_bullet_feed_check_top;
        static POS PosBoxIn = MT.pos_bullet_feed_boxIN;
        static POS PosTrayStar = MT. pos_get_plate_star;
        static POS PosTrayRow = MT. pos_get_plate_row;
        static POS PosTrayLine = MT. pos_get_plate_line;
        static Mact ActTrayOut;
        static Mact ActTrayReady;
        static Mact ActBoxIn;//进料盒动作
        static Mact ActBoxOut;//出料盒动作
        static Mact ActBoxChange;//换料盒动作
        public static EM_STA status = EM_STA.UNKNOW;
        static  ST_XYZA pos_tray_star, pos_tray_row, pos_tray_line;
       public static string disc = "弹夹出料仓工站";
        public static bool bOK
        {
            get
            {
                if (!mbOK) return false;
                if (TaskRun != null && !TaskRun.IsCompleted) return false;
                return true;
            }
            set
            {
                if (TaskRun == null || TaskRun.IsCompleted)
                    mbOK = value;
            }
        }
        static bool mbOK;

      public  static string GetStaString
        {
            get
            {
                return status.GetDescription();
            }
        }
        /// <summary>
        /// 运行条件
        /// </summary>
        public static bool isReady
        {
            get
            {
                
                if (VAR.gsys_set.bquit) return false;
                if (VAR.gsys_set.bpause) return false;
                if (!(WSGet.TaskRun == null || WSGet.TaskRun.IsCompleted))
                {
                    status = EM_STA.WAIT;
                    return false;
                }
                if (!(WsTrayMove.TaskRun == null || WsTrayMove.TaskRun.IsCompleted))
                {
                    status = EM_STA.WAIT;
                    return false;
                }
                //if (isErr) return false;
                //if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                return true;
            }
        }
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("盘拉出上料")]
            TRAYOUT,
            [Description("准备料盘")]
            TRAYREADY,
            [Description("上料盒")]
            BOXIN,
            [Description("出料盒")]
            BOXOUT,
            [Description("换料盒二")]
            BOXCHG,
            [Description("错误")]
            ERR,
            [Description("安全位")]
            SAFE,
            [Description("等待")]
            STANDBY,
            [Description("等待上料")]
            WAIT


        }
        /// <summary>
        /// 初始化动作
        /// </summary>
        static void AllAct()
        {
            if (ActTrayOut == null)
                ActTrayOut = new Mact(trayOut);
            if (ActTrayReady == null)
                ActTrayReady = new Mact(trayReady);
            if (ActBoxIn == null)
                ActBoxIn = new Mact(boxIN);
            if (ActBoxOut == null)
                ActBoxOut = new Mact(boxOUT);
            if (ActBoxChange == null)
                ActBoxChange = new Mact(boxChange);
        }
        public static EM_RES DoAct(Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;


                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;

                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.STANDBY;

                }

                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }

        }

        public static EM_RES trayReady(ref bool bquit)
        {
            if (!isReady) return EM_RES.QUIT;
            if (tbox_get == null) return EM_RES.QUIT;
            EM_RES ret = tbox_get.TrayReadyOUT(ref  bquit);
            return ret;
        }
        public static EM_RES trayOut(ref bool bquit)
        {
            ////gy-临时修改
            COM.product.TrayGet.NewTray(true, Product.EM_CM_RES.OK);
            WSGet.tray_now = COM.product.TrayGet;
            return EM_RES.OK;

            if (!isReady) return EM_RES.QUIT;
            if (tbox_get == null) return EM_RES.QUIT;
            if (sen_plate_at_get.AssertON())
            { 
                
                return EM_RES.ERR;
            }
            EM_RES ret = EM_RES.OK;
            ret = tbox_get.TrayOut(ref  bquit);
            if (ret == EM_RES.OK)
            {
                if (sen_plate_at_get.AssertON())
                {
                    COM.product.TrayGet.NewTray(false, Product.EM_CM_RES.UNTEST);
                    WSGet.tray_now = COM.product.TrayGet;
                    return EM_RES.OK;
                }
                else
                    return EM_RES.ERR;
            }

            return ret;
        }
        public static EM_RES boxIN(ref bool bquit)
        {
            try
            {
                EM_RES ret = EM_RES.OK;
                status = EM_STA.BOXIN;
                if (sen_box_in.AssertON())
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "有料盘在", disc, GetStaString));
                    return EM_RES.ERR;
                }

                ret = cyl_lock_up.SetOff(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;


                //接料盒位高位一个也有传感反应
                ret = MT.pos_bullet_feed_get_box.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                while (isReady)
                {
                    Action.MsgShow("请上料盒");
                    if (sen_box_in.AssertON())
                        break;
                }

                //check box在
                if (sen_box_in.AssertOFF())
                {
                    return EM_RES.ERR;
                }
                //到到检测备料盒位置，检测有两个，再挂起，否则一直不挂
                ret = MT.pos_bullet_feed_boxIN.MoveTo(ref bquit, true); //安全
                if (ret != EM_RES.OK) return ret;
                if (sen_box_in.AssertON())
                {
                    ret = cyl_lock_up.SetOn(ref bquit, 3000);
                    if (ret != EM_RES.OK) return ret;
                }

                COM.traybox_get.NewBox(Product.EM_CM_RES.OK);
                COM.traybox_get.SetSta(TrayBox.EM_STA.FULL);
                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                return EM_RES.ERR;
            }

        }
        public static EM_RES boxOUT(ref bool bquit)
        {

            EM_RES ret = EM_RES.OK;
            //下降到出料位
            status = EM_STA.BOXOUT;
            ret = PosBoxOUT.MoveTo(ref bquit, true); //安全
            if (ret != EM_RES.OK) return ret;
            //检测到达出料位

            //皮带运动
            ret = belet_move.SetOn();
            if (ret != EM_RES.OK) return ret;
            ret = sen_box_out.WaitON(ref bquit);
            if (ret != EM_RES.OK) { belet_move.SetOff(); return ret; }
            ret = sen_box_out.WaitOFF(ref bquit);
            if (ret != EM_RES.OK) { belet_move.SetOff(); return ret; }
            //皮带停止
            ret = belet_move.SetOff();
            if (ret != EM_RES.OK) return ret;

            //如果成功启动下一动作
            COM.traybox_get.SetSta(TrayBox.EM_STA.EMPTY);//所有料盘满料
            COM.traybox_get.NewBox(Product.EM_CM_RES.NONE);//所有模组ok
            return ret;
        }
        public static EM_RES boxChange(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (sen_box_in.AssertOFF())
            {
                return EM_RES.ERR;
            }
            //到获取料盒位置

            ret = PosGetBox.MoveTo(ref bquit, true); //安全
            if (ret != EM_RES.OK) return ret;
            //关锁
            ret = cyl_lock_up.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            //下降到料盘位
            ret = PosTrayTop.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) return ret;
            COM.traybox_get.NewBox(Product.EM_CM_RES.OK);
            COM.traybox_get.SetSta(UI.TrayBox.EM_STA.FULL);
            return ret;
        }
        /// <summary>
        /// 委托轴安全防护
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool bsafe;//取消防护true
        public static EM_RES axZ_Safe(int id, double pos = 0)
        {
            EM_RES ret = EM_RES.OK;
            if (bsafe) return ret;
            if (sen_box_in.AssertOFF())
            {
                ret = cyl_lock_up.SetOff(ref VAR.gsys_set.bquit, 3000);
                if (ret != EM_RES.OK) return ret;
            }
            ret = cyl_tray_out.SetOn(ref VAR.gsys_set.bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            return ret;
        }

        public static void act_run()
        {

            if (!isReady) goto MSTOP;
            AllAct();
            EM_RES ret = EM_RES.OK;
            if (sen_plate_at_get.AssertON())
                return;
            ret = DoAct(ActTrayReady);
            if (ret != EM_RES.OK && isReady)
            {
                if (tbox_get.status == TrayBox.EM_STA.EMPTY)
                {
                    ret = DoAct(ActBoxOut);
                    if (ret != EM_RES.OK) goto MSTOP;
                    ret = MT.pos_bullet_feed_boxIN.MoveTo(ref VAR.gsys_set.bquit, true); //安全
                    if (ret != EM_RES.OK) goto MSTOP;
                    if (sen_box_in.AssertON())
                    {
                        ret = DoAct(ActBoxChange);
                        if (ret != EM_RES.OK) goto MSTOP;

                    }
                    else
                    {
                        ret = DoAct(ActBoxIn);
                        if (ret != EM_RES.OK) goto MSTOP;

                    }
                    ret = DoAct(ActTrayReady);
                    if (ret != EM_RES.OK) goto MSTOP;

                }
                else
                    if (tbox_get.status == TrayBox.EM_STA.NOBOX)
                    {
                        //进料盒命令
                        ret = DoAct(ActBoxIn);
                        if (ret != EM_RES.OK) goto MSTOP;
                        ret = DoAct(ActTrayReady);
                        if (ret != EM_RES.OK) goto MSTOP;

                    }
                    else
                    {
                        goto MSTOP;
                    }

            }

            ret = DoAct(ActTrayOut);
            if (ret != EM_RES.OK) goto MSTOP;
            mbOK = true;
        MSTOP:
            Stop();

        }
        public static Task TaskRun;
        public static EM_RES home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            status = EM_STA.HOME;
            if (!(TaskRun == null || TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 工站工作中，回原失败!", disc));
                return EM_RES.ERR;
            }
            if (sen_box_in.AssertON())
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 检测到有料盒，回原失败!", disc));
                return EM_RES.ERR;
            }
            res = cyl_lock_up.SetOff(1000);
            if (res != EM_RES.OK)
                return res;
            res = MT.AxisHome(ref bquit, ax_Z);
            status = EM_STA.STANDBY;
            return res;
        }
        public static void Stop()
        {
            ax_Z.bhomequit = true;
            ax_Z.Stop();
        }
        public static void task_run()
        {
            if (TaskRun == null || (TaskRun != null && TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建出料仓线程");
                if (TaskRun != null)
                    TaskRun.Dispose();
                TaskRun = new Task(act_run);
                TaskRun.Start();

            }
        }
    }
    public static class WsBuBK
    {
        public static TrayBox tbox_get = COM.traybox_back;
        static Product.Tray tray_out = COM.traybox_back.tray_cur;
        static AXIS ax_Z = COM.traybox_back.ax_z;
        public static string warn_msg = "";
        public static bool isShow = true;//弹窗
        static Cylinder cyl_tray_in = MT.CYL_bullet_back_plate;//拉盘气缸
        static GPIO sen_box_in = MT.CKPOS_bull_belt_topos;//皮带进料处感应料盒来
        static GPIO sen_tray_in = MT.CKPOS_bull_back_plate;
        static GPIO sen_box_out = MT.CKPOS_bull_back_box;//料盒外出感应
        static GPIO belet_move = MT.GPIO_OUT_belet;
        static POS PosBoxOUT = MT.pos_bullet_back_boxOUT;
        static POS PosSafe = MT.pos_bullet_back_safe;
        static POS PosTrayLow = MT.pos_bullet_back_check_low;
        static POS PosTrayTop = MT.pos_bullet_back_check_top;
        static POS PosBoxIn = MT.pos_bullet_back_boxIN;
        static Mact ActTrayIn;
        static Mact ActTrayReady;
        static Mact ActBoxIn;//进料盒动作
        static Mact ActBoxOut;//出料盒动作
        static Mact ActBoxChange;//换料盒动作
        static Mact ActToSafe;
        public static bool isErr;
        public static EM_STA status = EM_STA.UNKNOW;
        public static string disc = "料盘移栽工站";
        public static string GetStaString
        {
            get
            {
                return status.GetDescription();
            }
        }
        /// <summary>
        /// 运行条件
        /// </summary>
        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                if (VAR.gsys_set.bpause) return false;
                if (!(WSBack.TaskRun == null || WSBack.TaskRun.IsCompleted))
                    return false;
                if (!(WsTrayMove.TaskRun == null || WsTrayMove.TaskRun.IsCompleted))
                    return false;
                return true;
            }
        }
        public static bool mbOK;
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("盘拉进")]
            TRAYIN,
            [Description("准备收料位")]
            TRAYREADY,
            [Description("进料盒")]
            BOXIN,
            [Description("出料盒")]
            BOXOUT,
            [Description("错误")]
            ERR,
            [Description("等待")]
            STANDBY,
            [Description("等待取料盒")]
            WAIT
        }
        /// <summary>
        /// 初始化动作
        /// </summary>
        static void AllAct()
        {

            if (ActTrayReady == null)
                ActTrayReady = new Mact(trayReady);
            if (ActBoxIn == null)
                ActBoxIn = new Mact(boxIN);
            if (ActBoxOut == null)
                ActBoxOut = new Mact(boxOUT);
            if (ActTrayIn == null)
                ActTrayIn = new Mact(trayIN);


        }
        static EM_RES DoAct(Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;
                AllAct();
                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;

                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.STANDBY;

                }

                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }

        }
        public static EM_RES trayReady(ref bool bquit)
        {
            if (isReady) return EM_RES.QUIT;
            if (WSBack.tbox_back == null) return EM_RES.QUIT;
            EM_RES ret = WSBack.tbox_back.TrayReadyIN(ref  bquit);
            return ret;
        }
        public static EM_RES trayIN(ref bool bquit)
        {
            status = EM_STA.TRAYIN;
            if (WSBack.tbox_back == null) return EM_RES.QUIT;
            EM_RES ret = WSBack.tbox_back.TrayIn(ref  bquit);
            return ret;
        }
        public static EM_RES boxIN(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            try
            {
                status = EM_STA.BOXIN;
                int i = 0;
                //到进料位
                res = PosBoxIn.MoveTo(ref bquit, true);
                if (res != EM_RES.OK) return res;
                //check box在
                res = belet_move.SetOn();
                if (res != EM_RES.OK) return res;
                while (sen_box_in.AssertOFF() && isReady)
                {
                    Thread.Sleep(20);
                    Application.DoEvents();
                    i++;
                    if (i > 1000)
                    {
                        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}等待超时", GetStaString));
                        res = belet_move.SetOff();
                        return EM_RES.ERR;
                    }
                }
                for (int j = 0; j < 100; j++)
                {
                    Thread.Sleep(30);
                    Application.DoEvents();
                    if (!isReady) break;
                }
                res = belet_move.SetOff();
                if (res != EM_RES.OK) return res;
                return res;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}-{1}未知错误", disc, GetStaString));
                return EM_RES.ERR;
            }

        }
        public static EM_RES boxOUT(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            try
            {
                status = EM_STA.BOXOUT;
                //到出料位
                res = PosBoxOUT.MoveTo(ref bquit, true);
                if (res != EM_RES.OK) return res;
                if (!sen_box_out.AssertON())
                {
                    VAR.ErrMsg(string.Format("{0}+{1}无料盒", disc, status.GetDescription()));
                    return EM_RES.ERR;
                }
                //检测到达出料位等待拿走
                int i = 0;
                while (sen_box_out.AssertON() && isReady)
                {
                    Thread.Sleep(30);
                    Application.DoEvents();
                    i++;
                    if (i > 1000)
                    {
                        VAR.ErrMsg(string.Format("{0}+{1}等待拿料盒超时", disc, status.GetDescription()));
                        return EM_RES.ERR;
                    }
                }
                return res;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}-{1}未知错误", disc, GetStaString));
                return EM_RES.ERR;
            }

        }
        public static void act_run()
        {
            try
            {
                if (!isReady) goto MSTOP;
                EM_RES ret = EM_RES.OK;
               

                ret = DoAct(ActTrayReady);
                if (ret != EM_RES.OK && isReady)
                {
                    if (WSBack.tbox_back.status == TrayBox.EM_STA.FULL)
                    {
                        ret = DoAct(ActBoxOut);
                        if (ret != EM_RES.OK) goto MSTOP;
                        ret = DoAct(ActBoxIn);
                        if (ret != EM_RES.OK) goto MSTOP;
                        ret = DoAct(ActTrayReady);
                        if (ret != EM_RES.OK) goto MSTOP;
                    }
                }
                ret = DoAct(ActTrayIn);
                if (ret != EM_RES.OK) goto MSTOP;
            MSTOP:
                Stop();
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}-{1}未知错误", disc, GetStaString));

            }

        }
        public static Task TaskRun;
        public static void task_run()
        {
            if (TaskRun == null || (TaskRun != null && TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建料仓线程");
                if (TaskRun != null)
                    TaskRun.Dispose();
                TaskRun = new Task(act_run);
                TaskRun.Start();
            }
        }
        public static EM_RES home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            if (!(TaskRun == null || TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 工站工作中，回原失败!", disc));
                return EM_RES.ERR;
            }
            if (sen_box_out.AssertON())
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 检测到有料盒，回原失败!", disc));
                return EM_RES.ERR;
            }
            res = MT.AxisHome(ref bquit, ax_Z);
            return res;
        }
        public static void Stop()
        {
            ax_Z.bhomequit = true;
            ax_Z.Stop();
        }
        public static EM_RES axZ_Safe(int id, double pos = 0)
        {
            EM_RES ret = EM_RES.OK;
            ret = cyl_tray_in.SetOff(ref VAR.gsys_set.bquit, 3000);
            return ret;
        }
    }
    public static class WsTrayMove
    {
       
        public static GPIO sen_tray_get = MT.CKPOS_MOVE_get_plate;
        public static GPIO sen_tray_store = MT.CKPOS_MOVE_middle_plate;
        public static GPIO sen_tray_back = MT.CKPOS_MOVE_back_plate;
        public static Cylinder vacu_move = MT.VACUM_move_plate;
        public static AXIS ax_Z = MT.AXIS_bullet_move;
        public static Mact GetTray;
        public static Mact PutTray;
        public static Mact ToSafe;
        public static EM_STA status = EM_STA.UNKNOW;
        public static bool bhome = false;
        public static string disc = "料盘移栽工站";
        public static string GetStaString
        {
            get
            {
                return status.GetDescription();
            }
        }
        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                if (VAR.gsys_set.bpause) return false;
                if (!(WSBack.TaskRun == null || WSBack.TaskRun.IsCompleted))
                {
                    status = EM_STA.BUSY;
                    return false;
                }
                if (!(WSGet.TaskRun == null || WSGet.TaskRun.IsCompleted))
                {
                    status = EM_STA.BUSY;
                    return false;
                }
                // if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                return true;
            }
        }
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("空盘上料")]
            PLACE,
            [Description("空盘存料")]
            PICK,
            [Description("错误")]
            ERR,
            [Description("安全位")]
            SAFE,
            [Description("等待")]
            STANDBY
        }
        static void AllAct()
        {
            if (GetTray == null)
                GetTray = new Mact(m_GET);
            if (PutTray == null)
                PutTray = new Mact(m_PUT);
            if (ToSafe == null)
                ToSafe = new Mact(m_SAFE);
        }
        public static EM_RES DoAct(ref Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;
                AllAct();
                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "执行", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;

                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.STANDBY;

                }

                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "系统异常", disc, GetStaString));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }

        }

        //下降放料
        static EM_RES m_down_put(ref bool bquit)
        {
            try
            {
                EM_RES ret = EM_RES.OK;
                //检测真空
                if (!vacu_move.io_sen_on.AssertON())
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "吸嘴无料", disc, GetStaString));
                    return EM_RES.ERR;

                }

                //气缸下降
                ret = MT.CYL_bullet_move_plate_up.SetOn(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = MT.VACUM_move_plate.SetOff(ref bquit);
                if (ret != EM_RES.OK) return ret;
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                //气缸上升
                ret = MT.CYL_bullet_move_plate_up.SetOff(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                //检测真空
                if (!vacu_move.io_sen_on.AssertOFF())
                    return EM_RES.ERR;
                else
                    return EM_RES.OK;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "未知错误", disc, GetStaString));
                return EM_RES.ERR;
            }
        }
        public static EM_RES ck_ax_safe(int id, double pos = 0)
        {
            EM_RES ret = EM_RES.OK;
            if (MT.CYL_bullet_move_plate_up == null) return EM_RES.ERR;
            if (MT.CYL_bullet_move_plate_up.io_sen_off.AssertON()) return EM_RES.OK;
            else
                ret = MT.CYL_bullet_move_plate_up.SetOff(ref VAR.gsys_set.bquit, 3000);
            return ret;

        }
        //移栽下降取料
        static EM_RES m_down_get(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            try
            {
                ret = MT.VACUM_move_plate.SetOn(ref bquit);
                if (ret != EM_RES.OK) return ret;
                //气缸下降
                ret = MT.CYL_bullet_move_plate_up.SetOn(ref bquit, 3000);
                //打开真空
                if (ret != EM_RES.OK) return ret;
                ret = MT.VACUM_move_plate.SetOn(ref bquit, 6000);
                if (ret != EM_RES.OK) return ret;
                //气缸升
                ret = MT.CYL_bullet_move_plate_up.SetOff(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                //检测真空
                if (MT.VACUM_move_plate.io_sen_on.AssertOFF())
                    return EM_RES.ERR;
                else
                    return EM_RES.OK;
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "未知错误", disc, GetStaString));
                return EM_RES.ERR;
            }
            finally
            {
                ret = MT.CYL_bullet_move_plate_up.SetOff(ref bquit, 3000);
            }

        }
        //移栽取料
        public static EM_RES m_GET(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            try
            {
                status = EM_STA.PICK;
                //检测料盘在位
                if (MT.CKPOS_MOVE_get_plate.isOFF) return EM_RES.OK;
                ret = MT.CYL_bullet_move_plate_up.SetOff(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                //轴运动                    

                ret = MT.pos_bullet_move_get_tray.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = m_down_get(ref bquit);
                if (ret != EM_RES.OK) return ret;
                //检测料盘不在位

                if (!MT.CKPOS_MOVE_get_plate.AssertON())
                    return EM_RES.ERR;
                ret = MT.pos_bullet_move_center.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = m_down_put(ref bquit);
                if (ret != EM_RES.OK) return ret;
                //检测料盘在位
                if (MT.CKPOS_MOVE_get_plate.AssertON())
                    return EM_RES.ERR;
                else

                    return EM_RES.OK;
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "未知错误", disc, GetStaString));
                return EM_RES.ERR;
            }



        }
        public static EM_RES m_PUT(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            try
            {
                //检测有无料盘            
                ret = MT.pos_bullet_move_center.MoveTo(ref bquit);
                if (ret != EM_RES.OK) return ret;
                //检测料盘在位                    
                if (!MT.CKPOS_MOVE_middle_plate.AssertON())
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "中间位无料盘请上料盘从新开始", disc, GetStaString));
                    return EM_RES.ERR;
                }
                ret = m_down_get(ref bquit);
                if (ret != EM_RES.OK) return ret;
                ret = MT.pos_bullet_move_put.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = m_down_put(ref bquit);
                if (ret != EM_RES.OK) return ret;
                //检测料盘在位
                if (!MT.CKPOS_MOVE_back_plate.AssertON())
                    return EM_RES.ERR;
                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "未知错误", disc, GetStaString));
                return EM_RES.ERR;

            }

        }
        //移栽安全位
        public static EM_RES m_SAFE(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;

            try
            {

                //轴运动       
                ret = MT.pos_bullet_move_safe.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                if (!ax_Z.isORG)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "移栽轴不在原点", disc, GetStaString));
                    return EM_RES.ERR;
                }
                else
                    return EM_RES.OK;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "未知错误", disc, GetStaString));
                return EM_RES.ERR;
            }
        }
        public static void act_run()
        {

            try
            {
                EM_RES ret = EM_RES.OK;
                if (!isReady)
                    return;

                if (sen_tray_get.AssertON())
                {
                    ret = DoAct(ref GetTray);
                    if (ret != EM_RES.OK)
                        goto MSTOP;
                    WSGet.tray_now = null;
                }
                if (!sen_tray_back.AssertON())
                {
                    if (sen_tray_store.AssertON())
                    {
                        ret = DoAct(ref PutTray);
                        if (ret != EM_RES.OK)
                            goto MSTOP;
                        WSBack.tray_now = null;
                       
                    }
                    else
                    {
                        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "中间无空盘", disc, GetStaString));
                        status = EM_STA.ERR;
                        goto MSTOP;
                    }
                }
                if (!ax_Z.isORG)
                {
                    ret = DoAct(ref ToSafe);
                    if (ret != EM_RES.OK) goto MSTOP;
                }
            MSTOP:

                Stop();
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "未知错误", disc, GetStaString));

            }


        }
        public static Task TaskRun;
        public static void task_run()
        {
            if (TaskRun == null || (TaskRun != null && TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建料仓线程");
                if (TaskRun != null)
                    TaskRun.Dispose();
                TaskRun = new Task(act_run);
                TaskRun.Start();

            }
        }
        public static EM_RES home(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (bquit) return EM_RES.QUIT;
            status = EM_STA.HOME;
            if (!((WSGet.TaskRun == null || WSGet.TaskRun.IsCompleted) &&
               (WSBack.TaskRun == null || WSBack.TaskRun.IsCompleted)))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 取料和收料工站未退出!", disc));
                return EM_RES.ERR;
            }
            if (!WSGet.ax_x.isORG)
            {
                ret = WSGet.ax_z.MoveTo(ref bquit, 99999, 5000);
                if (ret != EM_RES.OK) return ret;
                ret = WSGet.ax_x.MoveTo(ref bquit, 99999, 5000);
                if (ret != EM_RES.OK) return ret;
                if (!WSGet.ax_x.isELP)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} q取料工站到正限位异常!", disc));
                    return EM_RES.ERR;
                }

            }
            if (!WSBack.ax_x.isORG)
            {
                ret = WSBack.ax_z.MoveTo(ref bquit, 99999, 5000);
                if (ret != EM_RES.OK) return ret;
                ret = WSBack.ax_x.MoveTo(ref bquit, 99999, 5000);
                if (ret != EM_RES.OK) return ret;
                if (!WSBack.ax_x.isELP)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 取料工站到正限位异常!", disc));
                    return EM_RES.ERR;
                }

            }
            ret = MT.AxisHome(ref bquit, ax_Z);
            if (ret != EM_RES.OK) return ret;
            else
                status = EM_STA.STANDBY;
            return ret;
        }
        public static void Stop()
        {
            ax_Z.bhomequit = true;
            ax_Z.Stop();
        }

    }
    #endregion

    #region 光箱
    public class LightBox
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string disc;
        #region 位置
        /// <summary>
        /// 近焦与转台的安全距离
        /// </summary>
        public float pos_safe_y_naf;
        /// <summary>
        /// 近焦与远焦的安全距离
        /// </summary>
        public float pos_safe_yy;
        /// <summary>
        /// 近焦位置
        /// </summary>
        public ST_YZ st_pos_naf;
        /// <summary>
        /// 远焦位置
        /// </summary>
        public ST_YZ st_pos_faf;
        /// <summary>
        /// 污坏点位置
        /// </summary>
        public ST_YZ st_pos_dust;
        /// <summary>
        /// 暗态位置
        /// </summary>
        public ST_YZ st_pos_dark;
        /// <summary>
        /// OTP位置
        /// </summary>
        public ST_YZ st_pos_otp;
        #endregion
        #region 硬件
        /// <summary>
        /// Y轴:近焦/增距镜/污坏点/暗态/
        /// </summary>
        public AXIS ax_y_naf = null;
        /// <summary>
        /// Y轴:远焦
        /// </summary>
        public AXIS ax_y_faf = null;
        /// <summary>
        /// Z轴(后):左：增距镜(下)/暗态(上)，右：增距镜(下)/污坏点(上)
        /// </summary>
        public AXIS ax_z_dust = null;
        /// <summary>
        /// Z轴(前):近焦光源
        /// </summary>
        public AXIS ax_z_naf = null;
        /// <summary>
        /// Z轴:OTP
        /// </summary>
        public AXIS ax_z_otp = null;
        #endregion
        #region 状态
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("远焦")]
            FAF,
            [Description("近焦")]
            NAF,
            [Description("污坏点")]
            DUST,
            [Description("暗态")]
            DARK,
            [Description("OTP")]
            OTP,
            [Description("错误")]
            ERR
        }
        public EM_STA status = EM_STA.UNKNOW;
        #endregion

        #region 初始化
        /// <summary>
        /// 光箱初始化
        /// </summary>
        /// <param name="ax_y_naf">Y轴:近焦/增距镜/污坏点/暗态/</param>
        /// <param name="ax_y_faf">Y轴:远焦</param>
        /// <param name="ax_z_dust">Z轴(后):左：增距镜(下)/暗态(上)，右：增距镜(下)/污坏点(上)</param>
        /// <param name="ax_z_naf">Z轴(前):近焦光源</param>
        /// <param name="ax_otp">Z轴:OTP</param>
        public LightBox(string disc, AXIS ax_y_naf = null, AXIS ax_y_faf = null, AXIS ax_z_dust = null, AXIS ax_z_naf = null, AXIS ax_z_otp = null)
        {
            this.disc = disc;
            this.ax_y_naf = ax_y_naf;
            this.ax_y_faf = ax_y_faf;
            this.ax_z_dust = ax_z_dust;
            this.ax_z_naf = ax_z_naf;
            this.ax_z_otp = ax_z_otp;
            status = EM_STA.UNKNOW;
        }
        #endregion

        //参数存储
        public EM_RES SaveCfg(string filename = "")
        {
            return EM_RES.OK;
        }
        public EM_RES LoadCfg(string filename = "")
        {
            return EM_RES.OK;
        }
        //切换状态
        /// <summary>
        /// 切换光箱状态
        /// </summary>
        /// <param name="sta">状态</param>
        /// <returns></returns>
        public EM_RES ChangeToSta(ref bool bquit, EM_STA sta)
        {
            if (status == EM_STA.UNKNOW || status != EM_STA.ERR || status == EM_STA.BUSY)
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} 状态异常，STA={1}，请先回零!", disc, Utility.GetDescription(status)));
                return EM_RES.ERR;
            }

            EM_RES res = EM_RES.OK;
            switch (sta)
            {
                case EM_STA.READY:
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} 归位...", disc));
                    status = EM_STA.BUSY;
                    try
                    {
                        //res = MT.Move(ref bquit, ref ax_y_faf, 0);
                        //if (res != EM_RES.OK) break;
                        //res = MT.Move(ref bquit, ref ax_y_naf, 0);
                        //if (res != EM_RES.OK) break;
                        //res = MT.Move(ref bquit, ref ax_z_naf, 0, ref ax_z_dust, 0);
                        //if (res != EM_RES.OK) break;
                    }
                    finally
                    {
                        if (res == EM_RES.OK) status = sta;
                        else status = EM_STA.ERR;
                    }
                    break;

                case EM_STA.FAF:
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} 远焦...", disc));
                    status = EM_STA.BUSY;
                    //res = MT.Move(ref bquit, ref ax_y_faf, st_pos_faf.y);
                    if (res == EM_RES.OK) status = sta;
                    else status = EM_STA.ERR;
                    break;
                case EM_STA.NAF:
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} 近焦...", disc));
                    status = EM_STA.BUSY;
                    //res = MT.Move(ref bquit, ref ax_y_naf, st_pos_naf.y,ref ax_z_naf, st_pos_naf.z);
                    break;
                case EM_STA.DARK:
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} 暗态...", disc));
                    status = EM_STA.BUSY;
                    //res = MT.Move(ref bquit, ref ax_y_naf, st_pos_dark.y, ref ax_z_dust, st_pos_dark.z);
                    if (res == EM_RES.OK) status = sta;
                    else status = EM_STA.ERR;
                    break;
                case EM_STA.DUST:
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} 污坏点...", disc));
                    status = EM_STA.BUSY;
                    //res = MT.Move(ref bquit, ref ax_y_naf, st_pos_dust.y, ref ax_z_dust, st_pos_dust.z);
                    if (res == EM_RES.OK) status = sta;
                    else status = EM_STA.ERR;
                    break;
                case EM_STA.OTP:
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, string.Format("{0} OTP...", disc));
                    status = EM_STA.BUSY;
                    //res = MT.Move(ref bquit, ref ax_z_otp, st_pos_otp.z);
                    if (res == EM_RES.OK) status = sta;
                    else status = EM_STA.ERR;
                    break;
            }
            return res;
        }

        //复位
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        public EM_RES Home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;

            if (status == EM_STA.HOME) return res;
            try
            {
                //stop first
                if (status == EM_STA.BUSY)
                {
                    //COM.LeftLightBox.Stop();
                    Thread.Sleep(500);
                }

                status = EM_STA.HOME;
                //先退出远焦
                res = MT.AxisHome(ref bquit, ax_y_faf);
                if (res != EM_RES.OK) return res;

                //再往外复位近焦
                res = MT.AxisHome(ref bquit, ax_y_naf);
                if (res != EM_RES.OK) return res;

                //然后Z复位
                res = MT.AxisHome(ref bquit, ax_z_dust, ax_z_naf);
                if (res != EM_RES.OK) return res;

                return EM_RES.OK;
            }
            finally
            {
                if (res == EM_RES.OK) status = EM_STA.READY;
                else status = EM_STA.UNKNOW;
            }
        }
        /// <summary>
        /// 停止轴运动，停止Home动作
        /// </summary>
        public void Stop()
        {
            ax_y_faf.bhomequit = true;
            ax_y_faf.Stop();

            ax_y_naf.bhomequit = true;
            ax_y_naf.Stop();

            ax_z_dust.bhomequit = true;
            ax_z_dust.Stop();

            ax_z_naf.bhomequit = true;
            ax_z_naf.Stop();
        }
    }
    #endregion
    #region 料仓
    public class TrayBox
    {
        //料盘       
        public List<EM_STA> list_sta = new List<EM_STA>();
        public Product.Tray tray_cur = null;
        int m_tray_cnt; //格数、盘数
        public POS pos_low;
        public POS pos_high;
        public POS pos_out;
        public POS pos_in;
       
        public int tray_cnt
        {
            get { return VAR.gsys_set.box_tray_cnt; }         
        }
        //当前格
        int m_tray_idx;
        public int tray_idx
        {
            get
            {

                for (int n = 0; n < tray_cnt; n++)
                {
                    //search down
                    if ((n < tray_cnt) && list_sta[n] != EM_STA.EMPTY)

                        return n;
                }
                return 99;
            }
            set
            {
                if (value < 0 || value > tray_cnt) m_tray_idx = 0;
                else m_tray_idx = value;
            }
        }
        public int out_id
        {
            get
            {

                for (int n = 0; n < tray_cnt; n++)
                {
                    //search down
                    if ((n < tray_cnt) && list_sta[n] != EM_STA.EMPTY)

                        return n;
                }
                return 0;
            }
            set
            {
                if (value < 0 || value > tray_cnt) value = 0;

            }
        }
        public int in_id
        {
            get
            {

                for (int n = 0; n < tray_cnt; n++)
                {
                    //search down
                    if ((n < tray_cnt) && list_sta[n] == EM_STA.EMPTY)

                        return n;
                }
                return 0;
            }
            set
            {
                if (value < 0 || value > tray_cnt) value = 0;

            }
        }
        string disc;
        //料仓参数

        //第一格位置
        public double pos_tray_top;
        //最后一格
        public double pos_tray_low;
        //当前格

        public bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                if (isErr) return false;
                if (VAR.gsys_set.bpause) return false;
                if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                return true;
            }
        }
        //每格高度
        public double tray_height
        {
            get { return Math.Abs((pos_tray_top - pos_tray_low) / (tray_cnt - 1)); }
        }
        //取放料脱离高度
        public double tray_feed_ofs_h;
        //安区X位置
        public double fd_safe_x;
        public EM_DIR direction;
        //硬件
        public AXIS ax_z;
        public AXIS ax_x;
        //料夹感应
        public GPIO in_box_sen;
        //料盘拉出到位感应
        public GPIO in_tray_sen;
        //料盘真空吸组件
        public Cylinder vacu_mov;
        //料盘进出气缸
        public Cylinder tray_out_in_cyl;
        //料盘运料抬升气缸
        public Cylinder tray_mov_up_cyl;
        //料盘真空吸out
        public GPIO out_tray_zk;
        //料盘真空吸感应
        public GPIO in_tray_zk;
        public string warn_msg;
        public bool isErr;
        public int boxID;
        //料仓的盘状态
        public EM_STA status;
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOWN,
            [Description("就绪")]
            STANBY,
            [Description("空仓")]
            EMPTY,
            [Description("满仓")]
            FULL,
            [Description("待测")]
            UNTEST,
            [Description("完成")]
            DONE,
            [Description("复位中")]
            HOME,
            [Description("无料仓")]
            NOBOX,
            [Description("错误")]
            ERR
        }
       
        public string GetStaStr
        {
            get
            {
                return status.GetDescription();
            }
        }
        public enum EM_DIR
        {
            [Description("只进")]
            ONLY_IN,
            [Description("只出")]
            ONLY_OUT,
            [Description("进/出")]
            IN_OUT,
        }

        //初始化
        public void TrayBox_init(string disc = "料仓", EM_DIR dir = EM_DIR.IN_OUT, int cnt = 10, AXIS ax_x = null, AXIS ax_z = null, GPIO in_box_sen = null, GPIO in_tray_sen = null, GPIO out_tray_zk = null, GPIO in_tray_zk = null)
        {
            this.direction = dir;
            this.disc = disc;
            this.m_tray_cnt = cnt;
            this.ax_x = ax_x;
            this.ax_z = ax_z;
            this.in_box_sen = in_box_sen;
            this.in_tray_sen = in_tray_sen;
            this.out_tray_zk = out_tray_zk;
            this.in_tray_zk = in_tray_zk;
            list_sta.Clear();
            //list_tray.Clear();
            Random rdm = new Random();
            for (int n = 0; n < cnt; n++)
            {
                Thread.Sleep(1);
                list_sta.Add((EM_STA)rdm.Next(0, 5));
                //Product.Tray tray = new Product.Tray(tray_row, tray_col, (Product.EM_CM_RES)rdm.Next(0, 5));
                //list_tray.Add(tray);
            }
        }
        //public TrayBox(string disc = "料仓", AXIS ax_x = null, EM_DIR dir = EM_DIR.IN_OUT, int cnt = 10, AXIS ax_z = null, GPIO in_box_sen = null, GPIO in_tray_sen = null, Cylinder tray_mouth=null)
        //{

        //    TrayBox_init(disc, dir, cnt, ax_x, ax_z, in_box_sen, in_tray_sen, tray_mouth.io_out, tray_mouth.io_sen_on);
        //}
        public TrayBox(string disc = "料仓", EM_DIR dir = EM_DIR.IN_OUT, int cnt = 10, AXIS ax_x = null, AXIS ax_z = null, GPIO in_box_sen = null, GPIO in_tray_sen = null, GPIO out_tray_zk = null, GPIO in_tray_zk = null)
        {
            TrayBox_init(disc, dir, cnt, ax_x, ax_z, in_box_sen, in_tray_sen, out_tray_zk, in_tray_zk);
        }
        public TrayBox(string disc, EM_DIR dir, Cylinder tray_out_in_cyl, int cnt = 10, AXIS ax_x = null, AXIS ax_z = null, GPIO in_box_sen = null, GPIO in_tray_sen = null, Cylinder tray_mov_up_cyl = null)
        {
            TrayBox_init(disc, dir, cnt, ax_x, ax_z, in_box_sen, in_tray_sen);
            this.tray_out_in_cyl = tray_out_in_cyl;
            this.tray_mov_up_cyl = tray_mov_up_cyl;
        }
        public TrayBox(string disc, ref POS pos_low, ref POS pos_high, int boxID = 1, EM_DIR dir = EM_DIR.ONLY_IN, Cylinder tray_out_in_cyl = null, int cnt = 10, AXIS ax_x = null, AXIS ax_z = null, 
             POS pos_out = null, POS pos_in = null, GPIO in_box_sen = null, GPIO in_tray_sen = null, Cylinder tray_mov_up_cyl = null)
        {
            TrayBox_init(disc, dir, cnt, ax_x, ax_z, in_box_sen, in_tray_sen);
            this.boxID = boxID;
            this.tray_out_in_cyl = tray_out_in_cyl;
            this.tray_mov_up_cyl = tray_mov_up_cyl;
            if (pos_high != null) pos_high.UpdatePos();
            if (pos_low != null) pos_low.UpdatePos();      
            if (pos_out != null) pos_out.UpdatePos();
            if (pos_in != null) pos_in.UpdatePos();       
            this.pos_low = pos_low;//-gy-1204-
            this.pos_high = pos_high;
            this.pos_out = pos_out;
            this.pos_in = pos_in;
            pos_tray_top = pos_high.pos_z;
            pos_tray_low = pos_low.pos_z;
            for (int n = 0; n < tray_cnt; n++)
            {

                if (direction == EM_DIR.ONLY_IN)

                    list_sta.Add(EM_STA.EMPTY);
                else
                    list_sta.Add(EM_STA.FULL);
            }

            //if (direction == EM_DIR.ONLY_IN)

            //    tray_cur = list_tray[out_id];
            //else
            //    tray_cur = list_tray[in_id];

        }
        public TrayBox(string disc, int boxID = 1, EM_DIR dir = EM_DIR.ONLY_IN, int cnt = 10)
        {
            this.direction = dir;
            this.disc = disc;
            this.m_tray_cnt = cnt;
            this.boxID = boxID;
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// 
        // EM_RES LoadCfg(string filename = "")
        //{
        //    if (filename.Length < 3)
        //        filename = Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\TrayBox.inf";
        //    if (!File.Exists(filename)) File.Create(filename);
        //    IniFile inf = new IniFile(filename);
        //    string str_section = "TRAY_BOX" + boxID;
        //  //  tray_cnt = inf.ReadInteger(str_section, "TRAY_CNT", tray_cnt);
        //    return EM_RES.OK;
        //}
        ///// <summary>
        ///// 保存参数
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <returns></returns>
        // EM_RES SaveCfg(string filename = "")
        //{
        //    if (filename.Length < 3)
        //        filename = Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\feedcfg.inf";
        //    if (!File.Exists(filename)) return EM_RES.PARA_ERR;

        //    IniFile inf = new IniFile(filename);

        //    string str_section = "TRAY_BOX" + boxID;
        //    inf.WriteInteger(str_section, "TRAY_CNT", tray_cnt);
        //    return EM_RES.OK;
        //}
        public bool isSafe
        {
            get
            {
                if (ax_x == null) return true;
                if (ax_x.status == AXIS.AX_STA.ALM || ax_x.status == AXIS.AX_STA.UNKOWN || ax_x.status == AXIS.AX_STA.HOMEING) return false;
                return true;
            }
        }

        public EM_RES NewBox(Product.EM_CM_RES cm_res)
        {
            list_sta.Clear();
            for (int n = 0; n < tray_cnt; n++)
            {
                list_sta.Add(EM_STA.FULL);
            }
            return EM_RES.OK;
        }

        /// <summary>
        /// 移动料仓到指定位置编号
        /// </summary>
        /// <param name="bquit"></param>
        /// <param name="idx">指定位置编号</param>
        /// <param name="btrayin">True：后续动作为TRAY盘入仓，定位自动降低ofs_z。</param>
        /// <returns></returns>
        public EM_RES BoxMoveToPosIdx(ref bool bquit, int idx = -1)
        {
            //bquit
            if (bquit) return EM_RES.QUIT;

            //current idx
            if (idx < 0) idx = tray_idx;
            //check idx
            if (idx >= tray_cnt)
                return EM_RES.PARA_OUTOFRANG;


            //calc pos
            //double pos = pos_tray_low + idx * tray_height - (btrayin ? tray_feed_ofs_h : 0);

            double pos = pos_tray_low + idx * tray_height;
            //  move
            if (ax_z != null)
            {
                EM_RES res = ax_z.MoveTo(ref bquit, pos, 10000, true);
                if (res != EM_RES.OK)
                    return res;

            }
            else
                return EM_RES.ERR;
            return EM_RES.OK;
        }
        public EM_RES SetSta(EM_STA sta)
        {
            for (int n = 0; n < list_sta.Count; n++)
            {
                list_sta[n] = sta;
            }
            return EM_RES.OK;
        }
        public EM_RES TrayOut(ref bool bquit, int idx = -2, EM_STA sta = EM_STA.UNTEST)
        {
            EM_RES ret;
            if (bquit) return EM_RES.QUIT;
            //ret = TrayReadyOUT(ref bquit, idx, sta);
            //if (ret != EM_RES.OK) goto END;
            ret = ax_z.MoveTo(ref bquit, ax_z.fcmd_pos - 4, 10000, true);
            if (ret != EM_RES.OK) goto END;
            //cyl_work
            if (tray_out_in_cyl == null)
            { ret = EM_RES.ERR; goto END; }

            ret = tray_out_in_cyl.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto END;
            Thread.Sleep(800);
            Application.DoEvents();
            //  下降8再拉出
            WsBuFD.bsafe = true;
            ret = ax_z.MoveTo(ref bquit, ax_z.fcmd_pos - 4, 10000, true);
            WsBuFD.bsafe = false;

            if (ret != EM_RES.OK)
                return ret;
            Thread.Sleep(200);
            Application.DoEvents();
            Thread.Sleep(200);
            Application.DoEvents();
            ret = tray_out_in_cyl.SetOn(ref bquit, 10000);
            if (ret != EM_RES.OK)
                return ret;
            Thread.Sleep(800);
            Application.DoEvents();

            Application.DoEvents();
            //tray_cur = list_tray[out_id];
            list_sta[out_id] = EM_STA.EMPTY;
        END:
            tray_out_in_cyl.SetOn(ref bquit, 3000);
            return ret;

        }
        public EM_RES TrayReadyOUT(ref bool bquit, int idx = -1, EM_STA sta = EM_STA.UNTEST)
        {
            try
            {
                EM_RES res = EM_RES.OK;
                if (bquit) return EM_RES.QUIT;
                //check idx

                if (idx < -1 || idx >= tray_cnt) return EM_RES.QUIT;
                if (idx == -1)
                    idx = out_id;
                res = BoxMoveToPosIdx(ref bquit, idx);
                if (res != EM_RES.OK) return res;
                if (in_tray_sen.isOFF)
                {
                    list_sta[idx] = EM_STA.EMPTY;//标记当前位置空盘
                }
                else
                    return EM_RES.OK;
                //search


                if (list_sta[idx] == EM_STA.EMPTY)
                {

                    for (int n = 1; n < tray_cnt; n++)
                    {
                        //search down
                        if ((idx + n < tray_cnt) && list_sta[idx + n] != EM_STA.EMPTY)
                        {
                            out_id = idx + n;
                            res = BoxMoveToPosIdx(ref bquit, tray_idx);
                            if (res != EM_RES.OK)
                                return res;
                            if (in_tray_sen.isOFF)
                            {
                                list_sta[idx + n] = EM_STA.EMPTY;
                            }
                            else
                                break;
                        }

                        //search up
                        if ((idx - n >= 0) && (idx - n) < tray_cnt && list_sta[idx - n] != EM_STA.EMPTY)
                        {

                            out_id = idx - n;
                            res = BoxMoveToPosIdx(ref bquit, tray_idx);
                            if (res != EM_RES.OK)
                                return res;
                            if (in_tray_sen.isOFF)
                            {
                                list_sta[idx - n] = EM_STA.EMPTY;
                            }
                            else
                                break;
                        }
                    }

                }
                //check_result
                if (!in_tray_sen.isOFF) return EM_RES.OK;
                //检测有料盒子
                res = pos_low.MoveTo(ref bquit, true);
                if (res != EM_RES.OK) return res;
                res = ax_z.MoveTo(ref bquit, ax_z.fcmd_pos - tray_height, 9000);
                if (res != EM_RES.OK) return res;
                if (in_tray_sen.isON) status = EM_STA.EMPTY;
                else status = EM_STA.NOBOX;
                return EM_RES.END;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--找料盘-" + "未知系统异常", disc));
                return EM_RES.ERR;
            }




        }
        public EM_RES TrayReadyIN(ref bool bquit, int idx = -2, EM_STA sta = EM_STA.UNTEST)
        {
            try
            {
                EM_RES res = EM_RES.OK;
                //check box
                if (bquit) return EM_RES.QUIT;
                if (list_sta[idx] == EM_STA.FULL) return EM_RES.QUIT;
                if (idx < 0 || idx >= list_sta.Count)
                    idx = in_id;
                res = BoxMoveToPosIdx(ref bquit, idx);
                if (res != EM_RES.OK) return res;
                if (in_tray_sen.isON)
                {
                    list_sta[idx] = EM_STA.FULL;//set statue
                }
                else
                    return EM_RES.OK;
                //check idx

                //search
                if (list_sta[idx] == EM_STA.FULL)
                {

                    for (int n = 0; n < list_sta.Count; n++)
                    {
                        //search down
                        if ((idx + n < list_sta.Count) && list_sta[idx + n] != EM_STA.FULL)
                        {
                            idx = idx + n;
                            res = BoxMoveToPosIdx(ref bquit, idx);
                            if (res != EM_RES.OK) return res;
                            if (in_tray_sen.isON)
                            {
                                list_sta[idx] = EM_STA.FULL;
                            }
                            else
                                break;
                        }

                        //search up
                        if ((idx - n >= 0) && list_sta[idx - n] != EM_STA.FULL)
                        {
                            idx = idx - n;
                            res = BoxMoveToPosIdx(ref bquit, idx);
                            if (res != EM_RES.OK) return res;
                            if (in_tray_sen.isON)
                            {
                                list_sta[idx] = EM_STA.FULL;
                            }
                            else
                                break;
                        }
                    }
                }
                //check_result
                if (in_tray_sen.isON)
                {
                    status = EM_STA.FULL;
                    return EM_RES.ERR;
                }
                else
                    return EM_RES.OK;
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--找料盘-" + "未知系统异常", disc));
                return EM_RES.ERR;
            }

        }
        public EM_RES TrayIn(ref bool bquit, int idx = -2, EM_STA sta = EM_STA.UNTEST)
        {
            try
            {

                EM_RES ret;
                if (bquit) return EM_RES.QUIT;
                if (isReady) return EM_RES.QUIT;
                ret = TrayReadyIN(ref bquit, idx, sta);
                if (ret != EM_RES.OK) return ret;
                //cyl_work
                if (tray_out_in_cyl != null)
                {
                    ret = MT.OutCYL(ref bquit, true, tray_out_in_cyl);
                    if (ret != EM_RES.OK) return ret;
                    ret = MT.OutCYL(ref bquit, false, tray_out_in_cyl);
                    if (ret != EM_RES.OK) return ret;

                }
                else
                    return EM_RES.ERR;
                //update status
                list_sta[idx] = EM_STA.FULL;
                tray_cur = null;
                //check status
                //ret = TrayReadyIN(ref bquit, idx, sta);
                //if (ret == EM_RES.OK)
                //{
                //    status = EM_STA.STANBY;
                //    return EM_RES.OK;
                //}
                //else
                //    status = EM_STA.FULL;


                return EM_RES.OK;
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--找料盘-" + "未知系统异常", disc));
                return EM_RES.ERR;
            }
        }


        /// <summary>
        /// 抬升到指定位置编号
        /// </summary>
        /// <param name="bquit"></param>
        /// <param name="idx">指定位置编号，-1为当前位置的上一位置</param>
        /// <returns></returns>
        public EM_RES Up(ref bool bquit)
        {
            return BoxMoveToPosIdx(ref bquit, tray_idx++);
        }
        public EM_RES Down(ref bool bquit)
        {
            return BoxMoveToPosIdx(ref bquit, tray_idx--);
        }

    }
    #endregion
    #region 取料
    public class WSGet
    {
       
        public static TrayBox tbox_get = COM.traybox_get;
        public static Product.Tray tray_now=new Product.Tray(9);
        
        public static AXIS ax_x = MT.AXIS_GET_X;
        public static AXIS ax_y = MT.AXIS_GET_Y;
        public static AXIS ax_z = MT.AXIS_GET_Z;
        public static AXIS ax_a = MT.AXIS_GET_A;
        static Cylinder cyl_z = MT.CYL_get_up;
        static Cylinder zk_z = MT.VACUM_get_mouth;
        static GPIO sen_plate_at_get = MT.CKPOS_MOVE_get_plate;
        static POS ps_sf = MT.pos_get_safe;
      public  static POS ps_pho_L = MT.pos_get_photo_L;
      public  static POS ps_pho_R = MT.pos_get_photo_R;
        static POS ps_put_L = MT.pos_get_put_L;
        static POS ps_put_R = MT.pos_get_put_R;
        static POS ps_put_to_L = MT.pos_get_to_put_L;
        static POS ps_put_to_R = MT.pos_get_to_put_R;
        static POS ps_zDown = MT.pos_get_z_dwn;
        static POS ps_zUP = MT.pos_get_z_up;

        static List<POS> ps_list = MT.pos_list_get;

        public static bool bOK
        {
            get 
            {
                if (bLPutOK && bRPutOK)
                    return true;
                else return false;
            }
            set 
            {
                if (value )
                {
                    bLPutOK = true;
                    bRPutOK = true;
                }
                else
                {
                    bLPutOK = false;
                    bRPutOK = false;
                }
            }
        }
        //单点取料次数 

        //连续取料失败次数要报警
        public static int modu_id;
        public static string disc = "取料工站-";
        //拍照选择左右
        public static bool bLPutOK;
        public static bool bRPutOK;
        static ST_XYZ Move_L;//左放料视觉偏移量
        static ST_XYZ Move_R;//左放料视觉偏移量
        //运动函数委托
        static Mact ActGet = null;
        static Mact ToPhoto = null;
        static Mact ToPut = null;
        static Mact ActPut = null;
        public static Task TaskRun = null;
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("拍照")]
            PHOTO,
            [Description("左放料")]
            PLACE,
            [Description("右放料")]
            PLACE_R,
            [Description("取料")]
            PICK,
            [Description("等待料盘")]
            WAIT,
            [Description("错误")]
            ERR
        }

        public static void AllAct()
        {
            if (ActGet == null)
                ActGet = new Mact(m_ActGet);
            if (ToPhoto == null)
                ToPhoto = new Mact(m_ToPhoto);
            if (ToPut == null)
                ToPut = new Mact(m_ToPut);
            if (ActPut == null)
                ActPut = new Mact(m_ActPut);
        }
        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                if (VAR.gsys_set.bpause) return false;
                if (!(WsBuFD.TaskRun == null || WsBuFD.TaskRun.IsCompleted))
                {
                    status = EM_STA.WAIT;
                    return false;
                }
                if (!(WsTrayMove.TaskRun == null || WsTrayMove.TaskRun.IsCompleted))
                {
                    status = EM_STA.WAIT;
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 轴安全
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static EM_RES ck_ax_safe(int id, double pos = 0)
        {
            bool bquit=false;
            int i = 0;
            double max = 9999;
            try
            {
                if (!ax_z.isELP)
                {
                    ax_z.MoveTo(ref bquit, max);
                    while (!ax_z.isELP)
                    {
                        i++;
                        Thread.Sleep(100);
                        Application.DoEvents();
                        if (i > 100)
                        {
                            VAR.ErrMsg(ax_z.disc + "到正限位异常");
                            return EM_RES.ERR;
                        }
                    }
                }

                return EM_RES.OK;
            }
            catch(Exception e)
            {
                VAR.ErrMsg(e.ToString());
                return EM_RES.ERR;
            }
           
           

        }
        public static string GetStaString
        {
            get
            {
                return status.GetDescription() + "过程";

            }
        }//获取状态翻译
        public static EM_RES DoAct(ref  Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;
                AllAct();
                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;

                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.READY;

                }
                return ret;
            }
            catch(Exception e)
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}-{}-" , disc, GetStaString,e.ToString()));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }

        }
        //需要保存，当前流程状态
        public static EM_STA status = EM_STA.UNKNOW;
        /// <summary>
        /// 取料料模块复位
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        /// 
        public static EM_RES Home(ref bool bquit)
        {
            //status = EM_STA.READY;
            //return EM_RES.OK;
            try
            {
                EM_RES res = EM_RES.OK;
                if (bquit)
                    return EM_RES.QUIT;
                status = EM_STA.HOME;
                //气缸回位
                res = cyl_z.SetOff(ref bquit, 1000);
                if (res != EM_RES.OK) return res;
                //先抬升
                if (!ax_z.isELP)
                {
                    ax_z.MoveTo(ref bquit, ax_z.fcmd_pos + 9999, 1000);
                    Thread.Sleep(20);
                    if (!ax_z.isELP)
                    {
                        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}到正限位异常!", disc, ax_z.disc));
                        return EM_RES.ERR; ;
                    }
                }
                //other axis
                res = MT.AxisHome(ref bquit, ax_x, ax_y, ax_a);
                if (res != EM_RES.OK) return res;
                res = MT.AxisHome(ref bquit, ax_z);
                if (res != EM_RES.OK) return res;
                status = EM_STA.READY;
                return res;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR; ;
            }
        }
        /// <summary>
        /// 停止轴运动，停止Home动作
        /// </summary>   
        public static void Stop()
        {
            ax_x.bhomequit = true;
            ax_x.Stop();
            ax_y.bhomequit = true;
            ax_y.Stop();
            ax_z.bhomequit = true;
            ax_z.Stop();
            ax_a.bhomequit = true;
            ax_a.Stop();
        }
        /// <summary>
        /// 准备取料
        /// </summary>   
        /// <summary>
        /// 从安全位开始到取料动作
        /// </summary>     
        public static EM_RES m_ActGet(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
          
            try
            {
                
                tray_now = COM.product.TrayGet;
                if (tray_now == null)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}料盘空!", disc, GetStaString));
                    return EM_RES.PARA_ERR;
                }
                if (tray_now.bEmpty)
                    return EM_RES.QUIT;
                status = EM_STA.PICK;
                //检测吸嘴有料
                ret = zk_z.SetOn(ref bquit);
                if (ret != EM_RES.OK) return ret;
                if (zk_z.io_sen_on.AssertON(10, 100))
                    return EM_RES.OK;
                ret = zk_z.SetOff(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = ps_sf.MoveTo(ref bquit, true); //安全
                if (ret != EM_RES.OK) return ret;

                ret = tray_now.ToPosId( MT.pos_get_plate_star,ref bquit);   //到取料位置
                if (ret != EM_RES.OK) return ret;
                ret = ps_zDown.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = cyl_z.SetOn(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                if (!Action.bNullRun)
                {
                    ret = zk_z.SetOn(ref bquit, 4000);
                    if (ret != EM_RES.OK) return ret;
                }
                    ret = ps_zUP.MoveTo(ref bquit, true);
                    if (ret != EM_RES.OK) return ret;

                if (zk_z.isONByChkSen || Action.bNullRun)
                {
                    tray_now.list_mask[tray_now.get_id] = false;
                    return EM_RES.OK;
                }
                else
                    return EM_RES.ERR;
            }
            catch
            {

                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }
            finally
            {
                ret = ps_zUP.MoveTo(ref bquit, true);
              

            }
        }
        public static EM_RES m_ToPhoto(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            status = EM_STA.PHOTO;
            try
            {
                status = EM_STA.PHOTO;
                if (!bLPutOK)
                {
                    ret = ps_pho_L.MoveTo(ref bquit, true); //安全
                    if (ret != EM_RES.OK) return ret;
                    ret = COM.MVS.m_camera_action(1, out Move_L);
                   if (ret != EM_RES.OK) return ret;                
                   return ret;
                }
                else if (!bRPutOK)
                {
                   
                    ret = ps_pho_R.MoveTo(ref bquit, true); //安全
                    if (ret != EM_RES.OK) return ret;
                    ret = COM.MVS.m_camera_action(2, out Move_R);
                    if (ret != EM_RES.OK) return ret;
                    return ret;
                }
                else
                {
                    return EM_RES.OK;
                }

            }

            catch
            {

                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }
        }
        public static EM_RES m_ToPut(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            status = EM_STA.PLACE;
            try
            {
                if (!zk_z.isONByChkSen&&!Action.bNullRun)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}吸嘴无料!", disc, GetStaString));
                    return EM_RES.ERR;
                }
                //ret = ps_sf.MoveTo(ref bquit, true); //安全
                //if (ret != EM_RES.OK) return ret;
                if (!bLPutOK)
                {
                    ret = ps_put_to_L.MoveTo(ref bquit, true); //安全
                    if (ret != EM_RES.OK) return ret;
                    ret = ps_put_L.MoveTo(ref bquit, true); //安全
                    if (ret != EM_RES.OK) return ret;
                    //POS mPos = new POS(MT.AXIS_GET_X, MT.AXIS_GET_Y, MT.AXIS_GET_A,null, "视觉偏移", 0, 0, Move_L);
                    ret = MT.AXIS_GET_X.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_GET_X.fcmd_pos + Move_L.x, 2000);
                    if (ret != EM_RES.OK) return ret;
                    ret = MT.AXIS_GET_Y.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_GET_X.fcmd_pos + Move_L.y, 2000);
                    if (ret != EM_RES.OK) return ret;
                    ret = MT.AXIS_GET_A.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_GET_A.fcmd_pos + Move_L.z, 2000);
                    if (ret != EM_RES.OK) return ret;
                    ret = m_ActPut(ref bquit);
                    if (ret != EM_RES.OK) return ret;
                    ret = ps_put_L.MoveTo(ref bquit, true); //安全
                    if (ret != EM_RES.OK) return ret;
                    ret = ps_put_to_L.MoveTo(ref bquit, true); //安全
                    if (ret != EM_RES.OK) return ret;

                }
                else
                    if (!bRPutOK)
                    {
                        ret = ps_put_to_R.MoveTo(ref bquit, true); //安全
                        if (ret != EM_RES.OK) return ret;
                        ret = ps_put_R.MoveTo(ref bquit, true); //安全
                        if (ret != EM_RES.OK) return ret;
                        ret = MT.AXIS_GET_X.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_GET_X.fcmd_pos + Move_R.x, 2000);
                        if (ret != EM_RES.OK) return ret;
                        ret = MT.AXIS_GET_Y.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_GET_X.fcmd_pos + Move_R.y, 2000);
                        if (ret != EM_RES.OK) return ret;
                        ret = MT.AXIS_GET_A.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_GET_A.fcmd_pos + Move_R.z, 2000);
                        if (ret != EM_RES.OK) return ret;
                        ret = m_ActPut(ref bquit);
                        if (ret != EM_RES.OK) return ret;
                        ret = ps_put_R.MoveTo(ref bquit, true); //安全
                        if (ret != EM_RES.OK) return ret;
                        ret = ps_put_to_R.MoveTo(ref bquit, true); //安全
                        if (ret != EM_RES.OK) return ret;
                    }
                    else
                        return EM_RES.OK;
                return ret;
            }
            catch
            {

                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }
        }
        public static EM_RES m_ActPut(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            try
            {
                status = EM_STA.PLACE;
                if (!isReady) return EM_RES.QUIT;
                if (!zk_z.isONByChkSen && !Action.bNullRun)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}吸嘴无料!", disc, GetStaString));
                    return EM_RES.ERR;
                }
                ret = ps_zDown.MoveTo(ref bquit, true); //安全
                if (ret != EM_RES.OK) return ret;
                zk_z.SetOff(ref bquit);
                ret = ps_zUP.MoveTo(ref bquit, true); //安全
                if (ret != EM_RES.OK) return ret;
                if (!bLPutOK)
                    bLPutOK = true;
                else if (!bRPutOK)
                    bRPutOK = true;
                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}吸嘴无料!", disc, GetStaString));
                return EM_RES.ERR;
            }

        }
        public static void act_run()
        {
            EM_RES ret = EM_RES.OK;
            ////gy0114测试系统
            //bOK = true;
            //return;
            try
            {
                if (bOK)
                {
                    status = EM_STA.READY;
                    goto MEND;
                }
                if (!isReady) goto MEND;
                AllAct();
                if (!sen_plate_at_get.AssertON())//检测盘
                {
                    tray_now = null;
                    WsBuFD.task_run();//出料盘
                    status = EM_STA.WAIT;
                    goto MEND;
                }
                else
                {
                    
                    tray_now = COM.product.TrayGet;
                    if (tray_now==null)
                    {
                        goto MEND;
                    }
                    else if ( tray_now.bEmpty )  //空盘收料
                    {
                        WsTrayMove.task_run();
                        status = EM_STA.WAIT;
                        goto MEND;
                    }
                    if (!WsTrayMove.ax_Z.isORG)
                    {
                        WsTrayMove.task_run();
                        status = EM_STA.WAIT;
                        goto MEND;
                    }
                    ret = DoAct(ref ActGet);
                    if (ret != EM_RES.OK) goto MEND;
                    ret = DoAct(ref ToPhoto);
                    if (ret != EM_RES.OK) goto MEND;
                    ret = DoAct(ref ToPut);
                    if (ret != EM_RES.OK) goto MEND;
              
                }
            MEND:

                Stop();
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return;
            }
        }
        public static void task_run()
        {
            if (TaskRun == null || (TaskRun != null && TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建取料线程");
                if (TaskRun != null)
                    TaskRun.Dispose();
                TaskRun = new Task(act_run);
                TaskRun.Start();

            }
        }
        public static EM_RES home(ref bool bquit)
        {
            try
            {
                EM_RES res = EM_RES.OK;
                if (bquit) return EM_RES.QUIT;
                if (!(WSGet.TaskRun == null || WSGet.TaskRun.IsCompleted))
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}运行线程未退出!", disc));
                    return EM_RES.ERR;
                }
                res = cyl_z.SetOff(3000);
                if (res != EM_RES.OK) return res;
                if (bquit) return EM_RES.QUIT;
                res = ax_z.MoveTo(ref bquit, 9999, 5000);
                if (res != EM_RES.OK && res != EM_RES.PARA_OUTOFRANG) return res;
                if (bquit) return EM_RES.QUIT;
                res = MT.AxisHome(ref bquit, ax_y, ax_x, ax_a);
                if (res != EM_RES.OK) return res;
                res = MT.AxisHome(ref bquit, ax_z);
                return res;
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--回原-" + "未知系统异常", disc));
                return EM_RES.ERR;
            }
        }



    }
    #endregion
    #region   收料
    public static class WSBack
    {
        public static TrayBox tbox_back = COM.traybox_back;
        public static Product.Tray tray_now =COM.product.TrayBackOK;
        public static Product.Tray tray_AANG = COM.product.TrayBackAANG;
        public static Product.Tray tray_PPNG = COM.product.TrayBackPPNG;
        public static Product.Tray tray_TTNG = COM.product.TrayBackTTNG;
        public static AXIS ax_x = MT.AXIS_BACK_X;
        public static AXIS ax_y = MT.AXIS_BACK_Y;
        public static AXIS ax_z = MT.AXIS_BACK_Z;
        public static AXIS ax_a = MT.AXIS_BACK_A;
        static Cylinder cyl_z = MT.CYL_back_up;
        static Cylinder zk_z = MT.VACUM_back_mouth;
       public static POS ps_sf = MT.pos_back_safe;
        static POS ps_pho_R = MT.pos_get_photo_R;

        static POS ps_get_L = MT.pos_back_L;
        static POS ps_get_R = MT.pos_back_R;
        static POS ps_get_to_L = MT.pos_back_to_L;
        static POS ps_get_to_R = MT.pos_back_to_R;
        static POS ps_zDown = MT.pos_back_z_dwn;
        static POS ps_zUP = MT.pos_back_z_up;


        static POS ps_row = MT.pos_back_plate_row;
        static POS ps_col = MT.pos_back_plate_line;
        static POS ps_star = MT.pos_back_plate_star;
        public static List<POS> ps_list = MT.pos_list_back;
        public static ST_XYZA pos_now;
        public static bool bOK
        {
            get
            {
                if (bLGet && bRGet)
                    return true;
                else return false;
            }
            set
            {
                if (value)
                {
                    bLGet = true;
                    bRGet = true;
                }
                else
                {
                    bLGet = false;
                    bRGet = false;
                }
            }
        }
        public static string disc = "收料工站-";
        public static EM_STA status = EM_STA.UNKNOW;
        public static string GetStaString
        {
            get
            {
                return status.GetDescription() + "-";

            }
        }

        //取料选择左右
        public static bool bLGet;
        public static bool bRGet;
        //收料盘是否放好
        public static bool b_tray_OK;
        static Mact ActGet;
        static Mact ToGet;
        static Mact ActPut;
        public static Task TaskRun;
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("放料")]
            PLACE,
            [Description("取料")]
            PICK,
            [Description("错误")]
            ERR,
            [Description("等待料盘")]
            WAIT
        }
        static void AllAct()
        {
            ActGet = new Mact(m_ActGet);
            ToGet = new Mact(m_ToGet);
            ActPut = new Mact(m_ActPut);
        }
        //需要保存，当前状态     
        /// <summary>
        /// 取料料模块复位
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        public static EM_RES Home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            if (bquit)
                return EM_RES.QUIT;
            status = EM_STA.HOME;
            //气缸回位
            res = cyl_z.SetOff(ref bquit, 1000);
            if (res != EM_RES.OK) return res;
            //先抬升
            if (!ax_z.isELP)
            {
                ax_z.MoveTo(ref bquit, ax_z.fcmd_pos + 9999, 1000);
                Thread.Sleep(20);
                if (!ax_z.isELP)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}到正限位异常!", disc, ax_z.disc));
                    return EM_RES.ERR; ;
                }
            }

            //other axis
            res = MT.AxisHome(ref bquit, ax_x, ax_y, ax_a);
            if (res != EM_RES.OK) return res;

            //AXIS[] ax_xy = new AXIS[] { ax_x, ax_y, ax_a };
            //res = MT.AxisHome(ref bquit, ax_xy);
            //if (res != EM_RES.OK) return res;

            res = MT.AxisHome(ref bquit, ax_z);
            if (res != EM_RES.OK) return res;

            return res;
        }
        /// <summary>
        /// 停止轴运动，停止Home动作
        /// </summary>   
        public static void Stop()
        {
            ax_x.bhomequit = true;
            ax_x.Stop();
            ax_y.bhomequit = true;
            ax_y.Stop();
            ax_z.bhomequit = true;
            ax_z.Stop();
            ax_a.bhomequit = true;
            ax_a.Stop();
        }


        public static EM_RES ck_ax_safe(int id, double pos = 0)
        {
            bool bquit = false;
            int i = 0;
            double max = 9999;
            try
            {
                if (!ax_z.isELP)
                {
                    ax_z.MoveTo(ref bquit, max);
                    while (!ax_z.isELP)
                    {
                        i++;
                        Thread.Sleep(100);
                        Application.DoEvents();
                        if (i > 100)
                        {
                            VAR.ErrMsg(ax_z.disc + "到正限位异常");
                            return EM_RES.ERR;
                        }
                    }
                }

                return EM_RES.OK;
            }
            catch (Exception e)
            {
                VAR.ErrMsg(e.ToString());
                return EM_RES.ERR;
            }



        }
        /// <summary>
        /// 准备取料
        /// </summary>   

        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                if (!(WsBuBK.TaskRun == null || WsBuBK.TaskRun.IsCompleted))
                    return false;
                if (!(WsTrayMove.TaskRun == null || WsTrayMove.TaskRun.IsCompleted))
                    return false;
                //   if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                return true;
            }
        }

        static EM_RES DoAct(ref Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;
                AllAct();
                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;

                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.READY;

                }

                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }

        }

        /// <summary>
        /// 从安全位开始到取料动作
        /// </summary>     
        public static EM_RES m_ActGet(ref bool bquit)
        {
            try
            {
                status = EM_STA.PICK;
                EM_RES ret = EM_RES.OK;
                if (!isReady) return EM_RES.QUIT;
                //检测到吸嘴有料
                if (zk_z.isONByChkSen)
                {
                    return EM_RES.OK;
                }
                //Z轴下降到位          
                if (bquit) return EM_RES.QUIT;
                ret = ps_zDown.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                //气缸下降
                ret = cyl_z.SetOff(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;
                //真空吸
                ret = zk_z.SetOn(ref bquit, 3000);
                if (ret != EM_RES.OK) return ret;

                if (!bLGet)
                {
                    ret = ax_a.MoveTo(ref bquit, ax_a.fcmd_pos + 20, 3000);
                    if (ret != EM_RES.OK) return ret;
                   
                }
                 
                else if (!bRGet)
                {
                    ret = ax_a.MoveTo(ref bquit, ax_a.fcmd_pos - 20, 3000);
                    if (ret != EM_RES.OK) return ret;
                   
                }        
                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }
        }
        public static EM_RES m_ToGet(ref bool bquit)
        {
            try
            {
                status = EM_STA.PICK;
                EM_RES ret = EM_RES.OK;
                if (zk_z.isONByChkSen)
                {
                    return EM_RES.OK;
                }

                if (!bLGet)
                {
                    ret = ps_get_to_L.MoveTo(ref bquit, true);
                    if (ret != EM_RES.OK) return ret;
                    ret = ps_get_L.MoveTo(ref bquit, true);
                    if (ret != EM_RES.OK) return ret;
                }
                else
                    if (!bRGet)
                    {

                        ret = ps_get_to_R.MoveTo(ref bquit, true);
                        if (ret != EM_RES.OK) return ret;
                        ret = ps_get_R.MoveTo(ref bquit, true);
                        if (ret != EM_RES.OK) return ret;
                    }
                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }

        }
        public static EM_RES m_ActPut(ref bool bquit)
        {
            try
            {
                status = EM_STA.PLACE;
                EM_RES ret = EM_RES.OK;
                if (bLGet && bRGet) return ret;
                if (tray_now == null) return EM_RES.QUIT;
                if (tray_now.bFull) return EM_RES.QUIT;
                if (!zk_z.isONByChkSen)
                {

                    VAR.ErrMsg(string.Format("{0}{1} 吸嘴无料", disc, GetStaString));
                    return EM_RES.ERR;
                }

              
           
                   

                //需要根据状态放不同料盘-gy-1227
                ret = tray_now.ToPosId(MT.pos_back_plate_star,ref VAR.gsys_set.bquit, tray_now.put_id);
                if (ret != EM_RES.OK) return ret;
                ret = ps_zDown.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = zk_z.SetOff(ref bquit, 2000);
                if (ret != EM_RES.OK) return ret;
                ret = ps_zUP.MoveTo(ref bquit, true);
                if (ret != EM_RES.OK) return ret;

                if (!bLGet)
                    bLGet = true;
                else
                    if (!bRGet)
                        bRGet = true;

                tray_now.list_mask[tray_now.put_id] = true;
                return ret;
            }

            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }

        }

        public static void act_run()
        {

            EM_RES ret = EM_RES.OK;
            //gy0114
            bOK = true;
            return;
            if (bOK)
            {
                status = EM_STA.READY;
                goto MEND;
            }
            if (!isReady) goto MEND;
            AllAct();
            if (!WsTrayMove.sen_tray_back.AssertON())//检测盘不在位
            {
                tray_now = null;
                WsBuFD.task_run();//出料盘
                status = EM_STA.WAIT;
                goto MEND;
            }
            tray_now = COM.product.TrayBackOK;
            if (tray_now == null) return;
            if (tray_now.bFull && WsTrayMove.sen_tray_back.AssertON())  //满盘进料
            {
                WsTrayMove.task_run();
                goto MEND;
            }
            if (!WsTrayMove.ax_Z.isORG)
            {
                WsTrayMove.task_run();
                status = EM_STA.WAIT;
                goto MEND;
            }
            ret = DoAct(ref  ToGet);
            if (ret != EM_RES.OK) goto MEND;
            ret = DoAct(ref  ActGet);
            if (ret != EM_RES.OK) goto MEND;
            ret = DoAct(ref  ActPut);
            if (ret != EM_RES.OK) goto MEND;
            if (!ax_y.isORG)
              ret=  ps_sf.MoveTo(ref VAR.gsys_set.bquit, true);
            if (ret != EM_RES.OK) goto MEND;

        MEND:
              
            Stop();
            //EM_RES ret = EM_RES.OK;
            //AllAct();
            //if (!isReady) return;
            //bool bhomeerr = false;
            //if ((status == EM_STA.UNKNOW) && (!bhomeerr))
            //{
            //    Action.ErrShow(disc + "请复位！");
            //    bhomeerr = true;
            //}

            //    while (isReady)
            //    {                  
            //        warn_msg = "";
            //        Thread.Sleep(20);
            //        Application.DoEvents();
            //        switch (status)
            //        {
            //            case EM_STA.PICK://取料
            //                ret=  DoAct(ToGet);
            //                if(ret == EM_RES.OK)
            //                DoAct(ActGet, EM_STA.PLACE);
            //                break;
            //            case EM_STA.PLACE://放料
            //                DoAct(ActPut, EM_STA.READY);
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //    if (!isErr) return;
            //    Action.ErrMsg = disc + GetStaString + warn_msg + "异常";
            //    VAR.gsys_set.status = EM_SYS_STA.ERR;
        }

        public static void task_run()
        {
            if (TaskRun == null || (TaskRun != null && TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建收料线程");
                if (TaskRun != null)
                    TaskRun.Dispose();
                TaskRun = new Task(act_run);
                TaskRun.Start();

            }
        }
        public static EM_RES home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            try
            {
                if (bquit) return EM_RES.QUIT;
                if (!(TaskRun == null || TaskRun.IsCompleted))
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 工站工作中，回原失败!", disc));
                    return EM_RES.ERR;
                }

                res = cyl_z.SetOff(3000);
                if (res != EM_RES.OK) return res;
                if (bquit) return EM_RES.QUIT;
                res = ax_z.MoveTo(ref bquit, 99999, 5000);
                if (res != EM_RES.OK && res != EM_RES.PARA_OUTOFRANG) return res;
                if (!ax_z.isELP) return EM_RES.ERR;
                if (bquit) return EM_RES.QUIT;
                //res = MT.AxisHome(ref bquit, ax_y, ax_x, ax_a);//-gy-1201-旋转轴失败
                res = MT.AxisHome(ref bquit, ax_y, ax_x);
                if (res != EM_RES.OK) return res;
                res = MT.AxisHome(ref bquit, ax_z);
                return res;

            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}未知错误!", disc, GetStaString));
                return EM_RES.ERR;
            }

        }
    }
    #endregion
    #region AA上料
    public class WSFeed
    {

        static AXIS ax_x = MT.AXIS_FEED_X;
        static AXIS ax_y = MT.AXIS_FEED_Y;
        static AXIS ax_z = MT.AXIS_FEED_Z;
        static AXIS ax_a = MT.AXIS_FEED_A;

        static Cylinder cyl_up_L = MT.CYL_feed_left_up;
        static Cylinder cyl_up_R = MT.CYL_feed_right_up;
       
        //夹紧气缸
        static Cylinder cyl_clip_L = MT.CYL_feed_left_clip;
        static Cylinder cyl_clip_R = MT.CYL_feed_right_clip;
        //夹爪真空
        static Cylinder vacu_clip_L = MT.VACUM_feed_hand_L;
        static Cylinder vacu_clip_R = MT.VACUM_feed_hand_R;
         
        static POS ps_sf = MT.pos_feed_safe;
        static POS ps_get_L = MT.pos_feed_left_get;
        static POS ps_put_L = MT.pos_feed_left_feed;
        static POS ps_get_R = MT.pos_feed_right_get;
        static POS ps_put_R = MT.pos_feed_right_back;
      public   static POS ps_photo_L = MT.pos_feed_photo;
        static POS ps_zDwn = MT.pos_feed_z_dwn;
        static POS ps_zUP = MT.pos_feed_z_up;
        static List<POS> ps_list = MT.pos_list_feed;
        public static bool bOK
        {
            get
            {
               
                if ((bLput && bRput)) return true;
                else
                return false;
            }
            set
            {

                bLput = value;
                bRput = value;

            }
        }

        public static string disc = "上料工站-";
        //是否左取料
        static bool is_get_L
        {
            get
            {
                
                if (cyl_clip_L.io_out.isON && cyl_clip_L.io_sen_on.AssertON())
                    return true;//取料成功
                else return false;
            }

        }
        public static bool bLput;//左放料完成,重要变量
        //是否左取料
        static bool is_get_R
        {
            get
            {
                
                if (cyl_clip_R.io_out.isON && cyl_clip_R.io_sen_on.AssertON())
                    return true;//取料成功
                else return false;
            }

        }
        public static bool bAAcmd;//收到AA上料命令

        public static bool bRput;//右放料完成

        static Mact ActGet;
        static Mact ActGetR;
        static Mact ActPut;
        static Mact ActPutR;
        static Mact ToSafe;
        static Mact ToPhoto;
        static ST_XYZ Move_L;
        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("安全位")]
            SAFE,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("左放料")]
            PLACE_L,
            [Description("右放料")]
            PLACE_R,
            [Description("左取料")]
            PICK_L,
            [Description("右取料")]
            PICK_R,
            [Description("错误")]
            ERR,
            [Description("拍照")]
            PHOTO
        }
        public static bool isErr;
        public static void AllAct()
        {
            ActGet = new Mact(m_ActGet_L);
            ActGetR = new Mact(m_ActGet_R);
            ToSafe = new Mact(m_ToSafe);
            ActPut = new Mact(m_ActPut_L);
            ActPutR = new Mact(m_ActPut_R);
            ToPhoto = new Mact(m_ToPhoto);

        }

        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                //if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                return true;
            }
        }
        public static string GetStaString
        {
            get
            {
                return status.GetDescription() + "过程";

            }
        }

        //执行委托动作函数
        public static EM_RES DoAct(Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;
                AllAct();
                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;

                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.READY;

                }

                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.gsys_set.bquit = true;
                return EM_RES.ERR;
            }

        }
        public static EM_STA status = EM_STA.UNKNOW;
        public static EM_RES ck_ax_safe(int id, double pos = 0)
        {

            if (cyl_up_L == null || cyl_up_R == null) return EM_RES.ERR;
            if (cyl_up_L.io_sen_off == null || cyl_up_R.io_sen_off == null) return EM_RES.ERR;
         
            if (!cyl_up_L.io_sen_off.AssertON())
                cyl_up_L.SetOff(3000);
            if (!cyl_up_L.io_sen_off.AssertON())
            {
                VAR.ErrMsg("夹爪抬升到位异常");
                return EM_RES.ERR;
            }
            if (!cyl_up_R.io_sen_off.AssertON())
                cyl_up_R.SetOff(3000);
            if (!cyl_up_R.io_sen_off.AssertON())
            {
                VAR.ErrMsg("夹爪抬升到位异常");
                return EM_RES.ERR;
            }
            if (!MT.AXIS_FEED_Z.isELP)
                MT.AXIS_FEED_Z.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_FEED_Z.fcmd_pos+9999, 3000);
            if (!MT.AXIS_FEED_Z.isELP)
            {
                VAR.ErrMsg("Z轴到正限位异常");
                return EM_RES.ERR;
            }

            if (cyl_up_L.io_sen_off.AssertON() && cyl_up_R.io_sen_off.AssertON())
                return EM_RES.OK;
            else
                return EM_RES.ERR;

        }
        /// <summary>
        /// 取料料模块复位
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        public static EM_RES Home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            if (bquit)
                return EM_RES.QUIT;

            //先抬升
            if (!ax_z.isELP)
            {
                ax_z.MoveTo(ref bquit, ax_z.fcmd_pos + 9999, 1000);
                Thread.Sleep(20);
                if (!ax_z.isELP)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} {1}到正限位异常!", disc, ax_z.disc));
                    return EM_RES.ERR; ;
                }
            }

            //other axis
            res = MT.AxisHome(ref bquit, ax_x, ax_y, ax_a);
            if (res != EM_RES.OK) return res;

            //AXIS[] ax_xy = new AXIS[] { ax_x, ax_y, ax_a };
            //res = MT.AxisHome(ref bquit, ax_xy);
            //if (res != EM_RES.OK) return res;

            res = MT.AxisHome(ref bquit, ax_z);
            if (res != EM_RES.OK) return res;

            return res;
        }
        /// <summary>
        /// 停止轴运动，停止Home动作
        /// </summary>   
        public static void Stop()
        {
            ax_x.bhomequit = true;
            ax_x.Stop();
            ax_y.bhomequit = true;
            ax_y.Stop();
            ax_z.bhomequit = true;
            ax_z.Stop();
            ax_a.bhomequit = true;
            ax_a.Stop();
        }


        /// <summary>
        /// 从安全位开始到取料动作
        /// </summary>     
        public static EM_RES m_ActGet_L(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (!isReady) return EM_RES.QUIT;
            if (is_get_L) return EM_RES.OK;
            if (WSROLL.VacumRoll[WSROLL.aa_id].io_sen_off.isON)
            {

                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "无夹具在位", disc, GetStaString));            
                return EM_RES.ERR;
            }
            ret = ps_get_L.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) return ret;
            //打开夹爪
            ret = cyl_clip_L.SetOff(ref bquit, 3000);
            //用感应信号判断有没有夹到重要
            if (ret != EM_RES.OK) return ret;
            //气缸上升
            ret = cyl_up_L.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            //Z轴下降到位
            ret = ps_zDwn.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) return ret;
            //气缸下降
            ret = cyl_up_L.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            //夹爪收紧
            ret = cyl_clip_L.SetOn(ref bquit, 3000);
               if (ret != EM_RES.OK) return ret;
            //夹具真空关闭
               ret = WSROLL.VacumRoll[WSROLL.aa_id].SetOff();
               if (ret != EM_RES.OK) return ret;
            //夹爪真空打开
             ret=  vacu_clip_L.SetOn();
             if (ret != EM_RES.OK) return ret;
            //气缸上升
            ret = cyl_up_L.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            //Z轴回升        
            ret = ps_zUP.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) return ret;

            if (UI.Action.bNullRun)//空料运行
                return EM_RES.OK;
            if (is_get_L)
                return EM_RES.OK;
            else return EM_RES.ERR;
        }
        public static EM_RES m_ActGet_R(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (!isReady) return EM_RES.QUIT;
            if (is_get_R) return EM_RES.OK;
          
            ret = ps_get_R.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            //打开夹爪
            ret = cyl_clip_R.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //气缸上升
            ret = cyl_up_R.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //Z轴下降到位
            ret = ps_zDwn.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            //气缸下降
            ret = cyl_up_R.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //夹爪收紧
            ret = cyl_clip_R.SetOn(ref bquit, 3000);
              if (ret != EM_RES.OK) goto MSTOP;
              //夹具真空关闭
              //ret = WSROLL.VacumRoll[WSROLL.aa_id].SetOff();
              //if (ret != EM_RES.OK) return ret;
              //夹爪真空打开
              ret = vacu_clip_R.SetOn();
              if (ret != EM_RES.OK) return ret;
            //气缸上升
            ret = cyl_up_R.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //Z轴回升        
            ret = ps_zUP.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            if (is_get_R)
                ret = EM_RES.OK;
            else ret = EM_RES.ERR;
        MSTOP:
            m_ZSafe(ref  bquit);
            Stop();
            return ret;

        }
        /// <summary>
        /// 运动到取料位
        /// </summary> 
        public static EM_RES m_ZSafe(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (bquit) return EM_RES.QUIT;
            if (!isReady) return EM_RES.QUIT;

            ret = ps_zUP.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) return ret;
            //打开夹爪
            ret = cyl_up_L.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            ret = cyl_up_R.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            return ret;
        }
        public static EM_RES m_ToPhoto(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            status = EM_STA.PHOTO;
            if (!isReady) return EM_RES.QUIT;
            ret = m_ZSafe(ref  bquit);
            if (ret != EM_RES.OK) return ret;

            ret = ps_photo_L.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) return ret;
            ret = COM.MVS.m_camera_action(3, out Move_L);
            if (ret != EM_RES.OK) return ret; 
            return ret;

        }
        public static EM_RES m_ActPut_L(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (!isReady) return EM_RES.QUIT;
            if (!is_get_L&&!UI.Action.bNullRun) return EM_RES.QUIT;
            status = EM_STA.PLACE_L;
            ret = ps_put_L.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = MT.AXIS_FEED_X.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_FEED_X.fcmd_pos + Move_L.x, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = MT.AXIS_FEED_Y.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_FEED_Y.fcmd_pos + Move_L.y, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = MT.AXIS_FEED_A.MoveTo(ref VAR.gsys_set.bquit, MT.AXIS_FEED_A.fcmd_pos + Move_L.z, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = ps_zDwn.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            //气缸下降
            ret = cyl_up_L.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //打开夹爪
            ret = cyl_clip_L.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //夹具真空关闭
            //ret = WSROLL.VacumRoll[WSROLL.aa_id].SetOn();
            //if (ret != EM_RES.OK) return ret;
            //夹爪真空打开
            ret = vacu_clip_L.SetOff();
            if (ret != EM_RES.OK) return ret;
            //气缸回升
            ret = cyl_up_L.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = ps_zUP.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
        
            bLput = true;
        MSTOP:
            Stop();
            cyl_up_L.SetOn(ref bquit, 3000);
            ps_zUP.MoveTo(ref bquit, true);
            return ret;


        }
        public static EM_RES m_ActPut_R(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (!isReady) return EM_RES.QUIT;
            if (!is_get_R && !UI.Action.bNullRun) return EM_RES.QUIT;
            status = EM_STA.PLACE_R;
            ret = ps_put_R.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = ps_zDwn.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK) goto MSTOP;
            //气缸下降
            ret = cyl_up_R.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            //打开夹爪
            ret = cyl_clip_R.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;

            //转盘真空打开
            ret = WSROLL.VacumRoll[WSROLL.aa_id].SetOn();
            if (ret != EM_RES.OK) return ret;
            //夹爪真空关闭
            ret = vacu_clip_R.SetOff();
            if (ret != EM_RES.OK) return ret;
            //气缸回升
            ret = cyl_up_R.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) goto MSTOP;
            ret = ps_zUP.MoveTo(ref bquit, true);
            if (ret != EM_RES.OK)  goto MSTOP;
            bRput = true;
        MSTOP:
            Stop();
            cyl_up_L.SetOn(ref bquit, 3000);
            ps_zUP.MoveTo(ref bquit, true);
            return ret;
        }
        public static EM_RES m_ToSafe(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            if (!isReady) return EM_RES.QUIT;
            //Z轴回升
            ret = m_ZSafe(ref  bquit);
            if (ret != EM_RES.OK) return ret;
            ret = ps_sf.MoveTo(ref bquit, true);

            return ret;
        }

        public static void act_run()
        {

            EM_RES ret = EM_RES.OK;
            //gy0114
            bOK = true;
            AllAct();
            if (bOK) return;
            if (!isReady) return;
            Thread.Sleep(20);
            Application.DoEvents();
            if (bLput && bRput) goto MSTOP;
            if (!is_get_L && !bLput)
            {
                ret = DoAct(ActGet);
                if (ret != EM_RES.OK)
                    goto MSTOP;
            }
            if(!bAAcmd&&!UI.Action.bNullRun)
                goto MSTOP;
            if (!is_get_R && !bRput)
            {
                ret = DoAct(ActGetR);
                if (ret != EM_RES.OK)
                    goto MSTOP;
            }
            if (!bLput)
            {
                ret = DoAct(ToPhoto);
                if (ret != EM_RES.OK)
                    goto MSTOP;
                ret = DoAct(ActPut);
                if (ret != EM_RES.OK)
                    goto MSTOP;
            }
            if (!bRput)
            {
                ret = DoAct(ActPutR);
                if (ret != EM_RES.OK)
                    goto MSTOP;
            }
        MSTOP:
            Stop();
        }
        public static Task TaskRun;
        public static void task_run()
        {
            if (TaskRun == null || (TaskRun != null && TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建收料线程");
                if (TaskRun != null)
                    TaskRun.Dispose();
                TaskRun = new Task(act_run);
                TaskRun.Start();

            }
        }
        public static EM_RES home(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            if (bquit) return EM_RES.QUIT;
            if (!(TaskRun == null || TaskRun.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 工站工作中，回原失败!", disc));
                return EM_RES.ERR;
            }
            res = cyl_up_L.SetOff(3000);
            if (res != EM_RES.OK) return res;
            res = cyl_up_R.SetOff(3000);
            if (res != EM_RES.OK) return res;
            if (bquit) return EM_RES.QUIT;
            res = ax_z.MoveTo(ref bquit, 99999, 5000);
            if (res != EM_RES.OK && res != EM_RES.PARA_OUTOFRANG) return res;
            if (!ax_z.isELP) return EM_RES.ERR;
            if (bquit) return EM_RES.QUIT;
            res = MT.AxisHome(ref bquit, ax_y, ax_x, ax_a);
            if (res != EM_RES.OK) return res;
            res = MT.AxisHome(ref bquit, ax_z);
            return res;
        }
    }
    #endregion
    #region 转盘
    public static class WSROLL
    {
        public static Cylinder cyl_ck_up = MT.CYL_check_up;
        public static Cylinder cyl_open = MT.CYL_cover_open;
        public static Cylinder cyl_close = MT.CYL_cover_close;
        public static GPIO sen_open = MT.GPIO_IN_code_open;
        public static GPIO sen_closed = MT.GPIO_IN_code_closed;
        public static List<Cylinder> VacumRoll = MT.List_vacu_roll;

        public static GPIO sen_roll_topos = MT.CKPOS_roll_plate_topos;
        public static GPIO sen_home = MT.CKPOS_roll_plate_home_point;
        public static GPIO roll_star = MT.GPIO_OUT_roll_plate;
        public static GPIO roll_topos = MT.CKPOS_roll_plate_topos;
     
        public static string disc = "上料工站-";
  
        public static bool bRollOK;
        public static UI.form Mcom = new form();
        public static bool bOtherOK 
        {
            get
            {
                if (sen_closed.isON) return false;

                if (sen_open.isOFF) return false;

                if (ModSta[ck_id].LSta == STA_MOD.UNTEST || ModSta[ck_id].RSta == STA_MOD.UNTEST)
                    return false;
                return true;
            }
        
        }
        static Mact open;
        static Mact close;
        static Mact CKpoint;
        static Mact ckDW;
        static Mact ActHome;
        static Mact ActRoll;
        /// <summary>
        /// 工站数量
        /// </summary>
        public static int pos_num = 6;

        public struct RO_STA
        {
            public STA_MOD LSta ;
            public STA_MOD RSta ;
        }
        //    public  static List<RO_STA> RollSta = new List<RO_STA>();

        public static int get_id;//当前转盘取料位模组块编号
        //检测位置编号
        public static int ck_id
        {
            get
            {
                return (get_id + 1) % pos_num;
            }
        }
        //AA交换位置
        public static int aa_id
        {
            get
            {

                return (get_id + 2) % pos_num;
            }
        }
        //收料位置编号
        public static int bk_id
        {
            get
            {

                return (get_id + 4) % pos_num;
            }
        }
        //开盖位置
        public static int op_id
        {
            get
            {

                return (get_id + 3) % pos_num;
            }
        }
        public enum STA_MOD
        {
            [Description("未知")]
            UNKNOW,
            [Description("空")]
            NULL,
            [Description("开图NG")]
            PPNG,
            [Description("AANG")]
            AANG,
            [Description("OK")]
            OK,
            [Description("待测")]
            UNTEST,
            [Description("错误")]
            ERR,
            [Description("点亮NG")]
            POINTNG
        }
        // public static List<RO_STA> ModSta = new List<RO_STA>();
        public static RO_STA[] ModSta;
        public static void CK_cmd(string data)
        {
            string OKOK = "站号3命令上料产品OKOK";

            if (data.Equals(OKOK))
            {
                ModSta[aa_id].LSta = STA_MOD.OK;
                ModSta[aa_id].RSta = STA_MOD.OK;
            }

            string NGOKAA = "站号3命令上料产品NGOKAA";
            if (data.Equals(NGOKAA))
            {
                ModSta[aa_id].LSta = STA_MOD.AANG;
                ModSta[aa_id].RSta = STA_MOD.OK;
            }
            string OKNGAA = "站号3命令上料产品OKNGAA";
            if (data.Equals(OKNGAA))
            {
                ModSta[aa_id].LSta = STA_MOD.OK;
                ModSta[aa_id].RSta = STA_MOD.AANG;
            }
            string NGNGAAAA = "站号3命令上料产品NGNGAAAA";
            if (data.Equals(NGNGAAAA))
            {
                ModSta[aa_id].LSta = STA_MOD.AANG;
                ModSta[aa_id].RSta = STA_MOD.AANG;
            }
            string NGNGPP = "站号3命令上料产品NGNG开图开图";
            if (data.Equals(NGNGPP))
            {
                ModSta[aa_id].LSta = STA_MOD.PPNG;
                ModSta[aa_id].RSta = STA_MOD.PPNG;
            }
            string STANDY = "站号3命令待机";
            if (data.Equals(STANDY))
            {
                ;
            }
            String STOPSTA = "站号3命令暂停恢复";
            if (data.Equals(STOPSTA))
            {
                
            }
            String CLEAR = "站号3命令清料";
            if (data.Equals(CLEAR))
            {
                VAR.WarnMsg("请手动清料");
                Action.stop(); 
                
            }




        }
        //public enum STA_AA
        //{
        //    [Description("待机")]
        //    STANDAY = "站号3命令待机",
        //    [Description("暂停开始")]
        //    TSTOP = "站号3命令暂停恢复",
        //    [Description("清料")]
        //    RESTAR = "站号3命令清料",
        //    [Description("上料产品OK")]
        //    OK = "站号3命令上料产品OKOK",
        //    [Description("左AANG")]
        //    LAANG = "站号3命令上料产品NGOKAA",
        //    [Description("右AANG")]
        //    RAANG = "站号3命令上料产品OKNGAA",
        //    [Description("AANG")]
        //    AANG = "站号3命令上料产品NGNGAAAA",
        //    [Description("开图NG")]
        //    PPNG = "站号3命令上料产品NGNG开图开图",
        //    [Description("错误")]
        //    ERR,
        //}
        //public static STA_AA Client_Sta;

/// <summary>
/// 转盘安全检测
/// </summary>
/// <param name="id">委托格式未定义</param>
/// <returns></returns>
        public static EM_RES ck_roll_safe(int id)
        {
            EM_RES ret = EM_RES.OK;
            if (cyl_ck_up == null || cyl_ck_up.io_sen_off == null)
                return EM_RES.ERR;
            if (cyl_open == null || cyl_open.io_sen_off == null)
                return EM_RES.ERR;
            if (cyl_close == null || cyl_close.io_sen_off == null)
                return EM_RES.ERR;
            if (!cyl_ck_up.io_sen_off.AssertON())
            {
                ret = cyl_ck_up.SetOff(2000);
                if (ret != EM_RES.OK)
                    return ret;
            }
            if (!cyl_open.io_sen_off.AssertON())
            {
                ret = cyl_open.SetOff(2000);
                if (ret != EM_RES.OK)
                    return ret;
            }
            if (!cyl_close.io_sen_off.AssertON())
            {
                ret = cyl_close.SetOff(2000);
                if (ret != EM_RES.OK)
                    return ret;
            }
            //扩展卡读取异常
            if (!(cyl_ck_up.io_sen_off.AssertON() && cyl_open.io_sen_off.AssertON() && cyl_close.io_sen_off.AssertON()))
                //if (! cyl_open.io_sen_off.AssertON() )
                return EM_RES.ERR;
            ret = WSFeed.ck_ax_safe(0);
            if (ret != EM_RES.OK)
                return ret;
            else
                return EM_RES.OK;
        }
        public static EM_RES mopen(ref bool bquit)
        {
            EM_RES ret;
            ret = cyl_open.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            ret = cyl_open.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            ret = sen_open.WaitON(ref bquit);
            if (ret != EM_RES.OK) return ret;
            return ret;
        }
        public static EM_RES mclose(ref bool bquit)
        {
            EM_RES ret;
            ret = cyl_close.SetOn(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            ret = cyl_close.SetOff(ref bquit, 3000);
            if (ret != EM_RES.OK) return ret;
            ret = sen_closed.WaitON(ref bquit);
            if (ret != EM_RES.OK) return ret;
            return ret;
        }
        //点亮测试
        public static EM_RES mPoint(ref bool bquit)
        {
            EM_RES ret;
            status = EM_STA.POINT;
            try
            {
               ret= cyl_ck_up.SetOn(ref bquit, 3000);
               //发送测试命令，等待测试结果
               Mcom.SendData("BBB0401");
                //等待收到消息
               return ret;
            }
              
            
            finally
            {
               ret= cyl_ck_up.SetOff(ref bquit, 3000);              
            }
           

        }


        public enum EM_STA
        {
            [Description("未知")]
            UNKNOW,
            [Description("忙")]
            BUSY,
            [Description("回零中")]
            HOME,
            [Description("就绪")]
            READY,
            [Description("转动中")]
            ROLL,
            [Description("点亮中")]
            POINT,
            [Description("错误")]
            ERR
        }
       

        public static bool isErr;
        /// <summary>
        /// 委托动作初始化
        /// </summary>
        public static void AllAct()
        {
            if (open == null)
                open = new Mact(mopen);
            if (close == null)
                close = new Mact(mclose);
            if (CKpoint == null)
                CKpoint = new Mact(mPoint);
         
            if (ActHome == null)
                ActHome = new Mact(mHome);
            if (ActRoll == null)
                ActRoll = new Mact(mact_roll);

        }

        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                // if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                return true;
            }
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        public static string GetStaString
        {
            get
            {
                
                return status.GetDescription() + "过程";

            }
        }

        /// <summary>
        /// 执行委托动作函数
        /// </summary>
        /// <param name="act">委托动作</param>
        /// <returns></returns>
        public static EM_RES DoAct(Mact act)  //执行委托
        {
            EM_RES ret;
            try
            {
                if (!isReady) return EM_RES.QUIT;
                AllAct();
                ret = act(ref VAR.gsys_set.bquit);
                if (!isReady) return EM_RES.QUIT;
                if (ret == EM_RES.ERR)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                    //设置系统状态
                    status = EM_STA.ERR;
                    VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, disc + "错误", 0);
                    VAR.gsys_set.bquit = true;
                }
                if (ret == EM_RES.OK)
                {
                    //打印完成动作
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}-执行完成-{1}", disc, GetStaString));
                    status = EM_STA.READY;

                }

                return ret;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "异常", disc, GetStaString));
                //设置系统状态
                status = EM_STA.ERR;
                VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, disc + "错误", 0);
                VAR.gsys_set.bquit = true;              
                return EM_RES.ERR;
            }

        }
        //当前状态
        public static EM_STA status = EM_STA.UNKNOW;
        /// <summary>
        /// 回原
        /// </summary>
        /// <param name="bquit"></param>
        /// <returns></returns>
        public static EM_RES mHome(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            status = EM_STA.HOME;
            if (!isReady) return EM_RES.QUIT;
            res = ck_roll_safe(0);
            if (res != EM_RES.OK) return res;
            res = cyl_ck_up.SetOff(ref bquit, 3000);
            if (res != EM_RES.OK) return res;
            for (int i = 0; i < 12; i++)
            {
                if (!isReady) return EM_RES.QUIT;
                res = mact_roll(ref VAR.gsys_set.bquit);
                if (res != EM_RES.OK) return res;
                Thread.Sleep(10);
                Application.DoEvents();
                if (sen_home.AssertON())
                {
                    
                    return EM_RES.OK;
                }
            }
            status = EM_STA.ERR;
            return EM_RES.ERR;
            //other axis

        }
        public static EM_RES Home(ref bool bquit)
        {
            EM_RES ret = EM_RES.OK;
            AllAct();
            ret = DoAct(ActHome);
            return ret;
        }
 
        /// <summary>
        /// 转盘所有动作函数
        /// </summary>
        public static void act_roll()
        {
            EM_RES ret = EM_RES.OK;
            status = EM_STA.ROLL;
            AllAct();
            if (!bRollOK)
            {
                ret = DoAct(ActRoll);
                if (ret != EM_RES.OK)
                    return;
            }
            else
            if (!bOtherOK)
            {
                if (sen_closed.isON)
                {
                    ret = DoAct(open);
                    if (ret != EM_RES.OK)
                        return;
                }
                if (sen_open.isOFF)
                {
                    ret = DoAct(close);
                    if (ret != EM_RES.OK)
                        return;
                }
                if (ModSta[ck_id].LSta == STA_MOD.UNTEST || ModSta[ck_id].RSta == STA_MOD.UNTEST)
                {
                    ret = DoAct(CKpoint);
                    if (ret != EM_RES.OK)
                        return;
                }
            }

                 
              

        }
        public static EM_RES mact_roll(ref bool bquit)
        {
            EM_RES res = EM_RES.OK;
            try
            {
                status = EM_STA.ROLL;
                int i, j;
                if (!isReady) return EM_RES.QUIT;              
                res = roll_star.SetOff();
                if (res != EM_RES.OK) return res;
                res = ck_roll_safe(0);
                if (res != EM_RES.OK) return res;
                res = cyl_ck_up.SetOff(ref bquit, 3000);
                if (res != EM_RES.OK) return res;
                res = roll_star.SetOn();
                if (res != EM_RES.OK) return res;
                i = 0; j = 0;
                while (roll_topos.isOFF && isReady)
                {
                    Thread.Sleep(5);
                    Application.DoEvents();
                    i++;
                    if (i > 1000)
                    {
                        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "转盘运动等待出位超时", disc, GetStaString));
                        return EM_RES.ERR;
                    }

                }
                while (roll_topos.isON && isReady)
                {

                    Thread.Sleep(5);
                    Application.DoEvents();
                    j++;
                    if (j > 1000)
                    {
                        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "转盘运动等待到位超时", disc, GetStaString));
                        return EM_RES.ERR;
                    }

                }

                res = roll_star.SetOff();
                if (res != EM_RES.OK) return res; 
                res = VacumRoll[ck_id].SetOn();
                if (res != EM_RES.OK) return res;
                if (!(ModSta[get_id].LSta == STA_MOD.UNTEST && ModSta[get_id].RSta == STA_MOD.UNTEST))
                {
                    res = VacumRoll[get_id].SetOff();
                    if (res != EM_RES.OK) return res;
                }
                res = VacumRoll[op_id].SetOn();
                if (res != EM_RES.OK) return res;
                if (!(ModSta[bk_id].LSta == STA_MOD.UNTEST && ModSta[bk_id].RSta == STA_MOD.UNTEST))
                {
                    res = VacumRoll[bk_id].SetOff();
                    if (res != EM_RES.OK) return res;
                }               
                bRollOK = true;
                return EM_RES.OK;
            }
            catch
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}--{1}--" + "位置错误", disc, GetStaString));
                return EM_RES.ERR;
            }
            finally
            {
                roll_star.SetOff();

            }
        }
        public static Task TaskRoll;
        /// <summary>
        /// 转盘动作线程
        /// </summary>
        public static void task_star()
        {
            if (TaskRoll == null || (TaskRoll != null && TaskRoll.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建转盘运动线程");
                if (TaskRoll != null)
                    TaskRoll.Dispose();
                TaskRoll = new Task(act_roll);
                TaskRoll.Start();
            }
        }


    }
    #endregion
    #region 上料
    public static class UploadModle
    {
        //public static TrayBox traybox_fd = COM.traybox_fd;
        //public static AXIS ax_x = MT.AXIS_UL_X;
        //public static AXIS ax_y = MT.AXIS_UL_Y;
        //public static AXIS ax_z = MT.AXIS_UL_Z;
        //public static AXIS axis_z1 = MT.AXIS_UL_U1;
        //public static AXIS axis_z2 = MT.AXIS_UL_U2;
        //public static readonly object xlock = new object();
        //public enum EM_STA
        //{
        //    [Description("未知")]
        //    UNKNOW,
        //    [Description("忙")]
        //    BUSY,
        //    [Description("回零中")]
        //    HOME,
        //    [Description("就绪")]
        //    READY,
        //    [Description("放料")]
        //    PLACE,
        //    [Description("取料")]
        //    PICK,
        //    [Description("错误")]
        //    ERR
        //}
        //public static EM_STA status = EM_STA.UNKNOW;
        ////取料
        ////飞拍
        ////放料
        ///// <summary>
        ///// 下料模块复位
        ///// </summary>
        ///// <param name="bquit"></param>
        ///// <returns></returns>
        //public static EM_RES Home(ref bool bquit)
        //{
        //    EM_RES res = EM_RES.OK;

        //    if (bquit) return EM_RES.QUIT;

        //    //先抬升
        //    res = MT.AxisHome(ref bquit, ax_z);
        //    if (res != EM_RES.OK) return res;

        //    //确保Z原点感应
        //    if (!ax_z.isORG)
        //    {
        //        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 未在原点处，有撞机风险!", ax_z.disc));
        //        return EM_RES.ERR;
        //    }

        //    //检查下料模块是否已经抬起
        ////    if (!DownloadModle.isUp) return EM_RES.MOVE_PROTECT;

        //    //other axis
        //    res = MT.AxisHome(ref bquit, ax_x, ax_y, axis_z1, axis_z2, traybox_fd.ax_x);
        //    if (res != EM_RES.OK) return res;

        //    res = MT.AxisHome(ref bquit, traybox_fd.ax_z);
        //    if (res != EM_RES.OK) return res;

        //    return res;
        //}
        ///// <summary>
        ///// 停止轴运动，停止Home动作
        ///// </summary>
        //public static void Stop()
        //{
        //    ax_x.bhomequit = true;
        //    ax_x.Stop();

        //    ax_y.bhomequit = true;
        //    ax_y.Stop();

        //    axis_z1.bhomequit = true;
        //    axis_z1.Stop();

        //    axis_z2.bhomequit = true;
        //    axis_z2.Stop();

        //    traybox_fd.ax_x.bhomequit = true;
        //    traybox_fd.ax_x.Stop();

        //    traybox_fd.ax_z.bhomequit = true;
        //    traybox_fd.ax_z.Stop();
        //}
    }
    #endregion
    #region 下料
    public static class DownloadModle
    {
        //public static TrayBox traybox_ok = COM.traybox_ok;
        //public static TrayBox traybox_ng = COM.traybox_ng;
        //public static List<Cylinder> List_CLD_UD_HD =MT. List_cyl_get;
        //public static List<Cylinder> List_CLD_HD_HD = MT.List_cyl_back;
        //public static Cylinder CLD_DL_ZK_TRAY_OK = MT.CLD_DL_ZK_TRAY_OK;
        //public static AXIS ax_y = MT.AXIS_DL_Y;
        //public static AXIS ax_z = MT.AXIS_DL_Z;
        //public enum EM_STA
        //{
        //    [Description("未知")]
        //    UNKNOW,
        //    [Description("忙")]
        //    BUSY,
        //    [Description("回零中")]
        //    HOME,
        //    [Description("就绪")]
        //    READY,
        //    [Description("放料")]
        //    PLACE,
        //    [Description("取料")]
        //    PICK,
        //    [Description("错误")]
        //    ERR
        //}
        //public static EM_STA status = EM_STA.UNKNOW;
        ////取料
        ////放料
        ////检查是否抬起
        //public static bool isUp
        //{
        //    get
        //    {
        //        //check Cylinder
        //        foreach (Cylinder cy in List_CLD_UD_HD)
        //        {
        //            if (cy.isOFFByChkSen)
        //            {
        //                Thread.Sleep(300);
        //                if (cy.isOFFByChkSen)
        //                {
        //                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 气缸未抬起!", cy.io_out.disc));
        //                    return false;
        //                }
        //            }
        //        }

        //        //确保Z原点感应
        //        if (!ax_z.isORG)
        //        {
        //            Thread.Sleep(300);
        //            if (!ax_z.isORG)
        //            {
        //                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 未在原点处，有撞机风险!", ax_z.disc));
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //}
        ///// <summary>
        ///// 下料模块复位
        ///// </summary>
        ///// <param name="bquit"></param>
        ///// <returns></returns>
        //public static EM_RES Home(ref bool bquit)
        //{
        //    EM_RES res = EM_RES.OK;

        //    if (bquit) return EM_RES.QUIT;

        //    //确保上料X轴已复位，且位置安全
        //    if (UploadModle.ax_x.home_status != AXIS.HOME_STA.OK)
        //    {
        //        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 未复位，有撞机风险!", UploadModle.ax_x.disc));
        //        return EM_RES.ERR;
        //    }
        //    if (Math.Abs(UploadModle.ax_x.fenc_pos) > 10)
        //    {
        //        VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0} 未在原点附近(<10),X={1:F3}，有撞机风险!", UploadModle.ax_x.disc, UploadModle.ax_x.fenc_pos));
        //        return EM_RES.ERR;
        //    }

        //    //气缸抬升
        //    foreach (Cylinder cy in List_CLD_UD_HD) cy.SetOff();

        //    //先抬升
        //    res = MT.AxisHome(ref bquit, ax_z);
        //    if (res != EM_RES.OK) return res;

        //    //检查是否已经抬起
        //    if (!isUp) return EM_RES.MOVE_PROTECT;

        //    //Y复位
        //    res = MT.AxisHome(ref bquit, ax_y, traybox_ok.ax_x, traybox_ng.ax_x);
        //    if (res != EM_RES.OK) return res;


        //    res = MT.AxisHome(ref bquit, traybox_ok.ax_z, traybox_ng.ax_z);
        //    if (res != EM_RES.OK) return res;

        //    return EM_RES.OK;
        //}
        ///// <summary>
        ///// 停止轴运动，停止Home动作
        ///// </summary>
        //public static void Stop()
        //{
        //    ax_z.bhomequit = true;
        //    ax_z.Stop();

        //    ax_y.bhomequit = true;
        //    ax_y.Stop();

        //    traybox_ok.ax_x.bhomequit = true;
        //    traybox_ok.ax_x.Stop();

        //    traybox_ok.ax_z.bhomequit = true;
        //    traybox_ok.ax_z.Stop();

        //    traybox_ng.ax_x.bhomequit = true;
        //    traybox_ng.ax_x.Stop();

        //    traybox_ng.ax_z.bhomequit = true;
        //    traybox_ng.ax_z.Stop();
        //}
    }
    #endregion
    #region 基本动作
    public static class Action
    {
        public static bool isReady
        {
            get
            {
                if (VAR.gsys_set.bquit) return false;
                if (VAR.gsys_set.bpause) return false;
                // if (VAR.gsys_set.status != EM_SYS_STA.RUN) return false;
                if (VAR.gsys_set.bclose) return false;
                return true;
            }
        }
        static Task run_task = null;
        static Task show_task = null;
       public static bool bNullRun;
        

        public static bool bTaskOUT
        {
            get
            {
                bool bAllOUT = true;
                bAllOUT = task_isOUT(WSBack.TaskRun, WsBuBK.TaskRun, WsBuFD.TaskRun,
                    WSFeed.TaskRun, WSGet.TaskRun, WsTrayMove.TaskRun);
                if (!bAllOUT)
                    return true;
                else return false;
            }
        }


        #region 报警
        public static string ErrMsg
        {
            set
            {
                if (isReady)
                    mMsg = value;
            }
            get
            {

                if (mMsg == "")
                    return "未定义错误！设备停止";
                return mMsg;
            }

        }
        static string mMsg;
        public static MsgShow EShow;//错误显示委托
        public static MsgShow WShow;//警告显示委托

        public static void ErrShowTask()
        {
            if (show_task == null || (show_task != null && show_task.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "显示线程");
                if (show_task != null)
                    show_task.Dispose();
                show_task = new Task(ActErrShow);
                show_task.Start();
                Thread.Sleep(100);
                Application.DoEvents();

            }
        }
        public static void ActErrShow()
        {
            EShow = new UI.MsgShow(ErrShow);
            EShow(ErrMsg);

        }
        public static bool task_isOUT(params Task[] mtask)
        {
            for (int i = 0; i < 5; i++)//检测五遍
            {
                foreach (Task tk in mtask)
                {
                    if (!(tk == null || tk.IsCompleted))

                    { return false; }
                }
            }
            return true;
        }
        public static void ErrShow(string msg = "")
        {
            //设备暂停，蜂鸣开始
            //   VAR.gsys_set.beep_en = true;

            if (msg == "")
                msg = "未定义错误！设备停止";
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}", msg));
            warning fr_warning = new warning();//错误窗体
            fr_warning.TopMost = true;
            fr_warning.BackColor = Color.Red;
            fr_warning.lb_msg.Text = msg;
            fr_warning.ShowDialog();
        }
        public static void MsgShow(string msg = "")
        {
            //设备暂停，蜂鸣开始
            //   VAR.gsys_set.beep_en = true;
            if (msg == "")
                msg = "未定义操作提示";
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.SAVE_WAR, string.Format("{0}", msg));
            warning fr_warning = new warning();//错误窗体
            fr_warning.TopMost = true;
            fr_warning.BackColor = Color.Yellow;
            fr_warning.lb_msg.Text = msg;
            fr_warning.ShowDialog();

        }
        public static DialogResult WarningShow(string msg = "", bool EnCancel = true)
        {
            //设备暂停，蜂鸣开始
            VAR.gsys_set.beep_en = true;
            if (msg == "")
                msg = "未定义警告！";
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.WAR, string.Format("{0}", msg));
            warning fr_warning = new warning();//警告窗体
            if (EnCancel)
                fr_warning.btn_cancle.Visible = true;
            fr_warning.TopMost = true;
            fr_warning.BackColor = Color.Yellow;
            fr_warning.lb_msg.Text = msg;
            fr_warning.ShowDialog();
            VAR.gsys_set.beep_en = false;
            return fr_warning.DialogResult;
        }
        public static EM_RES WarnShow(EM_ALM_STA type = EM_ALM_STA.WAR_YELLOW, string str = "")
        {
            DialogResult res = DialogResult.OK;
            if (!isReady) return EM_RES.QUIT;
            //设备暂停，蜂鸣开始
            VAR.gsys_set.beep_en = true;
            EM_RES ret = EM_RES.OK;
            switch (type)
            {
                case EM_ALM_STA.WAR_YELLOW:
                    res = Action.WarningShow(str + "异常,是否停止？?");
                    if (res == DialogResult.Cancel)
                        ret = EM_RES.OK;
                    else
                        ret = EM_RES.ERR;
                    break;
                case EM_ALM_STA.WAR_RED:
                    Action.ErrShow(str + "设备停止");
                    ret = EM_RES.ERR;
                    break;
                case EM_ALM_STA.NOR_BLUE:
                    Action.MsgShow(str + "提示");
                    ret = EM_RES.OK;
                    break;
                case EM_ALM_STA.NOR_GREEN:
                    ret = EM_RES.OK;
                    break;
                default:
                    ret = EM_RES.OK;
                    break;
            }
            VAR.gsys_set.beep_en = false;
            if (ret == EM_RES.ERR)
                VAR.gsys_set.status = EM_SYS_STA.ERR;
            return ret;
        }
        #endregion
        #region 运行
       public  static EM_RES Update_pro()
        {
            EM_RES ret = MT.PosInit();
            if (ret != EM_RES.OK)
            return ret;        
            VAR.gsys_set.SaveSysCfg();
            ret = COM.product.LoadDat(VAR.gsys_set.cur_product_name);
            if (ret != EM_RES.OK) return ret;
            ret = COM.MVS.LoadInf();
            if (ret != EM_RES.OK) return ret;
            return ret;
        }

        public static void th_run()
        {
            if (run_task == null || (run_task != null && run_task.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建运行线程");
                if (run_task != null)
                    run_task.Dispose();
                run_task = new Task(run_th);
                run_task.Start();

                MT.GPIO_OUT_belet.SetOff();//停止皮带
            }
        }
        public static void run_get()
        {
            EM_RES ret = EM_RES.OK;
            VAR.gsys_set.bquit = false;
            bool bhomeerr = false;

            while (true)
            {
                //if ((StationGet.status == StationGet.EM_STA.UNKNOW) && (!bhomeerr))
                //{
                //   StationGet. ErrShow("请先取料复位");
                //    bhomeerr = true;

                //}


            }
        }
        public static void run_th()
        {
            int n;
            EM_RES ret;
            bool bsafe = false;//安全门检测
            bool bkeystart = false;//按键开始
            bool bkeystop = false;//按键停止
            bool bemg = false;//急停按键

            int t_temp = System.Environment.TickCount;//系统毫秒时间
            int tmr_wl = System.Environment.TickCount;//计时

            bool brun = true;//运行控制

            bool bnew = false;//未定义
            bool bstandy = false;//就绪进入
            bool bHome = true;
            int standby_cnt;//就绪次数

            //初始化运行条件
            VAR.gsys_set.bpause = false;
            VAR.gsys_set.bquit = false;
            VAR.gsys_set.bclose = false;
            VAR.gsys_set.status = EM_SYS_STA.RUN;
            COUNT_DATA.ct_pause = 0;
            COUNT_DATA.tmr_wl = 0;
            MT.GPIO_OUT_light.SetOn();//开灯
            //检测所有轴已经回原
            //gy0114
            //foreach (AXIS ax in MT.AxList_ALL)
            //{
            //    if (ax.home_status != AXIS.HOME_STA.OK)
            //        bHome = false;
            //}

            //if (!bHome)
            //{
            //    VAR.sys_inf.Set(EM_ALM_STA.NOR_BLUE, "请复位", -1);
            //    return;
            //}
            //检测所有线程已经退出
            //停止所有线程
            while (VAR.gsys_set.bclose == false && VAR.gsys_set.bquit == false)
            {
                Thread.Sleep(10);
                Application.DoEvents();
                if (!brun) goto RUN_STAGE;
                    VAR.gsys_set.status = EM_SYS_STA.RUN;
                    VAR.sys_inf.Set(EM_ALM_STA.NOR_BLUE, "正在运行", -1);
                    tmr_wl = System.Environment.TickCount;
                    if (!WSROLL.bRollOK && (WSROLL.TaskRoll == null || WSROLL.TaskRoll.IsCompleted))
                    {
                        WSROLL.task_star();
                        WSBack.bOK = false;
                        WSFeed.bOK = false;
                        WSGet.bOK = false;
                        Thread.Sleep(500);
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    else
                    {

                        if (!WSGet.bOK && WSGet.TaskRun == null || WSGet.TaskRun.IsCompleted)
                            WSGet.task_run();
                        Thread.Sleep(500);
                        Thread.Sleep(500);
                        Application.DoEvents();
                        if (!WSBack.bOK && WSBack.TaskRun == null || WSBack.TaskRun.IsCompleted)
                            WSBack.task_run();
                        if (!WSFeed.bOK && WSFeed.TaskRun == null || WSFeed.TaskRun.IsCompleted)
                            WSFeed.task_run();
                        if (WSGet.bOK && WSBack.bOK && WSFeed.bOK)
                            WSROLL.bRollOK = false;
                        Thread.Sleep(500);
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
              

                #region 状态更新
                if ((WsBuFD.TaskRun == null || WsBuFD.TaskRun.IsCompleted) && (WsBuBK.TaskRun == null || WsBuBK.TaskRun.IsCompleted))
                {

                }
                #endregion
                #region  按键处理

            //if (MT.GPIO_IN_key_start.AssertON())
            //{
            //    if (bkeystart == false)
            //    {
            //        //MT.GPIO_OUT_KL_RESET.SetOff();
            //        bkeystart = true;
            //        VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "开始按键按下");
            //        if (MT.isSafeSen)
            //        {

                //            //TD.Task_Start();
            //            //非就绪，暂停 则继续，不能从新开始运行
            //            if (VAR.gsys_set.status != EM_SYS_STA.STANDBY && VAR.gsys_set.status != EM_SYS_STA.PAUSE) continue;
            //            if (bnew == false && VAR.gsys_set.status != EM_SYS_STA.RUN && VAR.gsys_set.status != EM_SYS_STA.PAUSE)
            //            {
            //                //初始化速度等
            //                MT.SetAllAxToWorkSpd();
            //                VAR.gsys_set.bpause = false;
            //                brun = true;
            //            }
            //            else if (VAR.gsys_set.status == EM_SYS_STA.PAUSE)
            //            {
            //                //初始化速度等
            //                MT.SetAllAxToWorkSpd();
            //                //暂停时，重新启动
            //                VAR.gsys_set.bpause = false;
            //                //等待线程处理异常
            //                Thread.Sleep(500);

                //                //如果已经退出，则重新启动
            //                //TD.Task_Start();
            //            }
            //        }
            //    }
            //}
            //else bkeystart = false;
            //if (false)
            //{
            //    if (bkeypause == false)
            //    {
            //        VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "暂停按键按下");
            //        bpause_kl = true;
            //        bkeypause = true;
            //        VAR.sys_inf.Set(CONST.EM_ALM_STA.NOR_BLUE, "暂停", 100, true);
            //        VAR.gsys_set.bpause = true;
            //    }
            //}
            //else bkeypause = false;

                //if (MT.GPIO_IN_emg_key2.AssertON())
            //{
            //    if (bkeystop == false)
            //    {
            //        bkeystop = true;
            //        //光栅保护
            //        if (!MT.isSafeSen) continue;

                //        VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "停止按键按下");
            //        if (VAR.gsys_set.status != EM_SYS_STA.RUN)
            //        {
            //            VAR.sys_inf.Set(EM_ALM_STA.NOR_GREEN, "就绪", 100, true);
            //            MT.GPIO_OUT_KL_STOP.SetOn();
            //        }
            //        else
            //        {
            //            VAR.gsys_set.bquit = true;
            //            //TD.Task_Stop();
            //        }
            //    }
            //}
            //else bkeystop = false;

                //if (!MT.isSafeSen)
            //{
            //    if (bsafe == false) VAR.msg.AddMsg(Msg.EM_MSGTYPE.WAR, "安全光栅/门锁触发1");
            //    bsafe = true;
            //    if (VAR.gsys_set.status == EM_SYS_STA.RUN)
            //    {
            //        VAR.sys_inf.Set(EM_ALM_STA.WAR_YELLOW_FLASH, "安全防护");
            //        VAR.gsys_set.bpause = true;
            //    }
            //}
            //else bsafe = false;            
            //if (MT.GPIO_IN_EMG.AssertOFF())
            //{
            //    VAR.gsys_set.status = EM_SYS_STA.EMG;
            //    VAR.gsys_set.bpause = true;
            //    VAR.gsys_set.bquit = true;
            //    brun = false;
            //    VAR.sys_inf.Set(EM_ALM_STA.WAR_RED_FLASH, "急停");
            //    if (bemg == false) VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "急停按键按下");
            //    bemg = true;
            //}
            //else bemg = false;
                #endregion
            RUN_STAGE:
                Thread.Sleep(500);
                
            }

            //int t = Environment.TickCount;          
            //t = Environment.TickCount - t;
            //VAR.msg.AddMsg(Msg.EM_MSGTYPE.NOR, "U T=" + t.ToString());
            Thread.Sleep(100);
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "运行线程结束");
        }

        #endregion
        #region 保存读取
        public static EM_RES LoadCfg()
        {
            //产品参数
            string filename = string.Format("{0}\\syscfg\\syscfg.ini", Path.GetFullPath(".."));
            if (!File.Exists(filename))
            {
                File.Create(filename);
                //VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}初始化对应产品名{1}配置文件不存在!", disc, productname));
                //return EM_RES.PARA_ERR;
            }
            IniFile inf = new IniFile(filename);
            string section = string.Format("RUNcfg");
           // row = inf.ReadInteger(section, "TARY_ROW", 1);
            return EM_RES.OK;
        }

        public static EM_RES SavCfg()
        {
     
            //产品参数
            string filename = string.Format("{0}\\syscfg\\syscfg.ini", Path.GetFullPath(".."));

            if (!File.Exists(filename))
            {
                File.Create(filename);
                //   VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}初始化对应产品名{1}配置文件不存在!", disc, productname));
                // return EM_RES.PARA_ERR;
            }

            IniFile inf = new IniFile(filename);
            string section = string.Format("RUNcfg");
           // inf.WriteInteger(section, "TARY_ROW", row);
            return EM_RES.OK;
        }
        #endregion
        #region 轴安全
        //public static Task ws_bull_move = null;

        #endregion
        #region 取料
        static Task ws_get_run = null;
        public static void th_ws_get_run()
        {
            if (ws_get_run == null || (ws_get_run != null && ws_get_run.IsCompleted))
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "创建取料线程");
                if (ws_get_run != null)
                    ws_get_run.Dispose();
                ws_get_run = new Task(WSGet.act_run);
                ws_get_run.Start();
                //  ws_get_run.Wait();

            }
        }


        public static void run_get_test()
        {

            MT.GPIO_OUT_belet.SetOn();
            Thread.Sleep(2000);
            MT.GPIO_OUT_belet.SetOff();
            WSGet.Home(ref VAR.gsys_set.bquit);

        }


        //    }

        #endregion
        #region 停止
        public static void stop()
        {
            int n;
            if (VAR.gsys_set.status == EM_SYS_STA.EMG || VAR.gsys_set.status == EM_SYS_STA.ERR || VAR.gsys_set.status == EM_SYS_STA.UNKOWN)
            {
                MT.AllAxStop();
                //    return;
            }
            //唤醒再停止
            VAR.gsys_set.bpause = false;
         
          
            for (n = 0; n < 50; n++)
            {
                VAR.gsys_set.bquit = true;
                Thread.Sleep(10);
                Application.DoEvents();
            }     
               if (VAR.gsys_set.status != EM_SYS_STA.EMG && VAR.gsys_set.status != EM_SYS_STA.ERR && VAR.gsys_set.status != EM_SYS_STA.UNKOWN)
                    VAR.gsys_set.status = EM_SYS_STA.STANDBY;
            
        }
        #endregion
        #region 暂停
        //public static bool pause(ref EM_SYS_STA status, ref bool bquit, bool bquit2 = false)
        //{
        //    bool bpause = false;
        //    //if (VAR.gsys_set.bpause == true || MT.GPIO_IN_FR_DOOR.isOFF)
        //    //{
        //    //    bpause = true;
        //    //    VAR.gsys_set.bpause = true;
        //    //    if (VAR.gsys_set.status != EM_SYS_STA.PAUSE) MT.Beeper(100);
        //    //}

        //    //while ((VAR.gsys_set.mode & EM_SYS_MODE.STEP) == EM_SYS_MODE.STEP && VAR.gsys_set.status == EM_SYS_STA.RUN || (VAR.gsys_set.bpause == true || MT.GPIO_IN_FR_DOOR.isOFF))
        //    //{
        //        //if (VAR.gsys_set.bpause == true || MT.GPIO_IN_FR_DOOR.isOFF)
        //        //{
        //        //    status = EM_SYS_STA.PAUSE;
        //        //    VAR.gsys_set.status = EM_SYS_STA.PAUSE;
        //        //}
        //        ////继续运行
        //        //if (MT.GPIO_IN_key_start.AssertON())
        //        //{
        //        //    VAR.sys_inf.Set(EM_ALM_STA.NOR_GREEN, "运行", 0, true);
        //        //    break;
        //        //}
        //        ////复位键退出
        //        //if (MT.GPIO_IN_KEY_STOP.AssertON())
        //        //{
        //        //    VAR.sys_inf.Set(EM_ALM_STA.NOR_GREEN, "运行", 0, true);
        //        //    //if (!VAR.isStepMode)
        //        //    bquit = true;
        //        //    break;
        //        //}
        //    //    if (bquit || bquit2) break;

        //    //    //发生错误
        //    //    if (VAR.gsys_set.status == EM_SYS_STA.EMG || VAR.gsys_set.status == EM_SYS_STA.ERR || VAR.gsys_set.status == EM_SYS_STA.UNKOWN)
        //    //    {
        //    //        bquit = true;
        //    //        break;
        //    //    }
        //    //    Thread.Sleep(10);
        //    //    Application.DoEvents();
        //    //}
        //    //检查系统状态
        //    //if (VAR.gsys_set.status == EM_SYS_STA.RUN || VAR.gsys_set.status == EM_SYS_STA.PAUSE)
        //    //{
        //    //    status = EM_SYS_STA.RUN;
        //    //    VAR.gsys_set.status = EM_SYS_STA.RUN;
        //    //    VAR.sys_inf.Set(EM_ALM_STA.NOR_GREEN, "运行", 0, true);
        //    //}
        //    //else
        //    //{
        //    //    //bquit = true;
        //    //}

        //    //return bpause;
        //}
        #endregion
        #region 退出
        public static EM_RES close()
        {
            //stop
            stop();
            VAR.gsys_set.bclose = true;
            Thread.Sleep(100);
            Application.DoEvents();
            Thread.Sleep(100);
            Application.DoEvents();
            for (int n = 0; n < 100; n++)
            {
                VAR.gsys_set.bquit = true;
                VAR.gsys_set.bpause = false;
                VAR.gsys_set.bclose = true;
                Thread.Sleep(10);
                Application.DoEvents();
            }

            VAR.gsys_set.bquit = false;
            //MT.ResetIO();

            //close card
            MT.Close();

            ////close vison
            //CogFrameGrabbers FG_List = new CogFrameGrabbers();
            //if (FG_List.Count > 0)
            //{
            //    for (int i = 0; i < FG_List.Count; i++)
            //    {
            //        FG_List[i].Disconnect(true);
            //    }
            //}
            return EM_RES.OK;
        }
        #endregion
        #region 执行
        public static EM_RES pos_to()
        {
            //stop
            stop();
            VAR.gsys_set.bclose = true;
            Thread.Sleep(100);
            Application.DoEvents();
            Thread.Sleep(100);
            Application.DoEvents();
            for (int n = 0; n < 100; n++)
            {
                VAR.gsys_set.bquit = true;
                VAR.gsys_set.bpause = false;
                VAR.gsys_set.bclose = true;
                Thread.Sleep(10);
                Application.DoEvents();
            }

            VAR.gsys_set.bquit = false;
            //MT.ResetIO();

            //close card
            MT.Close();

            ////close vison
            //CogFrameGrabbers FG_List = new CogFrameGrabbers();
            //if (FG_List.Count > 0)
            //{
            //    for (int i = 0; i < FG_List.Count; i++)
            //    {
            //        FG_List[i].Disconnect(true);
            //    }
            //}
            return EM_RES.OK;
        }
        #endregion

        
    }
    #endregion
    #region 按键HOOK钩子
    public class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13; //键盘 

        private const int WM_KEYDOWN = 0x100;//KEYDOWN

        private const int WM_KEYUP = 0x101;  //KEYUP

        private const int WM_SYSKEYDOWN = 0x104; //SYSKEYDOWN

        private const int WM_SYSKEYUP = 0x105;  //SYSKEYUP




        public event KeyEventHandler KeyDownEvent;
        public event KeyPressEventHandler KeyPressEvent;
        public event KeyEventHandler KeyUpEvent;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0; //声明键盘钩子处理的初始值
        //值在Microsoft SDK的Winuser.h里查询
        HookProc KeyboardHookProcedure; //声明KeyboardHookProcedure作为HookProc类型
        //键盘结构 
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;  //定一个虚拟键码。该代码必须有一个价值的范围1至254
            public int scanCode; // 指定的硬件扫描码的关键
            public int flags;  // 键标志
            public int time; // 指定的时间戳记的这个讯息
            public int dwExtraInfo; // 指定额外信息相关的信息
        }
        //使用此功能，安装了一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);


        //调用此函数卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);


        //使用此功能，通过信息钩子继续下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
        // 取得当前线程编号（线程钩子需要用到） 
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void Start()
        {
            // 安装键盘钩子
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                //************************************ 
                //键盘线程钩子 
                //SetWindowsHookEx( 2,KeyboardHookProcedure, IntPtr.Zero, GetCurrentThreadId());//指定要监听的线程idGetCurrentThreadId(),
                //键盘全局钩子,需要引用空间(using System.Reflection;) 
                //SetWindowsHookEx( 13,MouseHookProcedure,Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),0); 
                // 
                //关于SetWindowsHookEx (int idHook, HookProc lpfn, IntPtr hInstance, int threadId)函数将钩子加入到钩子链表中，说明一下四个参数： 
                //idHook 钩子类型，即确定钩子监听何种消息，上面的代码中设为2，即监听键盘消息并且是线程钩子，如果是全局钩子监听键盘消息应设为13， 
                //线程钩子监听鼠标消息设为7，全局钩子监听鼠标消息设为14。lpfn 钩子子程的地址指针。如果dwThreadId参数为0 或是一个由别的进程创建的 
                //线程的标识，lpfn必须指向DLL中的钩子子程。 除此以外，lpfn可以指向当前进程的一段钩子子程代码。钩子函数的入口地址，当钩子钩到任何 
                //消息后便调用这个函数。hInstance应用程序实例的句柄。标识包含lpfn所指的子程的DLL。如果threadId 标识当前进程创建的一个线程，而且子 
                //程代码位于当前进程，hInstance必须为NULL。可以很简单的设定其为本应用程序的实例句柄。threaded 与安装的钩子子程相关联的线程的标识符
                //如果为0，钩子子程与所有的线程关联，即为全局钩子
                //************************************ 
                //如果SetWindowsHookEx失败
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("安装键盘钩子失败");
                }
            }
        }
        public void Stop()
        {
            bool retKeyboard = true;


            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            if (!(retKeyboard)) throw new Exception("卸载钩子失败！");
        }
        //ToAscii职能的转换指定的虚拟键码和键盘状态的相应字符或字符
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, //[in] 指定虚拟关键代码进行翻译。 
                                         int uScanCode, // [in] 指定的硬件扫描码的关键须翻译成英文。高阶位的这个值设定的关键，如果是（不压）
                                         byte[] lpbKeyState, // [in] 指针，以256字节数组，包含当前键盘的状态。每个元素（字节）的数组包含状态的一个关键。如果高阶位的字节是一套，关键是下跌（按下）。在低比特，如果设置表明，关键是对切换。在此功能，只有肘位的CAPS LOCK键是相关的。在切换状态的NUM个锁和滚动锁定键被忽略。
                                         byte[] lpwTransKey, // [out] 指针的缓冲区收到翻译字符或字符。 
                                         int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 

        //获取按键的状态
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 侦听键盘事件
            if ((nCode >= 0) && (KeyDownEvent != null || KeyUpEvent != null || KeyPressEvent != null))
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                // raise KeyDown
                if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDownEvent.Invoke(this, e);
                }

                //键盘按下
                if (KeyPressEvent != null && wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.vkCode, MyKeyboardHookStruct.scanCode, keyState, inBuffer, MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        KeyPressEvent.Invoke(this, e);
                    }
                }

                // 键盘抬起 
                if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUpEvent.Invoke(this, e);
                }

            }
            //如果返回1，则结束消息，这个消息到此为止，不再传递。
            //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者 
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
    }

    #endregion
    #region 统计数据
    public static class COUNT_DATA
    {
        public static bool bct_page_start = false;  //单张贴补时间计时开始
        public static int cnt_page_ttl;     //总贴补张数
        public static int cnt_pcs_ttl;      //总贴补次数
        public static int cnt_page;        //今日贴补张数
        public static int cnt_pcs;         //今日贴补次数
        public static int cnt_pcs_pl;      //今日抛料次数

        public static int ct_fdh;           //机械手进料用时
        public static int ct_fly;           //飞拍用时
        public static int ct_mark;          //目标定位用时
        public static int ct_tf;            //贴附用时
        public static int ct_pcs;           //单次贴补时间
        public static int ct_page;          //单张贴补时间 
        public static int ct_pause;         //单张暂停时间 
        public static int cnt_per_page;     //单张贴补次数

        public static int cnt_vs_ng;       //视觉识别失败次数 
        public static int cnt_fd_ng;       //供料失败次数    
        public static int cnt_tf_ng;       //贴付失败次数          

        public static int runtime;         //运行时间sec
        public static int waittime;        //运行时间sec
        public static int tmr_wl;          //涡轮空闲计时
        public static int tmr_no_op;       //空闲计时
        public static DateTime dt;         //日期

        private static int cnt_pcs_temp;
        private static int timer_temp;

        #region  加载
        public static void LoadDat(string productname)
        {
            string fileroad = Path.GetFullPath("..") + "\\product\\" + productname + "\\cfg.inf";
            IniFile inf = new IniFile(fileroad);

            cnt_pcs_ttl = inf.ReadInteger("CNT", "CNT_PCS_TTL", cnt_pcs_ttl);
            cnt_page_ttl = inf.ReadInteger("CNT", "CNT_PAGE_TTL", cnt_page_ttl);

            cnt_pcs = inf.ReadInteger("CNT", "CNT_PCS", cnt_pcs);
            cnt_page = inf.ReadInteger("CNT", "CNT_PAGE", cnt_page);

            cnt_pcs_pl = inf.ReadInteger("CNT", "CNT_PCS_PL", cnt_pcs_pl);

            ct_pcs = inf.ReadInteger("CNT", "CT_PCS", ct_pcs);
            ct_page = inf.ReadInteger("CNT", "CT_PAGE", ct_page);
            runtime = inf.ReadInteger("CNT", "RUNTIME", runtime);
            waittime = inf.ReadInteger("CNT", "WAITTIME", waittime);
            dt = Convert.ToDateTime(inf.ReadString("CNT", "DT", dt.ToString()));

            cnt_pcs_temp = cnt_pcs;
        }
        #endregion
        #region  保存
        public static void SaveDat(string productname, bool bsave = false)
        {
            if (!bsave && (cnt_pcs_temp == cnt_pcs) && (runtime - timer_temp) < 60) return;
            cnt_pcs_temp = cnt_pcs;
            timer_temp = runtime;

            string fileroad = Path.GetFullPath("..") + "\\product\\" + productname + "\\cfg.inf";
            IniFile inf = new IniFile(fileroad);

            inf.WriteInteger("CNT", "CNT_PCS_TTL", cnt_pcs_ttl);
            inf.WriteInteger("CNT", "CNT_PAGE_TTL", cnt_page_ttl);

            inf.WriteInteger("CNT", "CNT_PCS", cnt_pcs);
            inf.WriteInteger("CNT", "CNT_PAGE", cnt_page);

            inf.WriteInteger("CNT", "CNT_PCS_PL", cnt_pcs_pl);

            inf.WriteInteger("CNT", "CT_PCS", ct_pcs);
            inf.WriteInteger("CNT", "CT_PAGE", ct_page);
            inf.WriteInteger("CNT", "RUNTIME", runtime);
            inf.WriteInteger("CNT", "WAITTIME", waittime);
            inf.WriteString("CNT", "DT", dt.ToString());
        }
        #endregion
        #region 清零
        public static void Clear()
        {
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}pcs,{1}张，抛料{2}，运行时间{3:0.0}h,待机时间{4:0.0}h", cnt_pcs, cnt_page, cnt_pcs_pl, (double)(runtime / 3600), (double)(waittime / 3600)));
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("上次清零时间:{0}", dt.ToString()));
            cnt_page = 0;
            cnt_pcs = 0;
            cnt_pcs_pl = 0;
            cnt_vs_ng = 0;
            cnt_fd_ng = 0;
            cnt_tf_ng = 0;
            runtime = 0;
            waittime = 0;
            dt = System.DateTime.Now;
        }

        #endregion
    }
    #endregion
    #region 绘图
    //public class TG_DRAW
    //{
    //    public ST_XY st_ul_pos;    // mm
    //    public ST_XY st_sc;        // mm/pix
    //    int w = 30;             // pix
    //    int h = 30;             // pix
    //    int bdir_x = 1;
    //    int bdir_y = 1;
    //    public bool bmousedonw = false;
    //    public bool bsel_rect = false;
    //    Rectangle sel_rect;
    //    Point pt_start = new Point();
    //    List<Rectangle> list_area = new List<Rectangle>();

    //    #region 初始化
    //    public int Init(List<TG_DATA> list_tg, int pic_width, int pic_height)
    //    {
    //        Rectangle rect = new Rectangle();
    //        double min_dis = 0;
    //        int ret = GetTgMaxRegion(list_tg, ref rect, ref min_dis);
    //        if (ret == CONST.RES_OK)
    //        {
    //            st_sc.x = (pic_width < 0 ? -1 : 1) * (double)rect.Width / (double)(Math.Abs(pic_width) - 30.0 * 2) * bdir_x;
    //            st_sc.y = (pic_height < 0 ? -1 : 1) * (double)rect.Height / (double)(Math.Abs(pic_height) - 30.0 * 2) * bdir_y;
    //            st_ul_pos.x = rect.X - st_sc.x * 30 + (pic_width < 0 ? rect.Width : 0);
    //            st_ul_pos.y = rect.Y - st_sc.y * 30 + (pic_height < 0 ? rect.Height : 0);

    //            w = 30;
    //            if ((int)Math.Abs(min_dis / st_sc.x) < 30) w = (int)Math.Abs(min_dis / st_sc.x);
    //            if (w < 10) w = 10;
    //            h = w;
    //        }
    //        return ret;
    //    }
    //    public TG_DRAW()
    //    { }
    //    public TG_DRAW(List<TG_DATA> list_tg, int pic_width, int pic_height)
    //    {
    //        Init(list_tg, pic_width, pic_height);
    //    }
    //    public TG_DRAW(VAR.ST_XY st_ul_pos, VAR.ST_XY st_sc, int w = 30, int h = 30)
    //    {
    //        this.st_sc = st_sc;
    //        this.st_ul_pos = st_ul_pos;
    //        this.w = w;
    //        this.h = h;
    //    }
    //    #endregion
    //    #region 坐标转换
    //    public Point Pix2MM(int x, int y)
    //    {
    //        Point pt = new Point();
    //        pt.X = (int)(st_ul_pos.x + x * st_sc.x);
    //        pt.Y = (int)(st_ul_pos.y + y * st_sc.y);
    //        return pt;
    //    }
    //    public VAR.ST_XY MM2Pix(double x, double y)
    //    {
    //        VAR.ST_XY st_pos = new VAR.ST_XY();
    //        st_pos.x = (x - st_ul_pos.x) / st_sc.x;
    //        st_pos.y = (y - st_ul_pos.y) / st_sc.y;
    //        return st_pos;
    //    }
    //    public Rectangle MM2Pix(Rectangle rect)
    //    {
    //        rect.X = (int)(((double)rect.X - st_ul_pos.x) / st_sc.x);
    //        rect.Y = (int)(((double)rect.Y - st_ul_pos.y) / st_sc.y);
    //        rect.Width = (int)(((double)rect.Width - 0) / st_sc.x);
    //        rect.Height = (int)(((double)rect.Height - 0) / st_sc.y);
    //        return rect;
    //    }
    //    public PointF MM2Pix(VAR.ST_XYZ st_pos_mm)
    //    {
    //        VAR.ST_XY pos = MM2Pix(st_pos_mm.x, st_pos_mm.y);
    //        return new PointF((float)pos.x, (float)pos.y);
    //    }
    //    public Rectangle Pos2Rec(VAR.ST_XYZ st_pos_mm)
    //    {
    //        PointF ptf = MM2Pix(st_pos_mm);
    //        return new Rectangle((int)(ptf.X - w / 2), (int)(ptf.Y - h / 2), w, h);
    //    }
    //    double DisOfP2P(VAR.ST_XYZ pt1, VAR.ST_XYZ pt2)
    //    {
    //        double dx = pt1.x - pt2.x;
    //        double dy = pt1.y - pt2.y;
    //        return Math.Sqrt(dx * dx + dy * dy);
    //    }
    //    #endregion
    //    #region 计算最小包围区域，目标最小距离
    //    public Rectangle GetTgMaxRegion(List<TG_DATA> list_tg)
    //    {
    //        Rectangle rect = new Rectangle(0, 0, -1, -1);
    //        if (list_tg.Count == 0) return rect;

    //        //最小包含区域  
    //        double min_x = double.MaxValue;
    //        double max_x = double.MinValue;
    //        double min_y = double.MaxValue;
    //        double max_y = double.MinValue;
    //        foreach (TG_DATA tg in list_tg)
    //        {
    //            if (min_x > tg.st_pos_cap_set.x) min_x = tg.st_pos_cap_set.x;
    //            if (max_x < tg.st_pos_cap_set.x) max_x = tg.st_pos_cap_set.x;

    //            if (tg.bBM)
    //            {
    //                if (min_x > tg.st_pos_bm_set.x) min_x = tg.st_pos_bm_set.x;
    //                if (max_x < tg.st_pos_bm_set.x) max_x = tg.st_pos_bm_set.x;
    //            }

    //            if (min_y > tg.st_pos_cap_set.y) min_y = tg.st_pos_cap_set.y;
    //            if (max_y < tg.st_pos_cap_set.y) max_y = tg.st_pos_cap_set.y;

    //            if (tg.bBM)
    //            {
    //                if (min_y > tg.st_pos_bm_set.y) min_y = tg.st_pos_bm_set.y;
    //                if (max_y < tg.st_pos_bm_set.y) max_y = tg.st_pos_bm_set.y;
    //            }
    //        }

    //        rect.X = (int)(bdir_x == 1 ? min_x : max_x);
    //        rect.Y = (int)(bdir_y == 1 ? min_y : max_y);
    //        rect.Width = (int)Math.Abs(max_x - min_x);
    //        rect.Height = (int)Math.Abs(max_y - min_y);

    //        return rect;
    //    }
    //    public double GetTgMinDis(List<TG_DATA> list_tg)
    //    {
    //        //目标间最小距离
    //        double min_dis = double.MaxValue;
    //        double dis = 0;
    //        foreach (TG_DATA tg in list_tg)
    //        {
    //            if (tg.name == "BC" || tg.name == "MK") continue;
    //            foreach (TG_DATA tg_temp in list_tg)
    //            {
    //                if (tg.name == "BC" || tg.name == "MK") continue;
    //                if (tg.id == tg_temp.id) continue;
    //                dis = DisOfP2P(tg.st_pos_cap_set, tg_temp.st_pos_cap_set);
    //                if (min_dis > dis) min_dis = dis;
    //            }
    //        }
    //        return min_dis == double.MaxValue ? 0 : min_dis;
    //    }
    //    public int GetTgMaxRegion(List<TG_DATA> list_tg, ref Rectangle rect, ref double min_dis)
    //    {
    //        if (list_tg.Count == 0) return CONST.RES_PARA_ERR;

    //        rect = GetTgMaxRegion(list_tg);
    //        min_dis = GetTgMinDis(list_tg);
    //        if (rect.Width < 0 || rect.Height < 0 || min_dis == 0) return CONST.RES_ERR;
    //        else return CONST.RES_OK;
    //    }
    //    #endregion
    //    #region 生成区域
    //    public int CreateAreaRect(params List<TG_DATA>[] list_list_tg)
    //    {
    //        TARGET target = new TARGET();

    //        List<TG_DATA> list_area_tg = new List<TG_DATA>();
    //        list_area.Clear();
    //        for (int n = 0; n < 8; n++)
    //        {
    //            list_area_tg.Clear();
    //            foreach (List<TG_DATA> list_tg in list_list_tg)
    //            {
    //                list_area_tg.AddRange(target.GetTgByArea(n, list_tg));
    //            }
    //            Rectangle rect = GetTgMaxRegion(list_area_tg);
    //            rect = MM2Pix(rect);
    //            if (rect.Width < 0)
    //            {
    //                rect.Width = -rect.Width;
    //                rect.X -= rect.Width;
    //            }
    //            if (rect.Height < 0)
    //            {
    //                rect.Height = -rect.Height;
    //                rect.Y -= rect.Height;
    //            }
    //            rect.X -= w / 2 + 5;
    //            rect.Y -= h / 2 + 5;
    //            rect.Width += w + 10;
    //            rect.Height += h + 10;
    //            list_area.Add(rect);
    //        }
    //        return list_area.Count;
    //    }
    //    #endregion
    //    #region 目标颜色
    //    public Color GetColorByName(string tgname)
    //    {
    //        switch (tgname)
    //        {
    //            case "TA": return Color.DodgerBlue;
    //            case "TB": return Color.Lime;
    //            case "TC": return Color.Violet;
    //            case "TD": return Color.SkyBlue;
    //            case "MK": return Color.Orange;
    //            case "BC": return Color.Yellow;
    //            case "SEL": return Color.Red;
    //            case "ERR": return Color.Red;
    //            case "STA": return Color.Gray;
    //        }
    //        return Color.White;
    //    }
    //    #endregion
    //    #region 绘制单个目标
    //    public void DrawTg(TG_DATA tg, ref Graphics gg, bool bSolid = true, bool bMTF = false, bool bBM = false, bool bECNUM = false)
    //    {
    //        Color cl_sel = GetColorByName("SEL");
    //        Color cl_tg = GetColorByName(tg.name);
    //        Color cl_err = GetColorByName("ERR");
    //        Color cl_sta = GetColorByName("STA");

    //        Pen p = new Pen(Color.DodgerBlue, 3);

    //        if (gg == null) return;
    //        if (st_sc.x == 0 || st_sc.y == 0) return;

    //        if (bSolid)
    //        {
    //            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
    //            p.Width = 3;
    //        }
    //        else
    //        {
    //            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
    //            p.Width = 1;
    //        }

    //        if (tg.bselected) p.Color = cl_sel;
    //        else if (tg.btf_h_err) p.Color = cl_err;
    //        else if (tg.status > 0) p.Color = cl_sta;
    //        else p.Color = cl_tg;

    //        //显示非MARK点
    //        if (tg.name != "MK" && tg.name != "BC")
    //        {
    //            gg.DrawRectangle(p, Pos2Rec(tg.st_pos_cap_set));
    //            //手动补贴时候打X
    //            if (bMTF && tg.bmtf == false || tg.status < 0 || tg.cap_mode == TG_DATA.EM_CAP_MODE.DISABLE)
    //            {
    //                PointF pt1 = MM2Pix(tg.st_pos_cap_set);
    //                pt1.X -= w / 2;
    //                pt1.Y -= h / 2;
    //                PointF pt2 = pt1;
    //                pt2.X += w;
    //                pt2.Y += h;
    //                PointF pt3 = pt1;
    //                PointF pt4 = pt2;
    //                pt3.Y += h;
    //                pt4.Y -= h;

    //                p.Width = 2;
    //                if (tg.status < 0 || tg.cap_mode == TG_DATA.EM_CAP_MODE.DISABLE) p.Color = cl_tg;
    //                else p.Color = Color.Red;
    //                gg.DrawLine(p, pt1, pt2);
    //                gg.DrawLine(p, pt3, pt4);

    //                if (tg.bselected) p.Color = cl_sel;
    //                else p.Color = cl_tg;
    //            }
    //            //显示BM点
    //            if (bBM && tg.bBM)
    //            {
    //                p.Width = 1;
    //                gg.DrawLine(p, MM2Pix(tg.st_pos_cap_set), MM2Pix(tg.st_pos_bm_set));
    //                if (tg.bBMCap || tg.bselected) p.Color = cl_sel;
    //                else p.Color = cl_tg;

    //                if (bSolid) p.Width = 3;
    //                gg.DrawEllipse(p, Pos2Rec(tg.st_pos_bm_set));
    //            }
    //        }
    //        else
    //        {
    //            //mark
    //            if (!tg.bselected && tg.cap_mode == TG_DATA.EM_CAP_MODE.DISABLE) p.Color = Color.DarkGray;
    //            gg.DrawEllipse(p, Pos2Rec(tg.st_pos_cap_set));
    //        }

    //        //显示编号
    //        int ftsize = 14;
    //        if (w < 12) ftsize = 10;
    //        else if (w < 18) ftsize = 12;
    //        else if (w < 24) ftsize = 14;
    //        Font ft = new Font("宋体", ftsize);
    //        PointF pt = MM2Pix(tg.st_pos_cap_set);
    //        pt.X -= 5;
    //        pt.Y -= 5;
    //        gg.DrawString(tg.id.ToString(), ft, Brushes.White, pt);
    //        if (bECNUM && tg.name != "MK" && tg.name != "BC")
    //        {
    //            pt.X += (5 + w / 2);
    //            gg.DrawString(tg.ec_num.ToString(), ft, Brushes.Red, pt);
    //        }
    //    }
    //    #endregion
    //    #region 绘制目标序列
    //    public void DrawTgList(List<TG_DATA> list_tg, ref Graphics gg, bool bSolid = true, bool bMTF = false, bool bBM = false, bool bBCNum = false)
    //    {
    //        foreach (TG_DATA tg in list_tg)
    //        {
    //            DrawTg(tg, ref gg, bSolid, bMTF, bBM, bBCNum);
    //        }
    //        //显示选框
    //        if (bsel_rect)
    //        {
    //            Pen p = new Pen(Color.Gold, 1);
    //            gg.DrawRectangle(p, sel_rect);
    //        }
    //        //显示分区            
    //        foreach (Rectangle area in list_area)
    //        {
    //            Pen p = new Pen(Color.Gold, 2);
    //            p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
    //            gg.DrawRectangle(p, area);
    //            //显示编号
    //            int ftsize = 14;
    //            Font ft = new Font("宋体", ftsize);
    //            gg.DrawString("A" + list_area.IndexOf(area).ToString(), ft, Brushes.Gold, new Point(area.X, area.Y - 20));
    //        }
    //        //显示刻度
    //    }
    //    #endregion
    //    #region 点选
    //    public TG_DATA SelectByPoint(ref List<TG_DATA> list_tg, int x, int y, bool bsel_one = false)
    //    {
    //        if (bsel_rect) return null;
    //        int select_cnt = 0;

    //        TG_DATA tg_temp = null;

    //        foreach (TG_DATA tg in list_tg)
    //        {
    //            if (tg.bselected) select_cnt++;
    //            if (select_cnt >= 2) break;
    //        }
    //        foreach (TG_DATA tg in list_tg)
    //        {
    //            if (Pos2Rec(tg.st_pos_cap_set).Contains(new Point(x, y)))
    //            {
    //                //单个选择不取反
    //                if (select_cnt >= 2) tg.bselected = !tg.bselected;
    //                else tg.bselected = true;
    //                tg_temp = tg;
    //            }
    //        }
    //        //diselect all
    //        if (!bsel_rect || bsel_one)
    //        {
    //            foreach (TG_DATA tg in list_tg) tg.bselected = false;
    //        }
    //        if (tg_temp != null && bsel_one) tg_temp.bselected = true;

    //        return tg_temp;
    //    }
    //    #endregion
    //    #region 框选
    //    public void StartRectSelect(int x, int y)
    //    {
    //        pt_start.X = x;
    //        pt_start.Y = y;
    //        bmousedonw = true;
    //    }
    //    public void EndRectSelect()
    //    {
    //        bmousedonw = false;
    //        bsel_rect = false;
    //    }
    //    public int SelectByRect(ref List<TG_DATA> list_tg, int end_x, int end_y)
    //    {
    //        if (bmousedonw == false) return 0;

    //        sel_rect = new Rectangle(pt_start.X, pt_start.Y, end_x - pt_start.X, end_y - pt_start.Y);
    //        if (sel_rect.Width < 0)
    //        {
    //            sel_rect.X += sel_rect.Width;
    //            sel_rect.Width = -sel_rect.Width;
    //        }
    //        if (sel_rect.Height < 0)
    //        {
    //            sel_rect.Y += sel_rect.Height;
    //            sel_rect.Height = -sel_rect.Height;
    //        }

    //        if (sel_rect.Width > 3 || sel_rect.Height > 3) bsel_rect = true;
    //        else bsel_rect = false;
    //        if (bsel_rect == false) return 0;

    //        int selcnt = 0;
    //        foreach (TG_DATA tg in list_tg)
    //        {
    //            if (sel_rect.Contains(Pos2Rec(tg.st_pos_cap_set)))
    //            {
    //                tg.bselected = true;
    //                selcnt++;
    //            }
    //            else tg.bselected = false;
    //        }

    //        return selcnt;
    //    }
    //    #endregion
    //    #region 只选当前目标
    //    public void OnlySelCurTG(ref List<TG_DATA> list_tg, ref TG_DATA cur_tg)
    //    {
    //        foreach (TG_DATA tg in list_tg) tg.bselected = false;
    //        if (cur_tg != null) cur_tg.bselected = true;
    //    }
    //    #endregion
    //}
    //#endregion
    //#region 平台扫描
    //public class SCAN
    //{
    //    #region 参数
    //    public bool bscaning = false;
    //    public bool bsaveing = false;
    //    public bool bmousedonw = false;
    //    public bool bsel_rect = false;
    //    List<Rectangle> list_area = new List<Rectangle>();
    //    public Image img;

    //    public VAR.ST_XY st_ul_cap = new VAR.ST_XY();
    //    public VAR.ST_XYA st_ul = new VAR.ST_XYA();
    //    public VAR.ST_XYA st_br = new VAR.ST_XYA();
    //    public double xs, ys;
    //    public double ofs_x, ofs_y;
    //    public double x, y;

    //    public bool bReadyToArray = false;

    //    public string[] array_list_name = new string[7] { "TA", "TB", "TC", "TD", "MK", "BC", "COPY" };
    //    public bool[] array_chk = new bool[7];
    //    public List<TG_DATA> list_tg_a = new List<TG_DATA>();
    //    public List<TG_DATA> list_tg_b = new List<TG_DATA>();
    //    public List<TG_DATA> list_tg_c = new List<TG_DATA>();
    //    public List<TG_DATA> list_tg_d = new List<TG_DATA>();
    //    public List<TG_DATA> list_tg_mk = new List<TG_DATA>();
    //    public List<TG_DATA> list_tg_bc = new List<TG_DATA>();

    //    public List<TG_DATA> list_tg_copy = new List<TG_DATA>();
    //    public List<TG_DATA> list_tg_array = new List<TG_DATA>();
    //    public List<TG_DATA>[] array_list_tg = new List<TG_DATA>[7];
    //    public List<TG_DATA> list_bc_num = new List<TG_DATA>();
    //    #endregion
    //    #region 初始化
    //    public SCAN()
    //    {
    //    }
    //    #endregion
    //    #region 加载图片及参数
    //    public int LoadImg(string produtname)
    //    {
    //        string fileroad = Path.GetFullPath("..") + "\\product\\" + produtname + "\\temp.inf";
    //        IniFiles inf = new IniFiles(fileroad);
    //        st_ul_cap.x = inf.ReadDouble("SCAN", "START_CAP_X", 250);
    //        st_ul_cap.y = inf.ReadDouble("SCAN", "START_CAP_Y", 250);
    //        st_ul.x = inf.ReadDouble("SCAN", "UL_X", st_ul.x);
    //        st_ul.y = inf.ReadDouble("SCAN", "UL_Y", st_ul.y);
    //        st_ul.a = inf.ReadDouble("SCAN", "UL_A", st_ul.a);
    //        st_br.x = inf.ReadDouble("SCAN", "DR_X", st_br.x);
    //        st_br.y = inf.ReadDouble("SCAN", "DR_Y", st_br.y);
    //        st_br.a = inf.ReadDouble("SCAN", "DR_A", st_br.a);
    //        xs = inf.ReadDouble("SCAN", "XS", 0.23405);
    //        ys = inf.ReadDouble("SCAN", "YS", 0.23405);
    //        ofs_x = inf.ReadDouble("SCAN", "OFS_X", 0);
    //        ofs_y = inf.ReadDouble("SCAN", "OFS_Y", 0);

    //        try
    //        {
    //            Stream s = File.Open(Path.GetFullPath("..") + "\\product\\" + produtname + "\\view.bmp", FileMode.Open);
    //            img = Image.FromStream(s);
    //            s.Close();
    //        }
    //        catch
    //        {
    //            return CONST.RES_ERR;
    //        }
    //        return CONST.RES_OK;
    //    }
    //    #endregion
    //    #region 加载数据
    //    public void LoadTGData(bool bloadfrfile = true)
    //    {
    //        array_list_tg[0] = list_tg_a;
    //        array_list_tg[1] = list_tg_b;
    //        array_list_tg[2] = list_tg_c;
    //        array_list_tg[3] = list_tg_d;
    //        array_list_tg[4] = list_tg_mk;
    //        array_list_tg[5] = list_tg_bc;
    //        array_list_tg[6] = list_tg_copy;

    //        //clear
    //        for (int n = 0; n < array_list_tg.Count(); n++)
    //        {
    //            if (array_list_tg[n] != null) array_list_tg[n].Clear();
    //        }

    //        //load
    //        if (bloadfrfile) COM.tg.LoadDat(VAR.gsys_set.cur_product_name);

    //        for (int n = 0; n < COM.tg.list_tg.Count; n++)
    //        {
    //            for (int m = 0; m < 6; m++)
    //            {
    //                if (COM.tg.list_tg[n].name == array_list_name[m])
    //                {
    //                    TG_DATA tg = COM.tg.list_tg[n];
    //                    if (tg.status == -1) tg.bmtf = false;
    //                    else tg.bmtf = true;
    //                    tg.bselected = false;
    //                    array_list_tg[m].Add(tg);
    //                }
    //            }
    //        }
    //    }
    //    #endregion
    //    #region 更新目标状态
    //    public int UpdateTGStatus()
    //    {

    //        int idx = 0;

    //        //check
    //        idx = 0;
    //        for (int n = 0; n < 6; n++)
    //        {
    //            foreach (TG_DATA tg in array_list_tg[n])
    //            {
    //                if (COM.tg.list_tg[idx].name != tg.name || COM.tg.list_tg[idx].id != tg.id)
    //                {
    //                    MessageBox.Show("更新贴付状态数据异常!");
    //                    return CONST.RES_ERR;
    //                }
    //                idx++;
    //            }
    //        }
    //        //update
    //        idx = 0;
    //        for (int n = 0; n < 6; n++)
    //        {
    //            foreach (TG_DATA tg in array_list_tg[n])
    //            {
    //                COM.tg.list_tg[idx].bmtf = tg.bmtf;
    //                if (tg.bmtf) COM.tg.list_tg[idx].status = -1;
    //                idx++;
    //            }
    //        }

    //        MessageBox.Show("更新贴付状态成功!");
    //        return CONST.RES_OK;
    //    }
    //    #endregion
    //    #region 保存数据
    //    public void SaveTGData()
    //    {
    //        //save tg data
    //        COM.tg.list_tg.Clear();
    //        for (int n = 0; n < 6; n++)
    //        {
    //            foreach (TG_DATA tg in array_list_tg[n]) COM.tg.list_tg.Add(tg);
    //        }
    //        int ret = COM.tg.SaveTgDat(VAR.gsys_set.cur_product_name);


    //        //保存设置            
    //        COM.tg.SaveCfg(VAR.gsys_set.cur_product_name);

    //        //重载
    //        ret += COM.tg.LoadDat(VAR.gsys_set.cur_product_name);

    //        if (ret == CONST.RES_OK) MessageBox.Show("数据保存完成!");
    //        else MessageBox.Show("数据保存失败!");
    //    }
    //    #endregion
    //    #region 保存扫描数据及参数
    //    public int SaveImg(string produtname)
    //    {
    //        //if (!File.Exists(Path.GetFullPath("..") + "\\product\\" + produtname + "\\temp.bmp")) return CONST.RES_PARA_ERR;
    //        //File.Copy(Path.GetFullPath("..") + "\\product\\" + produtname + "\\temp.bmp", Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\view.bmp", true);
    //        if (img != null) img.Save(Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\view.bmp");
    //        else return CONST.RES_PARA_ERR;

    //        string fileroad = Path.GetFullPath("..") + "\\product\\" + produtname + "\\temp.inf";
    //        IniFiles inf = new IniFiles(fileroad);
    //        inf.WriteDouble("SCAN", "START_CAP_X", st_ul_cap.x);
    //        inf.WriteDouble("SCAN", "START_CAP_Y", st_ul_cap.y);
    //        inf.WriteDouble("SCAN", "XS", xs);
    //        inf.WriteDouble("SCAN", "YS", ys);
    //        ofs_x = inf.ReadDouble("SCAN", "OFS_X", 0);
    //        ofs_y = inf.ReadDouble("SCAN", "OFS_Y", 0);

    //        try
    //        {
    //            Stream s = File.Open(Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\view.bmp", FileMode.Open);
    //            img = Image.FromStream(s);
    //            s.Close();
    //        }
    //        catch
    //        {
    //            img = new Bitmap(2000, 2000);
    //            Graphics gg = Graphics.FromImage(img);
    //            gg.FillRectangle(Brushes.DarkGray, new Rectangle(new Point(), img.Size));
    //            return CONST.RES_PARA_ERR;
    //        }

    //        return CONST.RES_OK;
    //    }
    //    #endregion
    //    #region 坐标转换
    //    public VAR.ST_XY PosPix2MM(int x, int y)
    //    {
    //        VAR.ST_XY st_pos = new VAR.ST_XY();
    //        st_pos.x = st_ul_cap.x + x * xs;
    //        st_pos.y = st_ul_cap.y + y * ys;
    //        return st_pos;
    //    }
    //    public VAR.ST_XY PosMM2Pix(double x, double y)
    //    {
    //        VAR.ST_XY st_pos = new VAR.ST_XY();
    //        st_pos.x = (x - st_ul_cap.x) / xs;
    //        st_pos.y = (y - st_ul_cap.y) / ys;
    //        return st_pos;
    //    }
    //    public PointF MM2Pix(VAR.ST_XYZ st_pos_mm)
    //    {
    //        PointF st_pos = new PointF();
    //        st_pos.X = (float)((st_pos_mm.x - st_ul_cap.x) / xs);
    //        st_pos.Y = (float)((st_pos_mm.y - st_ul_cap.y) / ys);
    //        return st_pos;
    //    }
    //    public VAR.ST_XY PosMM2Pix(VAR.ST_XYZ st_pos_mm)
    //    {
    //        VAR.ST_XY st_pos = new VAR.ST_XY();
    //        st_pos.x = (st_pos_mm.x - st_ul_cap.x) / xs;
    //        st_pos.y = (st_pos_mm.y - st_ul_cap.y) / ys;
    //        return st_pos;
    //    }

    //    public Rectangle Pos2Rec(VAR.ST_XYZ st_pos_mm, int w = 30, int h = 30)
    //    {
    //        VAR.ST_XY st_pos = new VAR.ST_XY();
    //        st_pos.x = (st_pos_mm.x - st_ul_cap.x) / xs;
    //        st_pos.y = (st_pos_mm.y - st_ul_cap.y) / ys;
    //        Rectangle rect = new Rectangle((int)(st_pos.x - w / 2), (int)(st_pos.y - h / 2), w, h);
    //        return rect;
    //    }
    //    #endregion
    //    #region 扫描
    //    public int Scan(ref int percent, VAR.ST_XYA st_ul, VAR.ST_XYA st_br, int pic_w, int pic_h)
    //    {
    //        //计算行列
    //        int xn = 0, yn = 0;
    //        double dx = 0, dy = 0;
    //        int pic_dx = 0, pic_dy = 0;
    //        int img_w = 2482;
    //        int img_h = 2102;
    //        VAR.ST_XYZA st_stransform;
    //        CogTransform2DLinear stransform;
    //        stransform = (CogTransform2DLinear)VisionTasks.Calibs[0].mCheckerboardTool.Calibration.OwnedWarpParams.GetOutputImageRootFromCalibratedTransform();
    //        st_stransform.x = 1 / stransform.ScalingX;
    //        st_stransform.y = 1 / stransform.ScalingY;
    //        stransform = (CogTransform2DLinear)VisionTasks.Calibs[1].mCamNPointTool.Calibration.GetComputedUncalibratedFromCalibratedTransform();
    //        st_stransform.x = st_stransform.x / stransform.ScalingX;
    //        st_stransform.y = st_stransform.y / stransform.ScalingY;

    //        if (Math.Abs(stransform.ScalingX) < 0.001 || Math.Abs(stransform.ScalingX) < 0.001)
    //        {
    //            MessageBox.Show("相机像素比例异常，请先校准后再扫描!");
    //            return CONST.RES_PARA_ERR;
    //        }

    //        Visionimage.CaptrueAndWaitResult(CameraNumber.Camera1, TaskProcess.CameraScan);
    //        img_h = VisionRun.ListVisionData[0].OutPutImage.Height;
    //        img_w = VisionRun.ListVisionData[0].OutPutImage.Width;

    //        dx = img_w * st_stransform.x * -1;
    //        dy = img_h * st_stransform.y * -1;

    //        xn = (int)(Math.Abs(st_ul.x - st_br.x) / Math.Abs(dx) + 0.999) + 1;
    //        yn = (int)(Math.Abs(st_ul.y - st_br.y) / Math.Abs(dy) + 0.999) + 1;


    //        pic_dx = pic_w / xn;
    //        pic_dy = pic_h / yn;

    //        //保存比列
    //        if (((double)pic_dx / (double)pic_dy) > ((double)img_w / (double)img_h)) pic_dx = (int)(pic_dy * ((double)img_w / (double)img_h));
    //        else pic_dy = (int)((double)pic_dx * ((double)img_h / (double)img_w));

    //        img = new Bitmap(pic_w, pic_h);
    //        Graphics gg = Graphics.FromImage(img);
    //        gg.FillRectangle(Brushes.DarkGray, new Rectangle(new Point(), img.Size));

    //        percent = 0;

    //        xs = dx / (double)pic_dx;
    //        ys = dy / (double)pic_dy;
    //        st_ul_cap.x = st_ul.x - dx / 2;
    //        st_ul_cap.y = st_ul.y - dy / 2;
    //        bsaveing = false;
    //        bscaning = true;
    //        for (int n = 0; n < xn; n++)
    //        {
    //            for (int m = 0; m < yn; m++)
    //            {
    //                //防护
    //                if (CONST.RES_OK != MT.ChkSafeSen()) goto ERR_END;

    //                //按键取消
    //                if (MT.GPIO_IN_KEY_STOP.isON) goto ERR_END;

    //                //检查是否超范围
    //                double x = st_ul.x + n * dx;
    //                double y = st_ul.y + m * dy;
    //                float kx = 0;
    //                float ky = 0;
    //                if (x < MT.AXIS_X.sln)
    //                {
    //                    kx = (float)(Math.Abs((MT.AXIS_X.sln - x) / dx));
    //                    x = MT.AXIS_X.sln;
    //                }
    //                if (x > MT.AXIS_X.slp)
    //                {
    //                    kx = (float)(Math.Abs((MT.AXIS_X.slp - x) / dx));
    //                    x = MT.AXIS_X.slp;
    //                }
    //                if (y < MT.AXIS_Y.sln)
    //                {
    //                    ky = (float)(Math.Abs((MT.AXIS_Y.sln - y) / dy));
    //                    y = MT.AXIS_Y.sln;
    //                }
    //                if (y > MT.AXIS_Y.slp)
    //                {
    //                    ky = (float)(Math.Abs((MT.AXIS_Y.slp - y) / dy));
    //                    y = MT.AXIS_Y.slp;
    //                }
    //                //move
    //                int ret = MT.ZupMove(ref VAR.gsys_set.bquit, ref MT.AXIS_X, x, ref MT.AXIS_Y, y);
    //                if (ret != CONST.RES_OK)
    //                    goto ERR_END;

    //                //wait inp
    //                MT.AXIS_X.WaitINP(ref VAR.gsys_set.bquit);
    //                MT.AXIS_Y.WaitINP(ref VAR.gsys_set.bquit);

    //                //triger
    //                //int mode_temp = VAR.gsys_set.mode;
    //                //VAR.gsys_set.mode = CONST.SYS_RUN_MODE_DEMO;
    //                Visionimage.CaptrueAndWaitResult(CameraNumber.Camera1, TaskProcess.CameraScan);
    //                //VAR.gsys_set.mode = mode_temp;

    //                //get img
    //                Image img1 = VisionRun.ListVisionData[0].OutPutImage.ScaleImage(pic_dx, pic_dy).ToBitmap();
    //                //draw tu buf
    //                gg.DrawImage(img1, n * pic_dx - kx * pic_dx, m * pic_dy - ky * pic_dy, pic_dx, pic_dy);

    //                Thread.Sleep(1);
    //                Application.DoEvents();

    //                //进度
    //                percent = (int)((n * yn + m + 1) * 100.0 / (xn * yn));
    //            }
    //        }
    //        if (gg != null)
    //        {
    //            gg.Dispose();
    //            gg = null;
    //        }
    //        percent = 100;
    //        Thread.Sleep(100);
    //        Application.DoEvents();
    //        bscaning = false;

    //        //save pos
    //        string fileroad = Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\temp.inf";
    //        IniFiles inf = new IniFiles(fileroad);

    //        this.st_ul = st_ul;
    //        this.st_br = st_br;
    //        inf.WriteDouble("SCAN", "UL_X", st_ul.x);
    //        inf.WriteDouble("SCAN", "UL_Y", st_ul.y);
    //        inf.WriteDouble("SCAN", "UL_A", st_ul.a);
    //        inf.WriteDouble("SCAN", "DR_X", st_br.x);
    //        inf.WriteDouble("SCAN", "DR_Y", st_br.y);
    //        inf.WriteDouble("SCAN", "DR_A", st_br.a);
    //        inf.WriteDouble("SCAN", "SC_X", xs);
    //        inf.WriteDouble("SCAN", "SC_Y", ys);
    //        //save img
    //        //bsaveing = true;
    //        //img.Save(Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\temp.bmp");            
    //        //bsaveing = false;
    //        bscaning = false;
    //        return CONST.RES_OK;

    //        ERR_END:
    //        if (gg != null)
    //        {
    //            gg.Dispose();
    //            gg = null;
    //        }
    //        bscaning = false;
    //        return CONST.RES_ERR;
    //    }
    //    #endregion
    //    #region 阵列
    //    public int Aarry(int xn, int yn, int num_area, int cap_mode, string partname = "")
    //    {

    //        VAR.ST_XYZ[] st_pos = new VAR.ST_XYZ[1];
    //        int ret = COM.tg.ArrayPos2(list_tg_array[0].st_pos_cap_set, list_tg_array[1].st_pos_cap_set, list_tg_array[2].st_pos_cap_set, xn, yn, ref st_pos);
    //        if (ret != CONST.RES_OK) return ret;

    //        //copy to list            
    //        for (int n = 0; n < st_pos.Length; n++)
    //        {
    //            TG_DATA tg = new TG_DATA();
    //            tg.partname = partname;
    //            tg.cap_mode = (TG_DATA.EM_CAP_MODE)(cap_mode > -1 ? cap_mode : 0);
    //            tg.area_num = num_area;
    //            tg.st_pos_cap_set = st_pos[n];
    //            tg.name = list_tg_array[0].name;

    //            if (tg.bBM)
    //            {
    //                tg.st_pos_bm_set.x = tg.st_pos_cap_set.x + (list_tg_array[0].st_pos_bm_set.x - list_tg_array[0].st_pos_cap_set.x);
    //                tg.st_pos_bm_set.y = tg.st_pos_cap_set.y + (list_tg_array[0].st_pos_bm_set.y - list_tg_array[0].st_pos_cap_set.y);
    //            }
    //            else tg.st_pos_bm_set = new VAR.ST_XYZ();

    //            for (int m = 0; m < 4; m++)
    //            {
    //                if (tg.name == array_list_name[m])
    //                {
    //                    tg.id = array_list_tg[m].Count;
    //                    array_list_tg[m].Add(tg);
    //                }
    //            }
    //        }

    //        return CONST.RES_OK;
    //    }
    //    #endregion
    //    #region 准备阵列
    //    public int PrepareForArray(double mx, double my, int num_area, int cap_mode, bool bBM = false, string partname = "")
    //    {
    //        int ret = CONST.RES_ERR;
    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                list_tg_array.Clear();
    //                TG_DATA tg = new TG_DATA();
    //                tg.name = array_list_name[n];
    //                tg.partname = partname;
    //                tg.area_num = num_area;
    //                tg.cap_mode = (TG_DATA.EM_CAP_MODE)(cap_mode > -1 ? cap_mode : 0);

    //                //第1点
    //                tg.id = 0;
    //                tg.st_pos_cap_set.x = mx;
    //                tg.st_pos_cap_set.y = my;
    //                if (bBM)
    //                {
    //                    tg.bBM = true;
    //                    tg.st_pos_bm_set.x = tg.st_pos_cap_set.x + 10 * (xs > 0 ? 1 : -1); ;
    //                    tg.st_pos_bm_set.y = tg.st_pos_cap_set.y + 10 * (ys > 0 ? 1 : -1); ;
    //                }
    //                list_tg_array.Add(tg);

    //                //第2点
    //                tg = new TG_DATA();
    //                tg.name = array_list_name[n];
    //                tg.partname = partname;
    //                tg.area_num = num_area;
    //                tg.cap_mode = (TG_DATA.EM_CAP_MODE)(cap_mode > -1 ? cap_mode : 0);
    //                tg.id = 1;
    //                tg.bBM = false;
    //                tg.st_pos_cap_set.x = mx + 30 * (xs > 0 ? 1 : -1); ;
    //                tg.st_pos_cap_set.y = my;
    //                list_tg_array.Add(tg);

    //                //第3点
    //                tg = new TG_DATA();
    //                tg.name = array_list_name[n];
    //                tg.partname = partname;
    //                tg.area_num = num_area;
    //                tg.cap_mode = (TG_DATA.EM_CAP_MODE)(cap_mode > -1 ? cap_mode : 0);
    //                tg.id = 2;
    //                tg.bBM = false;
    //                tg.st_pos_cap_set.x = mx;
    //                tg.st_pos_cap_set.y = my + 30 * (ys > 0 ? 1 : -1);
    //                list_tg_array.Add(tg);

    //                //第2/3点用于阵列
    //                bReadyToArray = true;
    //                ret = CONST.RES_OK;
    //                break;
    //            }
    //        }

    //        return ret;
    //    }
    //    #endregion
    //    #region 双击定位
    //    public int DoubleClik(int pix_x, int pix_y)
    //    {
    //        int ret = CONST.RES_OK;
    //        x = st_ul_cap.x + pix_x * xs;
    //        y = st_ul_cap.y + pix_y * ys;

    //        int cap_mode = -1;
    //        for (int n = 0; n < 5; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    if (Pos2Rec(array_list_tg[n][m].st_pos_cap_set).Contains(new Point(pix_x, pix_y)))
    //                    {
    //                        x = array_list_tg[n][m].st_pos_cap_set.x;
    //                        y = array_list_tg[n][m].st_pos_cap_set.y;
    //                        if (CONST.RES_OK != COM.tg.GetCapMode(array_list_tg[n][m].name, ref cap_mode)) cap_mode = -1;
    //                        break;
    //                    }
    //                }
    //            }
    //        }

    //        //offset
    //        string fileroad = Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\temp.inf";
    //        IniFiles inf = new IniFiles(fileroad);
    //        ofs_x = inf.ReadDouble("SCAN", "OFS_X", 0);
    //        ofs_y = inf.ReadDouble("SCAN", "OFS_Y", 0);

    //        if (VAR.gsys_set.status == CONST.SYS_STATUS_STANDBY || VAR.gsys_set.status == CONST.SYS_STATUS_PAUSE)
    //        {
    //            if (DialogResult.Yes == MessageBox.Show(string.Format("是否定位到以下坐标？ \r\nX:{0:F3} + {1:F3} \r\nY:{2:F3} + {3:F3}", x, ofs_x, y, ofs_y), "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
    //            {
    //                ret = MT.ZupMove(ref VAR.gsys_set.bquit, ref MT.AXIS_X, x + ofs_x, ref MT.AXIS_Y, y + ofs_y);
    //                Thread.Sleep(300);
    //                return ret;
    //            }
    //        }

    //        return CONST.RES_ABORT;
    //    }
    //    #endregion
    //    #region 修改编号
    //    public bool SetTGNum(ref int num, int pix_x, int pix_y, string num_type = "EC")
    //    {
    //        for (int n = 0; n < 4; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    if (Pos2Rec(array_list_tg[n][m].st_pos_cap_set).Contains(new Point(pix_x, pix_y)))
    //                    {
    //                        TG_DATA tg = array_list_tg[n][m];
    //                        if (!list_bc_num.Contains(tg))
    //                        {
    //                            if (num >= 0)
    //                            {
    //                                switch (num_type)
    //                                {
    //                                    default:
    //                                    case "EC":
    //                                        tg.ec_num = num;
    //                                        break;
    //                                    case "TF":
    //                                        tg.tf_seq = num;
    //                                        break;
    //                                }

    //                                array_list_tg[n][m] = tg;
    //                                list_bc_num.Add(tg);
    //                                num++;
    //                            }
    //                        }
    //                        return true;
    //                    }
    //                }
    //            }
    //        }

    //        return false;
    //    }
    //    #endregion
    //    #region 选择
    //    public bool TGSel(bool bBM, bool barray, ref TG_DATA cur_tg, int pix_x, int pix_y, bool bset = false, bool bsel = false, bool brev = false)
    //    {
    //        bool bget_sel = false;
    //        bool btemp_sel = false;
    //        TG_DATA tg = new TG_DATA();

    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    btemp_sel = Pos2Rec(array_list_tg[n][m].st_pos_cap_set).Contains(new Point(pix_x, pix_y));
    //                    if (bBM && array_list_tg[n][m].bBM)
    //                    {
    //                        tg = array_list_tg[n][m];
    //                        tg.bBMCap = Pos2Rec(array_list_tg[n][m].st_pos_bm_set).Contains(new Point(pix_x, pix_y));
    //                        array_list_tg[n][m] = tg;
    //                    }
    //                    if (bset || btemp_sel)
    //                    {
    //                        tg = array_list_tg[n][m];
    //                        tg.bselected = btemp_sel || brev ? !tg.bselected : bsel;
    //                        array_list_tg[n][m] = tg;
    //                        bget_sel = bget_sel || btemp_sel;
    //                    }
    //                    if (btemp_sel)
    //                    {
    //                        cur_tg = array_list_tg[n][m];
    //                        break;
    //                    }
    //                }
    //            }
    //            if (btemp_sel) break;
    //        }

    //        if (barray)
    //        {
    //            for (int m = 0; m < list_tg_array.Count; m++)
    //            {
    //                for (int n = 0; n < 6; n++)
    //                {
    //                    if (array_chk[n] && array_list_name[n] == list_tg_array[0].name)
    //                    {
    //                        btemp_sel = Pos2Rec(list_tg_array[m].st_pos_cap_set).Contains(new Point(pix_x, pix_y));
    //                        if (bBM && list_tg_array[m].bBM)
    //                        {
    //                            tg = list_tg_array[m];
    //                            tg.bBMCap = Pos2Rec(list_tg_array[m].st_pos_bm_set).Contains(new Point(pix_x, pix_y));
    //                            list_tg_array[m] = tg;
    //                        }
    //                        if (bset || btemp_sel)
    //                        {
    //                            tg = list_tg_array[m];
    //                            tg.bselected = btemp_sel || brev ? !tg.bselected : bsel;
    //                            list_tg_array[m] = tg;
    //                            bget_sel = bget_sel || btemp_sel;
    //                        }
    //                        break;
    //                    }
    //                }
    //            }
    //        }

    //        return bget_sel;
    //    }
    //    #endregion
    //    #region 检查选中状态
    //    public bool TGSelChk(ref bool bsel, int pix_x, int pix_y, bool barray, bool bBM)
    //    {
    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    if (Pos2Rec(array_list_tg[n][m].st_pos_cap_set).Contains(new Point(pix_x, pix_y)))
    //                    {
    //                        bsel = array_list_tg[n][m].bselected;
    //                        return true;
    //                    }
    //                    if (bBM && array_list_tg[n][m].bBM)
    //                    {
    //                        if (Pos2Rec(array_list_tg[n][m].st_pos_bm_set).Contains(new Point(pix_x, pix_y)))
    //                        {
    //                            bsel = array_list_tg[n][m].bBMCap;
    //                            return true;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        if (barray)
    //        {
    //            for (int m = 0; m < list_tg_array.Count; m++)
    //            {
    //                for (int n = 0; n < 6; n++)
    //                {
    //                    if (Pos2Rec(list_tg_array[m].st_pos_cap_set).Contains(new Point(pix_x, pix_y)))
    //                    {
    //                        bsel = list_tg_array[m].bselected;
    //                        return true;
    //                    }
    //                    if (bBM && list_tg_array[m].bBM)
    //                    {
    //                        if (Pos2Rec(list_tg_array[m].st_pos_bm_set).Contains(new Point(pix_x, pix_y)))
    //                        {
    //                            bsel = list_tg_array[m].bBMCap;
    //                            return true;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        return false;
    //    }
    //    #endregion
    //    #region 拖放
    //    public void TGDrag(double dx, double dy, bool barray, bool bpaste, bool bBM = false)
    //    {
    //        TG_DATA tg = new TG_DATA();

    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    if (array_list_tg[n][m].bselected)
    //                    {
    //                        tg = array_list_tg[n][m];
    //                        tg.st_pos_cap_set.x += dx;
    //                        tg.st_pos_cap_set.y += dy;
    //                        if (bBM && tg.bBM)
    //                        {
    //                            tg.st_pos_bm_set.x += dx;
    //                            tg.st_pos_bm_set.y += dy;
    //                        }
    //                        array_list_tg[n][m] = tg;
    //                    }
    //                    else if (array_list_tg[n][m].bBMCap)
    //                    {
    //                        if (bBM && array_list_tg[n][m].bBM)
    //                        {
    //                            tg = array_list_tg[n][m];
    //                            tg.st_pos_bm_set.x += dx;
    //                            tg.st_pos_bm_set.y += dy;
    //                            array_list_tg[n][m] = tg;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        if (bpaste)
    //        {
    //            for (int n = 0; n < list_tg_copy.Count; n++)
    //            {
    //                if (list_tg_copy[n].bselected)
    //                {
    //                    tg = list_tg_copy[n];
    //                    tg.st_pos_cap_set.x += dx;
    //                    tg.st_pos_cap_set.y += dy;
    //                    if (bBM && tg.bBM)
    //                    {
    //                        tg.st_pos_bm_set.x += dx;
    //                        tg.st_pos_bm_set.y += dy;
    //                    }
    //                    list_tg_copy[n] = tg;
    //                }
    //                else if (list_tg_copy[n].bBMCap)
    //                {
    //                    if (bBM && list_tg_copy[n].bBM)
    //                    {
    //                        tg = list_tg_copy[n];
    //                        tg.st_pos_bm_set.x += dx;
    //                        tg.st_pos_bm_set.y += dy;
    //                        list_tg_copy[n] = tg;
    //                    }
    //                }
    //            }
    //        }

    //        if (barray)
    //        {
    //            for (int n = 0; n < list_tg_array.Count; n++)
    //            {
    //                if (list_tg_array[n].bselected)
    //                {
    //                    tg = list_tg_array[n];
    //                    tg.st_pos_cap_set.x += dx;
    //                    tg.st_pos_cap_set.y += dy;
    //                    if (bBM && tg.bBM)
    //                    {
    //                        tg.st_pos_bm_set.x += dx;
    //                        tg.st_pos_bm_set.y += dy;
    //                    }
    //                    list_tg_array[n] = tg;
    //                }
    //                else if (list_tg_array[n].bBMCap)
    //                {
    //                    if (bBM && list_tg_array[n].bBM)
    //                    {
    //                        tg = list_tg_array[n];
    //                        tg.st_pos_bm_set.x += dx;
    //                        tg.st_pos_bm_set.y += dy;
    //                        list_tg_array[n] = tg;
    //                    }
    //                }
    //            }
    //        }

    //    }
    //    #endregion
    //    #region 复制
    //    public bool Copy()
    //    {
    //        list_tg_copy.Clear();
    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                foreach (TG_DATA tg in array_list_tg[n])
    //                {
    //                    if (tg.bselected) list_tg_copy.Add(tg.clone());
    //                }
    //            }
    //        }

    //        if (list_tg_copy.Count == 0) return false;
    //        return true;
    //    }
    //    #endregion
    //    #region 粘贴
    //    public void Paste()
    //    {
    //        bool bcopy = false;

    //        //对应复制
    //        foreach (TG_DATA tg in list_tg_copy)
    //        {
    //            for (int n = 0; n < 6; n++)
    //            {
    //                if (array_chk[n] && array_list_tg[n].Count > 0 && tg.name == array_list_tg[n][0].name)
    //                {
    //                    TG_DATA tg_temp = array_list_tg[n][0].clone();
    //                    tg_temp.id = array_list_tg[n].Count;
    //                    tg_temp.bBM = tg.bBM;
    //                    tg_temp.area_num = tg.area_num;
    //                    tg_temp.st_pos_cap_set = tg.st_pos_cap_set;
    //                    tg_temp.st_pos_bm_set = tg.st_pos_bm_set;
    //                    array_list_tg[n].Add(tg_temp);
    //                    bcopy = true;
    //                }
    //            }
    //        }

    //        //没有对应复制，则复制到一个选择类
    //        if (!bcopy)
    //        {
    //            for (int n = 0; n < 6; n++)
    //            {
    //                if (array_chk[n])
    //                {
    //                    foreach (TG_DATA tg in list_tg_copy)
    //                    {
    //                        TG_DATA tg_temp = tg.clone();
    //                        if (array_list_tg[n].Count > 0)
    //                            tg_temp = array_list_tg[n][0].clone();
    //                        else
    //                            tg_temp.name = array_list_name[n];
    //                        tg_temp.id = array_list_tg[n].Count;
    //                        tg_temp.bBM = tg.bBM;
    //                        tg_temp.area_num = tg.area_num;
    //                        tg_temp.st_pos_cap_set = tg.st_pos_cap_set;
    //                        tg_temp.st_pos_bm_set = tg.st_pos_bm_set;
    //                        array_list_tg[n].Add(tg_temp);
    //                    }
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    #endregion
    //    #region 清除
    //    public void Clear()
    //    {
    //        List<TG_DATA> list_del = new List<TG_DATA>();

    //        //TA
    //        foreach (TG_DATA tg in list_tg_a)
    //        {
    //            if (tg.bselected) list_del.Add(tg);
    //        }
    //        foreach (TG_DATA tg in list_del) list_tg_a.Remove(tg);

    //        //TB
    //        foreach (TG_DATA tg in list_tg_b)
    //        {
    //            if (tg.bselected) list_del.Add(tg);
    //        }
    //        foreach (TG_DATA tg in list_del) list_tg_b.Remove(tg);

    //        //TC
    //        foreach (TG_DATA tg in list_tg_c)
    //        {
    //            if (tg.bselected) list_del.Add(tg);
    //        }
    //        foreach (TG_DATA tg in list_del) list_tg_c.Remove(tg);

    //        //TD
    //        foreach (TG_DATA tg in list_tg_d)
    //        {
    //            if (tg.bselected) list_del.Add(tg);
    //        }
    //        foreach (TG_DATA tg in list_del) list_tg_d.Remove(tg);

    //        //MK
    //        foreach (TG_DATA tg in list_tg_mk)
    //        {
    //            if (tg.bselected) list_del.Add(tg);
    //        }
    //        foreach (TG_DATA tg in list_del) list_tg_mk.Remove(tg);

    //        //BC
    //        foreach (TG_DATA tg in list_tg_bc)
    //        {
    //            if (tg.bselected) list_del.Add(tg);
    //        }
    //        foreach (TG_DATA tg in list_del) list_tg_bc.Remove(tg);
    //    }
    //    #endregion
    //    #region 框选
    //    public void RectSel(double start_mx, double start_my, double end_mx, double end_my)
    //    {
    //        //框选
    //        Rectangle rect = new Rectangle((int)start_mx, (int)start_my, (int)(end_mx - start_mx), (int)(end_my - start_my));
    //        if (rect.Width < 0)
    //        {
    //            rect.Width = -rect.Width;
    //            rect.X -= rect.Width;
    //        }
    //        if (rect.Height < 0)
    //        {
    //            rect.Height = -rect.Height;
    //            rect.Y -= rect.Height;
    //        }
    //        for (int n = 0; n < 6; n++)
    //        {
    //            TG_DATA tg = new TG_DATA();
    //            if (array_chk[n])
    //            {
    //                //draw.SlectByRect(array_list_tg[n], new Point(md_pix_x, md_pix_y), new Point(pix_x, pix_y));
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    tg = array_list_tg[n][m];
    //                    if (rect.Contains((int)tg.st_pos_cap_set.x, (int)tg.st_pos_cap_set.y))
    //                    {
    //                        tg.bselected = true;
    //                        array_list_tg[n][m] = tg;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    #endregion
    //    #region 增加
    //    public void AddTG(double mx, double my, int area_num, int cap_mode, bool bBM = false, string partname = "")
    //    {
    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                TG_DATA st_tg = new TG_DATA();
    //                st_tg.st_pos_cap_set.x = mx;
    //                st_tg.st_pos_cap_set.y = my;
    //                st_tg.name = array_list_name[n];
    //                st_tg.partname = partname;
    //                st_tg.id = array_list_tg[n].Count;
    //                st_tg.cap_mode = (TG_DATA.EM_CAP_MODE)(cap_mode);
    //                st_tg.area_num = area_num;

    //                if (array_list_tg[n].Count > 0)
    //                {
    //                    st_tg.st_offset = array_list_tg[n][0].st_offset;
    //                    st_tg.st_layer_offset = array_list_tg[n][0].st_layer_offset;
    //                }
    //                else
    //                {
    //                    st_tg.st_offset = new VAR.ST_XYZA();
    //                    st_tg.st_layer_offset = new VAR.ST_XYZA();
    //                }

    //                if (st_tg.name == "MK" || st_tg.name == "BC") st_tg.cap_mode = TG_DATA.EM_CAP_MODE.CAP;

    //                if ((st_tg.name != "MK" && st_tg.name != "BC") && bBM)
    //                {
    //                    st_tg.bBM = true;
    //                    if (array_list_tg[n].Count > 0 && array_list_tg[n][0].bBM)
    //                    {
    //                        st_tg.st_pos_bm_set.x = array_list_tg[n][0].st_pos_bm_set.x - array_list_tg[n][0].st_pos_cap_set.x + mx;
    //                        st_tg.st_pos_bm_set.y = array_list_tg[n][0].st_pos_bm_set.y - array_list_tg[n][0].st_pos_cap_set.y + my;
    //                    }
    //                    else
    //                    {
    //                        st_tg.st_pos_bm_set.x = 5 + mx;
    //                        st_tg.st_pos_bm_set.y = 5 + my;
    //                    }
    //                }

    //                array_list_tg[n].Add(st_tg);
    //                break;
    //            }
    //        }
    //    }
    //    #endregion
    //    #region 校验
    //    public int CapCali(ref List<TG_DATA> list_tg)
    //    {
    //        int ret = CONST.RES_OK;
    //        if (list_tg == null) return CONST.RES_PARA_ERR;
    //        if (list_tg.Count == 0) return CONST.RES_OK;

    //        //move R
    //        ret = MT.ZupMove(ref VAR.gsys_set.bquit, ref MT.AXIS_R, st_ul.a);
    //        if (ret != CONST.RES_OK)
    //        {
    //            //Move to Rdy pos first
    //            ret = MT.ZupMove(ref VAR.gsys_set.bquit, ref MT.AXIS_X, FDH.st_pos_ready.x, ref MT.AXIS_Y, FDH.st_pos_ready.y);
    //            if (ret != CONST.RES_OK) return ret;
    //            ret = MT.ZupMove(ref VAR.gsys_set.bquit, ref MT.AXIS_R, st_ul.a);
    //            if (ret != CONST.RES_OK) return ret;
    //        }

    //        TG_DATA tg = list_tg[0];
    //        for (int n = 0; n < list_tg.Count; n++)
    //        {
    //            tg = list_tg[n];
    //            tg.st_pos_cap_cur.x = tg.st_pos_cap_set.x + ofs_x;
    //            tg.st_pos_cap_cur.y = tg.st_pos_cap_set.y + ofs_y;
    //            if (tg.name == "MK") tg.st_pos_mk_cur = tg.st_pos_cap_cur;
    //            //cap
    //            ret = COM.tg.TgCap(ref tg, true, true, true);
    //            if (tg.status == -1) tg.bmtf = false;
    //            else tg.bmtf = true;
    //            if (ret == CONST.RES_OK)
    //            {
    //                tg.st_pos_cap_set = tg.st_pos_cap_cur;
    //                tg.st_pos_cap_set.x -= ofs_x;
    //                tg.st_pos_cap_set.y -= ofs_y;
    //                tg.st_pos_cap_set.z = tg.st_vs_cur.z;
    //                list_tg[n] = tg;
    //            }
    //            else break;

    //            //bm
    //            if (tg.bBM)
    //            {
    //                ret = COM.tg.TgCap(ref tg, false, true, true, tg.bBM);
    //                if (ret == CONST.RES_OK)
    //                {
    //                    tg.st_pos_bm_set = tg.st_pos_bm_cur;
    //                    list_tg[n] = tg;
    //                }
    //                else break;
    //            }
    //        }
    //        if (ret != CONST.RES_OK)
    //        {
    //            MessageBox.Show(string.Format("目标{0}，编号{1} 校正失败!", tg.name, tg.id));
    //        }
    //        return ret;
    //    }

    //    public int Cali()
    //    {
    //        int ret;
    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                ret = CapCali(ref array_list_tg[n]);
    //                if (ret == CONST.RES_OK)
    //                {
    //                    if (array_list_name[n] == "MK")
    //                    {
    //                        COM.tg.list_cur_mark.Clear();
    //                        foreach (TG_DATA tg in array_list_tg[n])
    //                            COM.tg.list_cur_mark.Add(tg.st_pos_cap_cur);
    //                        COM.tg.SaveCfg(VAR.gsys_set.cur_product_name);
    //                    }
    //                }
    //                else return ret;
    //            }
    //        }

    //        return CONST.RES_OK;
    //    }
    //    #endregion
    //    #region 修改拍照模式
    //    public void ModifyCapMode(int cap_mode)
    //    {
    //        bool bedit_all = false;

    //        if (cap_mode < 0) return;

    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    if (array_list_tg[n][m].bselected)
    //                    {
    //                        if ((int)array_list_tg[n][m].cap_mode != cap_mode)
    //                        {
    //                            if (bedit_all || DialogResult.Yes == MessageBox.Show(string.Format("是否修改整个类别？"), "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
    //                            {
    //                                bedit_all = true;
    //                                for (int mm = 0; mm < array_list_tg[n].Count; mm++)
    //                                {
    //                                    TG_DATA tg_temp = array_list_tg[n][mm];
    //                                    tg_temp.cap_mode = (TG_DATA.EM_CAP_MODE)(cap_mode);
    //                                    array_list_tg[n][mm] = tg_temp;
    //                                }
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    #endregion
    //    #region 修改区号
    //    public void ModifyAreaNum(int area_num)
    //    {
    //        bool bedit_all = false;

    //        if (area_num < 0) return;

    //        for (int n = 0; n < 6; n++)
    //        {
    //            if (array_chk[n])
    //            {
    //                for (int m = 0; m < array_list_tg[n].Count; m++)
    //                {
    //                    if (array_list_tg[n][m].bselected)
    //                    {
    //                        if ((int)array_list_tg[n][m].area_num != area_num)
    //                        {
    //                            if (bedit_all || DialogResult.Yes == MessageBox.Show(string.Format("是否修改整个类别？"), "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
    //                            {
    //                                bedit_all = true;
    //                                for (int mm = 0; mm < array_list_tg[n].Count; mm++)
    //                                {
    //                                    TG_DATA tg_temp = array_list_tg[n][mm];
    //                                    tg_temp.area_num = area_num;
    //                                    array_list_tg[n][mm] = tg_temp;
    //                                }
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    #endregion
    //}
    #endregion
    #region 补偿
    public class AXICMP
    {
        public bool enable = false;
        public double start_pos = 0;
        public double end_pos = 0;
        public double step = 1;
        public List<double> list_cmp = new List<double>();
        public string axis_disc = "";
        public AXICMP(string axis_disc)
        {
            this.axis_disc = axis_disc;
            ReadTextFileToList();
        }

        public void WriteListToTextFile()
        {
            //创建 
            string filename = Path.GetFullPath("..") + "\\syscfg\\" + axis_disc + ".cmp";
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            //定位 
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            //表头
            sw.WriteLine(axis_disc);
            sw.WriteLine(start_pos.ToString("F3"));
            sw.WriteLine(step.ToString("F3"));
            sw.WriteLine(list_cmp.Count.ToString());
            sw.WriteLine("----------------");
            //数据
            for (int i = 0; i < list_cmp.Count; i++) sw.WriteLine(list_cmp[i].ToString("F3"));
            //关闭
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public EM_RES ReadTextFileToList()
        {
            enable = false;
            string filename = Path.GetFullPath("..") + "\\syscfg\\" + axis_disc + ".cmp";
            if (!File.Exists(filename)) return EM_RES.OK;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            //定位
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            //disc
            string tmp = sr.ReadLine();
            if (tmp != null && tmp.Length > 0)
            {
                if (tmp != axis_disc)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}补偿文件,参数 axis_disc 异常,{1}", axis_disc, tmp));
                    return EM_RES.PARA_ERR;
                }
            }

            //start pos
            tmp = sr.ReadLine();
            if (tmp != null && tmp.Length > 0) start_pos = Convert.ToDouble(tmp);
            else
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}补偿文件,参数 start pos 异常", axis_disc));
                return EM_RES.PARA_ERR;
            }
            //step
            tmp = sr.ReadLine();
            if (tmp != null && tmp.Length > 0) step = Convert.ToDouble(tmp);
            else
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}补偿文件,参数 step 异常", axis_disc));
                return EM_RES.PARA_ERR;
            }
            //cnt
            int count = 0;
            tmp = sr.ReadLine();
            if (tmp != null && tmp.Length > 0) count = Convert.ToInt16(tmp);
            else
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}补偿文件,参数 count 异常", axis_disc));
                return EM_RES.PARA_ERR;
            }

            //----
            tmp = sr.ReadLine();

            //读取
            tmp = sr.ReadLine();
            list_cmp.Clear();
            while (tmp != null && tmp.Length > 0)
            {
                list_cmp.Add(Convert.ToDouble(tmp));
                tmp = sr.ReadLine();
            }

            if (list_cmp.Count != count)
            {
                VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}补偿文件,数据数量应为{1}，实际数量为{2}", axis_disc, list_cmp.Count, count));
                return EM_RES.PARA_ERR;
            }

            //关闭
            sr.Close();
            fs.Close();

            end_pos = start_pos + step * count;
            VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format("{0}启用补偿,start={1:F3},end={2:F3},step={3:F3},count={4}", axis_disc, start_pos, end_pos, step, list_cmp.Count));
            enable = true;

            return EM_RES.OK;
        }
        public double Cmp(double pos)
        {
            if (enable == false || list_cmp == null || list_cmp.Count == 0) return pos;
            if (pos < start_pos || pos > (start_pos + list_cmp.Count * step)) return pos;
            int idx = (int)((pos - start_pos) / step);
            if (idx > list_cmp.Count || idx < 0) return pos;
            if (idx >= list_cmp.Count - 1) return (pos + list_cmp.Last());

            return pos - ((list_cmp[idx + 1] - list_cmp[idx]) * (pos - (start_pos + idx * step)) / step + list_cmp[idx]);
        }

        public double DeCmp(double pos)
        {
            if (enable == false) return pos;
            if (pos < (start_pos - list_cmp.First()) || pos > (start_pos + list_cmp.Count * step) - list_cmp.Last()) return pos;
            int idx = (int)((pos - start_pos) / step);
            if (idx > list_cmp.Count) return pos;
            if (idx == list_cmp.Count) return (pos - list_cmp.Last());

            //pos = x + (list_cmp[idx + 1] - list_cmp[idx]) * (x - idx * step) / step + list_cmp[idx]
            return (pos + (list_cmp[idx + 1] - list_cmp[idx]) * idx - list_cmp[idx]) * step / (step + (list_cmp[idx + 1] - list_cmp[idx]));
        }
    }
    #endregion
    #region 通讯
    public static class COMM
    {
        //网口通讯连接

        public static bool bCnnect;//连接标志
      public  static string mip = "";
      public  static int mport ;
        public static int isServe;//是服务器还是客户端
        public static EM_RES Client_conn(SocketHelper.AxTcpClient qxTcpClient, string ip = "", int port =0)
        {
            if (!qxTcpClient.IsStartTcpthreading)
            {
                try
                {
                    qxTcpClient.ReConectedCount = 0;
                    qxTcpClient.ServerIp = ip;
                    qxTcpClient.ServerPort = port;
                    qxTcpClient.StartConnection();
                    System.Threading.Thread.Sleep(50);
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.SYS, "连接 " + qxTcpClient.ServerIp + ": " + qxTcpClient.ServerPort + "...!");
                }
                catch (Exception)
                {
                    VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, "连接 " + qxTcpClient.ServerIp + ": " + qxTcpClient.ServerPort + "...失败!");
                    return EM_RES.ERR;
                }
            }
            return EM_RES.OK;
        }
        public static void Client_send(SocketHelper.AxTcpClient qxTcpClient, string cmd)
        {
            if (qxTcpClient == null) return;
            qxTcpClient.SendCommand(cmd);
        }
        public static void Client_stop(SocketHelper.AxTcpClient qxTcpClient, string cmd)
        {
            if (qxTcpClient == null) return;
            qxTcpClient.StopConnection();
        }

        public static void server_send(SocketHelper.AxTcpServer qxTcpClient, string cmd)
        {
            if (qxTcpClient == null) return;
            LoadCfg();
            string client_ip = mip;
            int client_port = mport;
            string data = cmd;

            qxTcpClient.SendData(client_ip, client_port, data);

        }
        //其他状态，收到数据等在控件事件中，使用方便

        #region 保存读取
        public static EM_RES LoadCfg()
        {
            //产品参数
            string filename = string.Format("{0}\\syscfg\\syscfg.ini", Path.GetFullPath(".."));
            if (!File.Exists(filename))
            {
                File.Create(filename);
                //VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}初始化对应产品名{1}配置文件不存在!", disc, productname));
                //return EM_RES.PARA_ERR;
            }
            IniFile inf = new IniFile(filename);
            string section = string.Format("COMMcfg");
            mip = inf.ReadString(section, "IP", mip.ToString());
            mport = inf.ReadInteger(section, "PORT", 1);
            isServe = inf.ReadInteger(section, "isServe", 1);
            return EM_RES.OK;
        }

        public static EM_RES SavCfg()
        {

            //产品参数
            string filename = string.Format("{0}\\syscfg\\syscfg.ini", Path.GetFullPath(".."));

            if (!File.Exists(filename))
            {
                File.Create(filename);
                //   VAR.msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format("{0}初始化对应产品名{1}配置文件不存在!", disc, productname));
                // return EM_RES.PARA_ERR;
            }

            IniFile inf = new IniFile(filename);
            string section = string.Format("COMMcfg");
  
            inf.WriteString(section, "IP", mip);
            inf.WriteInteger(section, "PORT", mport);
            inf.WriteInteger(section, "isServe", isServe);
            return EM_RES.OK;
        }
        #endregion



        //}
    }


    #endregion
    #region 视觉
  
    public  class VS
    {
        //网口通讯连接
        public static bool bCam;//有无相机
        public  CAMERA CamGet;//拍照获取
  
        public struct ST_SET
        {
         
            //相机1 模板信息

            public ST_XYZ L_mode_modu_pos;   // 左工位—模组           
            public ST_XYZ R_mode_modu_pos;  // 右工位—模组
            //相机2模板
            public ST_XYZ Feed_mode_pos;    // 左工位—连接

            //相机1像素比例
            public ST_XYZ modu_x_scale;     //左工位 模组-x轴
            public ST_XYZ modu_y_scale;      //左工位模组-y轴
            //相机2像素比例
            public ST_XYZ Feed_x_scale;     //左工位连接-x轴
            public ST_XYZ Feed_y_scale;     //左工位连接-y轴
            // 通用旋转中心
            public ST_XYZ L_mouth1_center;         //吸嘴旋转中心，视觉
            public ST_XYZ L_mouth2_center;

            public ST_XYZ Feed_mouth_center;
            public double Ang_min;
            public double Ang_max;
        }
        public static ST_SET st_set;
        //List<ST_XYZ> list_xyz = new List<ST_XYZ> { st_set.Feed_mode_pos, st_set.Feed_mouth_center,
        //st_set.Feed_x_scale,  st_set.Feed_y_scale,  st_set.L_mode_modu_pos,   st_set.L_mouth1_center,
        //st_set.modu_x_scale,  st_set.modu_y_scale,  st_set.R_mode_modu_pos ,st_set.L_mouth2_center
        //};

        public EM_RES m_camera_action(int photo_id, out ST_XYZ m_result)
        {
            try
            {
               
                ST_XYZ m_camera_get;
              
                String camera_name;
                short m_camera_id;    // 四个相机编号
                ST_XYZ m_mouth_home;    ////在拍照位置吸嘴旋转中心位置
                ST_XYZ m_x_cli, m_y_cli;  //旋转偏移计算xy modu相机视觉比例           
                ST_XYZ m_mod_pos;         //  模板位置  或者连接板或者模组模板
                int i;
                m_result.x = 0;
                m_result.y = 0;
                m_result.z = 0;
                if(UI.Action.bNullRun)  //空跑直接成功            
                    return EM_RES.OK;                              
                switch (photo_id)
                {
                    case 1:
                        m_camera_id = 1;
                        camera_name = "左工位mod1拍照";
                        m_mouth_home = st_set.L_mouth1_center;
                        m_x_cli = st_set.modu_x_scale;
                        m_y_cli = st_set.modu_y_scale;
                        m_mod_pos = st_set.L_mode_modu_pos;
                        break;
                    case 2:
                        m_camera_id = 1;
                        camera_name = "左工位mod2拍照";
                        m_mouth_home = st_set.L_mouth2_center;
                        m_x_cli = st_set.modu_x_scale;
                        m_y_cli = st_set.modu_y_scale;
                        m_mod_pos = st_set.R_mode_modu_pos;
                        break;
                    case 3:
                        m_camera_id = 2;
                        camera_name = "上料夹爪拍照";
                        m_mouth_home = st_set.Feed_mouth_center;
                        m_x_cli = st_set.Feed_x_scale;
                        m_y_cli = st_set.Feed_y_scale;
                        m_mod_pos = st_set.Feed_mode_pos;
                        break;
                    default:
                        return EM_RES.ERR;
                }
                EM_RES ret;
                double d_a, d_x, d_y;
                double x_1;     //如果模组拍照 m_camera_modu= m_camera_get，连接板拍照 m_camera_modu等于与之扣合的模组数据
                double y_1;
                if (Action.bNullRun)
                    return EM_RES.OK;
                ret = get_vs(out m_camera_get, m_camera_id);  
                if (ret != EM_RES.OK) return ret;
                //判断角度合格
                x_1 = m_camera_get.x;
                y_1 = m_camera_get.y;

                double x_0 = m_mouth_home.x; //旋转中心，吸嘴位置
                double y_0 = m_mouth_home.y;

                d_a = -(m_camera_get.z - m_mod_pos.z);
                d_a = d_a * 3.1415926 / 180.0;

                d_x = (x_1 - x_0) * Math.Cos(d_a) - (y_1 - y_0) * Math.Sin(d_a) + x_0 - x_1;
                d_y = (x_1 - x_0) * Math.Sin(d_a) + (y_1 - y_0) * Math.Cos(d_a) + y_0 - y_1;
                double x_a = m_x_cli.x;
                double y_a = m_x_cli.y;
                double x_b = m_y_cli.x;
                double y_b = m_y_cli.y;

                double length_x1 = (d_x * y_b - d_y * x_b) / (x_a * y_b - x_b * y_a);
                double length_y1 = (d_x * y_a - d_y * x_a) / (x_b * y_a - x_a * y_b);
                double d_x2;
                double d_y2;
                d_x2 = m_mod_pos.x - x_1;
                d_y2 = m_mod_pos.y - y_1;

                double length_x2 = (d_x2 * y_b - d_y2 * x_b) / (x_a * y_b - x_b * y_a);
                double length_y2 = (d_x2 * y_a - d_y2 * x_a) / (x_b * y_a - x_a * y_b);
                double length_a = d_a;
                //  弧度变成角度
                length_a = length_a / 3.1415926 * 180.0;

                m_result.x = length_x1 + length_x2;
                m_result.y = length_y1 + length_y2;
                m_result.z = length_a;
                return EM_RES.OK;
            }
            catch(Exception e)
            {
                VAR.ErrMsg(e.ToString());
                m_result.x = 0;
                m_result.y = 0;
                m_result.z = 0;
                return EM_RES.ERR;
            }
        }
        public EM_RES get_vs(out ST_XYZ mres , int camera_id = 1,     int sch_id = 0)
        {
            EM_RES ret = EM_RES.OK;
            if (CamGet == null)
            {
                VAR.ErrMsg("拍照未定义");
                mres.x = 0;
                mres.y = 0;
                mres.z = 0;
                return EM_RES.ERR;
            }
            ret = CamGet(out mres, camera_id);   //0是非标定参数
            return ret;
        }
        #region 加载与保存参数
        EM_RES LoadInfXYZ(out ST_XYZ mxyz, string section, string filename = "")
        {
            try
            {

                if (filename.Length < 3)
                    filename = Path.GetFullPath("..") + "\\product\\"+ VAR.gsys_set.cur_product_name+"\\vscfg.ini";

                IniFile inf = new IniFile(filename);
                string Section = string.Format(section);
                mxyz.x = inf.ReadDouble(Section, "x", 0);
                mxyz.y = inf.ReadDouble(Section, "y", 0);
                mxyz.z = inf.ReadDouble(Section, "z", 0);
            }
            catch
            {
                //  VAR.msg.AddMsg();
                mxyz.x = 0;
                mxyz.y = 0;
                mxyz.z = 0;
                return EM_RES.ERR;
            }
            return EM_RES.OK;
        }
        EM_RES SaveInfXYZ(ST_XYZ mxyz, string section, string filename = "")
        {
            try
            {
                if (filename.Length < 3) 
                filename = Path.GetFullPath("..") + "\\product\\" + VAR.gsys_set.cur_product_name + "\\vscfg.ini";
                if (!File.Exists(filename))
                    File.Create(filename);
                IniFile inf = new IniFile(filename);
                string Section = string.Format(section);
                inf.WriteDouble(Section, "x", mxyz.x);
                inf.WriteDouble(Section, "y", mxyz.y);
                inf.WriteDouble(Section, "z", mxyz.z);
                return EM_RES.OK;
            }
            catch
            {
                return EM_RES.ERR;
            }
        }
        public EM_RES LoadInf()
        {
            EM_RES ret = EM_RES.OK;

            ret = LoadInfXYZ(out st_set.Feed_mode_pos, "Feed_mode_pos");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.Feed_mouth_center, "Feed_mouth_center");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.Feed_x_scale, "Feed_x_scale");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.Feed_y_scale, "Feed_y_scale");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.L_mode_modu_pos, "L_mode_modu_pos");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.L_mouth1_center, "L_mouth1_center");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.L_mouth2_center, "L_mouth2_center");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.modu_x_scale, "modu_x_scale");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.modu_y_scale, "modu_y_scale");
            if (ret != EM_RES.OK) return ret;
            ret = LoadInfXYZ(out st_set.R_mode_modu_pos, "R_mode_modu_pos");
            if (ret != EM_RES.OK) return ret;
            return ret;
        }
        public EM_RES SaveInf()
        {
            EM_RES ret = EM_RES.OK;
            ret = SaveInfXYZ(st_set.Feed_mode_pos, "Feed_mode_pos");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.Feed_mouth_center, "Feed_mouth_center");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.Feed_x_scale, "Feed_x_scale");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.Feed_y_scale, "Feed_y_scale");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.L_mode_modu_pos, "L_mode_modu_pos");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.L_mouth1_center, "L_mouth1_center");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.L_mouth2_center, "L_mouth2_center");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.modu_x_scale, "modu_x_scale");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.modu_y_scale, "modu_y_scale");
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(st_set.R_mode_modu_pos, "R_mode_modu_pos");
            if (ret != EM_RES.OK) return ret;
            return ret;
        }
        /// <summary>
        /// 拍照获取模板数据
        /// </summary>
        /// <param name="mxyz"></param>模板
        /// <param name="section"></param>模板名字
        /// <returns></returns>
        public EM_RES save_mode_pos(ST_XYZ mxyz, int cam_id = 2, string section = "Feed_mode_pos")
        {
            EM_RES ret = EM_RES.OK;
            ret = get_vs(out mxyz, cam_id);
            if (ret != EM_RES.OK) return ret;
            ret = SaveInfXYZ(mxyz, "section");
            if (ret != EM_RES.OK) return ret;
            return ret;
        }

        public EM_RES cam_get_scle(ref bool bquit, int cam_id = 1)
        {
            EM_RES ret=EM_RES.OK;
            double scale_length=2;//偏移距离2mm
            ST_XYZ start_pos,xmove_pos,ymove_pos;
            if(cam_id==1)
            {
                ret = WSGet.ps_pho_L.MoveTo(ref  bquit,true);
                ret = get_vs(out start_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSGet.ps_pho_L.AxisX.MoveTo(ref bquit, WSGet.ps_pho_L.AxisX.fcmd_pos + scale_length,3000);
                if (ret != EM_RES.OK) return ret; 
                ret = get_vs(out xmove_pos, cam_id);
                if (ret != EM_RES.OK) return ret; 
                st_set.modu_x_scale.x = (xmove_pos.x - start_pos.x) / scale_length;
                st_set.modu_x_scale.y = (xmove_pos.y - start_pos.y) / scale_length;
              
                ret = WSGet.ps_pho_L.MoveTo(ref  bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out start_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSGet.ps_pho_L.AxisY.MoveTo(ref bquit, WSGet.ps_pho_L.AxisY.fcmd_pos + scale_length, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out ymove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                st_set.modu_y_scale.x = (ymove_pos.x - start_pos.x) / scale_length;
                st_set.modu_y_scale.y = (ymove_pos.y - start_pos.y) / scale_length;             
            }
            else
            {
                ret = WSFeed.ps_photo_L.MoveTo(ref  bquit, true);       
                ret = get_vs(out start_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSFeed.ps_photo_L.AxisX.MoveTo(ref bquit, WSGet.ps_pho_L.AxisX.fcmd_pos + scale_length, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out xmove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                st_set.Feed_x_scale.x = (xmove_pos.x - start_pos.x) / scale_length;
                st_set.Feed_x_scale.y = (xmove_pos.y - start_pos.y) / scale_length;

                ret = WSFeed.ps_photo_L.MoveTo(ref  bquit, true);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out start_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSFeed.ps_photo_L.AxisY.MoveTo(ref bquit, WSGet.ps_pho_L.AxisY.fcmd_pos + scale_length, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out ymove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                st_set.Feed_y_scale.x = (ymove_pos.x - start_pos.x) / scale_length;
                st_set.Feed_y_scale.y = (ymove_pos.y - start_pos.y) / scale_length;  
            }
            SaveInf();
            return ret;
        }
        public EM_RES cam_get_center(ref bool bquit, int cam_id = 1)
        {
            EM_RES ret = EM_RES.OK;
            double scale_angle = 2;//偏移距离2度
            ST_XYZ start_pos, xmove_pos, ymove_pos;
            if (cam_id == 1)
            {
                ret = WSGet.ps_pho_L.MoveTo(ref  bquit, true);
                ret = get_vs(out start_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSGet.ps_pho_L.AxisA.MoveTo(ref bquit, WSGet.ps_pho_L.AxisA.fcmd_pos + scale_angle, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out xmove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSGet.ps_pho_L.AxisA.MoveTo(ref bquit, WSGet.ps_pho_L.AxisA.fcmd_pos + scale_angle, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out ymove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;

                double mpos_u, mpos_v, mpos_m, mpos_k;
                mpos_u = (start_pos.x * start_pos.x - xmove_pos.x * xmove_pos.x + start_pos.y * start_pos.y - xmove_pos.y * xmove_pos.y) / (2 * start_pos.x - 2 * xmove_pos.x);
                mpos_m = (start_pos.y - xmove_pos.y) / (start_pos.x - xmove_pos.x);
                mpos_v = (start_pos.x * start_pos.x - ymove_pos.x * ymove_pos.x + start_pos.y * start_pos.y - ymove_pos.y * ymove_pos.y) / (2 * start_pos.x - 2 * ymove_pos.x);
                mpos_k = (start_pos.y - ymove_pos.y) / (start_pos.x - ymove_pos.x);
                st_set.L_mouth1_center.x = (mpos_u - mpos_v) / (mpos_m - mpos_k);
                st_set.L_mouth1_center.y = mpos_v - (mpos_u - mpos_v) * mpos_k / (mpos_m - mpos_k);

            }
            else
            {

                ret = WSFeed.ps_photo_L.MoveTo(ref  bquit, true);
                ret = get_vs(out start_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSFeed.ps_photo_L.AxisA.MoveTo(ref bquit, WSFeed.ps_photo_L.AxisA.fcmd_pos + scale_angle, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out xmove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                ret = WSFeed.ps_photo_L.AxisA.MoveTo(ref bquit, WSFeed.ps_photo_L.AxisA.fcmd_pos + scale_angle, 3000);
                if (ret != EM_RES.OK) return ret;
                ret = get_vs(out ymove_pos, cam_id);
                if (ret != EM_RES.OK) return ret;
                //三点确定圆心

                double mpos_u, mpos_v, mpos_m, mpos_k;
                mpos_u = (start_pos.x * start_pos.x - xmove_pos.x * xmove_pos.x + start_pos.y * start_pos.y - xmove_pos.y * xmove_pos.y) / (2 * start_pos.x - 2 * xmove_pos.x);
                mpos_m = (start_pos.y - xmove_pos.y) / (start_pos.x - xmove_pos.x);
                mpos_v = (start_pos.x * start_pos.x - ymove_pos.x * ymove_pos.x + start_pos.y * start_pos.y - ymove_pos.y * ymove_pos.y) / (2 * start_pos.x - 2 * ymove_pos.x);
                mpos_k = (start_pos.y - ymove_pos.y) / (start_pos.x - ymove_pos.x);
                st_set.Feed_mouth_center.x = (mpos_u - mpos_v) / (mpos_m - mpos_k);
                st_set.Feed_mouth_center.y = mpos_v - (mpos_u - mpos_v) * mpos_k / (mpos_m - mpos_k);
            }
            SaveInf();
            return ret;
          
        }
        #endregion

    }

   
    #endregion

}
