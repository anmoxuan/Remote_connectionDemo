using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.IO;

namespace EH
{
    /// <summary>
    /// EPStart ��ժҪ˵����
    /// </summary>
    public class EHStart
    {
        public EHStart()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        [STAThread]
        static int Main(string[] args)
        {
            int EHMax = 5;
            try
            {
                System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("EH");
                if (process.Length > EHMax)
                {
                    MessageBox.Show("��ע�⣬ÿ̨����ֻ�����޴ε�½", "����MES��Ʒ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[EH] System.Diagnostics.Process Error : " + ex.ToString());
            }
            
            //��������XML�ļ�
            string strEhPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string strEpVerUpdatePath = System.IO.Path.Combine(strEhPath, ".\\EP\\EPVerUpdate.exe");
            string strEpVerUpdatePath2 = System.IO.Path.Combine(strEhPath, ".\\EPVerUpdate.exe");
            string strLaunchPath = System.IO.Path.Combine(strEhPath, ".\\EP\\Launch.exe");
            string strLaunchPath2 = System.IO.Path.Combine(strEhPath, ".\\Launch.exe");
            string strEpPath = System.IO.Path.Combine(strEhPath, ".\\EP\\EPEntry.dll");
            try
            {
                if (File.Exists(strEpVerUpdatePath))
                {
                    File.Copy(
                        strEpVerUpdatePath,
                        strEpVerUpdatePath2,
                        true);
                    File.Delete(strEpVerUpdatePath);
                }
                if (File.Exists(strLaunchPath))
                {
                    File.Copy(
                        strLaunchPath,
                        strLaunchPath2,
                        true);
                    File.Delete(strLaunchPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            string assamplyPath = strEpPath;
            try
            {
                Assembly assembly = Assembly.LoadFrom(assamplyPath);
                Type typeEP = assembly.GetType("EP.EPLogin");
                MethodInfo mi = typeEP.GetMethod("Startup", BindingFlags.Public | BindingFlags.Static);
                object[] i_args = new object[] { args };
                int retFlag = (int)mi.Invoke(null, i_args);
                Console.WriteLine("Return code is {0}", retFlag);
                return retFlag;
            }
            catch (Exception e)
            {
                log4net.ILog logger = log4net.LogManager.GetLogger("System Logs");

                if (logger.IsInfoEnabled)
                {
                    logger.Info(e.ToString());
                }

                return -1;
            }
        }

    }

}
