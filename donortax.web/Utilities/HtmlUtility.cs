using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MarkdownSharp;

namespace donortax.web.Utilities
{
    public class HtmlUtility
    {
        private static Regex _tags = new Regex("<[^>]*(>|$)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private static Regex _whitelist = new Regex(@"^</?(b(lockquote)?|code|d(d|t|l|el)|em|h(1|2|3)|i|kbd|li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|^<(b|h)r\s?/?>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static Regex _whitelist_a = new Regex(@"^<a\shref=""(\#\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+)""(\stitle=""[^""<>]+"")?\s?>$|^</a>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static Regex _whitelist_img = new Regex(@"^<img\ssrc=""https?://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+""(\swidth=""\d{1,3}"")?(\sheight=""\d{1,3}"")?(\salt=""[^""<>]*"")?(\stitle=""[^""<>]*"")?\s?/?>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// sanitize any potentially dangerous tags from the provided raw HTML input using
        /// a whitelist based approach, leaving the "safe" HTML tags
        /// CODESNIPPET:4100A61A-1711-4366-B0B0-144D1179A937
        /// </summary>
        public static string Sanitize(string html)
        {
            if (String.IsNullOrEmpty(html)) return html;
            string tagname;
            Match tag;
            // match every HTML tag in the input
            MatchCollection tags = _tags.Matches(html);
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLowerInvariant();
                if (!(_whitelist.IsMatch(tagname) || _whitelist_a.IsMatch(tagname) || _whitelist_img.IsMatch(tagname)))
                {
                    html = html.Remove(tag.Index, tag.Length);
                    System.Diagnostics.Debug.WriteLine("tag sanitized: " + tagname);
                }
            }
            return html;
        }
    }

    /// <summary>
    /// Helper class for transforming Markdown.
    /// </summary>
    public static class MarkdownHelper
    {
        /// <summary>
        /// An instance of the Markdown class that performs the transformations.
        /// </summary>
        static Markdown markdownTransformer = new Markdown();

        /// <summary>
        /// Transforms a string of Markdown into HTML.
        /// </summary>
        /// <param name="helper">HtmlHelper - Not used, but required to make this an extension method.</param>
        /// <param name="text">The Markdown that should be transformed.</param>
        /// <returns>The HTML representation of the supplied Markdown.</returns>
        public static IHtmlString Markdown(this HtmlHelper helper, string text)
        {
            // Transform the supplied text (Markdown) into HTML.
            string html = markdownTransformer.Transform(text);

            // Wrap the html in an MvcHtmlString otherwise it'll be HtmlEncoded and displayed to the user as HTML :(
            html = HtmlUtility.Sanitize(html);
            return new MvcHtmlString(html);
        }
    }
}