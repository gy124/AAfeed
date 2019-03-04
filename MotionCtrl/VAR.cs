using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;

namespace MotionCtrl
{
    public class VAR
    {
        #region 视觉数据
        //public struct ST_VS_DAT
        //{
        //    public int cs;		        //对应流程编号
        //    public ST_XYZ st_cap_pos;   //拍照位置
        //    public ST_XYZ st_cpp;	    //像素坐标
        //    public ST_XYZ st_cap;	    //mm坐标
        //    public ST_XYZ st_capc;	    //与画面中心偏差
        //    public ST_XYZ st_cppt;
        //    //..
        //    public string IDCode; //读取二维码数据
        //    //..
        //    public bool res;		    //结果
        //    public bool bupdate;	    //更新标志
        //    public ICogImage outputImage;//输出图像
        //    public CogGraphicCollection GraphicCollection;//输出界面绘图
        //    public CogCompositeShape GraphicsPMAlignTool;//输出PMAlignTool匹配图形
        //    public double CT;//CT时间
        //    public List<CogPointMarker> ListTopCamera;
        //}
        ////视觉参数
        //public static ST_VS_DAT[] gst_VsData_Temp = new ST_VS_DAT[4];
        //public static ST_VS_DAT[] gst_VsData = new ST_VS_DAT[4];
        //public static ST_VS_DAT[,] gst_VsData_Clib_Cam = new ST_VS_DAT[4, 4];
        //public static int[] RunMode = new int[4];
        //public static int[] CaliMode = new int[4];
        //public static ST_XYZ[] st_pos_calc =new ST_XYZ[4];
        //public static bool[] VisionStatus = new bool[2];
        #endregion

        /// <summary>
        ///系统设置
        /// </summary>
        public static SYS_SET gsys_set = new SYS_SET();
        public static readonly object lockthis = new object();

        /// <summary>
        /// 打印信息
        /// </summary>
        public static Msg msg = new Msg();
        public static void ErrMsg(string inf = "未知错误") 
        {

            msg.AddMsg(Msg.EM_MSGTYPE.ERR, string.Format(inf));
        }
        public static void SysMsg(string inf = "系统信息")
        {

            msg.AddMsg(Msg.EM_MSGTYPE.SYS, string.Format(inf));
        }
        public static void WarnMsg(string inf = "警告信息")
        {

            msg.AddMsg(Msg.EM_MSGTYPE.WAR, string.Format(inf));
        }
        /// <summary>
        /// 系统提示
        /// </summary>
        public static SYS_INF sys_inf = new SYS_INF();
    }
}
