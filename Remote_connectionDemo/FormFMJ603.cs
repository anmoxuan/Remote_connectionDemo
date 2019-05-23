using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using EF;
using EI;

namespace FM
{
    public partial class FormFMJ603 : EFFormMain
    {
        private String server;

        public string Server { get => server; set => server = value; }

        public FormFMJ603()
        {
            InitializeComponent();
            Logger.Info("修改快捷键启动----"+DateTime.Now.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int ii = fallone(comboBox1.SelectedItem.ToString());
                if (ii == 0)
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
                        if (this.textBox1.Text == xnl1.Item(4).InnerText)
                        {
                            xnl1.Item(6).InnerText = comboBox1.SelectedItem.ToString();
                            xmlData.Save(@"FM/Server.xml");
                            MessageBox.Show(this.textBox1.Text + "已修改为：" + this.comboBox1.SelectedItem.ToString());
                            this.Close();
                            Logger.Info("修改快捷键启动成功----"+DateTime.Now.ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("此键已被使用！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            } 
        }
        public int fallone(String key)
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
                    if (key.Equals(xnl1.Item(6).InnerText))
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
            }
            return 0;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.SelectedIndex = 0;
                this.textBox1.Text = server;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void FormL2RMF_EF_START_FORM_BY_EF(object sender, EF_Args i_args)
        {
            try
            {
                String[] name = i_args.callParams;
                this.Server = name[0];
                Logger.Info("获取需要的数据----"+DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void FMCCJ603_Resize(object sender, EventArgs e)
        {
            try
            {
                button1.Left = (this.Width - button1.Width) / 2;
                textBox1.Left = (this.Width - textBox1.Width) / 2;
                label1.Left = (this.Width - label1.Width) / 2;
                label2.Left = (this.Width - label2.Width) / 2;
                comboBox1.Left = (this.Width - comboBox1.Width) /2;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
