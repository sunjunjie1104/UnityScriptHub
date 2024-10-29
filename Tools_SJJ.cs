namespace NS_Tools_SJJ
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;



    //示例    [HighlightIfNull] public GameObject targetObject;
    #region 赋值框未赋值变红色

#if UNITY_EDITOR
    using UnityEditor;
    using TMPro;
    using System.Text.RegularExpressions;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.IO;
    using UnityEngine.Networking;
#endif

    // 自定义属性，用于标记需要高亮显示的字段
    public class HighlightIfNullAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    // 自定义属性绘制器，仅在编辑器中有效
    [CustomPropertyDrawer(typeof(HighlightIfNullAttribute))]
    public class HighlightIfNullDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 检查属性类型是否是 GameObject 或 Object
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                // 如果属性为空，则将背景颜色设置为红色
                if (property.objectReferenceValue == null)
                {
                    GUI.backgroundColor = Color.red;
                }

                // 绘制属性
                EditorGUI.PropertyField(position, property, label);

                // 恢复默认背景颜色
                GUI.backgroundColor = Color.white;
            }
            else
            {
                // 如果属性类型不是 ObjectReference，显示默认警告
                EditorGUI.LabelField(position, label.text, "Use [HighlightIfNull] with GameObject or Object.");
            }
        }
    }
