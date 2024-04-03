using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using TMPro;
using FlyRabbit.Text;
using System.Collections.Generic;
/* 
* 作者：天外飞兔 https://space.bilibili.com/16077272?spm_id_from=333.1007.0.0
*  使用须知：
*         文本物体的锚点在左下角，而且不可修改，如果要更改文字对齐方式请使用Alignment。
*         此功能依赖TextMeshPro，请事先导入相关资源包。
*         此功能依赖UGUI系统，其实TMP是有组件可以在世界内生成文本的，但是那样会有近大远小效果，所以我没用（后来还是用了，加了新功能）。
*/
namespace FlyRabbit.Text
{
    public class SpecialEffectText : MonoBehaviour
    {
        /// <summary>
        /// 要向哪个摄像机显示文字？
        /// 默认是Camera.Main。
        /// </summary>
        public Camera myCamera;

        #region Canvas设置
        private static RectTransform textCanvas;
        //屏幕适配比例，如果不对记得改。
        //一般手机是1080*1920，电脑是1920*1080。
        private static Vector2 screenSize = new Vector2(1920, 1080);
        //不建议改。
        private static RenderMode renderMode = RenderMode.ScreenSpaceOverlay;
        //同样不建议改
        private static CanvasScaler.ScaleMode scaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //sort order,影响多个canvas的渲染顺序，根据需求更改，这里设置999，在最上层渲染。
        private static int sortOrder = 999;
        /// <summary>
        /// 创建Text专用的Canvas
        /// </summary>
        private static void CreatTextCanvas()
        {
            GameObject canvasObject = new GameObject("textCanvas");
            textCanvas = canvasObject.AddComponent<RectTransform>();
            canvasObject.transform.SetParent(instance.transform);

            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = renderMode;
            canvas.sortingOrder = sortOrder;

            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = scaleMode;
            canvasScaler.referenceResolution = screenSize;
        }
        #endregion
        #region 单例
        private static volatile SpecialEffectText instance;
        private static object locker = new object();
        public static SpecialEffectText Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            Initialize();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        #region 对象池
        private static GameObject sample_UI;
        private static ObjectPool<TextMeshProUGUI> pool_UIText;

        private static GameObject sample_World;
        private static ObjectPool<TextMeshPro> pool_WorldText;
        /// <summary>
        /// 创建UIText样本
        /// </summary>
        private static void CreatSample_UI()
        {
            sample_UI = new GameObject("sample_UI");
            sample_UI.transform.SetParent(instance.transform);

            TextMeshProUGUI text = sample_UI.AddComponent<TextMeshProUGUI>();//会自动挂载RectTransform和canvas render
            text.enableWordWrapping = false;
            text.raycastTarget = false;
            text.verticalAlignment = VerticalAlignmentOptions.Baseline;

            RectTransform transform = sample_UI.GetComponent<RectTransform>();
            transform.localPosition = Vector3.zero;
            transform.sizeDelta = Vector2.zero;
            transform.anchorMax = Vector2.zero;
            transform.anchorMin = Vector2.zero;
            transform.pivot = Vector2.zero;

            sample_UI.SetActive(false);
        }
        /// <summary>
        /// 创建WorldText样本。
        /// </summary>
        private static void CreatSample_WorldText()
        {
            sample_World = new GameObject("sample_World");
            sample_World.transform.SetParent(instance.transform);

            TextMeshPro text = sample_World.AddComponent<TextMeshPro>();//会自动挂载RectTransform和MeshRenderer
            text.enableWordWrapping = false;
            text.raycastTarget = false;
            text.verticalAlignment = VerticalAlignmentOptions.Baseline;

            RectTransform transform = sample_UI.GetComponent<RectTransform>();
            transform.localPosition = Vector3.zero;
            transform.position = Vector3.zero;
            transform.sizeDelta = Vector2.zero;
            transform.anchorMax = Vector2.zero;
            transform.anchorMin = Vector2.zero;
            transform.pivot = Vector2.zero;
            sample_World.SetActive(false);
        }
        private static TextMeshProUGUI Creat_UIText()
        {
            GameObject gameObject = Instantiate(sample_UI, textCanvas, false);
            TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();

            gameObject.SetActive(true);
            return gameObject.GetComponent<TextMeshProUGUI>();
        }
        private static TextMeshPro Creat_WorldText()
        {
            GameObject gameObject = Instantiate(sample_World, instance.transform, false);
            TextMeshPro text = gameObject.GetComponent<TextMeshPro>();

            gameObject.SetActive(true);
            return gameObject.GetComponent<TextMeshPro>();
        }
        private static void Get_UIText(TextMeshProUGUI text)
        {
            text.gameObject.SetActive(true);
        }
        private static void Get_WorldText(TextMeshPro text)
        {
            text.gameObject.SetActive(true);
        }
        private static void Release_UIText(TextMeshProUGUI text)
        {
            text.gameObject.SetActive(false);
        }
        private static void Release_WorldText(TextMeshPro text)
        {
            text.gameObject.SetActive(false);
        }
        /// <summary>
        /// 初始化对象池
        /// </summary>
        private static void PoolInitialize()
        {
            pool_UIText = new ObjectPool<TextMeshProUGUI>(Creat_UIText, Get_UIText, Release_UIText);
            pool_WorldText = new ObjectPool<TextMeshPro>(Creat_WorldText, Get_WorldText, Release_WorldText);

        }
        #endregion
        #region private方法
        /// <summary>
        /// 初始化
        /// </summary>
        private static void Initialize()
        {


            GameObject @object = new GameObject("SpecialEffectText");
            DontDestroyOnLoad(@object);
            instance = @object.AddComponent<SpecialEffectText>();
            instance.myCamera = Camera.main;

            CreatTextCanvas();
            CreatSample_UI();
            CreatSample_WorldText();
            PoolInitialize();
        }


