namespace Touch.Controls
{
    public interface INavigableUserControl
    {
        bool Shown { get; set; }

        void OnShow();

        void OnHide();

        void ToggleAnimation();
    }
}