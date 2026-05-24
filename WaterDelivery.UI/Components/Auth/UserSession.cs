namespace WaterDelivery.UI.Components.Auth;

public class UserSession
{
    public Guid? UserId { get; private set; }

    public bool IsAuthenticated => UserId is not null;

    public void SignIn(Guid userId)
    {
        UserId = userId;
    }

    public void SignOut()
    {
        UserId = null;
    }
}