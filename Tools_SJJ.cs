using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Text;
using DG.Tweening;
using System.Diagnostics;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using Microsoft.Win32;
using WindowsInput;
using OfficeOpenXml;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.ComTypes;


#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class Tools_SJJ : MonoBehaviour
{
    public static Tools_SJJ INS;

    void Awake()
    {

        if (INS == null) { INS = this; DontDestroyOnLoad(this.gameObject); } else { Destroy(this.gameObject); }
        // 取消Unity启动画面();
        //读取表格并设置程序参数();
        //激活多屏显示();

    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

        UPDATE_获取点击的物体();
        UPDATA_获取点击的屏幕坐标();
    }



    #region UI相关

    [HideInInspector] public string ST_Text空一个字 = "\u3000";
    [HideInInspector] public string ST_Text空两个字 = "\u3000\u3000";
    [HideInInspector] public string ST_Text换行空两个字 = "\n\u3000\u3000";

    [HideInInspector] public string ST_TMP空一个字 = "<space=1em>";
    [HideInInspector] public string ST_TMP空两个字 = "<space=2em>";
    [HideInInspector] public string ST_TMP换行空两个字 = "\n<space=2em>";

    [HideInInspector] public string ST_换行符 = "\n";


    public void 按钮点击效果(RectTransform RE)
    {
        if (RE != null)
        {
            RE.DOKill();
            RE.DOScale(0.9f, 0.2f).OnComplete(() =>
            {
                RE.DOScale(1, 0.2f);
            });
        }
    }


    //示例  Tools_SJJ.INS.互换RE(ref rect_item_1, ref rect_item_2);
    public void 互换RE(ref RectTransform RE1, ref RectTransform RE2)
    {
        RectTransform RE_临时 = RE1;
        RE1 = RE2;
        RE2 = RE_临时;
    }

    //示例   Tools_SJJ.INS.互换Ima(ref ima_内容图1, ref ima_内容图2);
    public void 互换Ima(ref Image Ima1, ref Image Ima2)
    {
        Image Ima_临时 = Ima1;
        Ima1 = Ima2;
        Ima2 = Ima_临时;

    }

    //示例   Tools_SJJ.INS.互换RAWIma(ref ima_内容图1, ref ima_内容图2);
    public void 互换RAWIma(ref RawImage rawIma1, ref RawImage rawIma2)
    {
        RawImage Ima_临时 = rawIma1;
        rawIma1 = rawIma2;
        rawIma2 = Ima_临时;

    }




    Coroutine 当前倒计时协程_Text;
    //示例 Tools_SJJ.INS.数字倒计时_Text(3, tt, () => { Debug.Log("倒计时结束"); });
    public void 数字倒计时_Text(float F_时长, Text tt, Action 结束回调)
    {
        if (当前倒计时协程_Text != null) { StopCoroutine(当前倒计时协程_Text); }
        当前倒计时协程_Text = StartCoroutine(IE_倒计时_Text(F_时长, tt, 结束回调));
    }
    IEnumerator IE_倒计时_Text(float F_时长, Text tt, Action 结束回调)
    {
        float elapsedTime = 0f;
        while (elapsedTime < F_时长)
        {
            elapsedTime += Time.deltaTime;
            tt.text = ST_转为时间格式(F_时长 - elapsedTime);
            yield return null;
        }
        tt.text = ST_转为时间格式(0);
        结束回调?.Invoke();
    }



    Coroutine 当前倒计时协程_TMP;
    public void 数字倒计时_TMP(float F_时长, TextMeshProUGUI tt, Action 结束回调)
    {
        if (当前倒计时协程_TMP != null) { StopCoroutine(当前倒计时协程_TMP); }
        当前倒计时协程_TMP = StartCoroutine(IE_倒计时_TMP(F_时长, tt, 结束回调));
    }
    IEnumerator IE_倒计时_TMP(float F_时长, TextMeshProUGUI tt, Action 结束回调)
    {
        float elapsedTime = 0f;
        while (elapsedTime < F_时长)
        {
            elapsedTime += Time.deltaTime;
            tt.text = ST_转为时间格式(F_时长 - elapsedTime);
            yield return null;
        }
        tt.text = ST_转为时间格式(0);
        结束回调?.Invoke();
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




    //示例 Tools_SJJ.INS.屏幕截图并保存("/截图文件夹/");
    public void 屏幕截图并保存(string ST_保存路径)
    {
        StartCoroutine(IE_屏幕截图并保存在streaming下(ST_保存路径));
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




    public void 等比缩放_Ima(Image image, float maxWidth, float maxHeight)
    {
        if (image != null)
        {
            image.SetNativeSize();
        }

        image.SetNativeSize();

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
    public void 等比缩放_RAW(RawImage RAW, float maxWidth, float maxHeight)
    {
        if (RAW != null)
        {
            RAW.SetNativeSize();
        }

        RAW.SetNativeSize();

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






    // 示例  Tools_SJJ.INS.按钮组添加点击事件(List_Bu_首页按钮组, ButtonClick_首页的按钮);
    public void 按钮组添加点击事件(List<Button> buttonGroup, Action<int> onClickEvent)
    {
        for (int i = 0; i < buttonGroup.Count; i++)
        {
            int index = i; // 这里使用局部变量存储索引，避免闭包问题
            buttonGroup[i].onClick.AddListener(() => onClickEvent(index));
        }
    }

    public void 切换到某个界面(List<RectTransform> List_RE_界面, int Int_某个界面, float 用时)
    {
        for (int i = 0; i < List_RE_界面.Count; i++)
        {
            List_RE_界面[i].gameObject.SetActive(true);
            if (List_RE_界面[i].GetComponent<CanvasGroup>() == null)
            {
                List_RE_界面[i].AddComponent<CanvasGroup>();
            }
            List_RE_界面[i].GetComponent<CanvasGroup>().DOKill();
            List_RE_界面[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
            List_RE_界面[i].GetComponent<CanvasGroup>().DOFade(0, 用时);

        }
        List_RE_界面[Int_某个界面].GetComponent<CanvasGroup>().DOKill();
        List_RE_界面[Int_某个界面].GetComponent<CanvasGroup>().DOFade(1, 用时).OnComplete(() =>
        {
            List_RE_界面[Int_某个界面].GetComponent<CanvasGroup>().blocksRaycasts = true;
        });
    }


    #endregion



    #region  小方法

#if UNITY_EDITOR
    // 示例       Tools_SJJ.INS.相机画面保存到streamingassets(Camera.main, 5760, 1200, "1212");
    public void 相机画面保存到streamingassets(Camera cam, int Int_分辨率X, int Int_分辨率Y, string ST_图片名)
    {
        // 1. 创建 RenderTexture
        RenderTexture renderTexture = new RenderTexture(Int_分辨率X, Int_分辨率Y, 24);
        cam.targetTexture = renderTexture;

        // 2. 渲染摄像机画面
        RenderTexture.active = renderTexture;
        cam.Render();

        // 3. 创建 Texture2D 并读取 RenderTexture 数据
        Texture2D texture = new Texture2D(Int_分辨率X, Int_分辨率Y, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Int_分辨率X, Int_分辨率Y), 0, 0);
        texture.Apply();

        // 4. 编码为 PNG
        byte[] bytes = texture.EncodeToPNG();

        // 5. 确保 StreamingAssets 目录存在（运行时不推荐保存到该目录）
        string path = Application.streamingAssetsPath + "/" + ST_图片名 + ".png";

        Directory.CreateDirectory(Application.streamingAssetsPath); // 确保路径存在
        File.WriteAllBytes(path, bytes); // 保存图片




        // 6. 清理
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(texture);
    }


#endif




    public GameObject G_在鼠标指针处生成对象(GameObject G_预制体)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Figure out where the ground is
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~0))
        {
            Vector3 p = hit.point;
            var rot = Quaternion.identity;
            GameObject obj = Instantiate(G_预制体, p, rot);
            return obj;

        }
        else
        {
            return null;
        }

    }




    //示例    Tools_SJJ.INS.EVENT_OnClick_获取点击物体.AddListener(() => { Tools_SJJ.INS.打印对象组(Tools_SJJ.INS.List_G_点击获取的物体组); });
    [HideInInspector] public List<GameObject> List_G_点击获取的物体组 = new List<GameObject>();
    [HideInInspector] public UnityEvent EVENT_OnClick_获取点击物体;
    void UPDATE_获取点击的物体()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 clickPosition = Input.mousePosition;
            List_G_点击获取的物体组 = List_G_获取点击的物体组(Camera.main, clickPosition);
            //  打印对象组(List_G_点击获取的物体组);
            EVENT_OnClick_获取点击物体?.Invoke();
        }

        // 检测手指触摸
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPosition = Input.GetTouch(0).position;
            List_G_点击获取的物体组 = List_G_获取点击的物体组(Camera.main, touchPosition);
            // 打印对象组(List_G_点击获取的物体组);
            EVENT_OnClick_获取点击物体?.Invoke();
        }
    }
    public List<GameObject> List_G_获取点击的物体组(Camera cam, Vector3 position)
    {
        List<GameObject> objectList = new List<GameObject>();

        // 检测 UI 物体
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = position
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            objectList.Add(result.gameObject);
        }

        // 检测 3D 物体
        Ray ray = cam.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // 按照距离从近到远排序
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            objectList.Add(hit.collider.gameObject);
        }

        // 如果列表不为空，将最前面的物体移到第一个位置
        if (objectList.Count > 1)
        {
            var firstObject = objectList[0];
            objectList.RemoveAt(0);
            objectList.Insert(0, firstObject);
        }

        return objectList;
    }




    //示例 Tools_SJJ.INS.EVENT_OnClick_获取点击坐标.AddListener(() => { print(Tools_SJJ.INS.V2_点击的屏幕坐标); });
    [HideInInspector] public Vector2 V2_点击的屏幕坐标 = Vector2.zero;
    [HideInInspector] public UnityEvent EVENT_OnClick_获取点击坐标;
    void UPDATA_获取点击的屏幕坐标()
    {
        if (Input.GetMouseButtonDown(0)) { V2_点击的屏幕坐标 = Input.mousePosition; EVENT_OnClick_获取点击坐标?.Invoke(); }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { V2_点击的屏幕坐标 = Input.GetTouch(0).position; EVENT_OnClick_获取点击坐标?.Invoke(); }
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






    public void 打印对象组<T>(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            if (item != null)
            {
                print(item);
            }
        }
    }

    public string ST_转为时间格式(float timeSeconds, bool showMilliseconds = false)
    {
        float totalSeconds = timeSeconds;
        int hours = Mathf.FloorToInt(totalSeconds / (60f * 60f));
        float usedSeconds = hours * 60f * 60f;

        int minutes = Mathf.FloorToInt((totalSeconds - usedSeconds) / 60f);
        usedSeconds += minutes * 60f;

        int seconds = Mathf.FloorToInt(totalSeconds - usedSeconds);

        string result;
        if (hours <= 0)
        {
            if (showMilliseconds)
            {
                int milliSeconds = (int)((totalSeconds - Mathf.Floor(totalSeconds)) * 1000f);
                result = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
            }
            else
            {
                result = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
        else
        {
            if (showMilliseconds)
            {
                int milliSeconds = (int)((totalSeconds - Mathf.Floor(totalSeconds)) * 1000f);
                result = string.Format("{2}:{0:00}:{1:00}:{3:000}", minutes, seconds, hours, milliSeconds);
            }
            else
            {
                result = string.Format("{2}:{0:00}:{1:00}", minutes, seconds, hours);
            }
        }

        return result;
    }


    //示例 print(Tools_SJJ.INS.F_区间映射值(23, 1, 100, 2, 9));
    public float F_区间映射值(float F_输入值, float F_输入值的最小范围, float F_输入值的最大范围, float F_输出值的最小范围, float F_输出值的最大范围)
    {
        // 将输入值x减去输入范围的最小值，得到一个偏移量
        float offset = F_输入值 - F_输入值的最小范围;

        // 将偏移量乘以输出范围的大小（outMax - outMin）
        float scaled = offset * (F_输出值的最大范围 - F_输出值的最小范围);

        // 将结果除以输入范围的大小（inMax - inMin）
        float result = scaled / (F_输入值的最大范围 - F_输入值的最小范围);

        // 将上述结果加上输出范围的最小值outMin，得到映射后的值
        return result + F_输出值的最小范围;

        //return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }




    //示例    Int_当前选中内容在数组中的序号= Tools_SJJ.INS.Int_获取传参数的周围的某个数(Int_当前选中内容在数组中的序号, 1, List_SP_要切换的内容图组.Count);
    public int Int_获取传参数的周围的某个数(int Int_当前数, int 前后第几个, int Int_总数)
    {
        int bb = (Int_当前数 + 前后第几个 + Int_总数) % Int_总数;
        return bb;
    }




    //示例          Tools_SJJ.INS.开始延时执行_无参("飞入飞出切换图片", ButtonClick_下一页, F_延时);
    //示例          Tools_SJJ.INS.停止延时执行_无参("飞入飞出切换图片");
    private Dictionary<string, Coroutine> DIC_协程字典1 = new Dictionary<string, Coroutine>();
    public void 开始延时执行_无参(string key, Action method, float F_延时)
    {
        停止延时执行_无参(key);
        DIC_协程字典1[key] = StartCoroutine(IE_开始延时执行_无参(method, F_延时));
    }
    public void 停止延时执行_无参(string key)
    {
        if (DIC_协程字典1.TryGetValue(key, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            DIC_协程字典1.Remove(key);
        }
    }
    IEnumerator IE_开始延时执行_无参(Action method, float F_延时)
    {
        yield return new WaitForSeconds(F_延时);

        if (method != null)
        {
            method();
        }

    }






    //示例          Tools_SJJ.INS.开始循环切换_延时_无参("飞入飞出切换图片", ButtonClick_下一页, F_延时, F_间隔);
    //示例          Tools_SJJ.INS.停止循环切换_无参("飞入飞出切换图片");
    private Dictionary<string, Coroutine> DIC_协程字典2 = new Dictionary<string, Coroutine>();
    public void 开始循环执行_延时_无参(string key, Action method, float F_延时, float F_间隔时长)
    {
        停止循环执行_延时_无参(key);
        DIC_协程字典2[key] = StartCoroutine(IE_开始循环执行_延时_无参(method, F_延时, F_间隔时长));
    }
    public void 停止循环执行_延时_无参(string key)
    {
        if (DIC_协程字典2.TryGetValue(key, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            DIC_协程字典2.Remove(key);
        }
    }
    IEnumerator IE_开始循环执行_延时_无参(Action method, float F_延时, float F_间隔时长)
    {
        yield return new WaitForSeconds(F_延时);
        while (true)
        {
            if (method != null)
            {
                method();
            }
            yield return new WaitForSeconds(F_间隔时长);
        }
    }





    public int Int_限制数值范围(int Int_要限制的数, int 范围1, int 范围2)
    {
        // 确保 bound1 是最小值，bound2 是最大值
        int min = Math.Min(范围1, 范围2);
        int max = Math.Max(范围1, 范围2);

        if (Int_要限制的数 < min)
        {
            Int_要限制的数 = min;
        }
        else if (Int_要限制的数 > max)
        {
            Int_要限制的数 = max;
        }

        return Int_要限制的数;
    }

    public float F_限制数值范围(float F_要限制的数, float 范围1, float 范围2)
    {
        // 确保 bound1 是最小值，bound2 是最大值
        float min = Math.Min(范围1, 范围2);
        float max = Math.Max(范围1, 范围2);

        if (F_要限制的数 < min)
        {
            F_要限制的数 = min;
        }
        else if (F_要限制的数 > max)
        {
            F_要限制的数 = max;
        }

        return F_要限制的数;
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
    public void 清空数组<T>(T[] array)
    {
        if (array != null)
        {
            Array.Clear(array, 0, array.Length);
        }
    }
    public void 清空列表<T>(List<T> list)
    {
        if (list != null)
        {
            list.Clear();
        }
    }
    public void 清空字典<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        if (dict != null)
        {
            dict.Clear();
        }
    }





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



    #endregion



    #region  系统相关


#if  PLATFORM_STANDALONE_WIN
    int Int_激活多屏显示次数 = 0;
    public void 激活多屏显示()
    {
        Int_激活多屏显示次数 += 1;
        if (Int_激活多屏显示次数 > 1) { return; }
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
        }

    }
#endif



    //示例 Tools_SJJ.INS.模拟键盘按键_单键(KeyCode.F10);
    public void 模拟键盘按键_单键(KeyCode key)
    {
        InputSimulator sim = new InputSimulator();
        WindowsInput.Native.VirtualKeyCode vKey = MapKeyCode(key);
        sim.Keyboard.KeyPress(vKey);
    }

    //示例  Tools_SJJ.INS.模拟键盘按键_组合键(KeyCode.LeftWindows, KeyCode.Tab); //Alt +F4 好像不行
    public void 模拟键盘按键_组合键(KeyCode modifierKey, KeyCode key)
    {
        InputSimulator sim = new InputSimulator();
        WindowsInput.Native.VirtualKeyCode modKey = MapKeyCode(modifierKey);
        WindowsInput.Native.VirtualKeyCode vKey = MapKeyCode(key);
        sim.Keyboard.ModifiedKeyStroke(modKey, vKey);
    }

    private WindowsInput.Native.VirtualKeyCode MapKeyCode(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.A: return WindowsInput.Native.VirtualKeyCode.VK_A;
            case KeyCode.B: return WindowsInput.Native.VirtualKeyCode.VK_B;
            case KeyCode.C: return WindowsInput.Native.VirtualKeyCode.VK_C;
            case KeyCode.D: return WindowsInput.Native.VirtualKeyCode.VK_D;
            case KeyCode.E: return WindowsInput.Native.VirtualKeyCode.VK_E;
            case KeyCode.F: return WindowsInput.Native.VirtualKeyCode.VK_F;
            case KeyCode.G: return WindowsInput.Native.VirtualKeyCode.VK_G;
            case KeyCode.H: return WindowsInput.Native.VirtualKeyCode.VK_H;
            case KeyCode.I: return WindowsInput.Native.VirtualKeyCode.VK_I;
            case KeyCode.J: return WindowsInput.Native.VirtualKeyCode.VK_J;
            case KeyCode.K: return WindowsInput.Native.VirtualKeyCode.VK_K;
            case KeyCode.L: return WindowsInput.Native.VirtualKeyCode.VK_L;
            case KeyCode.M: return WindowsInput.Native.VirtualKeyCode.VK_M;
            case KeyCode.N: return WindowsInput.Native.VirtualKeyCode.VK_N;
            case KeyCode.O: return WindowsInput.Native.VirtualKeyCode.VK_O;
            case KeyCode.P: return WindowsInput.Native.VirtualKeyCode.VK_P;
            case KeyCode.Q: return WindowsInput.Native.VirtualKeyCode.VK_Q;
            case KeyCode.R: return WindowsInput.Native.VirtualKeyCode.VK_R;
            case KeyCode.S: return WindowsInput.Native.VirtualKeyCode.VK_S;
            case KeyCode.T: return WindowsInput.Native.VirtualKeyCode.VK_T;
            case KeyCode.U: return WindowsInput.Native.VirtualKeyCode.VK_U;
            case KeyCode.V: return WindowsInput.Native.VirtualKeyCode.VK_V;
            case KeyCode.W: return WindowsInput.Native.VirtualKeyCode.VK_W;
            case KeyCode.X: return WindowsInput.Native.VirtualKeyCode.VK_X;
            case KeyCode.Y: return WindowsInput.Native.VirtualKeyCode.VK_Y;
            case KeyCode.Z: return WindowsInput.Native.VirtualKeyCode.VK_Z;
            case KeyCode.Alpha0: return WindowsInput.Native.VirtualKeyCode.VK_0;
            case KeyCode.Alpha1: return WindowsInput.Native.VirtualKeyCode.VK_1;
            case KeyCode.Alpha2: return WindowsInput.Native.VirtualKeyCode.VK_2;
            case KeyCode.Alpha3: return WindowsInput.Native.VirtualKeyCode.VK_3;
            case KeyCode.Alpha4: return WindowsInput.Native.VirtualKeyCode.VK_4;
            case KeyCode.Alpha5: return WindowsInput.Native.VirtualKeyCode.VK_5;
            case KeyCode.Alpha6: return WindowsInput.Native.VirtualKeyCode.VK_6;
            case KeyCode.Alpha7: return WindowsInput.Native.VirtualKeyCode.VK_7;
            case KeyCode.Alpha8: return WindowsInput.Native.VirtualKeyCode.VK_8;
            case KeyCode.Alpha9: return WindowsInput.Native.VirtualKeyCode.VK_9;
            case KeyCode.Space: return WindowsInput.Native.VirtualKeyCode.SPACE;
            case KeyCode.Return: return WindowsInput.Native.VirtualKeyCode.RETURN;
            case KeyCode.Escape: return WindowsInput.Native.VirtualKeyCode.ESCAPE;
            case KeyCode.Backspace: return WindowsInput.Native.VirtualKeyCode.BACK;
            case KeyCode.Tab: return WindowsInput.Native.VirtualKeyCode.TAB;
            case KeyCode.LeftShift: return WindowsInput.Native.VirtualKeyCode.SHIFT;
            case KeyCode.RightShift: return WindowsInput.Native.VirtualKeyCode.RSHIFT;
            case KeyCode.LeftControl: return WindowsInput.Native.VirtualKeyCode.CONTROL;
            case KeyCode.RightControl: return WindowsInput.Native.VirtualKeyCode.RCONTROL;
            case KeyCode.LeftAlt: return WindowsInput.Native.VirtualKeyCode.MENU;
            case KeyCode.RightAlt: return WindowsInput.Native.VirtualKeyCode.RMENU;
            case KeyCode.UpArrow: return WindowsInput.Native.VirtualKeyCode.UP;
            case KeyCode.DownArrow: return WindowsInput.Native.VirtualKeyCode.DOWN;
            case KeyCode.LeftArrow: return WindowsInput.Native.VirtualKeyCode.LEFT;
            case KeyCode.RightArrow: return WindowsInput.Native.VirtualKeyCode.RIGHT;
            case KeyCode.Insert: return WindowsInput.Native.VirtualKeyCode.INSERT;
            case KeyCode.Delete: return WindowsInput.Native.VirtualKeyCode.DELETE;
            case KeyCode.Home: return WindowsInput.Native.VirtualKeyCode.HOME;
            case KeyCode.End: return WindowsInput.Native.VirtualKeyCode.END;
            case KeyCode.PageUp: return WindowsInput.Native.VirtualKeyCode.PRIOR;
            case KeyCode.PageDown: return WindowsInput.Native.VirtualKeyCode.NEXT;
            case KeyCode.F1: return WindowsInput.Native.VirtualKeyCode.F1;
            case KeyCode.F2: return WindowsInput.Native.VirtualKeyCode.F2;
            case KeyCode.F3: return WindowsInput.Native.VirtualKeyCode.F3;
            case KeyCode.F4: return WindowsInput.Native.VirtualKeyCode.F4;
            case KeyCode.F5: return WindowsInput.Native.VirtualKeyCode.F5;
            case KeyCode.F6: return WindowsInput.Native.VirtualKeyCode.F6;
            case KeyCode.F7: return WindowsInput.Native.VirtualKeyCode.F7;
            case KeyCode.F8: return WindowsInput.Native.VirtualKeyCode.F8;
            case KeyCode.F9: return WindowsInput.Native.VirtualKeyCode.F9;
            case KeyCode.F10: return WindowsInput.Native.VirtualKeyCode.F10;
            case KeyCode.F11: return WindowsInput.Native.VirtualKeyCode.F11;
            case KeyCode.F12: return WindowsInput.Native.VirtualKeyCode.F12;
            case KeyCode.LeftWindows: return WindowsInput.Native.VirtualKeyCode.LWIN;
            case KeyCode.RightWindows: return WindowsInput.Native.VirtualKeyCode.RWIN;

            default: throw new ArgumentException("Unsupported KeyCode");
        }
    }

    //示例   Tools_SJJ.INS.模拟鼠标点击("左键", new Vector2(1920, 1080));
    public void 模拟鼠标点击(string a, Vector2 position)
    {
        double x = position.x * 100;
        double y = position.y * 100;
        InputSimulator sim = new InputSimulator();
        sim.Mouse.MoveMouseTo(x, y);

        switch (a)
        {
            case "左键":
                sim.Mouse.LeftButtonClick();
                break;

            case "右键":
                sim.Mouse.RightButtonClick();
                break;



            default:
                UnityEngine.Debug.LogWarning("未识别的鼠标按钮: " + a);
                break;
        }
    }



    public void 显示FPS()
    {
        if (GetComponent<Renderer>() == null)
        {
            gameObject.AddComponent<FPSDisplay>().Is_显示FPS = true;
        }

    }

    public void 显示打印台()
    {
        SRDebug.Instance.ShowDebugPanel();
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
            print("获取本地IP地址时出错: " + ex.Message);
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
            print("获取可用UDP端口时出错: " + ex.Message);
        }

        return availablePort;
    }




    public void 打印进程名()
    {
        Process[] processes = Process.GetProcesses();
        foreach (Process p in processes)
        {
            if (!string.IsNullOrEmpty(p.MainWindowTitle))
            {
                // 打印出所有窗口的进程名称和标题
                print("Process Name: " + p.ProcessName + ", Window Title: " + p.MainWindowTitle);
            }
        }
    }

    public List<string> List_ST_获取的进程名()
    {

        List<string> List_ST = new List<string>();
        Process[] processes = Process.GetProcesses();
        foreach (Process p in processes)
        {
            if (!string.IsNullOrEmpty(p.MainWindowTitle))
            {
                // 打印出所有窗口的进程名称和标题
                List_ST.Add(p.ProcessName);
            }
        }
        return List_ST;
    }



    //示例  Tools_SJJ.INS.打开外部文件("灯光联动", "E:\\UnityOut\\业达党建\\业达党建_灯光联动\\灯光联动.exe");
    public void 打开外部文件(string 文件名_不要后缀, string 文件路径)
    {

        if (IsProgramRunning(文件名_不要后缀))
        {
            print($"{文件名_不要后缀} is already running.");
        }
        else
        {
            try
            {
                Process.Start(文件路径);
                print($"Started program from {文件路径}");
            }
            catch (System.Exception e)
            {
                print($"Failed to start program from {文件路径}: {e.Message}");
            }
        }

    }

    bool IsProgramRunning(string ST_窗口名)
    {
        Process[] processes = Process.GetProcessesByName(ST_窗口名);
        return processes.Length > 0;
    }




    // 示例  Tools_SJJ.INS.打开Quicker动作("30aad8bb-5368-40fa-a91c-741ecf823629");
    public void 打开Quicker动作(string 动作ID_无空格, string quicker路径 = @"C:\Program Files\Quicker\QuickerStarter.exe")
    {

        string arguments = $"runaction:{动作ID_无空格}";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = quicker路径,
            Arguments = arguments,
        };

        // 启动 Quicker 动作
        Process process = Process.Start(startInfo);

        if (process == null)
        {
            print("无法启动指定的 Quicker 动作");
        }

    }








    //示例  删除注册表键值(Application.productName);
#if PLATFORM_STANDALONE_WIN
    public void 删除注册表键值(string name)
    {
        try
        {
            string[] aimnames;
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey(Application.companyName, true);
            aimnames = aimdir.GetSubKeyNames();
            foreach (string aimKey in aimnames)
            {
                if (aimKey == name)
                    aimdir.DeleteSubKeyTree(name);

            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"An error occurred: {ex.Message}\n{ex.StackTrace}");
        }
    }
#endif





    string ST_程序窗口1_名字;
    string ST_程序窗口2_名字;

    string ST_程序窗口1_UDP消息;
    string ST_程序窗口2_UDP消息;

    string ST_程序窗口1_路径;
    string ST_程序窗口2_路径;

    string ST_程序窗口1_Quicker动作ID;
    string ST_程序窗口2_Quicker动作ID;

    //需要配合UDP用
    public void 启用切换程序窗口显示()
    {

        ST_程序窗口1_名字 = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 3, 2);
        ST_程序窗口2_名字 = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 4, 2);

        ST_程序窗口1_UDP消息 = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 3, 3);
        ST_程序窗口2_UDP消息 = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 4, 3);

        ST_程序窗口1_路径 = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 3, 4);
        ST_程序窗口2_路径 = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 4, 4);

        ST_程序窗口1_Quicker动作ID = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 3, 5);
        ST_程序窗口2_Quicker动作ID = ST_读取表格单格信息("/切换程序窗口/切换程序窗口配置表", 4, 5);

        UDP_Control.INS.Event_UDP_接收消息 += EVENT_UDP来消息执行一次;


    }
    void EVENT_UDP来消息执行一次(string a)
    {
        if (a == ST_程序窗口1_UDP消息)
        {
            打开外部文件(ST_程序窗口1_名字, ST_程序窗口1_路径);
            打开Quicker动作(ST_程序窗口1_Quicker动作ID);
        }
        else
        if (a == ST_程序窗口2_UDP消息)
        {
            打开外部文件(ST_程序窗口2_名字, ST_程序窗口2_路径);
            打开Quicker动作(ST_程序窗口2_Quicker动作ID);
        }
        else
        {
            print(a);
        }
    }



    //使用示例 Tools_SJJ.INS.ST_读取txt文件("窗口位置.txt",3);
    public string ST_读取txt文件(string fileName, int Int_第几行数据)
    {
        int aa = Int_第几行数据 - 1;
        try
        {
            // 获取StreamingAssets的路径
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            // 检查文件是否存在
            if (File.Exists(filePath))
            {
                // 读取文件的所有行
                string[] lines = File.ReadAllLines(filePath);

                // 检查指定的行号是否有效
                if (aa >= 0 && aa < lines.Length)
                {
                    // 返回指定行的内容
                    return lines[aa];
                }
                else
                {
                    print("Line number is out of bounds.");
                    return string.Empty;
                }
            }
            else
            {
                print("File does not exist at: " + filePath);
                return string.Empty;
            }
        }
        catch (Exception e)
        {
            print("Error reading file: " + e.Message);
            return string.Empty;
        }
    }

    //使用示例  Tools_SJJ.INS.写入txt数据("窗口位置.txt", "100111",1);
    public void 写入txt数据(string fileName, string 写入内容, int Int_第几行数据)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        // 如果文件不存在，则创建一个新文件
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }

        string[] lines = File.ReadAllLines(filePath);
        if (Int_第几行数据 > lines.Length)
        {
            // 创建一个新的数组，长度为指定行数
            string[] newLines = new string[Int_第几行数据];
            // 复制原有内容到新数组
            for (int i = 0; i < lines.Length; i++)
            {
                newLines[i] = lines[i];
            }
            // 将剩余的行设置为空字符串
            for (int i = lines.Length; i < Int_第几行数据 - 1; i++)
            {
                newLines[i] = "";
            }
            newLines[Int_第几行数据 - 1] = 写入内容;
            lines = newLines;
        }
        else
        {
            lines[Int_第几行数据 - 1] = 写入内容;
        }

        File.WriteAllLines(filePath, lines);
    }

    #region  读写表格

    //示例 ("/文件夹/文件名")
    public void 删除某列的某行到某行的内容(string 路径和表格名, int 列, int 开始行, int 结束行, int 表几 = 1)
    {
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);
        // 通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 根据名称获取工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[表几];

            if (工作表1 == null)
            {
                print("找不到指定的工作表：" + 路径和表格名);
                return;
            }
            else
            {
                for (int i = 开始行; i <= 结束行; i++)
                {
                    工作表1.Cells[i, 列].Value = "";
                }
                package.Save();
            }


        }
    }

    //示例 ("/文件夹/文件名")
    public string ST_读取表格单格信息(string 路径和表格名, int 行, int 列, int 表几 = 1)
    {
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);
        // 通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 根据名称获取工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[表几];

            if (工作表1 == null)
            {
                print("找不到指定的工作表：" + 路径和表格名);
                return "";
            }
            else
            {
                ExcelRange range = 工作表1.Cells[工作表1.Dimension.Address];
                string cellValue = 工作表1.Cells[行, 列].Value?.ToString();
                return cellValue;
            }

        }

    }

    //示例 ("/文件夹/文件名")
    public void 写入表格单格信息(string 路径和表格名, int 行, int 列, string 写入内容, int 表几 = 1)
    {
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        // 创建一个文件信息对象，用于引用 Excel 文件
        FileInfo _excelName = new FileInfo(_filePath);

        // 检查文件是否存在，不存在则创建一个新文件
        if (!_excelName.Exists)
        {
            print("文件不存在");
        }

        // 通过 ExcelPackage 打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 获取当前工作簿
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet;

            // 检查是否存在目标表格（Sheet），若存在直接引用，否则创建新表格
            string 表名 = "Sheet" + 表几.ToString();
            worksheet = workbook.Worksheets[表名] ?? workbook.Worksheets.Add(表名);

            // 写入单元格信息
            worksheet.Cells[行, 列].Value = 写入内容;

            // 保存修改
            try
            {
                package.Save();
            }
            catch
            {
                print("请关闭表格后重试");
            }
        }

    }

    //示例 ("/文件夹/文件名")
    public int Int_获取表格总行数(string 路径和表格名, int 表几 = 1)
    {
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);
        // 通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 根据名称获取工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[1];

            if (工作表1 == null) { print("找不到指定的工作表："); }

            // 找到有内容的范围
            ExcelRange range = 工作表1.Cells[工作表1.Dimension.Address];

            int 有内容的总行数 = 0;
            int 有内容的总列数 = 0;

            for (int Int_当前读取行 = range.Start.Row; Int_当前读取行 <= range.End.Row; Int_当前读取行++)
            {
                bool hasContentInRow = false;
                for (int Int_当前读取列 = range.Start.Column; Int_当前读取列 <= range.End.Column; Int_当前读取列++)
                {
                    // 判断单元格是否为空
                    if (工作表1.Cells[Int_当前读取行, Int_当前读取列].Value != null)
                    {

                        string cellValue = 工作表1.Cells[Int_当前读取行, Int_当前读取列].Value?.ToString();

                        // 更新有内容的总列数
                        有内容的总列数 = Mathf.Max(有内容的总列数, Int_当前读取列);
                        hasContentInRow = true;
                    }
                }

                if (hasContentInRow)
                {
                    有内容的总行数++;
                }
            }

            return 有内容的总行数;
        }

    }

    //示例 ("/文件夹/文件名")
    public int Int_获取表格总列数(string 路径和表格名, int 表几 = 1)
    {

        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);
        // 通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 根据名称获取工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[1];

            if (工作表1 == null)
            {
                print("找不到指定的工作表：");

            }

            // 找到有内容的范围
            ExcelRange range = 工作表1.Cells[工作表1.Dimension.Address];

            int 有内容的总行数 = 0;
            int 有内容的总列数 = 0;

            for (int Int_当前读取行 = range.Start.Row; Int_当前读取行 <= range.End.Row; Int_当前读取行++)
            {
                bool hasContentInRow = false;
                for (int Int_当前读取列 = range.Start.Column; Int_当前读取列 <= range.End.Column; Int_当前读取列++)
                {
                    // 判断单元格是否为空
                    if (工作表1.Cells[Int_当前读取行, Int_当前读取列].Value != null)
                    {

                        string cellValue = 工作表1.Cells[Int_当前读取行, Int_当前读取列].Value?.ToString();

                        // 更新有内容的总列数
                        有内容的总列数 = Mathf.Max(有内容的总列数, Int_当前读取列);

                        hasContentInRow = true;
                    }
                }

                if (hasContentInRow)
                {
                    有内容的总行数++;
                }
            }


            //  print("有内容的总行数：" + 有内容的总行数);
            //  print("有内容的总列数：" + 有内容的总列数);
            return 有内容的总列数;
        }


    }

    //示例 ("/文件夹/文件名")
    public void 某列写入数据(string 路径和表格名, int 列, int 起始行, List<string> 写入的内容组, int 表几 = 1)
    {
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);
        // 通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 根据名称获取工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[表几];

            if (工作表1 == null)
            {
                print("找不到指定的工作表：" + 路径和表格名);
                return;
            }
            else
            {
                for (int i = 0; i < 写入的内容组.Count; i++)
                {
                    工作表1.Cells[起始行 + i, 列].Value = 写入的内容组[i];
                }
                package.Save();
            }
        }
    }

    public int Int_获取表格某列的总行数(string 路径和表格名, int 列, int 表几 = 1)
    {
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);

        // 使用 ExcelPackage 打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 获取指定的工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[表几];

            if (工作表1 == null)
            {
                print("找不到指定的工作表：" + 路径和表格名);
                return 0;
            }
            else
            {
                int 总行数 = 0;

                // 获取工作表的最大行数
                int 最大行数 = 工作表1.Dimension.End.Row;

                // 遍历指定列，统计有内容的单元格数
                for (int 行 = 1; 行 <= 最大行数; 行++)
                {
                    if (工作表1.Cells[行, 列].Value != null && 工作表1.Cells[行, 列].Value.ToString().Trim() != "")
                    {
                        总行数++;

                    }
                }

                return 总行数;
            }
        }
    }

    public List<string> List_ST_获取某列中某行及以下的数据(string 路径和表格名, int 列, int 起始行, int 表几 = 1)
    {
        List<string> List_ST = new List<string>();
        string _filePath = Application.streamingAssetsPath + 路径和表格名 + ".xlsx";
        FileInfo _excelName = new FileInfo(_filePath);
        // 通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(_excelName))
        {
            // 根据名称获取工作表
            ExcelWorksheet 工作表1 = package.Workbook.Worksheets[表几];

            if (工作表1 == null)
            {
                print("找不到指定的工作表：" + 路径和表格名);
                return List_ST;
            }
            else
            {
                int aa = Int_获取表格总行数(路径和表格名, 列);
                for (int i = 起始行; i <= aa; i++)
                {
                    if (工作表1.Cells[i, 列].Value != null)
                    {
                        List_ST.Add(工作表1.Cells[i, 列].Value.ToString());
                    }
                }
                return List_ST;
            }

        }
    }

    public void 添加表格数据到List_String(object cellValue, List<string> list)
    {
        if (cellValue != null)
        {
            list.Add(cellValue.ToString());
        }
    }
    public void 添加表格数据到List_INT(object cellValue, List<int> list)
    {
        if (cellValue != null)
        {
            list.Add(int.Parse(cellValue.ToString()));
        }
    }

    public void 添加表格数据到List_FLOAT(object cellValue, List<float> list)
    {
        if (cellValue != null)
        {
            list.Add(float.Parse(cellValue.ToString()));
        }
    }

    #endregion


    #region 控制窗口

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool SetWindowLong(System.IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(System.IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

    // 窗口控制相关常量
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    // 显示状态相关常量
    const int SW_SHOWDEFAULT = 10;
    const int SW_FORCEMINIMIZE = 11;
    const int SW_HIDE = 0;
    const int SW_MAXIMIZE = 3;
    const int SW_MINIMIZE = 6;
    const int SW_RESTORE = 9;
    const int SW_SHOW = 5;
    const int SW_SHOWMAXIMIZED = 3;
    const int SW_SHOWMINIMIZED = 2;
    const int SW_SHOWNA = 8;
    const int SW_SHOWNOACTIVATE = 4;
    const int SW_SHOWNORMAL = 1;

    private const int GWL_STYLE = -16;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOACTIVATE = 0x0010;
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x00080000;


    // 程序路径和名字不能有中文
    public void 设置程序背景为透明()
    {
        IntPtr hwnd = FindWindow(null, Application.productName);
        if (hwnd == IntPtr.Zero)
        {
            print("没有找到窗口");
        }
        else
        {
            print("找到窗口" + hwnd);

            int intExTemp = GetWindowLong(hwnd, GWL_EXSTYLE);

            SetWindowLong(hwnd, GWL_EXSTYLE, intExTemp | WS_EX_LAYERED);

            SetLayeredWindowAttributes(hwnd, 0, 255, 1);

        }

    }

    //示例  Tools_SJJ.INS.将程序始终置于最前面("");
    public void 将程序始终置于最前面(string ST_程序名)
    {
        if (string.IsNullOrEmpty(ST_程序名))
        {
            ST_程序名 = Application.productName;
            // print(ST_程序名);
        }

        IntPtr windowHandle = FindWindow(null, ST_程序名);
        if (windowHandle != IntPtr.Zero)
        {
            SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            print("窗口置于最前面了");
        }
        else
        {
            print("未找到窗口句柄，请确保应用程序在独立模式下运行");
        }


    }




    // 示例 Tools_SJJ.INS.设置程序窗口化无边框并设置位置和宽高("", 400, 600, 1000, 400);
    int Int_设置分辨率次数 = 0;
    public void 设置程序窗口化无边框并设置位置和宽高(string ST_程序名, int _posX, int _posY, int _Txtwith, int _Txtheight)
    {
        Int_设置分辨率次数 += 1;
        if (Int_设置分辨率次数 > 1)
        {
            return;
        }
        if (string.IsNullOrEmpty(ST_程序名))
        {
            ST_程序名 = Application.productName;
        }
        IntPtr windowHandle = FindWindow(null, ST_程序名);

        Screen.fullScreen = false;

        // 设置窗口为无边框
        int style = GetWindowLong(windowHandle, GWL_STYLE);
        style &= ~WS_BORDER;
        style &= ~WS_CAPTION;
        SetWindowLong(windowHandle, GWL_STYLE, style);
        SetWindowPos(windowHandle, System.IntPtr.Zero, _posX, _posY, _Txtwith, _Txtheight, SWP_NOZORDER | SWP_NOACTIVATE);

    }


    public void 设置程序为全屏()
    {
        Int_设置分辨率次数 += 1;
        if (Int_设置分辨率次数 > 1)
        {
            return;
        }
        Screen.fullScreen = true;

    }






    [DllImport("user32.dll")]
    static extern bool GetWindowRect(IntPtr hwnd, ref RECT lpRect);

    // Define a RECT structure to hold the window position and size
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public Vector2 V2_获取当前程序窗口位置XY()
    {
        IntPtr windowHandle = FindWindow(null, Application.productName);  // Find the window by its title
        if (windowHandle != IntPtr.Zero)
        {
            RECT windowRect = new RECT();
            if (GetWindowRect(windowHandle, ref windowRect))
            {
                int windowPosX = windowRect.Left;
                int windowPosY = windowRect.Top;
                // UnityEngine.Debug.Log($"Window Position: X = {windowPosX}, Y = {windowPosY}");
                return new Vector2(windowPosX, windowPosY);
            }

        }
        else
        {
            UnityEngine.Debug.LogError("Window not found.");

        }
        return new Vector2(0, 0);
    }

    public void 读取表格并设置程序参数()
    {

        string ST_是否全屏;
        int Int_分辨率宽;
        int Int_分辨率高;
        int Int_窗口位置X;
        int Int_窗口位置Y;
        ST_是否全屏 = ST_读取表格单格信息("/程序配置表", 2, 2).ToString();
        if (ST_是否全屏 != "全屏")
        {
            Int_分辨率宽 = int.Parse(ST_读取表格单格信息("/程序配置表", 3, 2));
            Int_分辨率高 = int.Parse(ST_读取表格单格信息("/程序配置表", 4, 2));
            Int_窗口位置X = int.Parse(ST_读取表格单格信息("/程序配置表", 5, 2));
            Int_窗口位置Y = int.Parse(ST_读取表格单格信息("/程序配置表", 6, 2));
            设置程序窗口化无边框并设置位置和宽高("", Int_窗口位置X, Int_窗口位置Y, Int_分辨率宽, Int_分辨率高);
        }
        else
        {
            Screen.fullScreen = true;
        }

        if (ST_读取表格单格信息("/程序配置表", 7, 2) == "是")
        {
            将程序始终置于最前面("");
        }
    }
    #endregion


    #region 取消启动画面

    //取消启动画面
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void 取消Unity启动画面()
    {
#if UNITY_WEBGL
        Application.focusChanged += Application_focusChanged;
#else
        System.Threading.Tasks.Task.Run(BeforeSplashScreen);
#endif
    }

#if UNITY_WEBGL
    private static void Application_focusChanged(bool obj)
    {
        Application.focusChanged -= Application_focusChanged;
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
#else

    public static void BeforeSplashScreen()
    {
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
#endif
    #endregion


    #endregion


}



#region 帧数
public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private int frameCount = 0;
    private float fps = 0.0f;
    private float msec = 0.0f;
    private float updateInterval = 0.5f; // 刷新间隔为0.5秒
    private float lastUpdateTime = 0.0f;
    public bool Is_显示FPS;


    void Update()
    {
        if (Is_显示FPS)
        {
            // 每帧累加时间和帧数
            deltaTime += Time.unscaledDeltaTime;
            frameCount++;

            // 每0.5秒刷新一次FPS显示
            if (Time.unscaledTime > lastUpdateTime + updateInterval)
            {
                // 计算在刷新间隔内的平均帧率和延迟
                fps = frameCount / deltaTime;
                msec = (deltaTime / frameCount) * 1000.0f;

                // 重置累计的时间和帧数
                deltaTime = 0.0f;
                frameCount = 0;
                lastUpdateTime = Time.unscaledTime;
            }
        }
    }

    void OnGUI()
    {
        if (Is_显示FPS)
        {
            // 设置字体样式
            GUIStyle style = new GUIStyle();
            style.fontSize = 30;
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.white;

            // 绘制黑色背景框
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.black;
            GUI.Box(new Rect(10, 10, 250, 45), GUIContent.none);
            GUI.backgroundColor = originalColor;

            // 显示FPS信息
            string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, msec);
            GUI.Label(new Rect(15, 15, 220, 80), text, style);
        }
    }
}
#endregion


#region  自动运行初始场景
//自动运行初始场景  初始场景是buildsetting中的第一个场景

#if UNITY_EDITOR
[InitializeOnLoad]

public static class RedAutoRunSomeScene
{
    public static string filePath = "Red/Setting/AutoLoadSceneName.txt";
    public static string StartSceneName = "Scene_Main";


    static RedAutoRunSomeScene()
    {
        EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;
    }


    private static void OnPlayerModeStateChanged(PlayModeStateChange playModeState)
    {
        if (playModeState != PlayModeStateChange.ExitingEditMode)
        {
            return;
        }
        var currentStartScene = EditorSceneManager.GetActiveScene();
        if (currentStartScene.name == StartSceneName)
        {
            var targetScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
            EditorSceneManager.playModeStartScene = targetScene;
        }
        else
        {
            var targetScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentStartScene.path);
            EditorSceneManager.playModeStartScene = targetScene;
        }
    }


    static bool ValidatePlayModeUseFirstScene()
    {
        Menu.SetChecked("BuildTools/PlayModeUseFirstScene", EditorSceneManager.playModeStartScene != null);
        return !EditorApplication.isPlaying;
    }

    static void UpdatePlayModeUseFirstScene()
    {
        EditorApplication.playModeStateChanged += null;
        if (Menu.GetChecked("BuildTools/PlayModeUseFirstScene"))
        {
            EditorSceneManager.playModeStartScene = null;
        }
        else
        {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
            EditorSceneManager.playModeStartScene = scene;
        }
    }

    static void LoadSceneName()
    {

        string directoryPath = System.IO.Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            // StartSceneName = content;
        }
        else
        {
            string defaultContent = "SceneName";
            File.WriteAllText(filePath, defaultContent);
            StartSceneName = "SceneName";
        }
    }
}
#endif

