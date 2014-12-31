namespace FunctionalQuery
{
	public interface IDatabase
	{
		// returns a Query object that lets you map the results any way you want
		Query Query(string sql, object parameters = null, StatementOptions options = null);
		Query Query(string connectionKey, string sql, object parameters = null, StatementOptions options = null);
	}
}