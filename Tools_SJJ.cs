namespace NS_Tools_SJJ
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;


    //示例    [HighlightIfNull] public GameObject targetObject;
    #region 赋值框未赋值变红色

#if UNITY_EDITOR
    using UnityEditor;
    using TMPro;
    using System.Text.RegularExpressions;
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
        void Start()
        {

        }

        void Update()
        {

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


    }


}





