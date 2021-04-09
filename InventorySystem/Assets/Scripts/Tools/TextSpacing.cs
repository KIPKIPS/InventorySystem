using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/TextSpacing")]
public class TextSpacing : BaseMeshEffect
{
    public int Spacing = 0;
    private static HashSet<int> inRichTextTokenSet = new HashSet<int>();
    private static string tokenPattern = @"<b>|</b>|<i>|</i>|<size.*?>|</size>|<color.*?>|</color>|<material.*?>|</material>";


    public enum HorizontalAligmentType
    {
        Left,
        Center,
        Right
    }

    public class LineInfo
    {
        public List<char> charList = new List<char>();
        public List<int> charIndexList = new List<int>();
        public List<CharacterInfo> charinfoList = new List<CharacterInfo>();
        public int width;
    }


    public int GetLineHeight(List<UIVertex> vertexs, Font font, Text uitext, List<LineInfo> lineInfoList, out int yBase)
    {
        int size = uitext.fontSize;
        string text = uitext.text;
        float lineSpacing = uitext.lineSpacing;
        int lineHeight = (int)(size * lineSpacing);
        yBase = 0;

        if (lineInfoList.Count == 1)
        {
            LineInfo line0 = lineInfoList[0];
            char ch = line0.charList[0];
            CharacterInfo charInfo0 = line0.charinfoList[0];
            int index0 = line0.charIndexList[0] * 6 + 2;
            UIVertex v0 = vertexs[index0];
            int line0y = (int)v0.position.y - charInfo0.minY;
            yBase = line0y;
        }
        else if(lineInfoList.Count > 1)
        {
            LineInfo line0 = lineInfoList[0];
            LineInfo line1 = lineInfoList[1];

            char ch = line0.charList[0];
            CharacterInfo charInfo0 = line0.charinfoList[0];
            int index0 = line0.charIndexList[0] * 6 + 2;
            UIVertex v0 = vertexs[index0];
            int line0y = (int)v0.position.y - charInfo0.minY;
            yBase = line0y;

            char ch1 = line1.charList[0];
            CharacterInfo charInfo1 = line1.charinfoList[0];
            int index1 = line1.charIndexList[0] * 6 + 2;
            UIVertex v1 = vertexs[index1];
            int line1y = (int)v1.position.y - charInfo1.minY;

            if(Mathf.Abs(line1y - line0y) > size)
            {
                lineHeight = Mathf.Abs(line1y - line0y);
            }
        }
        return lineHeight;
    }

    private HorizontalAligmentType GetAlignment(Text text)
    {
        HorizontalAligmentType alignment;
        if (text.alignment == TextAnchor.LowerLeft || text.alignment == TextAnchor.MiddleLeft || text.alignment == TextAnchor.UpperLeft)
        {
            alignment = HorizontalAligmentType.Left;
        }
        else if (text.alignment == TextAnchor.LowerCenter || text.alignment == TextAnchor.MiddleCenter || text.alignment == TextAnchor.UpperCenter)
        {
            alignment = HorizontalAligmentType.Center;
        }
        else
        {
            alignment = HorizontalAligmentType.Right;
        }
        return alignment;
    }


