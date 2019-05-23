using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EF;
using EI;

namespace FM
{
    public partial class FormFMJ602 : EFFormMain
    {
        public String Server = null;
        public String Username = null;
        public String Password = null;
        public String serverName = null;
        public Dictionary<String,serverEntity> dictionary = new Dictionary<string, serverEntity>();
        public serverEntity serverentity = null;
        public static Dictionary<string, AxMSTSCLib.AxMsRdpClient4NotSafeForScripting> axMsRdpClientList = new Dictionary<string, AxMSTSCLib.AxMsRdpClient4NotSafeForScripting>();
        public FormFMJ602()
        {
            InitializeComponent();
        }
        //全屏模式
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control cle in this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls)
                {
                    if (cle is AxMSTSCLib.AxMsRdpClient4NotSafeForScripting)//挑选出是按钮类型的
                    {
                        foreach (var item in axMsRdpClientList)
                        {
                            if (cle.Name == item.Key)
                            {
                                if (item.Value.Connected != 0)
                                {
                                    uint width2 = (uint)Convert.ToUInt64(Screen.PrimaryScreen.Bounds.Width);
                                    uint heigth2 = (uint)Convert.ToUInt64(Screen.PrimaryScreen.Bounds.Height);
                                    //item.Value.Reconnect(width2, heigth2);
                                    item.Value.FullScreen = true;
                                }
                                else
                                {
                                    MessageBox.Show("服务已断开，请重新连接");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        //窗体第一次打开连接服务器

        Rectangle ScreenArea = Screen.PrimaryScreen.Bounds;
        private void Form2_Load_1(object sender, EventArgs e)
        {
            try
            {
                tabControl1.TabPages[0].Text = serverName;
                this.comboBox1.Text = this.comboBox1.Items[0].ToString();
                connect(Server, Username, Password);
                serverentity = new serverEntity();
                serverentity.Server = Server;
                serverentity.Username = Username;
                serverentity.Password = Password;
                dictionary.Add(this.tabControl1.SelectedTab.Tag.ToString(), serverentity);
                //this.timer1.Start();
                Logger.Info("服务启动加载成功----"+DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            
        }
        private int index = 1;
        private AxMSTSCLib.AxMsRdpClient4NotSafeForScripting axMsRdpClient4NotSafeForScripting = null;
        //添加tagpage页面并添加AxMsRdpClient9NotSafeForScripting组件
        public void button4_Click(String server, String userName, String passWord,String serverName)
        {
            try
            {
                int i1 = 1;
                TabPage Page = new TabPage();
                Page.Text = serverName;
                Page.Tag = index + i1;
                Page.UseVisualStyleBackColor = true;
                this.tabControl1.Controls.Add(Page);
                this.tabControl1.SelectedTab = Page;

                connect(server, userName, passWord);
                serverEntity serverentity = new serverEntity();
                serverentity.Server = server;
                serverentity.Username = userName;
                serverentity.Password = passWord;
                dictionary.Add(tabControl1.SelectedTab.Tag.ToString(), serverentity);
                index++;
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return;
            }
        }
        //使用AxMsRdpClient9NotSafeForScripting控件连接服务器
        int count = 1;
        private void connect(String server,String userName,String passWord)
        {
            try
            {
                axMsRdpClient4NotSafeForScripting = new AxMSTSCLib.AxMsRdpClient4NotSafeForScripting();
                axMsRdpClient4NotSafeForScripting.Name = "axMsRdpClient9NotSafeForScripting"+count;
                this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls.Add(axMsRdpClient4NotSafeForScripting);
                axMsRdpClient4NotSafeForScripting.Size = new Size(800, 500);
                axMsRdpClient4NotSafeForScripting.Dock = DockStyle.Fill;
                axMsRdpClient4NotSafeForScripting.DesktopWidth = ScreenArea.Width;
                axMsRdpClient4NotSafeForScripting.DesktopHeight = ScreenArea.Height;
                axMsRdpClient4NotSafeForScripting.Server = server;
                axMsRdpClient4NotSafeForScripting.UserName = userName;
                axMsRdpClient4NotSafeForScripting.AdvancedSettings2.ClearTextPassword = passWord;
                axMsRdpClient4NotSafeForScripting.Connect();
                count++;
                Logger.Info("服务启动成功----"+DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            try
            {
                axMsRdpClientList.Add(axMsRdpClient4NotSafeForScripting.Name, axMsRdpClient4NotSafeForScripting);
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        //重新连接服务器
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control cle in this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls)
                {
                    if (cle is AxMSTSCLib.AxMsRdpClient4NotSafeForScripting)//挑选出是按钮类型的
                    {
                        foreach (var item in axMsRdpClientList)
                        {
                            if (cle.Name == item.Key)
                            {
                                try
                                {
                                    if (item.Value.Connected == 0)
                                    {
                                        this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls.Remove(item.Value);
                                        foreach (var itmes in dictionary)
                                        {
                                            if (tabControl1.SelectedTab.Tag.ToString().Equals(itmes.Key))
                                            {
                                                connect(itmes.Value.Server, itmes.Value.Username, itmes.Value.Password);
                                                this.timer1.Start();
                                                Logger.Info("重连服务成功----"+DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    item.Value.Dispose();
                                    Logger.Error(ex.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return;
            }

        }
        //关闭连接
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex > -1)
                {
                    foreach (Control cle in this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls)
                    {
                        if (cle is AxMSTSCLib.AxMsRdpClient4NotSafeForScripting)//挑选出是按钮类型的
                        {
                            foreach (var item in axMsRdpClientList)
                            {
                                if (cle.Name == item.Key)
                                {
                                    try
                                    {
                                        if (item.Value.Connected == 0)
                                        {
                                            MessageBox.Show("服务已断开，请重新连接");
                                        }
                                        else
                                        {
                                            item.Value.Disconnect();
                                            item.Value.Dispose();
                                            this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls.Remove(item.Value);
                                            tabControl1.TabPages.RemoveAt(this.tabControl1.SelectedIndex);
                                            if (this.tabControl1.SelectedIndex < 0)
                                            {
                                                this.Close();
                                                Logger.Info("关闭服务连接----"+DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                return;
            }
        }
        //判断server是否启动
        public int ServerConnet(String server)
        {
            try
            {
                foreach (var item in axMsRdpClientList)
                {
                    if (item.Value != null)
                    {
                        if (server.Equals(item.Value.Server.ToString()))
                        {
                            Logger.Info("检查服务连接成功----"+DateTime.Now.ToString());
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return 0;
            }
            return 0;
        }
        //分辨率模式
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control cle in this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls)
            {
                if (cle is AxMSTSCLib.AxMsRdpClient4NotSafeForScripting)
                {
                    foreach (var items in axMsRdpClientList)
                    {
                        if (items.Key == cle.Name)
                        {
                            if (items.Value.Connected == 1)
                            {
                                if (this.comboBox1.SelectedIndex < 0)
                                {
                                    MessageBox.Show("请先选择分辨率");
                                }
                                else
                                {
                                    foreach (var ss in tabControl1.TabPages)
                                    {
                                        if (this.tabControl1.SelectedTab == ss)
                                        {
                                            String names = this.comboBox1.SelectedItem.ToString();
                                            String[] ss1 = names.Split('*');
                                            uint width1 = uint.Parse(ss1[0]);
                                            uint height1 = uint.Parse(ss1[1]);
                                            //items.Value.Reconnect(width1, height1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show("连接已断开，请从新连接！", "", MessageBoxButtons.OK);
                                if (dr == DialogResult.OK)
                                {
                                    if (cle.Name == items.Key) 
                                    {
                                        this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls.Remove(items.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //每隔五秒检测一次服务是否断开连接
        public void timer()
        {
            try
            {
                foreach (Control cle in this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls)
                {
                    if (cle is AxMSTSCLib.AxMsRdpClient4NotSafeForScripting)
                    {
                        foreach (var items in axMsRdpClientList)
                        {
                            if (items.Key == cle.Name)
                            {
                                if (items.Value.Connected == 0)
                                {
                                    foreach (var ss in tabControl1.TabPages)
                                    {
                                        if (this.tabControl1.SelectedTab == ss)
                                        {
                                            MessageBox.Show(ss.ToString() + "服务已断开");
                                            button4.Enabled = true;
                                            this.timer1.Stop();
                                            Logger.Info("服务断开连接----"+DateTime.Now.ToString());
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    this.button4.Enabled = false;
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
           
        }
       //定时器
        private void timer1_Tick(object sender,EventArgs e)
        {
            timer();
            Logger.Info("定时检查服务----"+DateTime.Now.ToString());
        }
        //EF框架传值
        private void FormL2RC_EF_START_FORM_BY_EF(object sender, EF_Args i_args)
        {
            try
            {
                String[] names = i_args.callParams;
                this.Server = names[0];
                this.Username = names[1];
                this.Password = names[2];
                this.serverName = names[3];
                Logger.Info("获取需要的参数----"+DateTime.Now.ToString());
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

        }
        //清空axMsRdpClientList集合的数据
        private void FormL2RC_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                axMsRdpClientList.Clear();
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

        }

        private void 全屏模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.button2_Click(sender, e);
        }
    }
}