#endregion


#region  变量折叠

//示例 
//#if UNITY_EDITOR
//[FoldoutGroup("其他")]
//#endif
#if UNITY_EDITOR
public class FoldoutGroupAttribute : PropertyAttribute
{
    public string GroupName { get; }
    public float ButtonWidth { get; }
    public float ButtonHeight { get; }
    public Color ButtonColor { get; }

    public FoldoutGroupAttribute(
        string groupName,
        float buttonWidth = 600f,
        float buttonHeight = 25f,
        float r = 0.2f,
        float g = 0.6f,
        float b = 0.8f)
    {
        GroupName = groupName;
        ButtonWidth = buttonWidth;
        ButtonHeight = buttonHeight;
        ButtonColor = new Color(r, g, b);
    }
}


[CustomEditor(typeof(MonoBehaviour), true)]
public class FoldoutGroupEditor : Editor
{
    private static Dictionary<string, bool> foldoutStates = new Dictionary<string, bool>(); // 保存折叠状态

    public override void OnInspectorGUI()
    {
        // 强制绘制脚本字段，让它能够双击打开脚本
        DrawScriptField();

        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true); // 跳过脚本字段

        string currentGroup = null; // 当前分组名
        bool isGroupOpen = true;    // 当前分组是否展开

        while (property.NextVisible(false))
        {
            var foldoutGroup = GetFoldoutGroupAttribute(property);

            if (foldoutGroup != null)
            {
                // 如果是新分组，绘制折叠按钮
                if (foldoutGroup.GroupName != currentGroup)
                {
                    currentGroup = foldoutGroup.GroupName;

                    // 获取分组状态的唯一标识
                    string groupKey = $"{serializedObject.targetObject.GetInstanceID()}.{currentGroup}";

                    // 初始化折叠状态
                    if (!foldoutStates.ContainsKey(groupKey))
                    {
                        foldoutStates[groupKey] = true; // 默认展开
                    }

                    // 绘制自定义折叠按钮
                    isGroupOpen = foldoutStates[groupKey] = DrawCustomFoldoutButton(foldoutGroup, foldoutStates[groupKey]);
                }
            }

            // 如果当前分组未展开，跳过其内容
            if (!isGroupOpen)
            {
                continue;
            }

            // 绘制字段
            EditorGUILayout.PropertyField(property, true);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        MonoScript script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);
        EditorGUI.BeginDisabledGroup(true); // 禁用编辑脚本字段
        EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();
    }

    private bool DrawCustomFoldoutButton(FoldoutGroupAttribute foldoutGroup, bool isOpen)
    {
        // 添加按钮上方的间隔
        GUILayout.Space(15);

        // 按钮样式
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        // 按钮区域
        Rect buttonRect = EditorGUILayout.GetControlRect(false, foldoutGroup.ButtonHeight);

        // 居中按钮位置
        buttonRect.width = foldoutGroup.ButtonWidth;
        buttonRect.x = (EditorGUIUtility.currentViewWidth - foldoutGroup.ButtonWidth) / 2;

        // 根据状态切换按钮颜色
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = isOpen ? Color.green : Color.gray; // 展开为绿色，折叠为红色

        // 根据状态切换按钮内容
        string buttonText = foldoutGroup.GroupName;
        Texture2D icon = (Texture2D)EditorGUIUtility.IconContent(isOpen ? "d_Toolbar Minus" : "d_Toolbar Plus").image;
        GUIContent buttonContent = new GUIContent($"  {buttonText}", icon);

        // 绘制按钮并返回切换的折叠状态
        bool toggled = GUI.Button(buttonRect, buttonContent, buttonStyle) ? !isOpen : isOpen;

        // 恢复原始背景颜色
        GUI.backgroundColor = originalColor;

        // 添加按钮下方的间隔
        GUILayout.Space(15);
        return toggled;
    }

    private FoldoutGroupAttribute GetFoldoutGroupAttribute(SerializedProperty property)
    {
        // 检查字段是否具有 FoldoutGroupAttribute
        var targetObject = serializedObject.targetObject;
        var targetType = targetObject.GetType();
        var field = targetType.GetField(property.name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            return (FoldoutGroupAttribute)System.Attribute.GetCustomAttribute(field, typeof(FoldoutGroupAttribute));
        }
        return null;
    }
}

