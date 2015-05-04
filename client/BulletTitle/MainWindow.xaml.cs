using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;
using System.Xml;
using System.Net;

namespace BulletTitle
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Queue<BulletItem> bulletQ = new Queue<BulletItem>();
        Queue<TextBlock> boxQ = new Queue<TextBlock>();
        Random randMan = new Random();
        delegate void PubDelegate(BulletItem val);
        delegate void CleanDelegate();
        public MainWindow()
        {
            InitializeComponent();
            this.Left = 0;
            this.Top = 0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            Thread receiver = new Thread(bulletReceiver);
            receiver.Start();
            Thread puber = new Thread(bulletPuber);
            puber.Start();
            Thread cleaner = new Thread(bulletCleaner);
            cleaner.Start();
            
        }

        public void bulletReceiver()
        {
            while (true)
            {
                HttpWebRequest getListRequest = (HttpWebRequest)WebRequest.Create("http://192.168.182.138:8000/manage/getlist/");
                string requestStr = "";
                byte[] requestBytes = Encoding.UTF8.GetBytes(requestStr);

                getListRequest.Method = "POST";
                getListRequest.ContentType = "application/x-www-form-urlencoded";
                getListRequest.ContentLength = Encoding.GetEncoding("utf-8").GetByteCount(requestStr);
                
                try
                {
                    Stream getListRequestStream = getListRequest.GetRequestStream();
                    getListRequestStream.Write(requestBytes, 0, requestBytes.Length);
                    getListRequestStream.Close();
                }
                catch (System.Exception)
                {
                    errorlog(DateTime.Now.ToString() + "  Error in Network Connection \r\n");
                    continue;
                }

                string listStr = "";
                try
                {
                    HttpWebResponse getListResponse = (HttpWebResponse)getListRequest.GetResponse();
                    Stream getListResponseStream = getListResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(getListResponseStream, Encoding.GetEncoding("utf-8"));
                    listStr = reader.ReadToEnd();
                }
                catch(System.Exception)
                {
                    errorlog(DateTime.Now.ToString() + "  Error in getting Response from server \r\n");
                    continue;
                }
                try
                {
                    XmlDocument listDoc = new XmlDocument();
                    listDoc.LoadXml(listStr);
                    XmlNodeList total_row = listDoc.GetElementsByTagName("total_row");
                    if (int.Parse(total_row[0].InnerText) > 0)
                    {
                        XmlNodeList listNodeList = listDoc.GetElementsByTagName("row");
                        for (int i = 0; i < listNodeList.Count; ++i)
                        {
                            XmlNode bullet_text = listNodeList[i].SelectSingleNode("text");
                            XmlNode bullet_time = listNodeList[i].SelectSingleNode("time");
                            XmlNode bullet_pos = listNodeList[i].SelectSingleNode("pos");
                            BulletItem bullet_item = new BulletItem(bullet_text.InnerText, 0, int.Parse(bullet_pos.InnerText));
                            bulletQ.Enqueue(bullet_item);

                        }
                    }
                    else
                    {
                        errorlog(DateTime.Now.ToString() + "  No bullets got from server \r\n");
                    }
                    
                }
                catch (System.Exception)
                {
                    errorlog(DateTime.Now.ToString() + "  Error in parsing XML packet from server \r\n");
                    continue;
                }
                
                Thread.Sleep(10000);
            }
        }

        public void bulletPuber()
        {
            PubDelegate pub = new PubDelegate(setTextBlock);
            while (true)
            {

                if (bulletQ.Count > 0)
                {
                    BulletItem bullet_to_pub = bulletQ.Dequeue();
                    //string bullet_text = bullet_to_pub.bullet_text;
                    this.Dispatcher.Invoke(pub, bullet_to_pub);
                }
                Thread.Sleep(1000);
               
            }
        }

        public void bulletCleaner()
        {
            CleanDelegate clean = new CleanDelegate(cleanTextBlock);
            while (true)
            {
                this.Dispatcher.Invoke(clean);
                Thread.Sleep(500);
            }
            
        }

        public void setTextBlock(BulletItem val)
        {
            TextBlock bullet_text_box = new TextBlock();
            bullet_text_box.Text = val.bullet_text;
            //bullet_text_box.Width = val.bullet_text.Length * 33;
            bullet_text_box.Height = 50;
            bullet_text_box.FontSize = 33;
            bullet_text_box.Foreground = BulletItem.getColor(randMan.Next(1, 1000));
            bullet_text_box.Margin = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth, -System.Windows.SystemParameters.PrimaryScreenHeight+150+randMan.Next(1,9)*50, 0, 0);
            //bullet_text_box.Margin = new Thickness(0, -System.Windows.SystemParameters.PrimaryScreenHeight + 150 + randMan.Next(1, 9) * 50, 0, 0);
            gridRoot.Children.Add(bullet_text_box);
            BulletItem.runBullet(bullet_text_box);
            boxQ.Enqueue(bullet_text_box);
        }

        public void cleanTextBlock()
        {
            if (boxQ.Count > 0)
            {
                TextBlock obj_to_delete = boxQ.Peek();
                if (obj_to_delete.Margin.Left < -System.Windows.SystemParameters.PrimaryScreenWidth / 2)
                {
                    gridRoot.Children.Remove(obj_to_delete);
                    boxQ.Dequeue();
                }
            }
            
        }

        public void errorlog(string text)
        {
            FileStream fs = new FileStream("error.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8"));
            sw.Write(text);
            sw.Close();
            //fs.Close();
        }
    }
}
