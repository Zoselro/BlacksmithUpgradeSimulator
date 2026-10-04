using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Resources/Intro/IntroScript.csv 를 읽어 IntroLine 목록으로 변환
public static class IntroScriptReader
{
    private const string PATH = "Intro/IntroScript"; // Resources 기준, 확장자 제외

    public static List<IntroLine> Load()
    {
        List<IntroLine> result = new List<IntroLine>();
        TextAsset csv = Resources.Load<TextAsset>(PATH);
        if (csv == null)
        {
            Debug.LogError($"[Intro] CSV를 찾을 수 없습니다: Resources/{PATH}.csv");
            return result;
        }

        string[] rows = csv.text.TrimStart('﻿').Split('\n');
        for (int i = 1; i < rows.Length; i++) // 0행은 헤더
        {
            string row = rows[i].TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(row))
                continue;

            List<string> cols = SplitCsvRow(row);
            if (cols.Count < 2 || !int.TryParse(cols[0].Trim(), out int order))
            {
                Debug.LogWarning($"[Intro] {i + 1}행 형식 오류 -> 건너뜀: {row}");
                continue;
            }

            result.Add(new IntroLine
            {
                Order = order,
                Image = cols[1].Trim(),
                Speaker = cols.Count > 2 ? cols[2].Trim() : "",
                Text = cols.Count > 3 ? cols[3] : "",
            });
        }

        result.Sort((a, b) => a.Order.CompareTo(b.Order));
        return result;
    }

    // 큰따옴표로 감싼 필드("a, b")와 이스케이프("")를 지원하는 한 줄 파서
    private static List<string> SplitCsvRow(string row)
    {
        List<string> cols = new List<string>();
        StringBuilder sb = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < row.Length; i++)
        {
            char c = row[i];
            if (inQuotes)
            {
                if (c == '"' && i + 1 < row.Length && row[i + 1] == '"') { sb.Append('"'); i++; }
                else if (c == '"') inQuotes = false;
                else sb.Append(c);
            }
            else if (c == '"') inQuotes = true;
            else if (c == ',') { cols.Add(sb.ToString()); sb.Clear(); }
            else sb.Append(c);
        }
        cols.Add(sb.ToString());
        return cols;
    }
}