#endif

    #endregion


    public class Tools_SJJ : MonoBehaviour
    {
        public static Tools_SJJ INS;

        void Awake()
        {

            if (INS == null)
            {
                INS = this;
                DontDestroyOnLoad(this.gameObject);

            }
            else
            {
                Destroy(this.gameObject); // 防止重复实例
            }
        }



        public List<int> List_Int_获取的随机数字组(int 获取的数字个数, int 范围开始数, int 范围结束数)
        {

            List<int> numbers = new List<int>();
            for (int i = 范围开始数; i <= 范围结束数; i++)
            {
                numbers.Add(i);
            }

            if (获取的数字个数 > numbers.Count)
            {
                throw new ArgumentException("获取的数字个数不能大于范围内的数字总数");
            }

            // 随机打乱列表中数字的顺序
            for (int i = 0; i < numbers.Count; i++)
            {
                int temp = numbers[i];
                int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
                numbers[i] = numbers[randomIndex];
                numbers[randomIndex] = temp;
            }

            // 获取前20个数字作为结果
            List<int> randomNumbers = numbers.GetRange(0, 获取的数字个数);
            return randomNumbers;
        }



        Coroutine 当前倒计时协程_Text;
        public void 数字倒计时_Text(float F_时长, Text tt, Action 结束回调)
        {
            tt.text = F_时长.ToString("00");
            // 如果已有倒计时协程在运行，则停止它
            if (当前倒计时协程_Text != null)
            {
                StopCoroutine(当前倒计时协程_Text);
            }

            // 启动新的倒计时协程并保存引用
            当前倒计时协程_Text = StartCoroutine(IE_倒计时_Text(F_时长, tt, 结束回调));
        }
        IEnumerator IE_倒计时_Text(float F_时长, Text tt, Action 结束回调)
        {
            float F_当前时间 = F_时长;

            while (F_当前时间 > 0)
            {
                // 每秒等待
                yield return new WaitForSeconds(1);
                F_当前时间--;

                // 更新UI文本
                if (tt != null)
                {
                    tt.text = F_当前时间.ToString("00");
                }
            }

            // 倒计时结束时执行回调方法
            结束回调?.Invoke();
            // 清除协程引用
            当前倒计时协程_Text = null;
        }



        Coroutine 当前倒计时协程_TMP;
        public void 数字倒计时_TMP(float F_时长, TextMeshProUGUI tt, Action 结束回调)
        {
            tt.text = F_时长.ToString("00");
            // 如果已有倒计时协程在运行，则停止它
            if (当前倒计时协程_TMP != null)
            {
                StopCoroutine(当前倒计时协程_TMP);
            }

            // 启动新的倒计时协程并保存引用
            当前倒计时协程_TMP = StartCoroutine(IE_倒计时_TMP(F_时长, tt, 结束回调));
        }
        IEnumerator IE_倒计时_TMP(float F_时长, TextMeshProUGUI tt, Action 结束回调)
        {
            float F_当前时间 = F_时长;

            while (F_当前时间 > 0)
            {
                // 每秒等待
                yield return new WaitForSeconds(1);
                F_当前时间--;

                // 更新UI文本
                if (tt != null)
                {
                    tt.text = F_当前时间.ToString("00");
                }
            }

            // 倒计时结束时执行回调方法
            结束回调?.Invoke();
            // 清除协程引用
            当前倒计时协程_TMP = null;
        }



        public void 清空列表及其物体(List<GameObject> listGA)
        {
            // 遍历并销毁所有元素
            foreach (var obj in listGA)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }

            listGA.Clear();
        }
        public void 清空数组及其物体<T>(ref T[] array) where T : UnityEngine.Object
        {
            // 遍历并销毁所有元素
            foreach (var obj in array)
            {
                if (obj != null)
                {
                    Destroy(obj);  // 异步销毁 GameObject
                }
            }

            // 将数组重新分配为一个长度为 0 的新数组
            array = new T[0];
        }


        public void 等比缩放_Ima(float maxWidth, float maxHeight, Image image)
        {
            if (image != null)
            {
                image.SetNativeSize();
            }
            // 假设您已经将Sprite赋值给了Image
            Sprite sprite = image.sprite;

            // 获取Sprite的原始宽度和高度
            float originalWidth = sprite.rect.width;
            float originalHeight = sprite.rect.height;

            // 计算宽度和高度的缩放因子
            float widthScale = maxWidth / originalWidth;
            float heightScale = maxHeight / originalHeight;

            // 选择较小的缩放因子以保持原始宽高比
            float minScale = Mathf.Min(widthScale, heightScale);

            // 计算新的宽度和高度
            float newWidth = originalWidth * minScale;
            float newHeight = originalHeight * minScale;

            // 更新Image的尺寸
            image.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }
        public void 等比缩放_RAW(float maxWidth, float maxHeight, RawImage RAW)
        {
            if (RAW != null)
            {
                RAW.SetNativeSize();
            }
            // 假设您已经将Sprite赋值给了Image
            Texture tex = RAW.texture;

            // 获取Sprite的原始宽度和高度
            float originalWidth = tex.width;
            float originalHeight = tex.height;

            // 计算宽度和高度的缩放因子
            float widthScale = maxWidth / originalWidth;
            float heightScale = maxHeight / originalHeight;

            // 选择较小的缩放因子以保持原始宽高比
            float minScale = Mathf.Min(widthScale, heightScale);

            // 计算新的宽度和高度
            float newWidth = originalWidth * minScale;
            float newHeight = originalHeight * minScale;

            // 更新Image的尺寸
            RAW.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }




        /// <summary>
        /// 泛型方法，根据元素中的第一个连续数字对列表进行排序。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型。</typeparam>
        /// <param name="list">需要排序的列表。</param>
        /// <param name="nameSelector">用于提取字符串属性的选择器函数。</param>
        public void 对List内元素进行数字排序<T>(List<T> list, Func<T, string> nameSelector)
        {
            if (list == null || nameSelector == null)
            {
                throw new ArgumentNullException("列表或选择器函数不能为null。");
            }

            list.Sort((a, b) =>
            {
                int numA = Int_提取第一个连续数(nameSelector(a));
                int numB = Int_提取第一个连续数(nameSelector(b));
                return numA.CompareTo(numB);
            });
        }

        /// <summary>
        /// 提取字符串中的第一个连续数字序列，并将其解析为整数。
        /// 如果没有找到数字，则返回0。
        /// </summary>
        /// <param name="name">需要解析的字符串。</param>
        /// <returns>解析出的整数，如果没有数字则返回0。</returns>
        public int Int_提取第一个连续数(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return 0;
            }

            // 使用正则表达式提取第一个连续的数字序列
            var match = Regex.Match(name, @"\d+");
            if (match.Success)
            {
                // 解析数字，处理可能的解析异常
                if (int.TryParse(match.Value, out int number))
                {
                    return number;
                }
            }

            return 0; // 如果没有找到数字或解析失败，返回0
        }



        public string ST_去掉扩展名(string filename)
        {
            int lastDotIndex = filename.LastIndexOf('.');
            if (lastDotIndex > 0)
            {
                return filename.Substring(0, lastDotIndex);
            }
            return filename; // 没有扩展名，返回原始文件名
        }


        public List<Sprite> List_SP_转换的图片组(List<Texture> textures)
        {
            if (textures.Count == 0) { return null; }
            List<Sprite> List_SP = new List<Sprite>();
            for (int i = 0; i < textures.Count; i++)
            {
                Sprite sprite = Sprite.Create(textures[i] as Texture2D, new Rect(0, 0, textures[i].width, textures[i].height), Vector2.zero);
                List_SP.Add(sprite);

            }
            return List_SP;
        }

        public List<Texture> List_TEX_转换的图片组(List<Sprite> sprites)
        {
            if (sprites.Count == 0) { return null; }
            List<Texture> List_TEX = new List<Texture>();
            for (int i = 0; i < sprites.Count; i++)
            {
                Texture2D texture = sprites[i].texture;
                List_TEX.Add(texture);
            }
            return List_TEX;
        }

        public Sprite SP_转换的图片(Texture texture)
        {
            Sprite sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sprite;
        }

        public Texture TEX_转换的图片(Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            return texture;
        }




        // 通用延时执行方法
        public void 延时执行_无参(float F_延时, Action action)
        {
            StartCoroutine(延时执行协程(action, F_延时));
        }

        private IEnumerator 延时执行协程(Action action, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            action?.Invoke(); // 执行传入的方法
        }


        public string ST_获取的本地IP()
        {
            string localIP = "";
            try
            {
                // 获取本地主机名
                string hostName = Dns.GetHostName();
                // 通过主机名获取所有与本机相关的IP地址
                IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);

                // 过滤出IPv4地址
                foreach (IPAddress ip in ipAddresses)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break; // 找到一个IPv4地址就返回
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("获取本地IP地址时出错: " + ex.Message);
            }

            return localIP;
        }



        public int Int_获取自动分配的UDP端口号()
        {
            int availablePort = 0;
            try
            {
                // 使用 UdpClient 自动分配端口
                UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                availablePort = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
                udpClient.Close(); // 使用完后关闭
            }
            catch (System.Exception ex)
            {
                Debug.LogError("获取可用UDP端口时出错: " + ex.Message);
            }

            return availablePort;
        }





        public int Int_限制数值范围(int value, int bound1, int bound2)
        {
            // 确保 bound1 是最小值，bound2 是最大值
            int min = Math.Min(bound1, bound2);
            int max = Math.Max(bound1, bound2);

            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public float F_限制数值范围(float value, float bound1, float bound2)
        {
            // 确保 bound1 是最小值，bound2 是最大值
            float min = Math.Min(bound1, bound2);
            float max = Math.Max(bound1, bound2);

            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }




        public byte[] BY_字符串转成16进制字节组(string str)
        {
            // 16进制 要发送的数据
            string[] strArray = str.Split(' ');
            byte[] sendData = new byte[strArray.Length];
            // 16进制 要发送的数据
            for (int i = 0; i < strArray.Length; i++)
            {
                sendData[i] = byte.Parse(strArray[i], System.Globalization.NumberStyles.HexNumber);
            }
            return sendData;


        }

        public byte[] BY_字符串转成UTF8字节组(string str)
        {

            byte[] sendData = Encoding.UTF8.GetBytes(str);

            return sendData;
        }

        public byte[] BY_字符串转成ASCII字节组(string str)
        {
            byte[] sendData = Encoding.ASCII.GetBytes(str);
            return sendData;
        }

        public byte[] BY_字符串转成GB2312字节组(string str)
        {
            byte[] sendData = Encoding.GetEncoding("GB2312").GetBytes(str);
            return sendData;
        }



        public void 屏幕截图并保存(string ST_路径)
        {
            StartCoroutine(IE_屏幕截图并保存在streaming下(ST_路径));
        }
        IEnumerator IE_屏幕截图并保存在streaming下(string 路径)
        {

            yield return new WaitForEndOfFrame();
            string filename;
            byte[] bytes;
            Texture2D tex;
            string oldFilePath;
            filename = DateTime.Now.ToString("yyyy_MMdd_HHmm_ss_") + UnityEngine.Random.Range(0, 9999).ToString("0000");
            oldFilePath = Application.streamingAssetsPath + 路径 + filename + ".png";
            FileStream f = new FileStream(oldFilePath, FileMode.Create);

            tex = new Texture2D(Screen.width, Screen.height);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
            tex.Apply();

            bytes = tex.EncodeToPNG();
            f.Write(bytes, 0, bytes.Length);
            f.Close();
            print("已截图并保存在" + oldFilePath);
        }


    }


}
