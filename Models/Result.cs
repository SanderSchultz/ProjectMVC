namespace ProjectMVC.Models
{
	public class Result
	{
		public bool Succeeded { get; set; }
		public string Error { get; set; } = "An error occurred";
		public string SuccessMessage { get; set; } = string.Empty;

		public static Result Success(string success) => new Result { Succeeded = true , SuccessMessage = success};
		public static Result Failure(string error) => new Result { Succeeded = false, Error = error };
	}
}
