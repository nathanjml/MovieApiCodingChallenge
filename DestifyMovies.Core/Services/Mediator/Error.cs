using System.Net;

namespace DestifyMovies.Core.Services.Mediator
{
    public struct Error
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
