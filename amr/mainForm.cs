using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace amr
{

    public partial class mainForm : Form
    {

        public TextBox tbNames;
        public TextBox tbFamilys;
        public string[] asAlphabet;
        public string[] asDomains;
        public Proxy pProxy;
        public string UserAgent;
        //public string KapchaKey;
        public TextBox tbSettings;

        public mainForm()
        {
            
            InitializeComponent();

            StreamReader file;

            tbNames = new TextBox();
            file = new StreamReader(@"names_f.txt", System.Text.Encoding.Default, true);
            tbNames.Text = file.ReadToEnd();
            tbFamilys = new TextBox();
            file = new System.IO.StreamReader(@"familys_f.txt", System.Text.Encoding.Default, true);
            tbFamilys.Text = file.ReadToEnd();
            asAlphabet = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", 
                                        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", 
                                        "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", 
                                        "u", "v", "w", "x", "y", "z", 
                                        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
                                        "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", 
                                        "U", "V", "W", "X", "Y", "Z",
                                        "_", "-", ".", "" };

            UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; InfoPath.1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; .NET CLR 3.0.04506.30; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";

            tbSettings = new TextBox();
            file = new StreamReader(@"settings.txt", System.Text.Encoding.Default, true);
            //KapchaKey = file.ReadLine();
            tbSettings.Text = file.ReadToEnd();

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            
            int checkedDomains = checkedListBoxDomains.CheckedItems.Count;

            if (checkedDomains == 0) { return; }

            asDomains = new string[checkedDomains];
            for (int i = 0; i < checkedDomains; i++)
            {
                asDomains[i] = checkedListBoxDomains.CheckedItems[i].ToString();
            }

            if (checkBoxUseProxy.Checked) pProxy = new Proxy();

            int q = 1;
            int aq = 1;
            //for (int nt = 1; nt <= numericUpDownThreads.Value; nt++)
            //{

            //    Thread t = new Thread(delegate() 
            //        { 
                        for (; q <= numericUpDownQuantity.Value; q++, aq++)
                        {
                            capchaForm CF; 
                            CF = new capchaForm(this, aq.ToString()); 
                            if (CF.Result != DialogResult.OK) { q--; }
                            labelQuantity.Text = q.ToString();
                            if (checkBoxUseProxy.Checked) pProxy.Next();
                        }
            //        });
            //    t.Start();

            //}
        }

    }

    public class Proxy
    {

        public TextBox tbList;
        public string IP;
        public string Port;
        public string Login;
        public string Password;
        public string Domain;

        public Proxy()
        {

            StreamReader file;
            file = new System.IO.StreamReader(@"proxy.txt", System.Text.Encoding.Default, true);
            tbList.Text = file.ReadToEnd();

            Next();

        }

        public void Next()
        {
            
            Random randObj = new Random();

            string[] arr = tbList.Lines[randObj.Next(0, tbList.Lines.Count() - 1)].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length >= 1) IP = arr[0]; else IP = "";
            if (arr.Length >= 2) Port = arr[1]; else Port = "";
            if (arr.Length >= 3) Login = arr[2]; else Login = "";
            if (arr.Length >= 4) Password = arr[3]; else Password = "";
            if (arr.Length >= 5) Domain = arr[4]; else Domain = "";

        }

    }

    public class capchaForm
    {
        string sWebResponse;
        string sCookies;
        CapchImage ciCapcha;
        public CapchText ctCapcha;
        public DialogResult Result;

        public capchaForm(mainForm MF, string s)
        {

            StreamWriter file;

            // открываем страницу регистрации 
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://win.mail.ru/cgi-bin/signup");
            if (MF.checkBoxUseProxy.Checked)
            {
                myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
            }
            // тут, вроде не надо
            //myHttpWebRequest.Referer = "http://mail.ru";
            myHttpWebRequest.UserAgent = MF.UserAgent;
            //myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.Headers.Add("Accept-Language", "ru");

            try
            {
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                sCookies = "";
                if (!String.IsNullOrEmpty(myHttpWebResponse.Headers["Set-Cookie"]))
                {
                    sCookies = myHttpWebResponse.Headers["Set-Cookie"];
                }

                StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
                sWebResponse = myStreamReader.ReadToEnd();

                myHttpWebResponse.Close();

                ciCapcha = new CapchImage(sWebResponse, s);

                ctCapcha = new CapchText(MF, s);

                sendData(MF, s);

            }
            catch
            {

                Result = DialogResult.Cancel;
                file = new StreamWriter(@"error_log.txt", true);
                file.WriteLine(s + ": catch1");
                file.Close();

            }

        }

        void sendData(mainForm MF, string s)
        {

            StreamWriter file;

            Match m;
            string HRefPattern;

            Query qQuery = new Query(MF, sCookies, sWebResponse, this);

            while (true)
            {

                // теперь заполним форму и отправим ее
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://win.mail.ru/cgi-bin/reg");
                if (MF.checkBoxUseProxy.Checked)
                {
                    myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                    myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
                }
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Referer = "http://win.mail.ru/cgi-bin/signup";
                myHttpWebRequest.UserAgent = MF.UserAgent;
                myHttpWebRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, */*";
                myHttpWebRequest.Headers.Add("Accept-Language", "ru");
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                // передаем куки, полученные в предыдущем запросе
                if (!String.IsNullOrEmpty(sCookies))
                {
                    myHttpWebRequest.Headers.Add(HttpRequestHeader.Cookie, sCookies);
                }
                // ставим False, чтобы при получении кода 302 не делать автоматический редирект
                myHttpWebRequest.AllowAutoRedirect = false;

                byte[] ByteArr = System.Text.Encoding.GetEncoding(1251).GetBytes(qQuery.text);
                myHttpWebRequest.ContentLength = ByteArr.Length;
                myHttpWebRequest.GetRequestStream().Write(ByteArr, 0, ByteArr.Length);

                try
                {
                    // отправляем запрос
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                    sCookies = "";
                    if (!String.IsNullOrEmpty(myHttpWebResponse.Headers["Set-Cookie"]))
                    {
                        sCookies = myHttpWebResponse.Headers["Set-Cookie"];
                    }

                    // посмотрим что получили :)
                    StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
                    sWebResponse = myStreamReader.ReadToEnd();

                    myHttpWebResponse.Close();

                    HRefPattern = "Ошибка регистрации";
                    m = Regex.Match(sWebResponse, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    if (m.Success) 
                    {
                        
                        HRefPattern = "Пользователь с таким именем уже зарегистрирован в системе";
                        m = Regex.Match(sWebResponse, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (m.Success)
                        {
                            //// попробуем заменить логин на предложенный
                            //string sDom = qQuery.fdData.Domain.Substring(0, qQuery.fdData.Domain.Length - 3);
                            //HRefPattern = "\\\".{4,31}@" + sDom + "\\.\\w{2,3}\\\"";
                            //m = Regex.Match(sWebResponse, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                            //if (m.Success) // выбирем первый попавшийся
                            //{
                            //    qQuery.EditQuery(MF, sCookies, sWebResponse, this, m.Value.Substring(1, m.Value.Length - sDom.Length - 6));
                            //    need = true; // отправим запрос заново с измененным логином
                            //    // только надо еще и капчу в этом случае заново получить, либо отправлять нужно уже другую форму...
                            //}
                            //else // почему-то не нашлись предложенные логины 
                            {
                                Result = DialogResult.Cancel;
                                file = new StreamWriter(@"error_log.txt", true);
                                file.WriteLine(s + ": wrong login");
                                file.Close();
                            }
                        }
                        
                        HRefPattern = "Неверно указан код защиты от автоматических регистраций";
                        m = Regex.Match(sWebResponse, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (m.Success)
                        {

                            // надо будет отправить сообщение об ошибке
                            //myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://antigate.com/res.php?key=" + MF.KapchaKey + "&action=reportbad&id=" + ctCapcha.id);
                            string KapchaKey;
                            if (MF.tbSettings.Lines.Count() > 0) KapchaKey = MF.tbSettings.Lines[0]; else KapchaKey = "";
                            myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://antigate.com/res.php?key=" + KapchaKey + "&action=reportbad&id=" + ctCapcha.id);
                            if (MF.checkBoxUseProxy.Checked)
                            {
                                myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                                myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
                            }
                            myHttpWebRequest.UserAgent = MF.UserAgent;
                            myHttpWebRequest.Accept = "*/*";
                            myHttpWebRequest.Headers.Add("Accept-Language", "ru");
                            myHttpWebRequest.KeepAlive = true;
                            myHttpWebRequest.AllowAutoRedirect = false;
                            myHttpWebRequest.Method = "GET";

                            HttpWebResponse ret = (HttpWebResponse)myHttpWebRequest.GetResponse();
                            ret.Close();

                            Result = DialogResult.Cancel;
                            file = new StreamWriter(@"error_log.txt", true);
                            file.WriteLine(s + ": wrong capcha");
                            file.Close();

                        }

                        HRefPattern = "Превышен лимит регистраций с Вашего IP. Попробуйте зарегистрировать ящик позже";
                        m = Regex.Match(sWebResponse, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        if (m.Success)
                        {

                            Result = DialogResult.Cancel;
                            file = new StreamWriter(@"error_log.txt", true);
                            file.WriteLine(s + ": wrong IP");
                            file.Close();

                        }

                        if (Result != DialogResult.Cancel) // какая-то непонятная ошибка...
                        {
                            Result = DialogResult.Cancel;
                            file = new StreamWriter(@"error_log.txt", true);
                            file.WriteLine(s + ": unknown error");
                            file.Close();
                        }

                        break;

                    }
                    else // типа все ok
                    {
                        Result = DialogResult.OK;
                        file = new StreamWriter(@"list_akk.txt", true);
                        file.WriteLine(qQuery.fdData.Login + "@" + qQuery.fdData.Domain + ";" + qQuery.fdData.PassWord);
                        file.Close();
                        break;
                    }

                }
                catch
                {
                    Result = DialogResult.Cancel;
                    file = new StreamWriter(@"error_log.txt", true);
                    file.WriteLine(s + ": catch2");
                    file.Close();
                    break;
                }

            }

            if (MF.checkBoxMyWorld.Checked) 
            {

                // открываем страницу заполнения данных "Моего Мира" 
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://win.mail.ru/cgi-bin/my_fill_edu");
                if (MF.checkBoxUseProxy.Checked)
                {
                    myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                    myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
                }
                myHttpWebRequest.Referer = "http://win.mail.ru/cgi-bin/signup"; // http://win.mail.ru/cgi-bin/reg
                myHttpWebRequest.UserAgent = MF.UserAgent;
                myHttpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                myHttpWebRequest.Headers.Add("Accept-Language", "ru");

                if (!String.IsNullOrEmpty(sCookies))
                {
                    myHttpWebRequest.Headers.Add(HttpRequestHeader.Cookie, sCookies);
                }

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                // заполняем форму данными и отправляем их на сервер 
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://win.mail.ru/cgi-bin/start?my_edu=save&mra=0 ");
                if (MF.checkBoxUseProxy.Checked)
                {
                    myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                    myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
                }
                myHttpWebRequest.Referer = "http://win.mail.ru/cgi-bin/my_fill_edu";
                myHttpWebRequest.UserAgent = MF.UserAgent;
                
                myHttpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                myHttpWebRequest.Headers.Add("Accept-Language", "ru");
                myHttpWebRequest.ContentType = "text/plain";

                if (!String.IsNullOrEmpty(sCookies))
                {
                    myHttpWebRequest.Headers.Add(HttpRequestHeader.Cookie, sCookies);
                }

                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                // выводим результат в консоль
                //StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
                //textBox1.Text = myStreamReader.ReadToEnd();

            }

        }

    }

    public class CapchImage
    {
        public Bitmap img;

        public CapchImage(string sWebResponse, string s)
        {

            // найдем ссылки на картинки капчи
            Match m;
            string[] asLink = new string[3];
            int n = 0;
            string HRefPattern = "x_image\\?[0-9]+&num=[1-3]&x_reg_id=[a-zA-Z0-9]+";

            m = Regex.Match(sWebResponse, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            while (m.Success && n <= 3)
            {
                asLink[n] = m.Value;
                m = m.NextMatch();
                n++;
            }

            string webAddress;
            string localAddress;
            WebClient httpClient;

            //System.IO.File.Delete("capcha1.jpg");
            //System.IO.File.Delete("capcha2.jpg");
            //System.IO.File.Delete("capcha3.jpg");
            //System.IO.File.Delete("capchaAll.jpg");

            webAddress = "http://win.mail.ru/cgi-bin/" + asLink[0];
            localAddress = s + "capcha1.jpg";
            httpClient = new WebClient();
            try { httpClient.DownloadFile(webAddress, localAddress); }
            catch { }
            httpClient.Dispose();
            webAddress = "http://win.mail.ru/cgi-bin/" + asLink[1];
            localAddress = s + "capcha2.jpg";
            httpClient = new WebClient();
            try { httpClient.DownloadFile(webAddress, localAddress); }
            catch { }
            httpClient.Dispose();
            webAddress = "http://win.mail.ru/cgi-bin/" + asLink[2];
            localAddress = s + "capcha3.jpg";
            httpClient = new WebClient();
            try { httpClient.DownloadFile(webAddress, localAddress); }
            catch { }
            httpClient.Dispose();

            //Загружаем Bitmap'ы
            Bitmap bitmap1 = (Bitmap)Bitmap.FromFile(s + "capcha1.jpg");
            Bitmap bitmap2 = (Bitmap)Bitmap.FromFile(s + "capcha2.jpg");
            Bitmap bitmap3 = (Bitmap)Bitmap.FromFile(s + "capcha3.jpg");
            //Создаем новый Bitmap нужного размера
            img = new Bitmap(bitmap1.Width + bitmap2.Width + bitmap3.Width, bitmap1.Height);
            //Создаем Graphics
            Graphics graphics = Graphics.FromImage(img);
            //И рисуем в нужных координатах наши Bitmap'ы
            graphics.DrawImage(bitmap1, 0, 0);
            graphics.DrawImage(bitmap2, bitmap1.Width, 0);
            graphics.DrawImage(bitmap3, bitmap1.Width * 2, 0);
            //Сохраняем новый Bitmap в файл
            img.Save(s + "capchaAll.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

        }

    }

    public class PostData
    {
        
        private string s_method = String.Empty;

        public string Method { get { return this.s_method; } }
        private string s_action = String.Empty;

        public string Action { get { return this.s_action; } }
        public string Param { get { return this.s_param; } }

        private string s_param = String.Empty;

        public PostData(string s_PostString)
        {
            if (s_PostString.IndexOf("=") != -1)
            {
                this.s_method = s_PostString.Substring(0, s_PostString.IndexOf("="));
                this.s_action = s_PostString.Substring(s_PostString.IndexOf("=") + 1);
                if (this.s_action.IndexOf("!") != -1)
                {
                    this.s_action = s_action.Substring(0, this.s_action.IndexOf("!")); this.s_param = s_PostString.Substring(s_PostString.IndexOf("!") + 1);
                }

            }

        }
        
        public static string MultiFormData(string Key, string Value, string Boundary)
        {
            string output = "--" + Boundary + "\r\n"; 
            output += "Content-Disposition: form-data; name=\"" + Key + "\"\r\n\r\n";
            output += Value + "\r\n";
            return output;
        }
        
        public static string MultiFormDataFile(string Key, string Value, string FileName, string FileType, string Boundary)
        {
            string output = "--" + Boundary + "\r\n"; 
            output += "Content-Disposition: form-data; name=\"" + Key + "\"; filename=\"" + FileName + "\"\r\n"; 
            output += "Content-Type: " + FileType + " \r\n\r\n";
            output += Value + "\r\n";
            return output;
        }

    }

    public class CapchText
    {
        public string txt;
        public string id;

        public CapchText(mainForm MF, string s)
        {

            StreamReader reader;
            string KapchaKey;

            //Запрос
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://antigate.com/in.php");
            if (MF.checkBoxUseProxy.Checked)
            {
                myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
            }
            myHttpWebRequest.UserAgent = MF.UserAgent;
            myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.Headers.Add("Accept-Language", "ru");
            myHttpWebRequest.KeepAlive = true;
            myHttpWebRequest.AllowAutoRedirect = false;
            myHttpWebRequest.Method = "POST";

            //пост параметры
            string sBoundary = DateTime.Now.Ticks.ToString("x");
            myHttpWebRequest.ContentType = "multipart/form-data; boundary=" + sBoundary;
            if (MF.tbSettings.Lines.Count() > 0) KapchaKey = MF.tbSettings.Lines[0]; else KapchaKey = "";
            string sPostMultiString = "";
            sPostMultiString += PostData.MultiFormData("method", "post", sBoundary);
            sPostMultiString += PostData.MultiFormData("soft_id", "28", sBoundary);
            //sPostMultiString += PostData.MultiFormData("key", MF.KapchaKey, sBoundary);
            sPostMultiString += PostData.MultiFormData("key", KapchaKey, sBoundary);
            sPostMultiString += PostData.MultiFormData("file", s + "capchaAll.jpg", sBoundary);
            //sPostMultiString += PostData.MultiFormData("numeric", "1", sBoundary);

            string sFileContent = "";
            StreamReader file = new StreamReader(s + "capchaAll.jpg", System.Text.Encoding.Default, true);
            sFileContent = file.ReadToEnd();
            sPostMultiString += PostData.MultiFormDataFile("file", sFileContent, s + "capchaAll.jpg", "image/pjpeg", sBoundary);
            sPostMultiString += "--" + sBoundary + "--\r\n\r\n";

            //Получаем массив байт
            byte[] byteArray = Encoding.Default.GetBytes(sPostMultiString);
            //Получаем и записываем общую длинну массива
            myHttpWebRequest.ContentLength = byteArray.Length;

            //Отправляем запрос и получаем ответ
            HttpWebResponse ret = null;
            try
            {
                myHttpWebRequest.GetRequestStream().Write(byteArray, 0, byteArray.Length);
                ret = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch
            {
                //AddDebugMessage("Не получен ответ на запрос http://antigate.com/in.php");
            }

            //Получаем и разбираем текст ответа сервера
            reader = new StreamReader(ret.GetResponseStream(), Encoding.GetEncoding(1251));
            string pg;
            pg = reader.ReadToEnd();
            string[] pars = pg.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            id = pars[1];

            ret.Close();

            //Надо добавить обработку ошибок

            //Ждём распознавания капчи
            Thread.Sleep(10000);

            while (true)
            {

                if (MF.tbSettings.Lines.Count() > 0) KapchaKey = MF.tbSettings.Lines[0]; else KapchaKey = "";
                //myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://antigate.com/res.php?key=" + MF.KapchaKey + "&action=get&id=" + id);
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://antigate.com/res.php?key=" + KapchaKey + "&action=get&id=" + id);
                if (MF.checkBoxUseProxy.Checked)
                {
                    myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                    myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
                }
                myHttpWebRequest.UserAgent = MF.UserAgent;
                myHttpWebRequest.Accept = "*/*";
                myHttpWebRequest.Headers.Add("Accept-Language", "ru");
                myHttpWebRequest.KeepAlive = true;
                myHttpWebRequest.AllowAutoRedirect = false;
                myHttpWebRequest.Method = "GET";

                ret = (HttpWebResponse)myHttpWebRequest.GetResponse();

                reader = new StreamReader(ret.GetResponseStream(), Encoding.GetEncoding(1251));
                pg = reader.ReadToEnd();
                pars = pg.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                //если капча распознана забираем значениеи и прекращаем ожидание
                if (pars[0] == "OK")
                {
                    //data = "img_code=" + pars[1];
                    txt = pars[1];
                    ret.Close();
                    break;
                }

                Thread.Sleep(5000);

            }

        }

    }

    public class FormParameters
    {
        public string Name;
        public string Family;
        public string Day;
        public string Month;
        public string Year;
        public string Login;
        public string Domain;
        public string PassWord1;
        public string PassWord2;
        public string Answer;
        public string Question2;
        public string EMail2;
        public string Phone;
        public string Sex;
        public string XRegID;
        public string Capcha;

        public FormParameters(string sWebResponse)
        {

            // найдем имена параметров
            int iMyIndex;
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Имя';");
            Name = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Фамилия';");
            Family = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'День рождения';");
            Day = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Месяц рождения';");
            Month = sWebResponse.Substring(iMyIndex - 4 - 10, 10);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Год рождения';");
            Year = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'E-mail';");
            Login = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            //iMyIndex = sWebResponse.IndexOf(@"var request = ""RegistrationDomain=""+(urlencode(domain))+");
            //Domain = sWebResponse.Substring(iMyIndex + 56 + 10, 18);
            iMyIndex = sWebResponse.IndexOf(@"<option value=""mail.ru"" selected>@mail.ru</option>");
            Domain = sWebResponse.Substring(iMyIndex - 45 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Пароль';");
            PassWord1 = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Подтверждение пароля';");
            PassWord2 = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Секретный вопрос';");
            Question2 = sWebResponse.Substring(iMyIndex - 6 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Ответ на секретный вопрос'");
            Answer = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Дополнительный e-mail';");
            EMail2 = sWebResponse.Substring(iMyIndex - 6 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Мобильный телефон';");
            Phone = sWebResponse.Substring(iMyIndex - 6 - 11, 11);
            iMyIndex = sWebResponse.IndexOf("prompts[prompts.length] = 'Ваш пол';");
            Sex = sWebResponse.Substring(iMyIndex - 4 - 18, 18);
            iMyIndex = sWebResponse.IndexOf("<td class=gr>Код&nbsp;на&nbsp;картинке</td>");
            Capcha = sWebResponse.Substring(iMyIndex + 42 + 77, 18);
            iMyIndex = sWebResponse.IndexOf("&x_reg_id=");
            XRegID = sWebResponse.Substring(iMyIndex + 10, 8);

        }

    }

    public class FormData
    {
        public string Name;
        public string Family;
        public string Day;
        public string Month;
        public string Year;
        public string Login;
        public string Domain;
        public string PassWord;
        public string Answer;

        public FormData(mainForm MF, string sCookies, FormParameters fpParameters)
        {
            Random randObj = new Random();

            // подготовим данные для заполнения формы
            Name = MF.tbNames.Lines[randObj.Next(0, MF.tbNames.Lines.Count() - 1)];
            Family = MF.tbFamilys.Lines[randObj.Next(0, MF.tbFamilys.Lines.Count() - 1)];
            Day = randObj.Next(1, 28).ToString();
            Month = randObj.Next(1, 12).ToString();
            //Year = randObj.Next(1973, 1993).ToString();
            int iSY;
            int iEY;
            if (MF.tbSettings.Lines.Count() > 2)
            {
                iSY = Convert.ToInt32(MF.tbSettings.Lines[1]);
                iEY = Convert.ToInt32(MF.tbSettings.Lines[2]);
            }
            else
            {
                iSY = 1973;
                iEY = 1993;
            }
            Year = randObj.Next(iSY, iEY).ToString();

            int checkedDomains = MF.checkedListBoxDomains.CheckedItems.Count;

            Domain = MF.asDomains[randObj.Next(0, checkedDomains - 1)];
            PassWord = MF.asAlphabet[randObj.Next(0, 61)] + MF.asAlphabet[randObj.Next(0, 61)] +
                MF.asAlphabet[randObj.Next(0, 61)] + MF.asAlphabet[randObj.Next(0, 61)] +
                MF.asAlphabet[randObj.Next(0, 61)] + MF.asAlphabet[randObj.Next(0, 61)] +
                MF.asAlphabet[randObj.Next(0, 61)] + MF.asAlphabet[randObj.Next(0, 61)] +
                MF.asAlphabet[randObj.Next(0, 61)];
            Answer = "hello, world";

            string[] asLoginData = new string[] { RusToEng(Name), 
                                                  RusToEng(Family), 
                                                  Year.ToString(), 
                                                  (Day.ToString() + Month.ToString()) };

            // создадим и проверим логин на доступность 

            while (true)
            {

                Login = "";
                int l = randObj.Next(0, 3);
                for (int i = 0; i <= l; i++)
                {
                    Login = Login + asLoginData[randObj.Next(0, 3)];
                }
                string suff = MF.asAlphabet[randObj.Next(62, 65)] + MF.asAlphabet[randObj.Next(0, 61)];
                if ((Login.Length + suff.Length) > 32)
                {
                    Login = Login.Remove(32 - suff.Length);
                }
                Login = Login + suff;
                while (Login.Length < 4)
                {
                    Login = Login + MF.asAlphabet[randObj.Next(0, 61)];
                }

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create("http://win.mail.ru/cgi-bin/checklogin");
                // прокси пока не используем
                if (MF.checkBoxUseProxy.Checked)
                {
                    myHttpWebRequest.Proxy = new WebProxy(MF.pProxy.IP, Convert.ToInt32(MF.pProxy.Port));
                    myHttpWebRequest.Proxy.Credentials = new NetworkCredential(MF.pProxy.Login, MF.pProxy.Password, MF.pProxy.Domain);
                }
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.Referer = "http://win.mail.ru/cgi-bin/signup";
                myHttpWebRequest.UserAgent = MF.UserAgent;
                myHttpWebRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, */*";
                myHttpWebRequest.Headers.Add("Accept-Language", "ru");
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                // передаем куки, полученные в предыдущем запросе
                if (!String.IsNullOrEmpty(sCookies))
                {
                    myHttpWebRequest.Headers.Add(HttpRequestHeader.Cookie, sCookies);
                }
                // ставим False, чтобы при получении кода 302 не делать автоматический редирект
                myHttpWebRequest.AllowAutoRedirect = false;

                string sQueryText = "RegistrationDomain=" + Domain + "&" + fpParameters.Login + "=" + Login + "&x_reg_id=" + fpParameters.XRegID;

                byte[] ByteArr = System.Text.Encoding.GetEncoding(1251).GetBytes(sQueryText);
                myHttpWebRequest.ContentLength = ByteArr.Length;
                myHttpWebRequest.GetRequestStream().Write(ByteArr, 0, ByteArr.Length);

                try
                {
                    // отправляем запрос
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                    // посмотрим что получили :)
                    StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.GetEncoding(1251));
                    string sWebResponse = myStreamReader.ReadToEnd();
                    myHttpWebResponse.Close();

                    string Ans = sWebResponse.Substring(0, 1);

                    if (Ans == "0")
                    {
                        break;
                    }

                }
                catch
                {
                }

            }

        }

        string RusToEng(string Rus)
        {
            Dictionary<string, string> translit = new Dictionary<string, string>();
            translit.Add("а", "a");
            translit.Add("б", "b");
            translit.Add("в", "v");
            translit.Add("г", "g");
            translit.Add("д", "d");
            translit.Add("е", "e");
            translit.Add("ё", "yo");
            translit.Add("ж", "zh");
            translit.Add("з", "z");
            translit.Add("и", "i");
            translit.Add("й", "y");
            translit.Add("к", "k");
            translit.Add("л", "l");
            translit.Add("м", "m");
            translit.Add("н", "n");
            translit.Add("о", "o");
            translit.Add("п", "p");
            translit.Add("р", "r");
            translit.Add("с", "s");
            translit.Add("т", "t");
            translit.Add("у", "u");
            translit.Add("ф", "f");
            translit.Add("х", "h");
            translit.Add("ц", "ts");
            translit.Add("ч", "ch");
            translit.Add("ш", "sh");
            translit.Add("щ", "sch");
            translit.Add("ъ", "");
            translit.Add("ы", "y");
            translit.Add("ь", "");
            translit.Add("э", "e");
            translit.Add("ю", "yu");
            translit.Add("я", "ya");
            translit.Add("А", "A");
            translit.Add("Б", "B");
            translit.Add("В", "V");
            translit.Add("Г", "G");
            translit.Add("Д", "D");
            translit.Add("Е", "E");
            translit.Add("Ё", "Yo");
            translit.Add("Ж", "Zh");
            translit.Add("З", "Z");
            translit.Add("И", "I");
            translit.Add("Й", "Y");
            translit.Add("К", "K");
            translit.Add("Л", "L");
            translit.Add("М", "M");
            translit.Add("Н", "N");
            translit.Add("О", "O");
            translit.Add("П", "P");
            translit.Add("Р", "R");
            translit.Add("С", "S");
            translit.Add("Т", "T");
            translit.Add("У", "U");
            translit.Add("Ф", "F");
            translit.Add("Х", "H");
            translit.Add("Ц", "Ts");
            translit.Add("Ч", "Ch");
            translit.Add("Ш", "Sh");
            translit.Add("Щ", "Sch");
            translit.Add("Ъ", "");
            translit.Add("Ы", "Y");
            translit.Add("Ь", "");
            translit.Add("Э", "E");
            translit.Add("Ю", "Yu");
            translit.Add("Я", "Ya");

            string source = Rus;
            foreach (KeyValuePair<string, string> pair in translit)
            {
                source = source.Replace(pair.Key, pair.Value);
            }
            return source;
        }

    }

    public class Query
    {
        public FormData fdData;
        FormParameters fpParameters;
        public string text;

        public Query(mainForm MF, string sCookies, string sWebResponse, capchaForm CF)
        {
            EditQuery(MF, sCookies, sWebResponse, CF, "");
        }

        public void EditQuery(mainForm MF, string sCookies, string sWebResponse, capchaForm CF, string sLogin)
        {
            if (sLogin == "")
            {
                fpParameters = new FormParameters(sWebResponse);
                fdData = new FormData(MF, sCookies, fpParameters);
            }
            else
            {
                fdData.Login = sLogin;
            }

            // сформируем текст запроса
            text = "ID=LJUSoiSP&Count=1&back=&browserData=screen--%0D%0Anavigator--%60" +
                "appCodeName%60%3A%60Mozilla%60%2C%60appName%60%3A%60Microsoft+Internet+Explorer%60%2C%60" +
                "appMinorVersion%60%3A%600%60%2C%60cpuClass%60%3A%60x86%60%2C%60platform%60%3A%60Win32%60%2C%60" +
                "plugins%60%3A%60undefined%60%2C%60opsProfile%60%3Ainaccessible%2C%60userProfile%60%3Ainaccessible%2C%60" +
                "systemLanguage%60%3A%60ru%60%2C%60userLanguage%60%3A%60ru%60%2C%60appVersion%60%3A%604.0+%28" +
                "compatible%3B+MSIE+7.0%3B+Windows+NT+5.1%3B+Mozilla%2F4.0+%28compatible%3B+MSIE+6.0%3B+Windows+NT+5.1%3B+SV1%29+%3B+" +
                "InfoPath.1%3B+.NET+CLR+2.0.50727%3B+.NET+CLR+1.1.4322%3B+.NET+CLR+3.0.04506.30%3B+.NET+CLR+3.0.4506.2152%3B+" +
                ".NET+CLR+3.5.30729%29%60%2C%60userAgent%60%3A%60Mozilla%2F4.0+%28compatible%3B+MSIE+7.0%3B+Windows+NT+5.1%3B+" +
                "Mozilla%2F4.0+%28compatible%3B+MSIE+6.0%3B+Windows+NT+5.1%3B+SV1%29+%3B+InfoPath.1%3B+.NET+CLR+2.0.50727%3B+" +
                ".NET+CLR+1.1.4322%3B+.NET+CLR+3.0.04506.30%3B+.NET+CLR+3.0.4506.2152%3B+.NET+CLR+3.5.30729%29%60%2C%60onLine%60%3A%60" +
                "true%60%2C%60cookieEnabled%60%3A%60true%60%2C%60mimeTypes%60%3A%60undefined%60%0D%0Aflash--%60version%60%3A%60" +
                "WIN+9%2C0%2C28%2C0%60%0D%0A" +
                "&" + fpParameters.Name + "=" + fdData.Name +
                "&" + fpParameters.Family + "=" + fdData.Family +
                "&" + fpParameters.Day + "=" + fdData.Day +
                "&" + fpParameters.Month + "=" + fdData.Month +
                "&" + fpParameters.Year + "=" + fdData.Year +
                "&" + fpParameters.Login + "=" + fdData.Login +
                "&" + fpParameters.Domain + "=" + fdData.Domain +
                "&" + fpParameters.PassWord1 + "=" + fdData.PassWord +
                "&" + fpParameters.PassWord2 + "=" + fdData.PassWord +
                "&Password_Question=" + "%C4%E5%E2%E8%F7%FC%FF+%F4%E0%EC%E8%EB%E8%FF+%EC%E0%F2%E5%F0%E8" +
                "&" + fpParameters.Question2 + "=" + "" +
                "&" + fpParameters.Answer + "=" + fdData.Answer +
                "&" + fpParameters.EMail2 + "=" + "" +
                "&" + fpParameters.Phone + "=" +
                //1=m, 2=f
                "&" + fpParameters.Sex + "=" + "2" +
                "&Mrim.Country=" +
                "&Mrim.Region=" +
                "&geo_countryId=" + "undefined" +
                "&geo_regionId=" + "undefined" +
                "&geo_cityId=" + "undefined" +
                "&your_town=" +
                "&geo_country=" + "undefined" +
                "&geo_place=";
            //"&mra1=1" + //можно добавить Mail.Ru Agent
            if (MF.checkBoxMyWorld.Checked == true)
            {
                text = text + "&my_create=" + "1";
            }
            text = text +
                "&x_reg_id=" + fpParameters.XRegID +
                "&security_image_id=" +
                "&" + fpParameters.Capcha + "=" + CF.ctCapcha.txt +
                "&B1=+%C7%E0%F0%E5%E3%E8%F1%F2%F0%E8%F0%EE%E2%E0%F2%FC+%EF%EE%F7%F2%EE%E2%FB%E9+%FF%F9%E8%EA+";
        }

    }

}
