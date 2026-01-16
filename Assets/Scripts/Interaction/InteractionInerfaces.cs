namespace Interaction
{
    public interface IClickable
    {
        void OnClick();
    }
    public interface IPressable
    {
        void OnPress();
    }

    public interface ICancellable
    {
        void OnCancel();
    }
    public interface IHoldable
    {
        void OnHold();
    }
    public interface IReleasable
    {
        void OnRelease();
    }
    public interface IInteractable : IClickable, IPressable, ICancellable, IHoldable, IReleasable
    {
        
    }
}