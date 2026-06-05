namespace CoreApp.ValueObjects;

public record Pesel
{
    public string Value { get; }

    public Pesel(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 11 || !value.All(char.IsDigit))
            throw new ArgumentException("PESEL musi składać się dokładnie z 11 cyfr.");

        if (!IsValid(value))
            throw new ArgumentException("Nieprawidłowa suma kontrolna PESEL.");

        Value = value;
    }

    // Pozwala na niejawne rzutowanie z Pesel na string (np. w AutoMapperze)
    public static implicit operator string(Pesel pesel) => pesel.Value;
    
    // Jawne rzutowanie ze stringa na Pesel
    public static explicit operator Pesel(string value) => new(value);

    public override string ToString() => Value;

    private static bool IsValid(string pesel)
    {
        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        int sum = 0;
        
        for (int i = 0; i < 10; i++)
        {
            sum += (pesel[i] - '0') * weights[i];
        }
        
        int checksum = (10 - (sum % 10)) % 10;
        return checksum == (pesel[10] - '0');
    }
}