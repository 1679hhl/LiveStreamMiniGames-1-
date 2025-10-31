using UnityEngine;
public class GMClass
{
    public static GUIStyle Lable = new GUIStyle()
    {
        fontSize = 20,
    };

}
/// <summary>
/// 打印FPS
/// </summary>
public class FPS : MonoBehaviour
{
    float _updateInterval = 1f;//设定更新帧率的时间间隔为1秒  
    float _accum = .0f;//累积时间  
    int _frames = 0;//在_updateInterval时间内运行了多少帧  
    float _timeLeft;
    string fpsFormat;

    void Start()
    {
        _timeLeft = _updateInterval;
        Application.targetFrameRate = 60;
    }
    
    void OnGUI()
    {
        GUI.Label(new Rect(20, Screen.height-50, 100, 100), fpsFormat,GMClass.Lable);
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        //Time.timeScale可以控制Update 和LateUpdate 的执行速度,  
        //Time.deltaTime是以秒计算，完成最后一帧的时间  
        //相除即可得到相应的一帧所用的时间  
        _accum += Time.timeScale * Time.deltaTime;
        ++_frames;//帧数  

        
        if (_timeLeft <= 0)
        {
            float fps =  _frames / _accum;
            fpsFormat = System.String.Format("FPS:{0:F2}",fps);//保留两位小数  
            _timeLeft = _updateInterval;
            _accum = .0f;
            _frames = 0;
        }
    }
}