#endif

#endregion


#region 赋值框未赋值变红色
//示例[HighlightIfNull] public GameObject targetObject;
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


#region  重命名小工具

#if UNITY_EDITOR
public enum NumberedMethod
{
    BySelection = 0,
    ByHierarchy = 1
}
[Serializable]
public class TurboRename : EditorWindow
{
    UnityEngine.Object[] SelectedObjects = new UnityEngine.Object[0];
    GameObject[] SelectedGameObjectObjects = new GameObject[0];
    string[] PreviewSelectedObjects = new string[0];

    bool usebasename = true;
    string basename;
    bool useprefix;
    string prefix;
    bool usesuffix;
    string suffix;

    public NumberedMethod numbermeth;
    bool usenumbered = true;
    int basenumbered = 1;
    int stepnumbered = 1;

    bool usereplace;
    string replace;
    string replacewith;

    bool useremove;
    string remove;

    bool showselection;
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Tools/重命名小工具")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow(typeof(TurboRename));
        window.minSize = new Vector2(512, 128);
    }

    #region GUI
    void OnGUI()
    {

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Turbo Rename", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        usebasename = EditorGUILayout.Toggle(usebasename, GUILayout.MaxWidth(16));
        basename = EditorGUILayout.TextField("基础名称: ", basename);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        useprefix = EditorGUILayout.Toggle(useprefix, GUILayout.MaxWidth(16));
        prefix = EditorGUILayout.TextField("前缀: ", prefix);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        usesuffix = EditorGUILayout.Toggle(usesuffix, GUILayout.MaxWidth(16));
        suffix = EditorGUILayout.TextField("后缀: ", suffix);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        usenumbered = EditorGUILayout.Toggle(usenumbered, GUILayout.MaxWidth(16));
        EditorGUILayout.PrefixLabel("编号: ");
        EditorGUILayout.BeginVertical();
        basenumbered = EditorGUILayout.IntField("起始编号: ", basenumbered);
        stepnumbered = EditorGUILayout.IntField("步长: ", stepnumbered);
        numbermeth = (NumberedMethod)EditorGUILayout.EnumPopup(new GUIContent("编号方法", "按选择顺序编号，或按层次结构位置编号。注意：项目文件不能使用层次结构方法重命名，因为它们不在场景中。"), numbermeth);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        usereplace = EditorGUILayout.Toggle(usereplace, GUILayout.MaxWidth(16));
        EditorGUILayout.PrefixLabel("替换内容: ");
        EditorGUILayout.BeginVertical();
        replace = EditorGUILayout.TextField("替换: ", replace);
        replacewith = EditorGUILayout.TextField("替换为: ", replacewith);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        useremove = EditorGUILayout.Toggle(useremove, GUILayout.MaxWidth(16));
        remove = EditorGUILayout.TextField("移除所有: ", remove);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        // 重命名
        if (GUILayout.Button(new GUIContent("重命名", "使用当前设置重命名选定对象。"))) { Rename(); }
        EditorGUILayout.EndVertical();

        if (SelectedObjects.Length > 0)
        {
            showselection = EditorGUILayout.Foldout(showselection, "选定对象和预览");
            if (showselection)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("选定对象", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                for (int i = 0; i < SelectedObjects.Length; i++)
                {
                    EditorGUILayout.LabelField(SelectedObjects[i].name);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("预览", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                for (int i = 0; i < SelectedObjects.Length; i++)
                {
                    EditorGUILayout.LabelField(PreviewSelectedObjects[i]);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
            }
        }
        if (GUILayout.Button(new GUIContent("清除设置", "使用当前设置重命名选定对象。"))) { ClearSettings(); }

    }
    #endregion

    #region Functions
    private void Update()
    {
        SelectedObjects = Selection.objects;

        SelectedGameObjectObjects = Selection.gameObjects;

        PreviewSelectedObjects = new string[SelectedObjects.Length];

        for (int i = 0; i < SelectedObjects.Length; i++)
        {
            string str = SelectedObjects[i].name;
            if (usebasename) { str = basename; }
            if (useprefix) { str = prefix + str; }
            if (usesuffix) { str = str + suffix; }

            if (usenumbered && numbermeth == NumberedMethod.BySelection) { str = str + ((basenumbered + (stepnumbered * i)).ToString()); }

            if (useremove && remove != "") { str = str.Replace(remove, ""); }
            if (usereplace && replace != "") { str = str.Replace(replace, replacewith); }

            if (usenumbered && numbermeth == NumberedMethod.ByHierarchy)
            {
                for (int z = 0; z < SelectedGameObjectObjects.Length; z++)
                {
                    if ((UnityEngine.Object)SelectedGameObjectObjects[z] == (UnityEngine.Object)SelectedObjects[i])
                    {
                        str = str + ((basenumbered + (stepnumbered * SelectedGameObjectObjects[z].transform.GetSiblingIndex())).ToString());
                    }
                }
            }

            PreviewSelectedObjects[i] = str;
        }

    }

    void Rename()
    {

        for (int i = 0; i < SelectedObjects.Length; i++)
        {
            Undo.RecordObject(SelectedObjects[i], "Rename");
            if (usebasename) { SelectedObjects[i].name = basename; }
            if (useprefix) { SelectedObjects[i].name = prefix + SelectedObjects[i].name; }
            if (usesuffix) { SelectedObjects[i].name = SelectedObjects[i].name + suffix; }

            if (usenumbered && numbermeth == NumberedMethod.BySelection) { SelectedObjects[i].name = SelectedObjects[i].name + ((basenumbered + (stepnumbered * i)).ToString()); }

            if (useremove && remove != "") { SelectedObjects[i].name = SelectedObjects[i].name.Replace(remove, ""); }
            if (usereplace && replace != "") { SelectedObjects[i].name = SelectedObjects[i].name.Replace(replace, replacewith); }

            if (AssetDatabase.GetAssetPath(SelectedObjects[i]) != null)
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(SelectedObjects[i]), SelectedObjects[i].name);
            }

        }

        for (int i = 0; i < SelectedGameObjectObjects.Length; i++)
        {
            if (usenumbered && numbermeth == NumberedMethod.ByHierarchy) { SelectedGameObjectObjects[i].name = SelectedGameObjectObjects[i].name + ((basenumbered + (stepnumbered * SelectedGameObjectObjects[i].transform.GetSiblingIndex())).ToString()); }

        }
    }

    void ClearSettings()
    {
        usebasename = false;
        basename = "";
        useprefix = false;
        prefix = "";
        usesuffix = false;
        suffix = "";
        usenumbered = false;
        basenumbered = 0;
        stepnumbered = 1;

        usereplace = false;
        replace = "";
        replacewith = "";

        useremove = false;
        remove = "";

    }
    #endregion
}
#endif

