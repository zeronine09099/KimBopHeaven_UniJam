namespace Interaction
{
    public interface IClcikkable
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
    public interface IInteractable : IClcikkable, IPressable, ICancellable, IHoldable
    {
        
    }
}