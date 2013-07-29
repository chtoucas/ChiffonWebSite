namespace Pacr.WebSite.HttpHandlers {
    using System;
    using System.Web;
    using Narvalo.Common;
    using Narvalo.Common.Web;

    public class StringTestHandler : ValidatingHttpHandlerBase<StringModel> {
        public StringTestHandler() : base() { }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        public override bool IsReusable { get { return true; } }

        protected override StringModel BindRequest(HttpRequest request) {
            return new StringModel() { Value = request.QueryString["test"] };
        }

        protected override void ProcessValidatedRequest(HttpContext context, StringModel model) {
            throw new NotImplementedException();
        }
    }

    public class IntTestHandler : ValidatingHttpHandlerBase<ValueModel<int>> {
        public IntTestHandler() : base() { }

        public override HttpVerbs AcceptedVerbs { get { return HttpVerbs.Get; } }

        public override bool IsReusable { get { return true; } }

        protected override ValueModel<int> BindRequest(HttpRequest request) {
            return new ValueModel<int>() { Value = request.QueryString.GetInt32Value("test") };
        }

        protected override void ProcessValidatedRequest(HttpContext context, ValueModel<int> model) {
            throw new NotImplementedException();
        }
    }
}
