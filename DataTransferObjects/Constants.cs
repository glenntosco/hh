using System;

namespace Pro4Soft.DataTransferObjects
{
	public static class Constants
	{
        public const string SuperUsername = "sa";

        public const string ProductName = "P4W";

        public const string AuthenticationToken = nameof(AuthenticationToken);
		public const string ApiKey = nameof(ApiKey);
        public const string UserDefinedFields = nameof(UserDefinedFields);

        public const string SuperUser = nameof(SuperUser);
        public const string Translation = nameof(Translation);

        public const string TenantCache = nameof(TenantCache);
        public const string MasterCache = nameof(MasterCache);

        public const string Tenant = nameof(Tenant);
        public const string User = nameof(User);
        public const string UserSession = nameof(UserSession);
        public static readonly Guid SystemUserId = new Guid("0BE2C4D6-A793-489E-9792-FDBCB320D56C");
    }
}