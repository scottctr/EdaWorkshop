namespace BusinessLogic
{
    public class GetRequestForServiceCommand
    {
        public string UserName { get; set; }

        private GetRequestForServiceCommand()
        { /* for serialization only */ }

        public GetRequestForServiceCommand(string userName)
        {
            UserName = userName;
        }
    }
}