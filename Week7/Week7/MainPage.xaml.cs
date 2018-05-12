using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Week7
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void GetWeather(string tel)
        {
            try
            {
                // 创建一个HTTP client实例对象
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 
                /*
                默认情况下，HttpClient对象不会将用户代理标头随 HTTP 请求一起发送到 Web 服务。
                某些 HTTP 服务器（包括某些 Microsoft Web 服务器）要求从客户端发送的 HTTP 请求附带用户代理标头。
                如果标头不存在，则 HTTP 服务器返回错误。
                在 Windows.Web.Http.Headers 命名空间中使用类时，需要添加用户代理标头。
                我们将该标头添加到 HttpClient.DefaultRequestHeaders 属性以避免这些错误。
                */
                var headers = httpClient.DefaultRequestHeaders;

                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }


                string city = cityName.Text;
                string getWeatherCode = "https://www.sojson.com/open/api/weather/json.shtml?city="+city;

                //发送GET请求
                HttpResponseMessage response1 = await httpClient.GetAsync(getWeatherCode);

                response1.EnsureSuccessStatusCode();

                Byte[] getByte1 = await response1.Content.ReadAsByteArrayAsync();
                Encoding code1 = Encoding.GetEncoding("UTF-8");
                string result1 = code1.GetString(getByte1, 0, getByte1.Length);
                JObject jo1 = (JObject)JsonConvert.DeserializeObject(result1);
                  string date = jo1["date"].ToString();
                string status= jo1["status"].ToString();
                if (status != "200") infor.Text = "error";
                string data = jo1["data"].ToString();
                string type = jo1["data"]["forecast"][0]["type"].ToString();
                weather.Text = type;
                string high= jo1["data"]["forecast"][0]["high"].ToString();
                string low = jo1["data"]["forecast"][0]["low"].ToString();
                temperature.Text = high;
                temperature.Text += "到";
                temperature.Text += low;


               

            }
            catch (HttpRequestException ex1)
            {
                infor.Text = "错误输入";
            }
            catch (Exception ex2)
            {
                infor.Text = ex2.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            weather.Text = "";
            temperature.Text = "";
            GetWeather(cityName.Text);
        }
        private void Nav(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(XmlSearch));
        }
    }

}
