using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace Apix.Http.Client
{
    /// <summary>
    /// Provides the class for a collection of URL parameters. 
    /// </summary>
    public class UrlParameterCollection : NameValueCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance if <see cref="UrlParameterCollection"/>.
        /// </summary>
        public UrlParameterCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance if <see cref="UrlParameterCollection"/>.
        /// </summary>
        public UrlParameterCollection(UrlParameterCollection col)
            : base(col)
        {
        }

        /// <summary>
        /// Initializes a new instance if <see cref="UrlParameterCollection"/> and adds one URL parameter.
        /// </summary>
        public UrlParameterCollection(String param, String value)
        {
            Add(param, value);
        }

        /// <summary>
        /// Initializes a new instance if <see cref="UrlParameterCollection"/> and adds 2 URL parameters.
        /// </summary>
        public UrlParameterCollection(String param1, String value1, String param2, String value2)
        {
            Add(param1, value1);
            Add(param2, value2);
        }

        #endregion

        #region Fields

        private string pageAnchor = string.Empty;

        #endregion

        #region Properties

        public string PageAnchor
        {
            set
            {
                pageAnchor = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts query string params from <see cref="UrlParameterCollection"/> to a string.
        /// </summary>
        /// <param name="trimFirstAmpersand">Trim first ampersand if true.</param>
        /// <returns>String</returns>
        public string ToQueryString(bool trimFirstAmpersand)
        {
            StringBuilder result = new StringBuilder();

            foreach (String key in Keys)
            {
                result.Append(String.Format("&{0}={1}", key, WebUtility.UrlEncode(this[key])));
            }

            if (pageAnchor.Length > 0)
            {
                result.Append(String.Format("#{0}", WebUtility.UrlEncode(pageAnchor)));
            }

            if (trimFirstAmpersand)
            {
                return result.ToString().Trim('&');
            }
            return result.ToString();
        }

        #endregion
    }
}
