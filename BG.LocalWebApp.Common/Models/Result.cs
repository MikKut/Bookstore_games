namespace BG.LocalWebApp.Common.Models
{
    /// <summary>
    /// Represents the result of an operation, with information about success, errors, and validation errors.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class with validation errors.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successful.</param>
        /// <param name="validationErrors">A dictionary of validation errors.</param>
        public Result(bool succeeded, IDictionary<string, string[]> validationErrors)
        {
            Succeeded = succeeded;
            Errors = Array.Empty<string>();
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class with a list of errors.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successful.</param>
        /// <param name="errors">A collection of error messages.</param>
        public Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool Succeeded { get; init; }

        /// <summary>
        /// Gets a value indicating whether the validation has failed based on the presence of validation errors.
        /// </summary>
        public bool ValidationFailed => ValidationErrors?.Any() ?? false;

        /// <summary>
        /// Gets the list of error messages.
        /// </summary>
        public string[] Errors { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Gets a value indicating whether there are any errors.
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// Gets the dictionary of validation errors, if any.
        /// </summary>
        public IDictionary<string, string[]>? ValidationErrors { get; init; }

        /// <summary>
        /// Creates a successful <see cref="Result"/> instance with no errors.
        /// </summary>
        /// <returns>A successful <see cref="Result"/>.</returns>
        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        /// <summary>
        /// Creates a failed <see cref="Result"/> instance with a list of errors.
        /// </summary>
        /// <param name="errors">A collection of error messages.</param>
        /// <returns>A failed <see cref="Result"/>.</returns>
        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }

        /// <summary>
        /// Creates a failed <see cref="Result"/> instance with a single error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed <see cref="Result"/>.</returns>
        public static Result Failure(string error)
        {
            return new Result(false, new List<string> { error });
        }

        /// <summary>
        /// Creates a failed <see cref="Result"/> instance with validation errors.
        /// </summary>
        /// <param name="errors">A dictionary of validation errors.</param>
        /// <returns>A failed <see cref="Result"/>.</returns>
        public static Result Failure(IDictionary<string, string[]> errors)
        {
            var errorList = errors.Select(x => $"{string.Join(",", x.Value)}");
            return new Result(false, errorList);
        }

        /// <summary>
        /// Creates a failed <see cref="Result"/> instance with validation errors.
        /// </summary>
        /// <param name="validationErrors">A dictionary of validation errors.</param>
        /// <returns>A failed <see cref="Result"/> with validation errors.</returns>
        public static Result ValidationFailure(IDictionary<string, string[]> validationErrors)
        {
            return new Result(false, validationErrors);
        }
    }

    /// <summary>
    /// Represents the result of an operation, with additional data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the result.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class with a list of errors and associated data.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successful.</param>
        /// <param name="errors">A collection of error messages.</param>
        /// <param name="data">The data associated with the result.</param>
        public Result(bool succeeded, IEnumerable<string> errors, T data)
            : base(succeeded, errors)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class with a list of errors.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successful.</param>
        /// <param name="errors">A collection of error messages.</param>
        public Result(bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
        {
        }

        /// <summary>
        /// Gets the data associated with the result.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// Creates a successful <see cref="Result{T}"/> instance with associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <returns>A successful <see cref="Result{T}"/>.</returns>
        public static Result<T> Success(T data)
        {
            return new Result<T>(true, Array.Empty<string>(), data);
        }

        /// <summary>
        /// Creates a failed <see cref="Result{T}"/> instance with a single error message and associated data.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <returns>A failed <see cref="Result{T}"/>.</returns>
        public static Result<T> Failure(string error, T data)
        {
            return new Result<T>(false, new List<string> { error }, data);
        }

        /// <summary>
        /// Creates a failed <see cref="Result{T}"/> instance with a list of errors.
        /// </summary>
        /// <param name="errors">A collection of error messages.</param>
        /// <returns>A failed <see cref="Result{T}"/>.</returns>
        public static new Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, errors);
        }

        /// <summary>
        /// Creates a failed <see cref="Result{T}"/> instance with a single error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed <see cref="Result{T}"/>.</returns>
        public static new Result<T> Failure(string error)
        {
            return new Result<T>(false, new List<string> { error });
        }

        /// <summary>
        /// Creates a failed <see cref="Result{T}"/> instance with validation errors.
        /// </summary>
        /// <param name="errors">A dictionary of validation errors.</param>
        /// <returns>A failed <see cref="Result{T}"/>.</returns>
        public static new Result<T> Failure(IDictionary<string, string[]> errors)
        {
            var errorList = errors.Select(x => $"{string.Join(",", x.Value)}");
            return new Result<T>(false, errorList);
        }
    }
}
