using System;

namespace OrchardCore.Queries.Sql
{
    public class SqlParameterException : Exception
    {
        public SqlParameterException(string message) : base(message)
        {
        }
    }
}