        private IEnumerator Run(Style style, TMP_Text text, Transform followTarget, Vector3 position)
        {
            bool isUIText = false;
            if (text is TextMeshProUGUI)
            {
                isUIText = true;
            }
            text.horizontalAlignment = style.Alignment;
            text.fontSize = style.sizeMin;
            text.color = style.color;

            float startTime = Time.time;

            Vector3 worldPosition;
            Vector3 offset = Vector3.zero;
            Vector3 newVelocity = new Vector3(Random.Range(style.velocityMin.x, style.velocityMax.x), Random.Range(style.velocityMin.y, style.velocityMax.y), Random.Range(style.velocityMin.z, style.velocityMax.z));

            float alphaLerp = 0;
            float alphaTimer = style.life - style.alphaTime;

            float sizeChangeLerp = 0;

            float timer = Time.time - startTime;
            while (timer < style.life)
            {
                if (followTarget != null)
                {
                    worldPosition = followTarget.position + position;
                }
                else
                {
                    worldPosition = position;
                }

                if (timer < style.animateTime)
                {
                    newVelocity += style.gravity * Time.deltaTime;
                    offset += newVelocity * Time.deltaTime;
                }

                if (timer > style.alphaTime)
                {
                    alphaLerp += 1 * Time.deltaTime / alphaTimer;
                    text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, alphaLerp));
                }

