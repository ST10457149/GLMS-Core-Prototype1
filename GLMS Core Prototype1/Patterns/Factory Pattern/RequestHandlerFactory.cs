namespace GLMS_Core_Prototype.Patterns.Factory_Pattern
{
    public static class RequestHandlerFactory
    {
        public static IRequestHandler Create(string type)
        {
            return type == "Urgent" ? new UrgentRequestHandler() : new StandardRequestHandler();
        }
    }
}