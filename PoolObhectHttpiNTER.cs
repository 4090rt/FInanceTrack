using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinFormsApp4
{
    public class PoolObhectHttpiNTER
    {
        public readonly Stack<HttpClient> _pool = new Stack<HttpClient>();

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

        public void Close(HttpClient client)
        {
            //client.DefaultRequestHeaders.Clear();
            //client.CancelPendingRequests();
            lock (_pool)
            {
                if (_pool.Count < 10)
                {
                    _pool.Push(client);
                }
                client.Dispose();
            }
        }
    }


    public class PoolObhectJsonInter1
    {
        public readonly Stack<JsonDocument> _pool= new Stack<JsonDocument>();

        public JsonDocument Connect(string json)
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                { 
                    _pool.Pop();
                }
            }
            return JsonDocument.Parse(json);
        }

        public void Close(JsonDocument document)
        {
            lock (_pool)
            {
                document.Dispose();
            }
        }        
    }

    public class PoolObhectJsonInter2
    {
        public readonly Stack<JsonDocument> _pool = new Stack<JsonDocument>();

        public JsonDocument Connect(string json)
        {
            lock (_pool)
            {
                if (_pool.Count > 1)
                {
                    _pool.Pop();
                }
            }
            return JsonDocument.Parse(json);
        }

        public void close(JsonDocument document)
        {
            lock (_pool)
            { 
                document?.Dispose();
            }
        }
    }

    public class PoolObhectJsonInter3
    {
        public readonly Stack<JsonDocument> _pool = new Stack<JsonDocument>();

        public JsonDocument Connect(string json)
        {
            lock (_pool)
            {
                if (_pool.Count > 1)
                {
                    _pool.Pop();
                }
            }
            return JsonDocument.Parse(json);
        }

        public void close(JsonDocument document)
        {
            lock (_pool)
            {
                document?.Dispose();
            }
        }
    }
}
