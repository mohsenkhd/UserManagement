namespace Application.ViewModels.Main
{
    public class MainRes
    {
        protected MainRes()
        {
            Code = 0;
            ClientMessage = "";
        }


        protected MainRes(int code, string clientMessage)
        {
            Code = code;
            ClientMessage = clientMessage;
        }


        public int Code { get; set; }

        public string ClientMessage { get; set; }
    }
}