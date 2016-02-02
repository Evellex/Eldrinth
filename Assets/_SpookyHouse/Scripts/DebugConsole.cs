using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class DebugConsole : MonoBehaviour {
    public static DebugConsole dc;
    Text textBox;
    void Awake()
    {
        dc = this;
        textBox = GetComponent<Text>();
    }
    void Start()
    {
        textBox.text = "";
    }
    public void AddLine(string line)
    {
        textBox.text += line + "\n";
    }
    public void AddLine(string line, string colour)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<color=");
        sb.Append(colour);
        sb.Append(">");
        sb.Append(line);
        sb.Append("</color>");
        textBox.text += sb.ToString();
    }
}
