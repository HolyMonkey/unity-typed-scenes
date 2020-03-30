public class CustomClass
{
    private string _message;

    public CustomClass(string message)
    {
        _message = message;
    }

    public override string ToString()
    {
        return _message;
    }
}