                if (timer < style.sizeScaleTime)
                {
                    sizeChangeLerp += 1 * Time.deltaTime / style.sizeScaleTime;
                    text.fontSize = Mathf.Lerp(style.sizeMin, style.sizeMax, sizeChangeLerp);
                }
                worldPosition += offset;
                if (isUIText)
                {
                    text.transform.position = myCamera.WorldToScreenPoint(worldPosition);
                }
                else
                {
                    text.transform.position = worldPosition;
                    text.transform.rotation = myCamera.transform.rotation;
                }
                yield return null;
                timer = Time.time - startTime;
            }
            if (isUIText)
            {
                pool_UIText.Release(text as TextMeshProUGUI);
            }
            else
            {
                pool_WorldText.Release(text as TextMeshPro);
            }

        }
        #endregion
        #region public方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="style">样式</param>
        /// <param name="content">要显示的文字内容</param>
        /// <param name="localPosition">坐标</param>
        /// <param name="followTarget">要跟随移动的目标，为null则不跟随</param>
        /// <param name="useWorldText">是否使用世界文字，使用世界文字的话在3d游戏中文字会有近大远小的效果</param>
        public void CreatFloatingText(Style style, string content, Vector3 localPosition, Transform followTarget = null, bool useWorldText = false)
        {
            TMP_Text text;
            if (useWorldText)
            {
                text = pool_WorldText.Get();
            }
            else
            {
                text = pool_UIText.Get();
            }
            text.text = content;
            StartCoroutine(Run(style, text, followTarget, localPosition));
        }
        #endregion

    }
    public class Style
    {
        /// <summary>
        /// 文字起始大小,最小为0，最大为9999
        /// </summary>
        public float sizeMin;
        /// <summary>
        /// 文字最终大小,最小为0，最大为9999
        /// </summary>
        public float sizeMax;
        /// <summary>
        /// 文字从最小变化到最大需要的时间
        /// </summary>
        public float sizeScaleTime;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color color;
        /// <summary>
        /// 对齐方式
        /// Left左对齐，Right右对齐，Center居中对齐（推荐使用Geometry代替,居的更中)，Flush文字挤在中间。
        /// </summary>
        public HorizontalAlignmentOptions Alignment;
        /// <summary>
        /// 存在时间
        /// </summary>
        public float life;
        /// <summary>
        /// 文字可以进行动画的时间，此值不应该超过存在时间，否则没作用。
        /// </summary>
        public float animateTime;
        /// <summary>
        /// 文字显示多久后开始逐渐消失，如果将此值设置与life为同值，则不会有逐渐消失效果，而是直接消失。
        /// </summary>
        public float alphaTime;
        /// <summary>
        /// 最小初始力量，系统会在最大和最小初始力量之间随机一个数值，设为该次文字显示的力量
        /// </summary>
        public Vector3 velocityMin;
        /// <summary>
        /// 最大初始力量，系统会在最大和最小初始力量之间随机一个数值，设为该次文字显示的力量
        /// </summary>
        public Vector3 velocityMax;
        /// <summary>
        /// 如果有文字受到重力影响的需求，请修改此项。
        /// </summary>
        public Vector3 gravity;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sizeMin">最小文字大小</param>
        /// <param name="sizeMax">最大文字大小</param>
        /// <param name="sizeScaleTime">文字缩放需要的时间</param>
        /// <param name="color">文字颜色</param>
        /// <param name="alignment">对齐方式</param>
        /// <param name="life">文字存在多久</param>
        /// <param name="animateTime">文字禁用动画效果的时间点</param>
        /// <param name="alphaTime">多久之后文字开始逐渐消失</param>
        /// <param name="velocityMin">最小初始力量</param>
        /// <param name="velocityMax">最大初始力量</param>
        /// <param name="gravity">承受的重力</param>
        public Style(float sizeMin, float sizeMax, float sizeScaleTime, Color color, HorizontalAlignmentOptions alignment, float life, float animateTime, float alphaTime, Vector3 velocityMin, Vector3 velocityMax, Vector3 gravity)
        {
            this.sizeMin = sizeMin;
            this.sizeMax = sizeMax;
            this.sizeScaleTime = sizeScaleTime;
            this.color = color;
            Alignment = alignment;
            this.life = life;
            this.animateTime = animateTime;
            this.alphaTime = alphaTime;
            this.velocityMin = velocityMin;
            this.velocityMax = velocityMax;
            this.gravity = gravity;
        }
    }
}
public class StyleDictionary
{
    public static Dictionary<StyleList, Style> styleDictionary;
    static StyleDictionary()
    {
        styleDictionary = new Dictionary<StyleList, Style>(10);
        styleDictionary.Add(StyleList.UI_Style1, new Style(20f, 46f, 0.2f, Color.yellow, HorizontalAlignmentOptions.Geometry, 0.5f, 0.07f, 0.2f, new Vector3(-10f, 10f, 0f), new Vector3(10f, 10f, 0f), Vector3.zero));
        styleDictionary.Add(StyleList.UI_Style1Crit, new Style(20f, 70f, 0.2f, Color.red, HorizontalAlignmentOptions.Geometry, 1.07f, 0.07f, 0.87f, new Vector3(-10f, 10f, 0f), new Vector3(10f, 10f, 0f), Vector3.zero));
        styleDictionary.Add(StyleList.UI_Style2, new Style(20f, 46f, 0.2f, Color.yellow, HorizontalAlignmentOptions.Geometry, 1f, 0.5f, 0.5f, new Vector3(-5f, 10f, 0f), new Vector3(5f, 10f, 0f), new Vector3(0f, -50f, 0f)));
        styleDictionary.Add(StyleList.UI_Style2Crit, new Style(20f, 70f, 0.2f, Color.red, HorizontalAlignmentOptions.Geometry, 1.5f, 0.5f, 1f, new Vector3(-5f, 10f, 0f), new Vector3(5f, 10f, 0f), new Vector3(0f, -50f, 0f)));
        styleDictionary.Add(StyleList.UI_Style3, new Style(46f, 46f, 0f, Color.yellow, HorizontalAlignmentOptions.Geometry, 2f, 2f, 0.5f, new Vector3(0f, 1.1f, 0f), new Vector3(0f, 1.1f, 0f), Vector3.zero));
        styleDictionary.Add(StyleList.UI_Style3Crit, new Style(70f, 70f, 0f, Color.red, HorizontalAlignmentOptions.Geometry, 2f, 2f, 0.5f, new Vector3(0f, 1.1f, 0f), new Vector3(0f, 1.1f, 0f), Vector3.zero));
        styleDictionary.Add(StyleList.World_Style1, new Style(1f, 5f, 0.3f, Color.green, HorizontalAlignmentOptions.Geometry, 0.6f, 0.07f, 0.3f, new Vector3(0, 10, 0), new Vector3(0, 10, 0), Vector3.zero));
    }
}
/// <summary>
/// 由于UI与World的Size机制不一样，所以必须分开创建多个style，同样size为50的文字，在UI系统下很小，但是在World系统下就非常大。
/// </summary>
public enum StyleList
{
    UI_Style1,
    UI_Style1Crit,
    UI_Style2,
    UI_Style2Crit,
    UI_Style3,
    UI_Style3Crit,
    World_Style1,
}