#endregion


#region 显示模型面数

#if UNITY_EDITOR


[InitializeOnLoad]
public class MeshInfoEditor
{
    private const string ToggleKey = "ShowMeshInfoToggle"; // 用于保存 Toggle 状态的键

    // 静态构造函数
    static MeshInfoEditor()
    {
        // 根据保存的状态决定是否启用功能
        if (EditorPrefs.GetBool(ToggleKey, false))
        {
            Selection.selectionChanged += OnSelectionChanged;
        }
    }

    // 添加菜单项
    [MenuItem("Tools/输出模型面数")]
    private static void ToggleShowMeshInfo()
    {
        // 读取当前状态
        bool currentState = EditorPrefs.GetBool(ToggleKey, false);

        // 切换状态
        bool newState = !currentState;

        // 保存状态
        EditorPrefs.SetBool(ToggleKey, newState);

        // 注册或移除事件
        if (newState)
        {
            Selection.selectionChanged += OnSelectionChanged;
            UnityEngine.Debug.Log("输出模型面数功能开启");
        }
        else
        {
            Selection.selectionChanged -= OnSelectionChanged;
            UnityEngine.Debug.Log("输出模型面数功能关闭");
        }

        // 更新菜单项状态
        Menu.SetChecked("Tools/输出模型面数", newState);
    }

