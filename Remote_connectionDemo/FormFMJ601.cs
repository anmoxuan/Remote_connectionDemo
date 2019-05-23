using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using EF;
using EI;

namespace FM
{
    public partial class FormFMJ601 : EFFormMain
    {
        private Dictionary<String, String> FinAllTree = new Dictionary<string, string>();
        public FormFMJ601()
        {
            InitializeComponent();
            Logger.Info("程序启动----"+DateTime.Now.ToString());
        }
        //窗体启动显示参数
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                AddNodes();
                XmlDataDocument xmlData = new XmlDataDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                xmlData.Load(@"FM/Server.xml");
                XmlNode xn = xmlData.SelectSingleNode("Servers");
                XmlNodeList xnl = xn.ChildNodes;
                foreach (TreeNode n in treeView1.Nodes)
                {
                    foreach (XmlNode xnls in xnl)
                    {
                    XmlElement xe = (XmlElement)xnls;
                    XmlNodeList xnl1 = xe.ChildNodes;
                    if (n.Tag.ToString()==xe.GetAttribute("id"))
                    {
                            if (xe.GetAttribute("id") == "1")
                            {
                                this.label4.Text = xnl1.Item(0).InnerText;
                                this.label5.Text = xnl1.Item(4).InnerText;
                                this.label8.Text = xnl1.Item(1).InnerText;
                                this.label9.Text = xnl1.Item(2).InnerText;
                                this.label7.Text = xe.GetAttribute("id");
                                if (xnl1.Item(5).InnerText.Equals("0"))
                                {
                                    this.label6.Text = "未连接";
                                }
                                else if (xnl1.Item(5).InnerText.Equals("1"))
                                {
                                    this.label6.Text = "已连接";

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            Logger.Info("加载配置文件----"+DateTime.Now.ToString());
        }
        int  tags = 1;
        public void AddNodes()
        {
            try
            {
                XmlDataDocument xmlData = new XmlDataDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                xmlData.Load(@"FM/Server.xml");
                XmlNode xn = xmlData.SelectSingleNode("Servers");
                XmlNodeList xnl = xn.ChildNodes;
                foreach (XmlNode xnls in xnl)
                {
                    XmlElement xe = (XmlElement)xnls;
                    XmlNodeList xnl1 = xe.ChildNodes;
                    TreeNode Addnoede = new TreeNode();
                    if (FinAllTree.ContainsKey(xe.GetAttribute("id"))|| FinAllTree.ContainsValue(xnl1.Item(6).InnerText))
                    {
                            MessageBox.Show("配置文件具有相同的id或快捷键，请先修改！");
                            Process[] processes = Process.GetProcesses();
                            foreach (Process p in processes)
                            {
                                if (p.ProcessName == "EH")
                                {
                                    p.Kill();
                                }
                            }
                            return;
                    }
                    else
                    {
                        Addnoede.Text = xnl1.Item(4).InnerText;
                        Addnoede.Tag = tags;
                        treeView1.Nodes.Add(Addnoede);
                        tags++;
                        FinAllTree.Add(xe.GetAttribute("id"), xnl1.Item(6).InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        //获得xml文件的值
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                String tage = e.Node.Tag.ToString();
                XmlDataDocument xmlData = new XmlDataDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                xmlData.Load(@"FM/Server.xml");
                XmlNode xn = xmlData.SelectSingleNode("Servers");

                XmlNodeList xnl = xn.ChildNodes;
                foreach (XmlNode xnls in xnl)
                {
                    XmlElement xe = (XmlElement)xnls;
                    XmlNodeList xnl1 = xe.ChildNodes;
                    if (e.Node.Tag.ToString()==xe.GetAttribute("id"))
                    {
                        this.label4.Text = xnl1.Item(0).InnerText;
                        this.label5.Text = xnl1.Item(4).InnerText;
                        this.label8.Text = xnl1.Item(1).InnerText;
                        this.label9.Text = xnl1.Item(2).InnerText;
                        this.label7.Text = xe.GetAttribute("id");
                        //判断Form2是否启动
                        if (connect() == 1)
                        {
                            foreach (Form form in EF.EF_Args.ShellForm.MdiChildren)
                            {

                                if (form.Name == "FormFMJ602")
                                {
                                    if ((form as FormFMJ602).ServerConnet(this.label8.Text) == 1)
                                    {
                                        if (xe.GetAttribute("id") == this.treeView1.SelectedNode.Tag.ToString())
                                        {
                                            xnl1.Item(5).InnerText = "1";
                                            xmlData.Save(@"FM/Server.xml");
                                            Logger.Info("服务状态成功----" + DateTime.Now.ToString());
                                        }
                                    }
                                    else
                                    {
                                        xnl1.Item(5).InnerText = "0";
                                        xmlData.Save(@"FM/Server.xml");
                                        Logger.Info("服务状态失败----" + DateTime.Now.ToString());
                                    }
                                }
                            }

                        }
                        else
                        {
                            xnl1.Item(5).InnerText = "0";
                            xmlData.Save(@"FM/Server.xml");
                            Logger.Info("窗体关闭，服务连接失败----" + DateTime.Now.ToString());
                        }
                        if (xnl1.Item(5).InnerText.Equals("0"))
                        {
                            this.label6.Text = "未连接";
                            this.button2.BackColor = Color.DodgerBlue;
                            Logger.Info("服务连接失败----" + DateTime.Now.ToString());
                        }
                        else if (xnl1.Item(5).InnerText.Equals("1"))
                        {
                            this.label6.Text = "已连接";
                            this.button2.BackColor = Color.Silver;
                            Logger.Info("服务连接成功----"+DateTime.Now.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        //判断除主窗体以外其他窗体是否开启
        public int connect()
        {
            try
            {
                System.Collections.IDictionaryEnumerator ef_form_enumerator = EF.EF_Args.ht_ef_form.GetEnumerator();
                while (ef_form_enumerator.MoveNext())
                {
                    var form_ename_ht = ef_form_enumerator.Key.ToString().Split(':')[0];
                    //画面已打开
                    if (form_ename_ht == "FMJ602")
                    {
                        Logger.Info("F2窗体程序启动----" + DateTime.Now.ToString());
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("F2窗体程序启动失败----" + DateTime.Now.ToString());
                return 0;
            } 
                    return 0;
        }
        //连接服务器
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.label7.Text == null || this.label7.Text == "")
                {
                    MessageBox.Show("请先选择要连接的服务！");
                }
                else
                {
                    if (this.radioButton1.Checked)
                    {
                        //判断Form2是否启动
                        if (connect() == 1)
                        {
                            foreach (Form forms in EF.EF_Args.ShellForm.MdiChildren)
                            {
                                if (forms.Name == "FormFMJ602")
                                {
                                    if ((forms as FormFMJ602).ServerConnet(this.label8.Text) == 0)
                                    {
                                        this.label6.Text = "已连接";
                                        this.button2.BackColor = Color.Silver;
                                        (forms as FormFMJ602).button4_Click(this.label8.Text, this.label4.Text, this.label9.Text, this.label5.Text);
                                        (forms as FormFMJ602).Activate();
                                        Logger.Info("连接服务----" + DateTime.Now.ToString());
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("该服务已启动！");
                                        Logger.Info("服务已启动----" + DateTime.Now.ToString());
                                    }
                                }
                                
                            }
                        }
                        else
                        {
                            this.EFCallForm("FMJ602", new object[] { this.label8.Text, this.label4.Text, this.label9.Text, this.label5.Text });
                            foreach (Form form in EF.EF_Args.ShellForm.MdiChildren)
                            {
                                if (form.Name == "FormFMJ602")
                                {
                                    if ((form as FormFMJ602).ServerConnet(this.label8.Text) == 1)
                                    {
                                        this.label6.Text = "已连接";
                                        this.button2.BackColor = Color.Silver;
                                        Logger.Info("服务连接成功----" + DateTime.Now.ToString());
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (this.radioButton2.Checked)
                    {
                        Process[] app = Process.GetProcessesByName("RMS_RDPClient");
                        if (app.Length > 0)
                        {
                            MessageBox.Show("请关闭已经启动的程序后再进行尝试");
                            return;
                        }
                        else
                        {
                            String server = this.label8.Text;
                            VNCcliennot("10.25.25.111");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            
        }
        //vnc模式连接服务
        public void VNCcliennot(String ipAdders)
        {
            try
            {
                string path = Path.GetPathRoot(Directory.GetCurrentDirectory());
                Process process = new Process();
                string fileName = string.Format(path + "/Remote_connectionDemo/server/RMS_RDPClient.exe", System.Environment.CurrentDirectory);
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(fileName, string.Format("{0} {1} {2} {3}", ipAdders, 12005, false, ""));
                process.StartInfo = myProcessStartInfo;
                process.EnableRaisingEvents = true;
                process.Start();
                Logger.Info("RMS_RDPClient程序启动----"+DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

        }

        //修改快捷键
        private void 修改快捷键ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.IDictionaryEnumerator ef_form_enumerator = EF.EF_Args.ht_ef_form.GetEnumerator();
                while (ef_form_enumerator.MoveNext())
                {
                    var form_ename_ht = ef_form_enumerator.Key.ToString().Split(':')[0];
                    if (form_ename_ht == "FromFMJ603")
                    {
                        MessageBox.Show("请先关闭正在修改的快捷键");
                        Logger.Info("重复打开修改快捷键页面"+DateTime.Now.ToString());
                        break;
                    }
                    else
                    {
                        this.EFCallForm("FMJ603", new object[] { this.label5.Text });
                        Logger.Info("修改启动快捷键"+DateTime.Now.ToString());
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            
        }
        
        public void denglu(String server, String userName, String passWord, String serverName)
        {
            try
            {
                if (this.label7.Text == null || this.label7.Text == "")
                {
                    MessageBox.Show("请先选择要连接的服务！");
                }
                else
                {
                    //判断Form2是否启动
                    System.Collections.IDictionaryEnumerator ef_form_enumerator = EF.EF_Args.ht_ef_form.GetEnumerator();
                    while (ef_form_enumerator.MoveNext())
                    {
                        var form_ename_ht = ef_form_enumerator.Key.ToString().Split(':')[0];
                        if (form_ename_ht == "FMJ602")
                        {
                            foreach (Form forms in EF.EF_Args.ShellForm.MdiChildren)
                            {
                                if (forms.Name == "FormFMJ602")
                                {
                                    if ((forms as FormFMJ602).ServerConnet(this.label8.Text) == 0)
                                    {
                                        this.label6.Text = "已连接";
                                        this.button2.BackColor = Color.Silver;
                                        (forms as FormFMJ602).button4_Click(this.label8.Text, this.label4.Text, this.label9.Text, this.label5.Text);
                                        (forms as FormFMJ602).Activate();
                                        Logger.Info("快捷键连接服务----"+DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        MessageBox.Show("该服务已启动！");
                                        Logger.Info("快捷键连接服务已启动----" + DateTime.Now.ToString());
                                    }
                                }
                            }
                            break;
                        }
                        else
                        {
                            this.EFCallForm("FMJ602", new object[] { server, userName, passWord, serverName });
                            foreach (Form form in EF.EF_Args.ShellForm.MdiChildren)
                            {
                                if (form.Name == "FormFMJ602")
                                {
                                    if ((form as FormFMJ602).ServerConnet(this.label8.Text) == 1)
                                    {
                                        this.label6.Text = "已连接";
                                        this.button2.BackColor = Color.Silver;
                                        Logger.Info("快捷键第一次连接服务----" + DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        MessageBox.Show("该服务已启动！");
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        //快捷键连接服务

        private void FormL2RM_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                XmlDataDocument xmlData = new XmlDataDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                xmlData.Load(@"FM/Server.xml");
                XmlNode xn = xmlData.SelectSingleNode("Servers");

                XmlNodeList xnl = xn.ChildNodes;
                foreach (TreeNode n in treeView1.Nodes)
                {
                    foreach (XmlNode xnls in xnl)
                    {
                        XmlElement xe = (XmlElement)xnls;
                        XmlNodeList xnl1 = xe.ChildNodes;
                        if (n.Tag.ToString()==xe.GetAttribute("id"))
                        {
                            if (e.KeyData.ToString() == xnl1.Item(6).InnerText)
                            {
                                denglu(xnl1.Item(1).InnerText, xnl1.Item(0).InnerText, xnl1.Item(2).InnerText, xnl1.Item(4).InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        private void FormL2RM_FormClosing(object sender, FormClosingEventArgs e)
        {       

            //if (DialogResult.OK == MessageBox.Show("是否删除", "Warning",MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
            //{

            //    try
            //    {
            //        Process[] processes = Process.GetProcesses();
            //        foreach (Process p in processes)
            //        {
            //            if (p.ProcessName == "RMS_RDPClient")
            //            {
            //                p.Kill();
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    //关闭本窗体

            //    this.Dispose();

            //    //退出程序

            //    Application.Exit();
            //}
            //else
            //{

            //}



        }
        //控件居中
        private void FormL2RM_Resize(object sender, EventArgs e)
        {
            try
            {
                button2.Left = (this.Width - button2.Width) / 2;
                groupBox1.Left = (this.Width - groupBox1.Width) / 2;
                panel1.Left = (this.Width - panel1.Width) / 2;
                pictureBox1.Left = (this.Width - pictureBox1.Width) / 2;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
