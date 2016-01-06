using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeKicker.BBCode;

namespace NewsDotNet.WebUI.Infrastracture
{
    public static class ParserSingletone
    {
        private static BBCodeParser _parser;
        private static List<BBTag> _tags = new List<BBTag> {
                new BBTag("b", "<b>", "</b>"),
                new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
                new BBTag("img", "<img src=\"${content}\" />", "", false, true),
                new BBTag("quote", "<blockquote>", "</blockquote>"),
                new BBTag("list", "<ul>", "</ul>"),
                new BBTag("*", "<li>", "</li>", true, false),
                new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href"))
            };

        private static BBCodeParser Instance()
        {
            if (null == _parser)
                _parser = new BBCodeParser(ErrorMode.ErrorFree, null, _tags);
            return _parser;
        }

        public static string ToHtml(string bbCode)
        {
            string tmp = Instance().ToHtml(bbCode);
            Regex exprBrackets = new Regex(@"amp;(?<letter>[lg])t;");
            Regex exprOlOpen = new Regex(@"\[list=1\]");
            Regex exprOlClose = new Regex(@"\[/list\]");
            Regex exprNewLine = new Regex(@"\r\n");
            Regex exprColorOpen = new Regex(@"\[color=(\S)+\]");
            Regex exprColorClose = new Regex(@"\[/color\]");
            tmp = exprBrackets.Replace(tmp, @"${letter}t;");
            tmp = exprOlOpen.Replace(tmp, @"<ol>");
            tmp = exprOlClose.Replace(tmp, @"</ol>");
            tmp = exprNewLine.Replace(tmp, @"<br>");
            tmp = exprColorOpen.Replace(tmp, "");
            tmp = exprColorClose.Replace(tmp, "");
            return tmp;
        }
    }
}