    // 初始化菜单项状态
    [MenuItem("Tools/输出模型面数", true)]
    private static bool ValidateToggleShowMeshInfo()
    {
        Menu.SetChecked("Tools/输出模型面数", EditorPrefs.GetBool(ToggleKey, false));
        return true;
    }

    private static void OnSelectionChanged()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            // 获取物体的网格信息
            MeshInfo meshInfo = GetMeshInfo(selectedObject);

            // 打印该物体的网格信息
            UnityEngine.Debug.Log($" {selectedObject.name} \n                Tris三角 : {FormatNumber(meshInfo.TriangleCount)}   Verts顶点 : {FormatNumber(meshInfo.VertexCount)}");
        }
    }

    // 格式化数字，超过一万时显示为 "万" 的形式
    private static string FormatNumber(int number)
    {
        return number >= 10000 ? $"{number / 10000f:F1}万" : number.ToString();
    }

    // 获取物体的网格信息
    private static MeshInfo GetMeshInfo(GameObject selectedObject)
    {
        int totalVertices = 0;
        int totalTriangles = 0;

        // 获取该物体及其子物体上的 MeshFilter 信息
        foreach (MeshFilter meshFilter in selectedObject.GetComponentsInChildren<MeshFilter>())
        {
            if (meshFilter.sharedMesh != null)
            {
                totalVertices += meshFilter.sharedMesh.vertexCount;
                totalTriangles += meshFilter.sharedMesh.triangles.Length / 3;
            }
        }

        return new MeshInfo
        {
            VertexCount = totalVertices,
            TriangleCount = totalTriangles
        };
    }

    // 用于存储网格信息的结构体
    private struct MeshInfo
    {
        public int VertexCount;
        public int TriangleCount;
    }
}

