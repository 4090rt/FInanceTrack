using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public class PoolObjectHTTP
    {
        private readonly Stack<HttpClient> _pool = new Stack<HttpClient>();

        public HttpClient Connect()
        { 
            lock (_pool)
            {
                if (_pool.Count > 0)
                { 
                    return _pool.Pop();
                }
            }
            return new HttpClient();
        }

        public void CloseConnect(HttpClient client)
        {
            //client.DefaultRequestHeaders.Clear();
            //client.CancelPendingRequests();
            lock (_pool)
            {
                if (_pool.Count < 10)
                {
                    _pool.Push(client);
                }
                else 
                {
                    client.Dispose();
                }
            }
        }
    }

    public class  PoolObjectsJson1
    {
        private readonly Stack<JsonDocument> _pool = new Stack<JsonDocument>();

        public JsonDocument Usings(string json)
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                {
                    return _pool.Pop();
                }
            }
            return JsonDocument.Parse(json);
        }

        public void CloseUsings(JsonDocument document)
        {
            lock (_pool)
            {
                document.Dispose();
            }
        }
    }

    public class PoolObjectsJson2
    {
        public readonly Stack<JsonDocument> _pools = new Stack<JsonDocument>();

        public JsonDocument Using(string json)
        {
            lock (_pools)
            {
                if (_pools.Count > 0)
                {
                    return _pools.Pop();
                }
            }
            return JsonDocument.Parse(json);
        }

        public void UsingClose(JsonDocument document)
        {
            lock (_pools)
            { 
                document.Dispose();
            }
        }
    }
    public class PoolObjectsJson3
    {
        public readonly Stack<JsonDocument> _pools = new Stack<JsonDocument>();

        public JsonDocument Using(string json)
        {
            lock (_pools)
            {
                if (_pools.Count > 0)
                {
                    return _pools.Pop();
                }
            }
            return JsonDocument.Parse(json);
        }

        public void UsingClose(JsonDocument document)
        {
            lock (_pools)
            {
                document.Dispose();
            }
        }
    }
}
