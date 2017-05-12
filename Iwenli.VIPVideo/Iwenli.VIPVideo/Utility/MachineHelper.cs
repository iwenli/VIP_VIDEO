using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace Iwenli.VIPVideo.Utility
{
    /// <summary>
    /// 获取机器信息
    /// </summary>
    public static class MachineHelper
    {

        #region 属性
        /// <summary>
        /// 获取操作系统版本(PC,PDA均支持)
        /// </summary>
        public static string OSVersion { get { return Environment.OSVersion.ToString(); } }

        /// <summary>
        /// 获取应用程序当前目录(PC支持)
        /// </summary>
        public static string CurrentDirectory { get { return Environment.CurrentDirectory; } }

        /// <summary>
        /// 获取硬盘盘符
        /// </summary>
        public static List<string> GetLogicalDrives
        {
            get
            {
                List<string> drives = new List<string>();
                drives.AddRange(Environment.GetLogicalDrives());
                return drives;
            }
        }

        /// <summary>
        /// 获取.Net Framework版本号(PC,PDA均支持)
        /// </summary>
        public static string NetVersion { get { return Environment.Version.ToString(); } }

        /// <summary>
        /// 获取机器名(PC支持)
        /// </summary>
        public static string MachineName { get { return Environment.MachineName; } }

        /// <summary>
        /// 获取当前登录用户(PC支持)
        /// </summary>
        public static string UserName { get { return Environment.UserName; } }

        /// <summary>
        /// 获取域名(PC支持)
        /// </summary>
        public static string UserDomainName { get { return Environment.UserDomainName; } }

        /// <summary>
        /// 获取开机时长，即截至到当前时间，操作系统启动的毫秒数(PDA,PC均支持)
        /// </summary>
        public static long TickCount { get { return Environment.TickCount; } }

        /// <summary>
        /// 获取开机时长, *天 *小时 *分 *秒 ,即截至到当前时间，操作系统启动的毫秒数(PDA,PC均支持)
        /// </summary>
        public static string TickCountString { get { return FormatTimestamp(Environment.TickCount); } }
        #endregion

        #region 时间操作相关
        /// <summary>
        /// 将毫秒转换成 *天 *小时 *分 *秒 的格式展示
        /// </summary>
        /// <param name="mss"></param>
        /// <returns></returns>
        public static String FormatTimestamp(long mss)
        {
            long days = mss / (1000 * 60 * 60 * 24);
            long hours = (mss % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60);
            long minutes = (mss % (1000 * 60 * 60)) / (1000 * 60);
            long seconds = (mss % (1000 * 60)) / 1000;
            return days + "天 " + hours + "小时 " + minutes + "分 "
                    + seconds + "秒";
        }

        /// <summary>
        ///  输入的两个Date类型数据之间的时间间格用 *天 *小时 *分 *秒 的格式展示 
        /// </summary>
        /// <param name="begin">时间段的开始</param>
        /// <param name="end">时间段的结束</param>
        /// <returns></returns>
        public static String FormatTimestamp(DateTime begin, DateTime end)
        {
            return FormatTimestamp(ConvertDateTimeInt(end) - ConvertDateTimeInt(begin));
        }

        /// <summary>
        /// 将ateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
        }

        #endregion

        #region 磁盘文件路径获取相关
        /// <summary>
        /// 保存文件路径信息到相对程序运行目录的文件中
        /// </summary>
        /// <param name="fileExtension">后缀名，默认包含txt doc wps xls ppt pdf jpg bmp rmvb mp3 exe js cs css html</param>
        /// <param name="savePath">文件保存相对路径</param>
        public static bool SaveAllDiskFile(List<string> fileExtension, string savePath = "ini")
        {
            try
            {
                if (fileExtension.Count == 0)
                {
                    fileExtension = new List<string>() { "txt", "doc", "wps", "xls", "ppt", "pdf", "jpg", "bmp", "rmvb", "mp3", "exe", "js", "cs", "css", "html" };
                }
                //保存文件的目录
                string saveFilePath = Path.Combine(CurrentDirectory, savePath);
                if (!Directory.Exists(saveFilePath))
                {
                    Directory.CreateDirectory(saveFilePath);
                }
                foreach (var currentExtension in fileExtension)
                {
                    List<string> allFilePaht = GetAllDiskFile(currentExtension);
                    if (allFilePaht.Count > 0)
                    {
                        string content = string.Join(Environment.NewLine, allFilePaht.ToArray());
                        File.WriteAllText(string.Format(@"{0}\\{1}_{2}.txt", saveFilePath, MachineName, currentExtension), content);
                        Thread.Sleep(2000);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 获取除C盘所有指定扩展名的文件全路径
        /// </summary>
        /// <param name="extension">扩展名</param>
        /// <returns></returns>
        public static List<string> GetAllDiskFile(string extension = "txt")
        {
            List<string> allFilePath = new List<string>();
            foreach (var item in GetLogicalDrives)
            {
                if (item.ToLower().Contains("c:"))
                {
                    continue;
                }
                allFilePath.AddRange(GetFile(item, extension));
            }
            return allFilePath;
        }

        /// <summary>
        /// 获取指定磁盘下所有指定扩展名的文件全路径
        /// </summary>
        /// <param name="diskPath">磁盘路径,默认d:\\</param>
        /// <param name="extension">扩展名</param>
        /// <returns></returns>
        public static List<string> GetFile(string diskPath = "d:\\", string extension = "txt")
        {
            List<string> allFilePath = new List<string>();
            allFilePath.AddRange(Directory.GetFiles(diskPath));//根目录文件检索
            //获取下级目录开始遍历
            string[] currentDiskAllDirectory = Directory.GetDirectories(diskPath);
            foreach (var childDirectory in currentDiskAllDirectory)
            {
                if (childDirectory.ToLower().Contains("system volume information") ||
                childDirectory.ToLower().Contains("$recycle.bin") ||
                childDirectory.ToLower().Contains("documents and settings"))
                {
                    continue;
                }
                string[] childFill = Directory.GetFiles(childDirectory, "*." + extension, System.IO.SearchOption.AllDirectories);
                allFilePath.AddRange(childFill);
            }
            return allFilePath;
        }
        #endregion

        #region 硬件信息相关

        /*
        /// <summary>
        /// 获取操作系统的相关信息
        /// </summary>
        /// <returns></returns>
        public static string GetManagmentByWin32()
        {
            StringBuilder strBuilder = new StringBuilder();
            ManagementClass searClass = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = searClass.GetInstances();
            foreach (ManagementObject obj in moc)
            {
                //PropertyDataCollection pdc=obj.Properties; 
                strBuilder.Append("系统名称:" + obj.Properties["Caption"].Value.ToString() + "\n");
                //strBuilder.Append("版本:" + obj.Properties["CSDVersion"].Value.ToString() + "\n");
                strBuilder.Append("计算机名:" + obj.Properties["CSName"].Value.ToString() + "\n");
                strBuilder.Append("单位:" + obj.Properties["Organization"].Value.ToString() + "\n");
                strBuilder.Append("序列号:" + obj.Properties["SerialNumber"].Value.ToString() + "\n");
                strBuilder.Append("系统所在分区:" + obj.Properties["SystemDrive"].Value.ToString() + "\n");
                strBuilder.Append("系统目录:" + obj.Properties["WindowsDirectory"].Value.ToString() + "\n");
                strBuilder.Append("可用内存:" + obj.Properties["TotalVisibleMemorySize"].Value.ToString() + "\n");
                strBuilder.Append("可用内存2：" + obj.Properties["FreePhysicalMemory"].Value.ToString() + "\n");
                strBuilder.Append("版本号:" + obj.Properties["Version"].Value.ToString() + "\n");
                //strBuilder.Append(obj);  

            }
            return strBuilder.ToString();

        }
       /// <summary>
       /// 获取机器的相关信息
       /// </summary>
       /// <returns></returns>
       public static string GetManagmentByPcSys()
       {
           StringBuilder strBuilder = new StringBuilder();
           ManagementClass searClass = new ManagementClass("Win32_ComputerSystem");
           ManagementObjectCollection moc = searClass.GetInstances();
           foreach (ManagementObject obj in moc)
           {
               //PropertyDataCollection pdc=obj.Properties;  

               strBuilder.Append("处理器数量:" + obj.Properties["NumberOfProcessors"].Value.ToString() + "\n");
               strBuilder.Append("描述:" + obj.Properties["Description"].Value.ToString() + "\n");
               strBuilder.Append("计算机全称:" + obj.Properties["Name"].Value.ToString() + "\n");
               strBuilder.Append("域:" + obj.Properties["Domain"].Value.ToString() + "\n");
               strBuilder.Append("系统类型:" + obj.Properties["SystemType"].Value.ToString() + "\n");
               strBuilder.Append("实际内存:" + obj.Properties["TotalPhysicalMemory"].Value.ToString() + "\n");
               strBuilder.Append("用户名:" + obj.Properties["UserName"].Value.ToString() + "\n");
               //strBuilder.Append(obj);  

           }
           return strBuilder.ToString();
       }

       /// <summary>
       /// 获取CPU相关信息
       /// </summary>
       /// <returns></returns>
       public static string GetManagmentByCPU()
       {
           StringBuilder strBuilder = new StringBuilder();
           ManagementClass searClass = new ManagementClass("Win32_Processor");
           ManagementObjectCollection moc = searClass.GetInstances();
           foreach (ManagementObject obj in moc)
           {
               //PropertyDataCollection pdc=obj.Properties;  

               strBuilder.Append("编号:" + obj.Properties["DeviceID"].Value.ToString() + "\n");
               strBuilder.Append("CPU序列:" + obj.Properties["ProcessorId"].Value.ToString() + "\n");
               strBuilder.Append("CPU状态:" + obj.Properties["CpuStatus"].Value.ToString() + "\n");
               strBuilder.Append("类别:" + obj.Properties["Caption"].Value.ToString() + "\n");
               strBuilder.Append("名称:" + obj.Properties["Name"].Value.ToString() + "\n");
               strBuilder.Append("描述:" + obj.Properties["Description"].Value.ToString() + "\n");
               strBuilder.Append("寻址:" + obj.Properties["AddressWidth"].Value.ToString() + "\n");
               strBuilder.Append("二级缓容量:" + obj.Properties["L2CacheSize"].Value.ToString() + "\n");
               //strBuilder.Append("二级缓频率:" + obj.Properties["L2CacheSpeed"].Value.ToString() + "\n");  
               strBuilder.Append("数据宽带:" + obj.Properties["DataWidth"].Value.ToString() + "\n");
               strBuilder.Append("最高时钟频率:" + obj.Properties["MaxClockSpeed"].Value.ToString() + "\n");
               strBuilder.Append("版本:" + obj.Properties["Version"].Value.ToString() + "\n");
               strBuilder.Append("时钟频率:" + obj.Properties["CurrentClockSpeed"].Value.ToString() + "\n");
               //strBuilder.Append(obj);  

           }
           return strBuilder.ToString();
       }

       /// <summary>
       /// 获取硬盘相关信息
       /// </summary>
       /// <returns></returns>
       public static string GetManagmentByDisk()
       {
           StringBuilder strBuilder = new StringBuilder();
           ManagementClass searClass = new ManagementClass("Win32_LogicalDisk");
           ManagementObjectCollection moc = searClass.GetInstances();
           try
           {
               foreach (ManagementObject obj in moc)
               {
                   if (int.Parse(obj.Properties["DriveType"].Value.ToString()) == 3)//固定磁盘，硬盘  
                   {
                       strBuilder.Append("卷标:" + obj.Properties["VolumeName"].Value.ToString() + "\n");
                       strBuilder.Append("序列号:" + obj.Properties["VolumeSerialNumber"].Value.ToString() + "\n");
                       strBuilder.Append("驱动器号:" + obj.Properties["Name"].Value.ToString() + "\n");
                       strBuilder.Append("分区大小:" + obj.Properties["Size"].Value.ToString() + "\n");
                       strBuilder.Append("可用空间:" + obj.Properties["FreeSpace"].Value.ToString() + "\n");
                       strBuilder.Append("文件系统:" + obj.Properties["FileSystem"].Value.ToString() + "\n");
                       strBuilder.Append("描述:" + obj.Properties["Description"].Value.ToString() + "\n");
                       //break;  
                   }
               }
           }
           catch (Exception ex)
           {
               throw new Exception("未找到该属性");
           }
           return strBuilder.ToString();
       } 
       */
        #endregion

        /// <summary>
        /// 获取当前设备相关信息
        /// </summary>
        /// <returns></returns>
        public new static string ToString()
        {
            StringBuilder _returnSb = new StringBuilder();
            _returnSb.AppendFormat("计算机名称：{0}{1}", MachineName, Environment.NewLine);
            _returnSb.AppendFormat("当前登录用户：{0}{1}", UserName, Environment.NewLine);
            _returnSb.AppendFormat("当前登录域：{0}{1}", UserDomainName, Environment.NewLine);
            _returnSb.AppendFormat("已开机：{0}{1}", TickCountString, Environment.NewLine);
            _returnSb.AppendLine();
            _returnSb.AppendFormat("启动路径：{0}{1}", CurrentDirectory, Environment.NewLine);
            _returnSb.AppendFormat("操作系统：{0}{1}", OSVersion, Environment.NewLine);
            _returnSb.AppendFormat("FreameWork版本：{0}{1}", NetVersion, Environment.NewLine);
            _returnSb.AppendLine();
            _returnSb.AppendLine();

            ManagementClass searClass = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = searClass.GetInstances();
            foreach (ManagementObject obj in moc)
            {
                _returnSb.AppendFormat("系统名称:{0}{1}", obj.Properties["Caption"].Value.ToString(), Environment.NewLine);
                _returnSb.AppendFormat("系统所在分区:{0}{1}", obj.Properties["SystemDrive"].Value.ToString(), Environment.NewLine);
                _returnSb.AppendFormat("系统目录:{0}{1}", obj.Properties["WindowsDirectory"].Value.ToString(), Environment.NewLine);
                _returnSb.AppendFormat("可用内存:{0}{1}", obj.Properties["TotalVisibleMemorySize"].Value.ToString(), Environment.NewLine);
                _returnSb.AppendFormat("可用内存2：{0}{1}", obj.Properties["FreePhysicalMemory"].Value.ToString(), Environment.NewLine);
                _returnSb.AppendFormat("版本号:{0}{1}", obj.Properties["Version"].Value.ToString(), Environment.NewLine);
            }

            return _returnSb.ToString();
        }

    }
}
