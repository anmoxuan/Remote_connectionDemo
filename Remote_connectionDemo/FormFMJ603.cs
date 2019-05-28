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
using WindowsFormsControlLibrary2;

namespace FM
{
    public partial class FormFMJ603 : EFFormMain
    {
        private String server;
        private WindowsFormsControlLibrary2.UserControl1 userControl;
        public string Server { get => server; set => server = value; }
        private Dictionary<String, UserControl1> userControls = new Dictionary<string, UserControl1>();//userControl对象
        private Dictionary<String,String> ListKey = new Dictionary<string,String>();
   
        public FormFMJ603()
        {
            InitializeComponent();
            Logger.Info("修改快捷键启动----"+DateTime.Now.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            ListKey.Clear();
            foreach (var ietm in userControls)
            {
                if (ListKey.ContainsValue(ietm.Value.ComText))
                {
                    MessageBox.Show(ietm.Value.ComText+"已存在！");
                    return;
                }
                else
                {
                    ListKey.Add(ietm.Value.LabelText, ietm.Value.ComText);
                }
            }
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
                    foreach (var item in ListKey)
                    {
                        if (item.Key == xnl1.Item(4).InnerText)
                        {
                            xnl1.Item(6).InnerText = item.Value.ToString();
                            xmlData.Save(@"FM/Server.xml");
                            
                            Logger.Info("修改快捷键启动成功----" + DateTime.Now.ToString());
                        }
                    }

                }
                MessageBox.Show("修改成功！");
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            } 
        }
        //窗体加载
        private void Form3_Load(object sender, EventArgs e)
        {
            int X = 90;
            int Y = 61;
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
                    AdduUserControl(X, Y);
                    X = X + 200;
                    XmlElement xe = (XmlElement)xnls;
                    XmlNodeList xnl1 = xe.ChildNodes;
                    TreeNode Addnoede = new TreeNode();
                    userControl.LabelText = xnl1.Item(4).InnerText;
                    if (xnl1.Item(4).InnerText==userControl.LabelText)
                    {
                        userControl.SelectedItem(xnl1.Item(6).InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            
        }
        //创建自定义控件并设置属性
        int index = 1;
        public void AdduUserControl(int X,int Y)
        {
            userControl = new WindowsFormsControlLibrary2.UserControl1
            {
                Width = 175,
                Height = 230
            };
            userControl.Name = "userControl"+index;
            panel1.Controls.Add(userControl);
            userControl.Location = new Point(X,Y);
            //把UserControl1对象添加到集合中
            userControls.Add(userControl.Name,userControl);
            index++;
        }
        //让控件居中
        private void FMCCJ603_Resize(object sender, EventArgs e)
        {
            try
            {
                button1.Left = (this.Width - button1.Width) / 2;
                panel1.Left = (this.Width - panel1.Width) / 2;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
