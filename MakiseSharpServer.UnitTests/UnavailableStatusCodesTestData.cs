using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace MakiseSharpServer.UnitTests
{
    public class UnavailableStatusCodesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { HttpStatusCode.ServiceUnavailable};
            yield return new object[] { HttpStatusCode.InternalServerError};
            yield return new object[] { HttpStatusCode.RequestTimeout };
            yield return new object[] { HttpStatusCode.GatewayTimeout};
            yield return new object[] { HttpStatusCode.BadGateway };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
