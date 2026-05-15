namespace CepApi.Exceptions;

public sealed class ViaCepUnavailableException : Exception
{
    public ViaCepUnavailableException()
    {
    }

    public ViaCepUnavailableException(Exception innerException)
        : base("ViaCEP indisponivel", innerException)
    {
    }
}