#endif


#endregion



#region 切换场景后自动展开Hierarchy根目录
#if UNITY_EDITOR

[InitializeOnLoad]
public static class HierarchyAutoExpand
{

    static HierarchyAutoExpand()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 延迟一帧执行，确保场景加载完成
        EditorApplication.delayCall += 展开主场景的对象;

    }

    static void 展开主场景的对象()
    {

        // 获取当前场景的所有根对象
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var rootObject in rootObjects)
        {
            // 选择对象
            //Debug.Log("rootObject.name: " + rootObject.name);
            Selection.activeGameObject = rootObject;
        }

        EditorApplication.delayCall += 展开DontDestroyOnLoad场景的对象;


    }

    static void 展开DontDestroyOnLoad场景的对象()
    {
        if (SceneManager.GetActiveScene().name == "Scene_Main")
        {
            var allGameObjects = new List<GameObject>();
            var DontDestroyOnLoadGameObjects = new List<GameObject>();
            allGameObjects.AddRange(Resources.FindObjectsOfTypeAll<GameObject>());


            foreach (var obj in allGameObjects)
            {
                if (obj.scene.name == "DontDestroyOnLoad")
                {
                    if (obj.transform.parent == null)
                    {
                        DontDestroyOnLoadGameObjects.Add(obj);
                        // Debug.Log(obj.name);
                        Selection.activeGameObject = obj;

                    }

                }


            }
        }


    }




}

#endif
#endregion
