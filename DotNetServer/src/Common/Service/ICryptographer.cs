namespace Common.Service
{
	public interface ICryptographer
	{
	    string CreateTemp();
		string CreateSalt();
		string ComputeHash(string valueToHash);
		string GetPasswordHash(string password, string salt);
	}
}