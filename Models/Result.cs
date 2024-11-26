namespace ProjectMVC.Models
{
	public class Result
	{
		public bool Succeeded { get; set; }
		public string? Error { get; set; }
		public string? SuccessMessage { get; set; }

		public static Result Success(string success) => new Result { Succeeded = true , SuccessMessage = success};
		public static Result Failure(string error) => new Result { Succeeded = false, Error = error };
		// public static Result SuccessMessage(string success) => new Result { Succeeded = true, SucceedMessage = success };
	}
}
