namespace CoreApp.Enums;

public enum GradeValue
{
    F = 20,      // 2.0
    D = 30,      // 3.0
    C = 35,      // 3.5
    B = 40,      // 4.0
    B_PLUS = 45, // 4.5
    A = 50       // 5.0
}

public static class GradeValueExtensions
{
    public static double Value(this GradeValue grade) => (double)grade / 10.0;

    public static string ToDisplayString(this GradeValue grade) => grade.Value().ToString("0.0");

    public static GradeValue FromDouble(double value) => value switch
    {
        2.0 => GradeValue.F,
        3.0 => GradeValue.D,
        3.5 => GradeValue.C,
        4.0 => GradeValue.B,
        4.5 => GradeValue.B_PLUS,
        5.0 => GradeValue.A,
        _ => throw new ArgumentOutOfRangeException(nameof(value), $"Invalid grade value: {value}. Allowed values are 2.0, 3.0, 3.5, 4.0, 4.5, 5.0.")
    };
}