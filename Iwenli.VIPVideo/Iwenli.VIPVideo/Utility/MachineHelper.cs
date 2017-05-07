using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Iwenli.VIPVideo.Utility
{
    /// <summary>
    /// 获取机器信息
    /// </summary>
    public static class MachineHelper
    {
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
    }
}