    private bool CheckRichTextTokenSet(string sText)
    {
        int count1 = 0, count2 = 0, count3 = 0, count4 = 0, count5 = 0;
        inRichTextTokenSet.Clear();
        foreach (Match match in Regex.Matches(sText, tokenPattern))
        {
            for (int j = 0; j < match.Length; j++)
            {
                inRichTextTokenSet.Add(match.Index + j);
            }

            string s = match.ToString();
            if (s.StartsWith("<b")) count1 += 1;
            else if (s.StartsWith("</b")) count1 -= 1;
            else if (s.StartsWith("<i")) count2 += 1;
            else if (s.StartsWith("</i")) count2 -= 1;
            else if (s.StartsWith("<size")) count3 += 1;
            else if (s.StartsWith("</size")) count3 -= 1;
            else if (s.StartsWith("<color")) count4 += 1;
            else if (s.StartsWith("</color")) count4 -= 1;
            else if (s.StartsWith("<material")) count5 += 1;
            else if (s.StartsWith("</material")) count5 -= 1;
        }
        return count1 == 0 && count2 == 0 && count3 == 0 && count4 == 0 && count5 == 0; 
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        
        if (!IsActive() || vh.currentVertCount == 0)
        {
            return;
        }

        var text = GetComponent<Text>();

        if (text == null)
        {
            Debug.LogError("Missing Text component");
            return;
        }
    
        var vertexs = new List<UIVertex>();
        vh.GetUIVertexStream(vertexs);

        //Debug.Log("vertexs " + vertexs.Count);
        HorizontalAligmentType alignment = GetAlignment(text);
        Font myFont = text.font;
        string sText = text.text;
        myFont.RequestCharactersInTexture(sText, text.fontSize, text.fontStyle);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;

        List<LineInfo> lineInfoList = new List<LineInfo>();

        LineInfo lineInfo = new LineInfo();
        lineInfoList.Add(lineInfo);

        HorizontalWrapMode horizontalOverflow = text.horizontalOverflow;
        VerticalWrapMode verticalOverflow = text.verticalOverflow;

        bool isRichTextSucess = false;
        if(text.supportRichText)
        {
            isRichTextSucess = CheckRichTextTokenSet(sText);
        }

        //重新计算行数,\n或者超过宽度换行
        int count = vertexs.Count / 6;
        for (int i = 0; i < count; i++)
        {
            if(isRichTextSucess && inRichTextTokenSet.Contains(i))
            {
                continue;
            }

            char ch = sText[i];
            CharacterInfo charInfo;
            bool haschar = myFont.GetCharacterInfo(ch, out charInfo, text.fontSize, text.fontStyle);

            if ('\n' == ch)
            {
                lineInfo.charinfoList.Add(charInfo);
                lineInfo.charList.Add(ch);
                lineInfo.charIndexList.Add(i);
                lineInfo = new LineInfo();
                lineInfoList.Add(lineInfo);
            }

            if (haschar)
            {
                if(horizontalOverflow == HorizontalWrapMode.Wrap && lineInfo.width + charInfo.advance  > rect.width)
                {
                    lineInfo = new LineInfo();
                    lineInfoList.Add(lineInfo);
                }
                lineInfo.charinfoList.Add(charInfo);
                lineInfo.charList.Add(ch);
                lineInfo.charIndexList.Add(i);
                lineInfo.width += (int)(charInfo.advance) + Spacing;
            }
        }

        int yBase = 0;
        int lineheight = GetLineHeight(vertexs, myFont, text, lineInfoList, out yBase);

        //按行重新排版
        int xMin = Mathf.FloorToInt(rect.xMin);
        int x = xMin;
        int y = yBase + lineheight;
        for (int i = 0; i < lineInfoList.Count; i++)
        {
            lineInfo = lineInfoList[i];
            x = xMin;
            y = y - lineheight;

            bool isVerticalCut = false;
            if(verticalOverflow == VerticalWrapMode.Truncate && y < rect.yMin)
            {
                isVerticalCut = true;
            }

            //对其方式，行偏移
            int lineOffset = 0;
            if (alignment == HorizontalAligmentType.Right)
            {
                lineOffset = (int)(rect.width  - lineInfo.width + Spacing);
            }
            else if (alignment == HorizontalAligmentType.Center)
            {
                lineOffset = (int)((rect.width  - lineInfo.width + Spacing) / 2);
            }


            for (int j = 0; j < lineInfo.charList.Count; j++)
            {
                char c = lineInfo.charList[j];
                CharacterInfo chinfo = lineInfo.charinfoList[j];
                
                for (int k = 0; k < 6; k++)
                {
                    int index = lineInfo.charIndexList[j] * 6 + k;
                    UIVertex vt = vertexs[index];

                    //两个三角形6个顶点信息偏移
                    int xx = x, yy = y;
                    if (k == 0 || k == 1 || k == 5)
                    {
                        yy += chinfo.glyphHeight;
                    }
                    if (k == 1 || k == 2 || k == 3)
                    {
                        xx += chinfo.glyphWidth;
                    }

                    if(isVerticalCut)
                    {
                        vt.position = new Vector3(0,0, 0);
                    }
                    else
                    {
                        vt.position = new Vector3(xx + chinfo.bearing + lineOffset, yy + chinfo.minY, 0);
                    }

                    //写入顶点数据，实际只要4个
                    vertexs[index] = vt;
                    if (index % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (index / 6) * 4 + index % 6);
                    }

                    if (index % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (index / 6) * 4 + index % 6 - 1);
                    }
                }

                x += (int)(chinfo.advance) + Spacing;
            }

        }
    }

}
