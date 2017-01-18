using System.Net.Http;
using System.Threading.Tasks;

namespace Apix.Http.Client
{
    public interface IHttpClient
    {
        /// <summary>Make HTTP request with no result</summary>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="method">Method call type</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns></returns>
        Task RequestAsync(string methodCallString = null, object context = null, HttpMethod method = null, RequestParameters requestParameters = null);

        /// <summary>Make HTTP request with custom result</summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="method">Method call type</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task<T> RequestAsync<T>(string methodCallString = null, object context = null, HttpMethod method = null, RequestParameters<T> requestParameters = null);

        /// <summary>Make GET request</summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task<T> GetAsync<T>(string methodCallString = null, object context = null, RequestParameters<T> requestParameters = null);

        /// <summary>Make GET request</summary>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task GetAsync(string methodCallString = null, object context = null, RequestParameters requestParameters = null);


        /// <summary>Make POST request</summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task<T> PostAsync<T>(string methodCallString = null, object context = null, RequestParameters<T> requestParameters = null);

        /// <summary>Make POST request</summary>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task PostAsync(string methodCallString = null, object context = null, RequestParameters requestParameters = null);

        /// <summary>Make PUT request</summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task<T> PutAsync<T>(string methodCallString = null, object context = null, RequestParameters<T> requestParameters = null);

        /// <summary>Make PUT request</summary>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task PutAsync(string methodCallString = null, object context = null, RequestParameters requestParameters = null);

        /// <summary>Make PATCH request</summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task<T> PatchAsync<T>(string methodCallString = null, object context = null, RequestParameters<T> requestParameters = null);


        /// <summary>Make DELETE request</summary>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task DeleteAsync(string methodCallString = null, object context = null, RequestParameters requestParameters = null);

        /// <summary>Make DELETE request</summary>
        /// <typeparam name="T">Result object type</typeparam>
        /// <param name="methodCallString">WebAPI method name or template</param>
        /// <param name="context">Request context</param>
        /// <param name="requestParameters">The request parameters.</param>
        /// <returns>Result object</returns>
        Task<T> DeleteAsync<T>(string methodCallString = null, object context = null, RequestParameters<T> requestParameters = null);

    }
}
