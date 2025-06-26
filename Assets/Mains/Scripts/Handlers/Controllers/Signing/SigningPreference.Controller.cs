namespace YNL.JAMOS
{
    public partial class SigningPreference
    {
        private class Controller : PageController
        {
            private SigningPreference _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningPreference;
            }
        }
    }
}