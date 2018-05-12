﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Week7
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class XmlSearch : Page
    {
        public XmlSearch()
        {
            this.InitializeComponent();
        }
       

        private void MyButton2_Click(object sender, RoutedEventArgs e)
        {
            queryAsync(MyTextBox2.Text);
        }

        async void queryAsync(String str)
        {
            String url = String.Format("http://v.juhe.cn/weather/index?format=2&cityname={0}&dtype=xml&key=938683d0ba3d21e4eeed3f935f86f2f4", str);
            HttpClient client = new HttpClient();
            String result = await client.GetStringAsync(url);
            XmlDocument document = new XmlDocument();
            document.LoadXml(result);
            XmlNodeList list = document.GetElementsByTagName("resultcode");
            IXmlNode node = list.Item(0);
            String judge = node.InnerText;
            if (judge != "202")
            {
                list = document.GetElementsByTagName("temp");
                node = list.Item(0);
                String info = "温度： " + node.InnerText + "\n";
                list = document.GetElementsByTagName("humidity");
                node = list.Item(0);
                info += "湿度：" + node.InnerText + "\n";
                list = document.GetElementsByTagName("wind_direction");
                node = list.Item(0);
                info += "风向：" + node.InnerText + "\n";
                MyTextBlock2.Text = info;
            }
            else
            {
                MyTextBlock2.Text = "Wrong Input!\n";
            }
        }
    }
}
