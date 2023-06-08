namespace Sannel.House.Web;

public static class AuthPolicies
{
	public static class Sprinklers
	{
		public const string ALL = "Sprinklers";
		public const string SCHEDULE_READ = "Sprinklers/Schedule/Read";
		public const string SCHEDULE_WRITE = "Sprinklers/Schedule/Write";
		public const string ZONE_READ = "Sprinklers/Zone/Read";
		public const string ZONE_WRITE = "Sprinklers/Zone/Write";
	}
}
