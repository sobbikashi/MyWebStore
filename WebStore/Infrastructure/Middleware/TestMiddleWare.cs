using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    #region Это промежуточное ПО нужно, чтобы, если сервер почему-то что-то не может обработать, можно было посмотреть, чем он там подавился (это будет в context)
    public class TestMiddleWare
    {
        private readonly ILogger<TestMiddleWare> _Logger;
        private readonly RequestDelegate _Next;

        
        public TestMiddleWare(RequestDelegate next, ILogger<TestMiddleWare> Logger)
        {
            _Next = next;
            _Logger = Logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //обработка контекста
            var processing = _Next(context);
            //выполнять работу в то время, пока оставшаяся часть конвейера что-то делает с контекстом
            await processing;
            //обработка результата оставшейся части конвейера
        }
       
    }
    #endregion
}
